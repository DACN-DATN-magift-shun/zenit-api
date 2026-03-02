namespace Zenit.Management.Business.Helpers
{
    public class SqlHelper
    {
        public static string UpsertDailyStatisticsWithAddedTransactions()
        {
            return @"
                /* CategoryGroupDailyStatistics */
                INSERT INTO ""CategoryGroupDailyStatistics"" (""Id"", ""Date"", ""TotalAmount"", ""Percentage"", ""PercentageChange"", ""GroupType"", ""AccountId"", ""CreatedById"", ""CreatedAt"", ""IsDeleted"")
                VALUES (@CategoryGroupStatsId, @Date::date, @TotalAmount, 0.0, 0.0, @GroupType, @AccountId, @CreatedById, NOW(), false)
                ON CONFLICT (""Date"", ""GroupType"", ""AccountId"") DO UPDATE
                SET ""TotalAmount"" = ""CategoryGroupDailyStatistics"".""TotalAmount"" + @TotalAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById;

                -- Cập nhật Percentage cho TẤT CẢ và PercentageChange cho 1 GroupType
                WITH TotalAmounts AS (
                    SELECT 
                        SUM(""TotalAmount"") as Total
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date::date
                        AND ""AccountId"" = @AccountId
                ), 
                PreviousDayData AS (
                    SELECT 
                        ""GroupType"",
                        ""Percentage"",
                        ""TotalAmount""
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date::date - INTERVAL '1 day' 
                        AND ""AccountId"" = @AccountId
                        AND ""GroupType"" = @GroupType  -- Chỉ lấy data của GroupType cần update PercentageChange
                ),
                CalculatedValues AS (
                    SELECT 
                        cgds.""Id"",
                        cgds.""GroupType"",
                        -- Percentage: tính cho TẤT CẢ GroupType
                        CASE 
                            WHEN ta.Total = 0 THEN 0.0
                            ELSE (cgds.""TotalAmount""::decimal / ta.Total * 100)
                        END AS ""NewPercentage"",
                        -- PercentageChange: chỉ tính cho GroupType được chỉ định
                        CASE 
                            WHEN cgds.""GroupType"" = @GroupType THEN
                                CASE 
                                    WHEN pdd.""TotalAmount"" IS NULL THEN 100.0
                                    ELSE ((cgds.""TotalAmount"" - pdd.""TotalAmount"")::decimal / pdd.""TotalAmount"" * 100)
                                END
                            ELSE cgds.""PercentageChange""  -- Giữ nguyên giá trị cũ cho các GroupType khác
                        END AS ""NewPercentageChange""
                    FROM ""CategoryGroupDailyStatistics"" cgds
                    CROSS JOIN TotalAmounts ta
                    LEFT JOIN PreviousDayData pdd 
                        ON pdd.""GroupType"" = cgds.""GroupType""
                    WHERE cgds.""Date"" = @Date::date 
                        AND cgds.""AccountId"" = @AccountId
                )
                UPDATE ""CategoryGroupDailyStatistics""
                SET ""Percentage"" = cv.""NewPercentage"",
                    ""PercentageChange"" = cv.""NewPercentageChange""
                FROM CalculatedValues cv
                WHERE ""CategoryGroupDailyStatistics"".""Id"" = cv.""Id"";

                /* CategoryDailyStatistics */
                WITH CategoryGroupId AS (
                    SELECT ""Id"" 
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date::date
                        AND ""GroupType"" = @GroupType
                        AND ""AccountId"" = @AccountId
                ) INSERT INTO ""CategoryDailyStatistics"" (""Id"", ""Date"", ""TotalAmount"", ""Percentage"", ""PercentageChange"", ""CategoryId"", ""GroupId"", ""CreatedById"", ""CreatedAt"", ""IsDeleted"")
                VALUES (@CategoryStatsId, @Date::date, @TotalAmount, 0.0, 0.0, @CategoryId, (SELECT ""Id"" FROM CategoryGroupId), @CreatedById, NOW(), false)
                ON CONFLICT (""Date"", ""CategoryId"") DO UPDATE
                SET ""TotalAmount"" = ""CategoryDailyStatistics"".""TotalAmount"" + @TotalAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById;

                -- Cập nhật Percentage và PercentageChange cho Category
                WITH UpdateData AS (
                    SELECT 
                        cds.""Id"",
                        cds.""CategoryId"",
                        cds.""TotalAmount"",
                        gds.""TotalAmount"" AS ""GroupTotalAmount"",
                        CASE 
                            WHEN gds.""TotalAmount"" = 0 THEN 0.0
                            ELSE (cds.""TotalAmount""::decimal / gds.""TotalAmount"" * 100)
                        END AS ""NewPercentage""
                    FROM ""CategoryDailyStatistics"" cds
                    INNER JOIN ""CategoryGroupDailyStatistics"" gds 
                        ON cds.""GroupId"" = gds.""Id""
                    WHERE cds.""Date"" = @Date::date
                        AND gds.""Date"" = @Date::date
                        AND gds.""AccountId"" = @AccountId
                        AND gds.""GroupType"" = @GroupType
                ),
                PreviousDayData AS (
                    SELECT 
                        cds_prev.""CategoryId"",
                        cds_prev.""Percentage"",
                        cds_prev.""TotalAmount""
                    FROM ""CategoryDailyStatistics"" cds_prev
                    INNER JOIN ""CategoryGroupDailyStatistics"" cgds_prev 
                        ON cds_prev.""GroupId"" = cgds_prev.""Id""
                    WHERE cds_prev.""Date"" = @Date::date - INTERVAL '1 day' 
                        AND cgds_prev.""GroupType"" = @GroupType
                        AND cgds_prev.""AccountId"" = @AccountId
                ),
                FinalCalculation AS (
                    SELECT 
                        ud.""Id"",
                        ud.""NewPercentage"",
                        -- PercentageChange = (TotalAmount hôm nay - TotalAmount hôm qua) / TotalAmount hôm qua * 100
                        CASE 
                            WHEN pdd.""TotalAmount"" IS NULL THEN 100.0
                            ELSE ((ud.""TotalAmount"" - pdd.""TotalAmount"")::decimal / pdd.""TotalAmount"" * 100)
                        END AS ""NewPercentageChange""
                    FROM UpdateData ud
                    LEFT JOIN PreviousDayData pdd 
                        ON pdd.""CategoryId"" = ud.""CategoryId""
                )
                UPDATE ""CategoryDailyStatistics""
                SET ""Percentage"" = fc.""NewPercentage"",
                    ""PercentageChange"" = fc.""NewPercentageChange""
                FROM FinalCalculation fc
                WHERE ""CategoryDailyStatistics"".""Id"" = fc.""Id"";
            ";
        }

        public static string UpsertDailyStatisticsWithDeletedTransactions()
        {
            return @"
                /* CategoryGroupDailyStatistics */
                UPDATE zenit_management_dev.""CategoryGroupDailyStatistics""
                SET ""TotalAmount"" = ""TotalAmount"" - @TotalAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById
                WHERE ""Date"" = @Date::date 
                    AND ""GroupType"" = @GroupType
                    AND ""AccountId"" = @AccountId;

                -- Cập nhật Percentage cho TẤT CẢ và PercentageChange cho 1 GroupType
                WITH TotalAmounts AS (
                    SELECT 
                        SUM(""TotalAmount"") as Total
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date::date 
                        AND ""AccountId"" = @AccountId
                ), 
                PreviousDayData AS (
                    SELECT 
                        ""GroupType"",
                        ""Percentage"",
                        ""TotalAmount""
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date::date - INTERVAL '1 day' 
                        AND ""AccountId"" = @AccountId
                        AND ""GroupType"" = @GroupType  -- Chỉ lấy data của GroupType cần update PercentageChange
                ),
                CalculatedValues AS (
                    SELECT 
                        cgds.""Id"",
                        cgds.""GroupType"",
                        -- Percentage: tính cho TẤT CẢ GroupType
                        CASE 
                            WHEN ta.Total = 0 THEN 0.0
                            ELSE (cgds.""TotalAmount""::decimal / ta.Total * 100)
                        END AS ""NewPercentage"",
                        -- PercentageChange: chỉ tính cho GroupType được chỉ định
                        CASE 
                            WHEN cgds.""GroupType"" = @GroupType THEN
                                CASE 
                                    WHEN pdd.""TotalAmount"" IS NULL AND cgds.""TotalAmount"" > 0 THEN 100.0
                                    WHEN pdd.""TotalAmount"" IS NULL AND cgds.""TotalAmount"" = 0 THEN 0.0
                                    ELSE ((cgds.""TotalAmount"" - pdd.""TotalAmount"")::decimal / pdd.""TotalAmount"" * 100)
                                END
                            ELSE cgds.""PercentageChange""  -- Giữ nguyên giá trị cũ cho các GroupType khác
                        END AS ""NewPercentageChange""
                    FROM ""CategoryGroupDailyStatistics"" cgds
                    CROSS JOIN TotalAmounts ta
                    LEFT JOIN PreviousDayData pdd 
                        ON pdd.""GroupType"" = cgds.""GroupType""
                    WHERE cgds.""Date"" = @Date::date 
                        AND cgds.""AccountId"" = @AccountId
                )
                UPDATE ""CategoryGroupDailyStatistics""
                SET ""Percentage"" = cv.""NewPercentage"",
                    ""PercentageChange"" = cv.""NewPercentageChange""
                FROM CalculatedValues cv
                WHERE ""CategoryGroupDailyStatistics"".""Id"" = cv.""Id"";

                /* CategoryDailyStatistics */
                UPDATE zenit_management_dev.""CategoryDailyStatistics"" cds
                SET ""TotalAmount"" = cds.""TotalAmount"" - @TotalAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById
                FROM ""CategoryGroupDailyStatistics"" cgd 
                WHERE cds.""GroupId"" = cgd.""Id""
                    AND cds.""Date"" = @Date::date 
                    AND cds.""CategoryId"" = @CategoryId
                    AND cgd.""AccountId"" = @AccountId;

                -- Cập nhật Percentage và PercentageChange cho Category
                WITH UpdateData AS (
                    SELECT 
                        cds.""Id"",
                        cds.""CategoryId"",
                        cds.""TotalAmount"",
                        gds.""TotalAmount"" AS ""GroupTotalAmount"",
                        CASE 
                            WHEN gds.""TotalAmount"" = 0 THEN 0.0
                            ELSE (cds.""TotalAmount""::decimal / gds.""TotalAmount"" * 100)
                        END AS ""NewPercentage""
                    FROM ""CategoryDailyStatistics"" cds
                    INNER JOIN ""CategoryGroupDailyStatistics"" gds 
                        ON cds.""GroupId"" = gds.""Id""
                    WHERE cds.""Date"" = @Date::date
                        AND gds.""Date"" = @Date::date
                        AND gds.""AccountId"" = @AccountId
                        AND gds.""GroupType"" = @GroupType
                ),
                PreviousDayData AS (
                    SELECT 
                        cds_prev.""CategoryId"",
                        cds_prev.""Percentage"",
                        cds_prev.""TotalAmount""
                    FROM ""CategoryDailyStatistics"" cds_prev
                    INNER JOIN ""CategoryGroupDailyStatistics"" cgds_prev 
                        ON cds_prev.""GroupId"" = cgds_prev.""Id""
                    WHERE cds_prev.""Date"" = @Date::date - INTERVAL '1 day' 
                        AND cgds_prev.""GroupType"" = @GroupType
                        AND cgds_prev.""AccountId"" = @AccountId
                ),
                FinalCalculation AS (
                    SELECT 
                        ud.""Id"",
                        ud.""NewPercentage"",
                        -- PercentageChange = (TotalAmount hôm nay - TotalAmount hôm qua) / TotalAmount hôm qua * 100
                        CASE 
                            WHEN pdd.""TotalAmount"" IS NULL AND ud.""TotalAmount"" > 0 THEN 100.0
                            WHEN pdd.""TotalAmount"" IS NULL AND ud.""TotalAmount"" = 0 THEN 0.0
                            ELSE ((ud.""TotalAmount"" - pdd.""TotalAmount"")::decimal / pdd.""TotalAmount"" * 100)
                        END AS ""NewPercentageChange""
                    FROM UpdateData ud
                    LEFT JOIN PreviousDayData pdd 
                        ON pdd.""CategoryId"" = ud.""CategoryId""
                )
                UPDATE ""CategoryDailyStatistics""
                SET ""Percentage"" = fc.""NewPercentage"",
                    ""PercentageChange"" = fc.""NewPercentageChange""
                FROM FinalCalculation fc
                WHERE ""CategoryDailyStatistics"".""Id"" = fc.""Id"";
            ";
        }

        public static string UpsertDailyStatisticsWithModifiedTransactions()
        {
            return @"
                -- Bước 1A: Trừ TotalAmount khỏi CategoryGroupDailyStatistics ngày cũ
                UPDATE zenit_management_dev.""CategoryGroupDailyStatistics""
                SET ""TotalAmount"" = ""TotalAmount"" - @OldAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById
                WHERE ""Date"" = @OldTransactionDate::date 
                    AND ""GroupType"" = @OldGroupType
                    AND ""AccountId"" = @AccountId;

                -- Bước 1B: Cập nhật Percentage cho CategoryGroupDailyStatistics ngày CŨ
                WITH TotalAmountsOld AS (
                    SELECT 
                        SUM(""TotalAmount"") as Total
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @OldTransactionDate::date
                        AND ""AccountId"" = @AccountId
                ), 
                PreviousDayDataOldGroup AS (
                    SELECT 
                        ""GroupType"",
                        ""Percentage"",
                        ""TotalAmount""
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @OldTransactionDate::date - INTERVAL '1 day' 
                        AND ""AccountId"" = @AccountId
                        AND ""GroupType"" = @OldGroupType
                ),
                CalculatedValuesOldGroup AS (
                    SELECT 
                        cgds.""Id"",
                        cgds.""GroupType"",
                        CASE 
                            WHEN tao.Total = 0 THEN 0.0
                            ELSE (cgds.""TotalAmount""::decimal / tao.Total * 100)
                        END AS ""NewPercentage"",
                        CASE 
                            WHEN cgds.""GroupType"" = @OldGroupType THEN
                                CASE 
                                    WHEN pddog.""TotalAmount"" IS NULL AND cgds.""TotalAmount"" > 0 THEN 100.0
                                    WHEN pddog.""TotalAmount"" IS NULL AND cgds.""TotalAmount"" = 0 THEN 0.0
                                    WHEN pddog.""TotalAmount"" = 0 THEN 0.0
                                    ELSE ((cgds.""TotalAmount"" - pddog.""TotalAmount"")::decimal / pddog.""TotalAmount"" * 100)
                                END
                            ELSE cgds.""PercentageChange""
                        END AS ""NewPercentageChange""
                    FROM ""CategoryGroupDailyStatistics"" cgds
                    CROSS JOIN TotalAmountsOld tao
                    LEFT JOIN PreviousDayDataOldGroup pddog 
                        ON pddog.""GroupType"" = cgds.""GroupType""
                    WHERE cgds.""Date"" = @OldTransactionDate::date 
                        AND cgds.""AccountId"" = @AccountId
                )
                UPDATE ""CategoryGroupDailyStatistics""
                SET ""Percentage"" = cvog.""NewPercentage"",
                    ""PercentageChange"" = cvog.""NewPercentageChange"",
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById
                FROM CalculatedValuesOldGroup cvog
                WHERE ""CategoryGroupDailyStatistics"".""Id"" = cvog.""Id"";

                -- Bước 1C: Trừ TotalAmount khỏi CategoryDailyStatistics ngày cũ
                UPDATE zenit_management_dev.""CategoryDailyStatistics"" cds
                SET ""TotalAmount"" = cds.""TotalAmount"" - @OldAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById
                FROM ""CategoryGroupDailyStatistics"" cgd 
                WHERE cds.""GroupId"" = cgd.""Id""
                    AND cds.""Date"" = @OldTransactionDate::date 
                    AND cds.""CategoryId"" = @OldCategoryId
                    AND cgd.""AccountId"" = @AccountId;
                    
                -- Bước 1D: Cập nhật Percentage cho CategoryDailyStatistics ngày CŨ
                WITH UpdateDataOld AS (
                    SELECT 
                        cds.""Id"",
                        cds.""CategoryId"",
                        cds.""TotalAmount"",
                        gds.""TotalAmount"" AS ""GroupTotalAmount"",
                        CASE 
                            WHEN gds.""TotalAmount"" = 0 THEN 0.0
                            ELSE (cds.""TotalAmount""::decimal / gds.""TotalAmount"" * 100)
                        END AS ""NewPercentage""
                    FROM ""CategoryDailyStatistics"" cds
                    INNER JOIN ""CategoryGroupDailyStatistics"" gds 
                        ON cds.""GroupId"" = gds.""Id""
                    WHERE cds.""Date"" = @OldTransactionDate::date
                        AND gds.""Date"" = @OldTransactionDate::date
                        AND gds.""AccountId"" = @AccountId
                        AND gds.""GroupType"" = @OldGroupType
                ),
                PreviousDayDataOld AS (
                    SELECT 
                        cds_prev.""CategoryId"",
                        cds_prev.""Percentage"",
                        cds_prev.""TotalAmount""
                    FROM ""CategoryDailyStatistics"" cds_prev
                    INNER JOIN ""CategoryGroupDailyStatistics"" cgds_prev 
                        ON cds_prev.""GroupId"" = cgds_prev.""Id""
                    WHERE cds_prev.""Date"" = @OldTransactionDate::date - INTERVAL '1 day' 
                        AND cgds_prev.""AccountId"" = @AccountId
                        AND cgds_prev.""GroupType"" = @OldGroupType
                ),
                FinalCalculationOld AS (
                    SELECT 
                        udo.""Id"",
                        udo.""NewPercentage"",
                        CASE 
                            WHEN pddo.""TotalAmount"" IS NULL AND udo.""TotalAmount"" > 0 THEN 100.0
                            WHEN pddo.""TotalAmount"" IS NULL AND udo.""TotalAmount"" = 0 THEN 0.0
                            WHEN pddo.""TotalAmount"" = 0 THEN 0.0
                            ELSE ((udo.""TotalAmount"" - pddo.""TotalAmount"")::decimal / pddo.""TotalAmount"" * 100)
                        END AS ""NewPercentageChange""
                    FROM UpdateDataOld udo
                    LEFT JOIN PreviousDayDataOld pddo 
                        ON pddo.""CategoryId"" = udo.""CategoryId""
                )
                UPDATE ""CategoryDailyStatistics""
                SET ""Percentage"" = fco.""NewPercentage"",
                    ""PercentageChange"" = fco.""NewPercentageChange"",
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById
                FROM FinalCalculationOld fco
                WHERE ""CategoryDailyStatistics"".""Id"" = fco.""Id"";

                -- Bước 2A: Thêm/Cập nhật TotalAmount cho CategoryGroupDailyStatistics ngày MỚI
                INSERT INTO ""CategoryGroupDailyStatistics"" (""Id"", ""Date"", ""TotalAmount"", ""Percentage"", ""PercentageChange"", ""GroupType"", ""AccountId"", ""CreatedById"", ""CreatedAt"", ""IsDeleted"")
                VALUES (@CategoryGroupStatsId, @Date::date, @TotalAmount, 0.0, 0.0, @GroupType, @AccountId, @CreatedById, NOW(), false)
                ON CONFLICT (""Date"", ""GroupType"", ""AccountId"") DO UPDATE
                SET ""TotalAmount"" = ""CategoryGroupDailyStatistics"".""TotalAmount"" + @TotalAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById;

                -- Bước 2B: Cập nhật Percentage cho CategoryGroupDailyStatistics ngày MỚI
                WITH TotalAmounts AS (
                    SELECT 
                        SUM(""TotalAmount"") as Total
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date::date
                        AND ""AccountId"" = @AccountId
                ), 
                PreviousDayData AS (
                    SELECT 
                        ""GroupType"",
                        ""Percentage"",
                        ""TotalAmount""
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date::date - INTERVAL '1 day' 
                        AND ""AccountId"" = @AccountId
                        AND ""GroupType"" = @GroupType
                ),
                CalculatedValues AS (
                    SELECT 
                        cgds.""Id"",
                        cgds.""GroupType"",
                        CASE 
                            WHEN ta.Total = 0 THEN 0.0
                            ELSE (cgds.""TotalAmount""::decimal / ta.Total * 100)
                        END AS ""NewPercentage"",
                        CASE 
                            WHEN cgds.""GroupType"" = @GroupType THEN
                                CASE 
                                    WHEN pdd.""TotalAmount"" IS NULL AND cgds.""TotalAmount"" > 0 THEN 100.0
                                    WHEN pdd.""TotalAmount"" IS NULL AND cgds.""TotalAmount"" = 0 THEN 0.0
                                    WHEN pdd.""TotalAmount"" = 0 THEN 0.0
                                    ELSE ((cgds.""TotalAmount"" - pdd.""TotalAmount"")::decimal / pdd.""TotalAmount"" * 100)
                                END
                            ELSE cgds.""PercentageChange""
                        END AS ""NewPercentageChange""
                    FROM ""CategoryGroupDailyStatistics"" cgds
                    CROSS JOIN TotalAmounts ta
                    LEFT JOIN PreviousDayData pdd 
                        ON pdd.""GroupType"" = cgds.""GroupType""
                    WHERE cgds.""Date"" = @Date::date
                        AND cgds.""AccountId"" = @AccountId
                )
                UPDATE ""CategoryGroupDailyStatistics""
                SET ""Percentage"" = cv.""NewPercentage"",
                    ""PercentageChange"" = cv.""NewPercentageChange"",
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById
                FROM CalculatedValues cv
                WHERE ""CategoryGroupDailyStatistics"".""Id"" = cv.""Id"";

                -- Bước 2C: Thêm/Cập nhật TotalAmount cho CategoryDailyStatistics ngày MỚI
                WITH CategoryGroupId AS (
                    SELECT ""Id"" 
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date::date
                        AND ""GroupType"" = @GroupType
                        AND ""AccountId"" = @AccountId
                ) 
                INSERT INTO ""CategoryDailyStatistics"" (""Id"", ""Date"", ""TotalAmount"", ""Percentage"", ""PercentageChange"", ""CategoryId"", ""GroupId"", ""CreatedById"", ""CreatedAt"", ""IsDeleted"")
                VALUES (@CategoryStatsId, @Date::date, @TotalAmount, 0.0, 0.0, @CategoryId, (SELECT ""Id"" FROM CategoryGroupId), @CreatedById, NOW(), false)
                ON CONFLICT (""Date"", ""CategoryId"") DO UPDATE
                SET ""TotalAmount"" = ""CategoryDailyStatistics"".""TotalAmount"" + @TotalAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById;

                -- Bước 2D: Cập nhật Percentage và PercentageChange cho CategoryDailyStatistics ngày MỚI
                WITH UpdateData AS (
                    SELECT 
                        cds.""Id"",
                        cds.""CategoryId"",
                        cds.""TotalAmount"",
                        gds.""TotalAmount"" AS ""GroupTotalAmount"",
                        CASE 
                            WHEN gds.""TotalAmount"" = 0 THEN 0.0
                            ELSE (cds.""TotalAmount""::decimal / gds.""TotalAmount"" * 100)
                        END AS ""NewPercentage""
                    FROM ""CategoryDailyStatistics"" cds
                    INNER JOIN ""CategoryGroupDailyStatistics"" gds 
                        ON cds.""GroupId"" = gds.""Id""
                    WHERE cds.""Date"" = @Date::date
                        AND gds.""Date"" = @Date::date
                        AND gds.""AccountId"" = @AccountId
                        AND gds.""GroupType"" = @GroupType
                ),
                PreviousDayData AS (
                    SELECT 
                        cds_prev.""CategoryId"",
                        cds_prev.""Percentage"",
                        cds_prev.""TotalAmount""
                    FROM ""CategoryDailyStatistics"" cds_prev
                    INNER JOIN ""CategoryGroupDailyStatistics"" cgds_prev 
                        ON cds_prev.""GroupId"" = cgds_prev.""Id""
                    WHERE cds_prev.""Date"" = @Date::date - INTERVAL '1 day' 
                        AND cgds_prev.""AccountId"" = @AccountId
                        AND cgds_prev.""GroupType"" = @GroupType
                ),
                FinalCalculation AS (
                    SELECT 
                        ud.""Id"",
                        ud.""NewPercentage"",
                        CASE 
                            WHEN pdd.""TotalAmount"" IS NULL AND ud.""TotalAmount"" > 0 THEN 100.0
                            WHEN pdd.""TotalAmount"" IS NULL AND ud.""TotalAmount"" = 0 THEN 0.0
                            WHEN pdd.""TotalAmount"" = 0 THEN 0.0
                            ELSE ((ud.""TotalAmount"" - pdd.""TotalAmount"")::decimal / pdd.""TotalAmount"" * 100)
                        END AS ""NewPercentageChange""
                    FROM UpdateData ud
                    LEFT JOIN PreviousDayData pdd 
                        ON pdd.""CategoryId"" = ud.""CategoryId""
                )
                UPDATE ""CategoryDailyStatistics""
                SET ""Percentage"" = fc.""NewPercentage"",
                    ""PercentageChange"" = fc.""NewPercentageChange"",
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById
                FROM FinalCalculation fc
                WHERE ""CategoryDailyStatistics"".""Id"" = fc.""Id"";
            ";
        }

        public static string GetAllStatisticsBySpecificInterval()
        {
            return @"
                WITH IntervalTotals AS (
                    SELECT
                        SUM(""TotalAmount"") AS TotalAmount
                    FROM zenit_management_dev.""CategoryGroupDailyStatistics""
                    WHERE DATE(""Date"") >= DATE(@FromDate) 
                        AND DATE(""Date"") <= DATE(@ToDate) 
                        AND ""AccountId"" = @AccountId
                ),
                PreviousIntervalTotals AS (
                    SELECT
                        SUM(""TotalAmount"") AS TotalAmount
                    FROM zenit_management_dev.""CategoryGroupDailyStatistics""
                    WHERE DATE(""Date"") >= DATE(@PreviousFromDate) 
                        AND DATE(""Date"") <= DATE(@PreviousToDate) 
                        AND ""AccountId"" = @AccountId
                ),
                GroupTypeStats AS (
                    SELECT
                        ""GroupType"",
                        SUM(""TotalAmount"") AS TotalAmount
                    FROM zenit_management_dev.""CategoryGroupDailyStatistics""
                    WHERE DATE(""Date"") >= DATE(@FromDate) 
                        AND DATE(""Date"") <= DATE(@ToDate) 
                        AND ""AccountId"" = @AccountId
                    GROUP BY ""GroupType""
                ),
                PreviousGroupTypeStats AS (
                    SELECT
                        ""GroupType"",
                        SUM(""TotalAmount"") AS TotalAmount
                    FROM zenit_management_dev.""CategoryGroupDailyStatistics""
                    WHERE DATE(""Date"") >= DATE(@PreviousFromDate) 
                        AND DATE(""Date"") <= DATE(@PreviousToDate) 
                        AND ""AccountId"" = @AccountId
                    GROUP BY ""GroupType""
                ),
                IncomeExpenseStats AS (
                    SELECT
                        COALESCE(SUM(CASE WHEN ""GroupType"" = 4 THEN ""TotalAmount"" ELSE 0 END), 0) AS TotalIncome,
                        COALESCE(SUM(CASE WHEN ""GroupType"" != 4 THEN ""TotalAmount"" ELSE 0 END), 0) AS TotalExpense
                    FROM zenit_management_dev.""CategoryGroupDailyStatistics""
                    WHERE DATE(""Date"") >= DATE(@FromDate) 
                        AND DATE(""Date"") <= DATE(@ToDate) 
                        AND ""AccountId"" = @AccountId
                ),
                PreviousIncomeExpenseStats AS (
                    SELECT
                        COALESCE(SUM(CASE WHEN ""GroupType"" = 4 THEN ""TotalAmount"" ELSE 0 END), 0) AS TotalIncome,
                        COALESCE(SUM(CASE WHEN ""GroupType"" != 4 THEN ""TotalAmount"" ELSE 0 END), 0) AS TotalExpense
                    FROM zenit_management_dev.""CategoryGroupDailyStatistics""
                    WHERE DATE(""Date"") >= DATE(@PreviousFromDate) 
                        AND DATE(""Date"") <= DATE(@PreviousToDate) 
                        AND ""AccountId"" = @AccountId
                ),
                GroupTypeValues AS (
                    SELECT
                        gts.""GroupType"",
                        gts.TotalAmount,
                        CASE 
                            WHEN it.TotalAmount = 0 OR it.TotalAmount IS NULL THEN 0
                            ELSE (gts.TotalAmount * 100.0) / it.TotalAmount
                        END AS Percentage,
                        CASE 
                            WHEN pgts.TotalAmount IS NULL OR pgts.TotalAmount = 0 THEN 100.0
                            ELSE ((gts.TotalAmount - pgts.TotalAmount) * 100.0) / pgts.TotalAmount
                        END AS PercentageChange
                    FROM GroupTypeStats gts
                    CROSS JOIN IntervalTotals it
                    LEFT JOIN PreviousGroupTypeStats pgts ON gts.""GroupType"" = pgts.""GroupType""
                ),
                CategoryStats AS (
                    SELECT
                        c.""GroupType"",
                        c.""Name"" AS CategoryName,
                        SUM(cds.""TotalAmount"") AS TotalAmount
                    FROM zenit_management_dev.""CategoryDailyStatistics"" cds
                    INNER JOIN zenit_management_dev.""Category"" c ON cds.""CategoryId"" = c.""Id""
                    WHERE DATE(""Date"") >= DATE(@FromDate) 
                        AND DATE(""Date"") <= DATE(@ToDate) 
                        AND (c.""AccountId"" = @AccountId OR c.""AccountId"" IS NULL)
                        AND c.""IsDeleted"" = false
                    GROUP BY c.""GroupType"", c.""Name"", c.""Id""
                ),
                PreviousCategoryStats AS (
                    SELECT
                        c.""GroupType"",
                        c.""Name"" AS CategoryName,
                        SUM(cds.""TotalAmount"") AS TotalAmount
                    FROM zenit_management_dev.""CategoryDailyStatistics"" cds
                    INNER JOIN zenit_management_dev.""Category"" c ON cds.""CategoryId"" = c.""Id""
                    WHERE DATE(""Date"") >= DATE(@PreviousFromDate) 
                        AND DATE(""Date"") <= DATE(@PreviousToDate) 
                        AND (c.""AccountId"" = @AccountId OR c.""AccountId"" IS NULL)
                        AND c.""IsDeleted"" = false
                    GROUP BY c.""GroupType"", c.""Name"", c.""Id""
                ),
                CategoryValues AS (
                    SELECT
                        cs.""GroupType"",
                        cs.CategoryName,
                        cs.TotalAmount,
                        CASE 
                            WHEN gtv.TotalAmount = 0 OR gtv.TotalAmount IS NULL THEN 0
                            ELSE (cs.TotalAmount * 100.0) / gtv.TotalAmount
                        END AS Percentage,
                        CASE 
                            WHEN pcs.TotalAmount IS NULL OR pcs.TotalAmount = 0 THEN 100.0
                            ELSE ((cs.TotalAmount - pcs.TotalAmount) * 100.0) / pcs.TotalAmount
                        END AS PercentageChange
                    FROM CategoryStats cs
                    INNER JOIN GroupTypeValues gtv ON cs.""GroupType"" = gtv.""GroupType""
                    LEFT JOIN PreviousCategoryStats pcs ON cs.""GroupType"" = pcs.""GroupType"" AND cs.CategoryName = pcs.CategoryName
                )
                SELECT 
                    jsonb_build_object(
                        'IncomeExpenseSummary', (
                            SELECT jsonb_build_object(
                                'TotalIncome', cur.TotalIncome,
                                'TotalExpense', cur.TotalExpense,
                                'IncomePercentageChange', ROUND(CAST(
                                    CASE 
                                        WHEN prev.TotalIncome = 0 THEN 100.0
                                        ELSE ((cur.TotalIncome - prev.TotalIncome) * 100.0) / prev.TotalIncome
                                    END AS numeric), 2),
                                'ExpensePercentageChange', ROUND(CAST(
                                    CASE 
                                        WHEN prev.TotalExpense = 0 THEN 100.0
                                        ELSE ((cur.TotalExpense - prev.TotalExpense) * 100.0) / prev.TotalExpense
                                    END AS numeric), 2)
                            )
                            FROM IncomeExpenseStats cur, PreviousIncomeExpenseStats prev
                        ),
                        'GroupStatistics', COALESCE(
                            (
                                SELECT jsonb_agg(
                                    jsonb_build_object(
                                        'GroupType', gtv.""GroupType"",
                                        'TotalAmount', gtv.TotalAmount,
                                        'Percentage', ROUND(CAST(gtv.Percentage AS numeric), 2),
                                        'PercentageChange', ROUND(CAST(gtv.PercentageChange AS numeric), 2),
                                        'Categories', COALESCE(
                                            (
                                                SELECT jsonb_agg(
                                                    jsonb_build_object(
                                                        'CategoryName', cv.CategoryName,
                                                        'TotalAmount', cv.TotalAmount,
                                                        'Percentage', ROUND(CAST(cv.Percentage AS numeric), 2),
                                                        'PercentageChange', ROUND(CAST(cv.PercentageChange AS numeric), 2)
                                                    )
                                                )
                                                FROM CategoryValues cv
                                                WHERE cv.""GroupType"" = gtv.""GroupType""
                                            ),
                                            '[]'::jsonb
                                        )
                                    )
                                )
                                FROM GroupTypeValues gtv
                            ),
                            '[]'::jsonb
                        )
                    ) AS ""Result"";
                ";
        }
    }
}
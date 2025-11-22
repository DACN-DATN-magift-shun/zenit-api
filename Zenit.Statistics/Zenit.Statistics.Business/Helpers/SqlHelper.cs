namespace Zenit.Statistics.Business.Helpers
{
    public class SqlHelper
    {
        public static string UpsertDailyStatistics()
        {
            return @"
                /* CategoryGroupDailyStatistics */
                INSERT INTO ""CategoryGroupDailyStatistics"" (""Id"", ""Date"", ""TotalAmount"", ""Percentage"", ""PercentageChange"", ""GroupType"", ""AccountId"", ""CreatedById"", ""CreatedAt"", ""IsDeleted"")
                VALUES (@CategoryGroupStatsId, @Date, @TotalAmount, 0.0, 0.0, @GroupType, @AccountId, @CreatedById, NOW(), false)
                ON CONFLICT (""Date"", ""GroupType"", ""AccountId"") DO UPDATE
                SET ""TotalAmount"" = ""CategoryGroupDailyStatistics"".""TotalAmount"" + @TotalAmount,
                    ""LastModifiedAt"" = NOW(),
                    ""ModifiedById"" = @ModifiedById;

                -- Cập nhật Percentage cho TẤT CẢ và PercentageChange cho 1 GroupType
                WITH TotalAmounts AS (
                    SELECT 
                        SUM(""TotalAmount"") as Total
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date 
                        AND ""AccountId"" = @AccountId
                ), 
                PreviousDayData AS (
                    SELECT 
                        ""GroupType"",
                        ""Percentage"",
                        ""TotalAmount""
                    FROM ""CategoryGroupDailyStatistics""
                    WHERE ""Date"" = @Date - INTERVAL '1 day' 
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
                    WHERE cgds.""Date"" = @Date 
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
                    WHERE ""Date"" = @Date 
                        AND ""GroupType"" = @GroupType
                        AND ""AccountId"" = @AccountId
                ) INSERT INTO ""CategoryDailyStatistics"" (""Id"", ""Date"", ""TotalAmount"", ""Percentage"", ""PercentageChange"", ""CategoryId"", ""GroupId"", ""AccountId"", ""CreatedById"", ""CreatedAt"", ""IsDeleted"")
                VALUES (@CategoryStatsId, @Date, @TotalAmount, 0.0, 0.0, @CategoryId, (SELECT ""Id"" FROM CategoryGroupId), @AccountId, @CreatedById, NOW(), false)
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
                    WHERE cds.""Date"" = @Date 
                        AND cds.""AccountId"" = @AccountId
                        AND gds.""Date"" = @Date
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
                    WHERE cds_prev.""Date"" = @Date - INTERVAL '1 day' 
                        AND cds_prev.""AccountId"" = @AccountId
                        AND cgds_prev.""GroupType"" = @GroupType
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

        public static string UpsertWeeklyStatistics()
        {
            return @"";
        }

        public static string UpsertMonthlyStatistics()
        {
            return @"";
        }
    }
}
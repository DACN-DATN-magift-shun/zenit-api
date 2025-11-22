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

        public static string GetAllStatisticsBySpecificInterval()
        {
            return @"
                WITH IntervalTotals AS (
                SELECT
                    SUM(""TotalAmount"") AS TotalAmount
                FROM zenit_statistics_dev.""CategoryGroupDailyStatistics""
                WHERE DATE(""Date"") >= DATE(@FromDate) 
                    AND DATE(""Date"") <= DATE(@ToDate) 
                    AND ""AccountId"" = @AccountId
            ),
            PreviousIntervalTotals AS (
                SELECT
                    SUM(""TotalAmount"") AS TotalAmount
                FROM zenit_statistics_dev.""CategoryGroupDailyStatistics""
                WHERE DATE(""Date"") >= DATE(@PreviousFromDate) 
                    AND DATE(""Date"") <= DATE(@PreviousToDate) 
                    AND ""AccountId"" = @AccountId
            ),
            GroupTypeStats AS (
                SELECT
                    ""GroupType"",
                    SUM(""TotalAmount"") AS TotalAmount
                FROM zenit_statistics_dev.""CategoryGroupDailyStatistics""
                WHERE DATE(""Date"") >= DATE(@FromDate) 
                    AND DATE(""Date"") <= DATE(@ToDate) 
                    AND ""AccountId"" = @AccountId
                GROUP BY ""GroupType""
            ),
            PreviousGroupTypeStats AS (
                SELECT
                    ""GroupType"",
                    SUM(""TotalAmount"") AS TotalAmount
                FROM zenit_statistics_dev.""CategoryGroupDailyStatistics""
                WHERE DATE(""Date"") >= DATE(@PreviousFromDate) 
                    AND DATE(""Date"") <= DATE(@PreviousToDate) 
                    AND ""AccountId"" = @AccountId
                GROUP BY ""GroupType""
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
                FROM zenit_statistics_dev.""CategoryDailyStatistics"" cds
                INNER JOIN zenit_management_dev.""Category"" c ON cds.""CategoryId"" = c.""Id""
                WHERE DATE(""Date"") >= DATE(@FromDate) 
                    AND DATE(""Date"") <= DATE(@ToDate) 
                    AND cds.""AccountId"" = @AccountId
                    AND c.""AccountId"" = @AccountId
                    AND c.""IsDeleted"" = false
                GROUP BY c.""GroupType"", c.""Name"", c.""Id""
            ),
            PreviousCategoryStats AS (
                SELECT
                    c.""GroupType"",
                    c.""Name"" AS CategoryName,
                    SUM(cds.""TotalAmount"") AS TotalAmount
                FROM zenit_statistics_dev.""CategoryDailyStatistics"" cds
                INNER JOIN zenit_management_dev.""Category"" c ON cds.""CategoryId"" = c.""Id""
                WHERE DATE(""Date"") >= DATE(@PreviousFromDate) 
                    AND DATE(""Date"") <= DATE(@PreviousToDate) 
                    AND cds.""AccountId"" = @AccountId
                    AND c.""AccountId"" = @AccountId
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
                COALESCE(
                    jsonb_agg(
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
                    ),
                    '[]'::jsonb
                )
            FROM GroupTypeValues gtv;
            ";
        }
    }
}
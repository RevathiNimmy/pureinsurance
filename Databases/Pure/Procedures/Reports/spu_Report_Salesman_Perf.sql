SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Salesman_Perf'
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


Create PROCEDURE spu_Report_Salesman_Perf
                    @PeriodDate varchar(255),
                    @HandlerCode varchar(255)
AS
SET NOCOUNT ON
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime
SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)
SELECT @SelectedPeriodID = period_id
FROM ..Period
WHERE period_end_date = @dtSelectedPeriodEnd
DECLARE @YearStartPeriodID int, @sub_branch_id int
EXECUTE spu_Report_GetYearStartID @sub_Branch_id, @SelectedPeriodID, @YearStartPeriodID OUTPUT
DECLARE @12PeriodsAgoID int
SELECT @12PeriodsAgoID = @SelectedPeriodID - 12
IF isnull(@12PeriodsAgoID,0) <=0 SELECT @12PeriodsAgoID = 0
DECLARE @24PeriodsAgoID int
SELECT @24PeriodsAgoID = @SelectedPeriodID - 24
IF isnull(@24PeriodsAgoID ,0) <=0 SELECT @24PeriodsAgoID = 0
DECLARE @YTDRange int
SELECT @YTDRange = (@SelectedPeriodID - @YearStartPeriodID)
CREATE TABLE #tempRSASalesPerf
(
            HandlerCnt int NULL,
            Handler varchar (20) NULL,
            PeriodID int,
            Period varchar (40),
            TransTypeID int,
            TransType varchar (10) NULL,
            RiskTypeCode varchar (10) NULL,
            RiskTypeDescription varchar (255) NULL,
            ProductCode varchar (10) NULL,
            Product varchar (255) NULL,
            NBPRemium money NULL,
            RenPremium money NULL,
            AllPremium money NULL,
            AllPremiumCompare money NULL,
            Commission money NULL,
            Claim money NULL,
            ClaimCompare money NULL,
            dtSelectedPeriodEnd datetime
)
INSERT INTO #tempRSASalesPerf
        SELECT  sf.account_handler_cnt,
                sf.account_handler_shortname,
                0,
                'Selected Period',
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number = @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')
                        AND sf.posting_period_number = @12PeriodsAgoID),
                    @dtSelectedPeriodEnd
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        WHERE isnull(sf.account_handler_cnt,0) <> 0
        AND sd.stats_detail_type = 'GRS'
        AND (
            isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
                )
        AND (sf.posting_period_number = @SelectedPeriodID
            OR sf.posting_period_number = @12PeriodsAgoID)
        AND sf.posting_period_number in (@SelectedPeriodID, @12PeriodsAgoID)
        AND (
            @HandlerCode = 'ALL'
            OR
            sf.account_handler_shortname = @HandlerCode
            )
INSERT INTO #tempRSASalesPerf
        SELECT  sf.account_handler_cnt,
                sf.account_handler_shortname,
                1,
                'Selected Year',
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID - @YTDRange AND @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID - @YTDRange AND @12PeriodsAgoID),
                @dtSelectedPeriodEnd
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        WHERE isnull(sf.account_handler_cnt,0) <> 0
        AND sd.stats_detail_type = 'GRS'
        AND (
            isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
                )
        AND (sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID
            OR sf.posting_period_number BETWEEN @12PeriodsAgoID - @YTDRange AND @12PeriodsAgoID)
        AND (
            sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID
            OR
            sf.posting_period_number BETWEEN @12PeriodsAgoID - @YTDRange AND @12PeriodsAgoID
            )
        AND (
            @HandlerCode = 'ALL'
            OR
            sf.account_handler_shortname = @HandlerCode
            )
INSERT INTO #tempRSASalesPerf
        SELECT  sf.account_handler_cnt,
                sf.account_handler_shortname,
                2,
                '12 Periods',
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')
                        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID),
                @dtSelectedPeriodEnd
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        WHERE isnull(sf.account_handler_cnt,0) <> 0
        AND sd.stats_detail_type = 'GRS'
        AND (
            isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
                )
        AND (sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID
            OR sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID)
        AND sf.posting_period_number > @24PeriodsAgoID
        AND (
            @HandlerCode = 'ALL'
            OR
            sf.account_handler_shortname = @HandlerCode
            )
SET NOCOUNT OFF
SELECT * FROM #tempRSASalesPerf
    WHERE (isnull(NBPRemium ,0) <> 0
        OR isnull(RenPremium ,0) <> 0
        OR isnull(AllPremium ,0) <> 0
        OR isnull(AllPremiumCompare ,0) <> 0
        OR isnull(Commission ,0) <> 0
        OR isnull(Claim ,0) <> 0
        OR isnull(ClaimCompare ,0) <> 0
        )
DROP TABLE #tempRSASalesPerf


GO
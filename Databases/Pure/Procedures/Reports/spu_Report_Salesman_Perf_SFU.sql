
EXECUTE DDLDropProcedure 'spu_Report_Salesman_Perf_SFU'
GO
/****** Object:  Stored Procedure dbo.sp_Report_Salesman_Perf    Script Date: 16/10/00 12:26:03 ******/
/**********************************************************************************************************************************
** Created by Jude Killip
** 30/08/2000
** RSA Reports - Salesman_Performance.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 09/11/2000   Thinh Nguyen            real data
** 10/11/2000   Jude Killip             use temp table and add dummy records necessary for subreport
** 07/12/2000   Jude Killip             bug 297 - stats_detail_type
**
** 17/06/2001   Jude Killip             redo based on Agent Analysis
**
** 22/06/2001   Jude Killip             adjust the period selection to eliminate zeros
**                                      allow for very first period
**                                      add current period end date to output
**
** 04/07/2001   Jude Killip             filters for Claims/nonClaims details
**
** 19/09/2001   Jude Killip             amend claims - value in this_premium_home (not total_sum_insured)
**                                      add period id for report sort order
**
** 28/09/2001   Jude Killip             Filter out failed Export records
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     31/01/2002  JMK     Use new lookup parameter "Period" - user selects from list of
**                              ..current and previous period_end_dates (as a string)
***********************************************************************************************************************************/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Report_Salesman_Perf_SFU
                    @PeriodDate varchar (20),
                    @HandlerCode varchar (20)
AS
/*
-- for testing
DECLARE  @PeriodDate varchar (20), @HandlerCode varchar(20)
SELECT @PeriodDate = 'Dec 31 2001', @HandlerCode = 'ALL'
*/
-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd

-- get the year start period ID
DECLARE @YearStartPeriodID int
EXECUTE spu_Report_GetYearStartID_SFU @SelectedPeriodID, @YearStartPeriodID OUTPUT

-- get the 12 period ID
DECLARE @12PeriodsAgoID int
SELECT @12PeriodsAgoID = @SelectedPeriodID - 12
IF isnull(@12PeriodsAgoID,0) <=0 SELECT @12PeriodsAgoID = 0

-- get the 24 period ID
DECLARE @24PeriodsAgoID int
SELECT @24PeriodsAgoID = @SelectedPeriodID - 24
IF isnull(@24PeriodsAgoID ,0) <=0 SELECT @24PeriodsAgoID = 0

-- calc Last Year YTD range
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

 --print 'add "current period" records'
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
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number = @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
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
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
        AND sf.posting_period_number in (@SelectedPeriodID, @12PeriodsAgoID)
        AND (
            @HandlerCode = 'ALL'
            OR
            sf.account_handler_shortname = @HandlerCode
            )

 --print 'add "this year" records (year to this period)'
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
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID - @YTDRange AND @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
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
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
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

 --print 'add "12 month" records (12months to this period)'
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
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
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
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
        AND sf.posting_period_number > @24PeriodsAgoID
        AND (
            @HandlerCode = 'ALL'
            OR
            sf.account_handler_shortname = @HandlerCode
            )

SET NOCOUNT OFF
 --print 'select for the report filtering out 0 or null'
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
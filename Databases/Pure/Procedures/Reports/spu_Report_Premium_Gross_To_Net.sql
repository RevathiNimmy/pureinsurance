SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Premium_Gross_To_Net'
GO


CREATE PROCEDURE spu_Report_Premium_Gross_To_Net
                    @PeriodDate varchar(255),
		    @source_id int	
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 17/08/2000
** RSA Reports - Premium_Gross_To_Net.rpt
**
**********************************************************************************************************************************
** 04/10/2000 Jude Killip       Update to use DB
**
** 07/12/2000 Jude Killip       bug 297 - stats_detail_type
**
** 14/03/2001 Jude Kilip        amend commission calcs, uncomment coinsurance
**
** 26/04/2001 Jude Killip       add period end date
**                              fix Retained
**
** 11/06/2001 Jude Killip       filter on date on every insert instead of at the end
**                              filter on stats folder posting period number
**
** 29/06/2001 Jude Killip       'NET','TTY','TAF','TAC','TAN','TAT' record values * -1
**
** 04/07/2001   Jude Killip     filters for Claims/nonClaims details
**                              filter on stats_detail_type in main query
**
** 14/09/2001   Jude Killip     'FAC' values *-1
**                              Comment out 'NET' records - not needed??
**
** 28/09/2001   Jude Killip     Filter out failed Export records
**
** 18/12/2001   JMK             use new lookup parameter "Period" - user's selection from list of
**                                      current and previous period_end_dates (as a string)
**
** 19/12/2001   JMK             Add Agent, to allow grouping by agent
***********************************************************************************************************************************/
SET NOCOUNT ON

/*
-- for testing
DECLARE @PeriodDate varchar (20)
SELECT @PeriodDate = 'Oct 31 2001'
*/
CREATE TABLE #tempRSAPremGrossNet
(
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    CommissionOrPremium int NULL,           -- 0=premium, 1=commission
    PeriodRangeID int,                      -- 1=Current, 2=YTD, 3=12Months
    PeriodRangeName varchar(30),
    Agent varchar (100) NULL,
    RiskTypeCode varchar (10) NULL,
    RiskTypeDescription varchar (255) NULL,
    Gross decimal (19,4) NULL,
    Coinsurance decimal (19,4) NULL,
    Treaty decimal (19,4) NULL,
    Facultative decimal (19,4) NULL,
    TransDate datetime NULL,
    dtSelectedPeriodEnd datetime,
    PostingPeriodID int,
    SelectedPeriodID int
)


declare @sub_branch_id int
EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd

-- get the year start period ID
DECLARE @YearStartPeriodID int
EXECUTE spu_Report_GetYearStartID @sub_branch_id, @SelectedPeriodID, @YearStartPeriodID OUTPUT

-- get the 12 period ID
DECLARE @12PeriodsAgoID int
SELECT @12PeriodsAgoID = @SelectedPeriodID - 11
IF @12PeriodsAgoID IS NULL SELECT @12PeriodsAgoID = 1

-- Selected Period
-- Add Premium Records - Gross
INSERT INTO #tempRSAPremGrossNet
    SELECT sf.product_code,
        p.description,
        0,      -- Premium
        1,      -- Selected Period
        'Selected Period',
        (SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
        sd.risk_type_code,
        rt.description,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'FAC'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    --sj 31/07/2002 - Start
    --AND(
    --    SELECT isnull(max(tef.accounts_export_status),'x')
    --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    --) = 'c'
    --sj 31/07/2002 - End

-- Now add Commission Records
-- Add Commission Records - Gross
INSERT INTO #tempRSAPremGrossNet
    SELECT sf.product_code,
        p.description,
        1,      -- Commission
        1,      -- Selected Period
        'Selected Period',
        (SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
        sd.risk_type_code,
        rt.description,
        (SELECT isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)
         WHERE sd.stats_detail_type = 'GRS'),
        (SELECT isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)
         WHERE sd.stats_detail_type = 'COI'),
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) * -1
         WHERE sd.stats_detail_type = 'TTY'),
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) * -1
         WHERE sd.stats_detail_type = 'FAC') ,
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.lead_commission_value_home,0) +
        isnull(sd.sub_commission_value_home,0) <> 0
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    --sj 31/07/2002 - Start
    --AND(
    --    SELECT isnull(max(tef.accounts_export_status),'x')
    --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    --) = 'c'
    --sj 31/07/2002 - End

-- Selected Year
-- Add Premium Records
INSERT INTO #tempRSAPremGrossNet
    SELECT sf.product_code,
        p.description,
        0,      -- Premium
        2,      -- Selected Year
        'Selected Year',
        (SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
        sd.risk_type_code,
        rt.description,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'FAC'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @YearStartPeriodID AND @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    --sj 31/07/2002 - Start
    --AND(
    --    SELECT isnull(max(tef.accounts_export_status),'x')
    --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    --) = 'c'
    --sj 31/07/2002 - End


-- Now add Commission Records
INSERT INTO #tempRSAPremGrossNet
    SELECT sf.product_code,
        p.description,
        1,      -- Commission
        2,      -- Selected Year
        'Selected Year',
        (SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
        sd.risk_type_code,
        rt.description,
        (SELECT isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)
         WHERE sd.stats_detail_type = 'GRS'),
        (SELECT isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)
         WHERE sd.stats_detail_type = 'COI'),
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) * -1
         WHERE sd.stats_detail_type = 'TTY'),
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) * -1
         WHERE sd.stats_detail_type = 'FAC'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.lead_commission_value_home,0) +
        isnull(sd.sub_commission_value_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @YearStartPeriodID AND @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
   --sj 31/07/2002 - Start
    --AND(
    --    SELECT isnull(max(tef.accounts_export_status),'x')
    --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    --) = 'c'
    --sj 31/07/2002 - End


-- MOVING 12 Periods
-- Add Premium Records
INSERT INTO #tempRSAPremGrossNet
    SELECT sf.product_code,
        p.description,
        0,      -- Premium
        3,      -- 12 Periods
        '12 Periods',
        (SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
        sd.risk_type_code,
        rt.description,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'FAC'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @12PeriodsAgoID AND @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    --sj 31/07/2002 - Start
    --AND(
    --    SELECT isnull(max(tef.accounts_export_status),'x')
    --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    --) = 'c'
    --sj 31/07/2002 - End


-- Now add Commission Records
INSERT INTO #tempRSAPremGrossNet
    SELECT sf.product_code,
        p.description,
        1,      -- Commission
        3,      -- 12 Periods
        '12 Periods',
        (SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
        sd.risk_type_code,
        rt.description,
        (SELECT isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)
         WHERE sd.stats_detail_type = 'GRS'),
        (SELECT isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)
         WHERE sd.stats_detail_type = 'COI'),
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) * -1
         WHERE sd.stats_detail_type = 'TTY'),
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) * -1
         WHERE sd.stats_detail_type = 'FAC'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.lead_commission_value_home,0) +
        isnull(sd.sub_commission_value_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @12PeriodsAgoID AND @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
   --sj 31/07/2002 - Start
    --AND(
    --    SELECT isnull(max(tef.accounts_export_status),'x')
    --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    --) = 'c'
    --sj 31/07/2002 - End


SET NOCOUNT OFF
Select * FROM #tempRSAPremGrossNet
DROP TABLE #tempRSAPremGrossNet
GO
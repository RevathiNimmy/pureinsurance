SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Summary_of_Premium'
GO


CREATE PROCEDURE spu_Report_Summary_of_Premium
 			@source_id int
AS

DECLARE @sub_branch_id int

/**********************************************************************************************************************************
** Created by Jude Killip
** 04/08/2000
** RSA Reports - Summary_Of_Premium.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 24/10/2000 JMK       - Update to use DB
** 25/10/2000 JMK       - silly date condition error in last month details
** 15/01/2001 JMK       - use document_date, amend date criteria (match Net Premium)
** 05/05/2001 JMK       - base on Period dates
** 12/06/2001 JMK       - adjust Period
** 03/08/2001 JMK       - rewrite: use Stats NOT Orion
** 05/09/2001 JMK       - add Stats description
***********************************************************************************************************************************/
SET NOCOUNT ON
CREATE TABLE #tempRSAPremSum
(
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    SectionID int NULL,           -- 0=premium, 1=commission, 2=Claims
    SectionName varchar (15) NULL,
    StatsType varchar (3) NULL,
    StatsTypeDesc varchar (30) NULL,
    ShortName varchar (20) NULL,
    TPAmount decimal (19,4) NULL,
    BFAmount decimal (19,4) NULL,
    TransDate datetime NULL,
    dtCurrentPeriodEnd datetime,
    dtLastYearEnd datetime,
    PostingPeriodID int,
    CurrentPeriodID int
)

-- get default sub-branch for supplied source_id
EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT


-- get period end complete date
DECLARE @dtPEndComplete datetime
EXECUTE spu_Report_GetPeriodEndComplete_Date  @sub_branch_id, @dtPEndComplete OUTPUT

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod  @sub_branch_id, @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

-- get current year values
DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
EXECUTE spu_Report_GetCurrentYear  @sub_branch_id, @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT

-- CURRENT PERIOD
-- Add Premium Records - Gross
INSERT INTO #tempRSAPremSum
    SELECT sf.product_code,
        p.description,
        0,      -- Premium
        'Premium',
        sd.stats_detail_type,
        (SELECT CASE sd.stats_detail_type
            WHEN 'GRS' THEN 'Gross'
            WHEN 'FAC' THEN 'Facultative'
            WHEN 'TTY' THEN 'Treaty'
            WHEN 'COI' THEN 'Coinsurance'
         END),
        (select isnull(agent_shortname, insurance_holder_shortname)),
        sd.this_premium_home,
        NULL,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    --JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number = @CurrentPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Add Commission Records
INSERT INTO #tempRSAPremSum
    SELECT sf.product_code,
        p.description,
        1,      -- Commission
        'Commission',
        sd.stats_detail_type,
        (SELECT CASE sd.stats_detail_type
            WHEN 'GRS' THEN 'Gross'
            WHEN 'FAC' THEN 'Facultative'
            WHEN 'TTY' THEN 'Treaty'
            WHEN 'COI' THEN 'Coinsurance'
         END),
        (select isnull(agent_shortname, insurance_holder_shortname)),
        isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0),
        NULL,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    --JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.lead_commission_value_home,0) +
        isnull(sd.sub_commission_value_home,0) <> 0
    AND sf.posting_period_number = @CurrentPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Add Claims Paid Records
INSERT INTO #tempRSAPremSum
    SELECT sf.product_code,
        p.description,
        3,      -- Claims
        'Claims Paid',
        sd.stats_detail_type,
        (SELECT CASE sd.stats_detail_type
            WHEN 'GRS' THEN 'Gross'
            WHEN 'FAC' THEN 'Facultative'
            WHEN 'TTY' THEN 'Treaty'
            WHEN 'COI' THEN 'Coinsurance'
         END),
        (select isnull(agent_shortname, insurance_holder_shortname)),
        sd.this_premium_home,
        NULL,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    --JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number = @CurrentPeriodID
    AND sf.transaction_type_code = ('C_CP')                   -- claims payments only


-- CURRENT YEAR TO DATE, not inclusive
-- Add Premium Records - Gross
INSERT INTO #tempRSAPremSum
    SELECT sf.product_code,
        p.description,
        0,      -- Premium
        'Premium',
        sd.stats_detail_type,
        (SELECT CASE sd.stats_detail_type
            WHEN 'GRS' THEN 'Gross'
            WHEN 'FAC' THEN 'Facultative'
            WHEN 'TTY' THEN 'Treaty'
            WHEN 'COI' THEN 'Coinsurance'
         END),
        (select isnull(agent_shortname, insurance_holder_shortname)),
        NULL,
        sd.this_premium_home,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    --JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @CurrentPeriodID -1
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Add Commission Records
INSERT INTO #tempRSAPremSum
    SELECT sf.product_code,
        p.description,
        1,      -- Commission
        'Commission',
        sd.stats_detail_type,
        (SELECT CASE sd.stats_detail_type
            WHEN 'GRS' THEN 'Gross'
            WHEN 'FAC' THEN 'Facultative'
            WHEN 'TTY' THEN 'Treaty'
            WHEN 'COI' THEN 'Coinsurance'
         END),
        (select isnull(agent_shortname, insurance_holder_shortname)),
        NULL,
        isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0),
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    --JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.lead_commission_value_home,0) +
        isnull(sd.sub_commission_value_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @CurrentPeriodID -1
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Add Claims Paid Records
INSERT INTO #tempRSAPremSum
    SELECT sf.product_code,
        p.description,
        3,      -- Claims
        'Claims Paid',
        sd.stats_detail_type,
        (SELECT CASE sd.stats_detail_type
            WHEN 'GRS' THEN 'Gross'
            WHEN 'FAC' THEN 'Facultative'
            WHEN 'TTY' THEN 'Treaty'
            WHEN 'COI' THEN 'Coinsurance'
         END),
        (select isnull(agent_shortname, insurance_holder_shortname)),
        NULL,
        sd.this_premium_home,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
    --JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @CurrentPeriodID -1
    AND sf.transaction_type_code = 'C_CP'                   -- claims payments only

SET NOCOUNT OFF
Select * FROM #tempRSAPremSum
DROP TABLE #tempRSAPremSum
GO
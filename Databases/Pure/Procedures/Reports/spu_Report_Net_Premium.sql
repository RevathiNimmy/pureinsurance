/**********************************************************************************************************************************
** Created by Jude Killip
** 15/08/2000
** RSA Reports - NetPremium.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 27/10/2000 JMK       - Update to use DB.
**
** 15/01/2001 JMK       - use document_date, amend date criteria
**
** 05/05/2001 JMK       - amend to conform to summary of premium (base dates on Current Period)
**
** 08/06/2001 JMK       - redo above: this SP got lost somehow...
**                      - amend selection codes, still guessing
**
** 04/09/2001 JMK       - rewrite: use Stats NOT Orion, similar to spu_Report_Summary_of_Premium
***********************************************************************************************************************************/
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Net_Premium'
GO


CREATE PROCEDURE spu_Report_Net_Premium
    @source_id int
AS

DECLARE @sub_branch_id int

CREATE TABLE #tempRSANetPrem
(
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    RiskTypeID int NULL,
    RiskTypeDesc varchar (255) NULL,
    PeriodFlag int NULL,           -- 0=Current Period, 1 = Cumulative (current year to date)
    PeriodName varchar (20) NULL,
    ShortName varchar (20) NULL,
    PremiumAmount decimal (19,4) NULL,
    CommissionAmount decimal (19,4) NULL,
    ClaimAmount decimal (19,4) NULL,
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
EXECUTE spu_Report_GetPeriodEndComplete_Date @sub_branch_id, @dtPEndComplete OUTPUT

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id, @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

-- get current year values
DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
EXECUTE spu_Report_GetCurrentYear @sub_branch_id, @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT

-- CURRENT PERIOD
-- Add Premium Records
INSERT INTO #tempRSANetPrem
    SELECT sf.product_code,
        p.description,
        sd.risk_type_id,
        (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
        0,
        'Current Period',
        (select isnull(agent_shortname, insurance_holder_shortname)),
        sd.this_premium_home,   --Premium
        NULL,
        NULL,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number = @CurrentPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Add Commission Records
INSERT INTO #tempRSANetPrem
    SELECT sf.product_code,
        p.description,
        sd.risk_type_id,
        (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
        0,
        'Current Period',
        (select isnull(agent_shortname, insurance_holder_shortname)),
        NULL,
        isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0),   --Commission
        NULL,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type  in ('GRS', 'TTY', 'FAC', 'COI')
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number = @CurrentPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Add Claims Paid Records
INSERT INTO #tempRSANetPrem
    SELECT sf.product_code,
        p.description,
        sd.risk_type_id,
        (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
        0,
        'Current Period',
        (select isnull(agent_shortname, insurance_holder_shortname)),
        NULL,
        NULL,
        sd.this_premium_home,       -- Claims Paid
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number = @CurrentPeriodID
    AND sf.transaction_type_code = ('C_CP')                   -- claims payments only

-- CURRENT YEAR TO DATE, inclusive
-- Add Premium Records
INSERT INTO #tempRSANetPrem
    SELECT sf.product_code,
        p.description,
        sd.risk_type_id,
        (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
        1,
        'Cumulative Period',
        (select isnull(agent_shortname, insurance_holder_shortname)),
        sd.this_premium_home,   --Premium
        NULL,
        NULL,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @CurrentPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Add Commission Records
INSERT INTO #tempRSANetPrem
    SELECT sf.product_code,
        p.description,
        sd.risk_type_id,
        (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
        1,
        'Cumulative Period',
        (select isnull(agent_shortname, insurance_holder_shortname)),
        NULL,
        isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0),   --Commission
        NULL,
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @CurrentPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Add Claims Paid Records
INSERT INTO #tempRSANetPrem
    SELECT sf.product_code,
        p.description,
        sd.risk_type_id,
        (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
        1,
        'Cumulative Period',
        (select isnull(agent_shortname, insurance_holder_shortname)),
        NULL,
        NULL,
        sd.this_premium_home,       -- Claims Paid
        sf.transaction_date,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        sf.posting_period_number,
        @CurrentPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
    WHERE isnull(sd.this_premium_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @CurrentPeriodID
    AND sf.transaction_type_code = ('C_CP')                   -- claims payments only

SET NOCOUNT OFF
Select * FROM #tempRSANetPrem
DROP TABLE #tempRSANetPrem
GO



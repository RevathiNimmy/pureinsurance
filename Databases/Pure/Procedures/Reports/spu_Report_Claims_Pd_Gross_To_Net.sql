SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Claims_Pd_Gross_To_Net'
GO

CREATE PROCEDURE spu_Report_Claims_Pd_Gross_To_Net
        @company_id int,
        @sub_branch_id int=NULL, --AMJ
                @PeriodDate varchar (20)
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 17/08/2000
** RSA Reports - Claims_Paid_Gross_To_Net.rpt
**
**********************************************************************************************************************************
** 14/03/2001 Jude Killip       Update for first release
**                                      *** limit to PAID only
**
** 03/07/2001 Jude Killip       Redo based on Premium Gross To Net
**
** 01/08/2002   AMJ - branch specific change
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     25/01/2002  JMK     use new lookup parameter "Period" - user selects from list of
**                              current and previous period_end_dates (as a string)
***********************************************************************************************************************************/
SET NOCOUNT ON

IF @sub_branch_id IS NULL
    EXECUTE spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT

/*
-- for testing
DECLARE @PeriodDate varchar (20)
SELECT @PeriodDate = 'Oct 31 2001'
*/

CREATE TABLE #tempRSAClmPaidGrossNet
(
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    PeriodRangeID int,                      -- 1=Current, 2=YTD, 3=12Months
    PeriodRangeName varchar(30),
    RiskTypeCode varchar (10) NULL,
    RiskTypeDescription varchar (255) NULL,
    Gross decimal (19,4) NULL,
    Coinsurance decimal (19,4) NULL,
    Treaty decimal (19,4) NULL,
    Facultative decimal (19,4) NULL,
    Retained decimal (19,4) NULL,
    TransDate datetime NULL,
    dtSelectedPeriodEnd datetime,
    PostingPeriodID int,
    SelectedPeriodID int
)

-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + " 23:59:59.000"
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd
AND   sub_branch_id = @sub_branch_id

-- get the year start period ID
DECLARE @YearStartPeriodID int
DECLARE @YearStartDate datetime

EXECUTE spu_Report_GetCurrentYear
    @sub_branch_id,
    @YearStartPeriodID OUTPUT,
    @YearStartDate OUTPUT

-- get the 12 period ID
DECLARE @12PeriodsAgoID int
--AMJ
SELECT @12PeriodsAgoID = period_id
FROM Period
WHERE period_end_date = dateadd(mm, -12, @dtSelectedPeriodEnd)
AND   sub_branch_id = @sub_branch_id
IF @12PeriodsAgoID IS NULL SELECT @12PeriodsAgoID = 1

-- SELECTED PERIOD
-- Add Premium Records - Gross
INSERT INTO #tempRSAClmPaidGrossNet
    SELECT sf.product_code,
        p.description,
        1,      -- Current Period
        'Selected Period',
        sd.risk_type_code,
        rt.description,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'NET'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home,0) <> 0
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code = 'C_CP'

-- SELECTED YEAR TO DATE
-- Add Premium Records - Gross
INSERT INTO #tempRSAClmPaidGrossNet
    SELECT sf.product_code,
        p.description,
        2,      -- Current Year To Date
        'Selected Year',
        sd.risk_type_code,
        rt.description,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'NET'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home,0) <> 0
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE sf.posting_period_number IN
    (   SELECT period_id
        FROM Period
        WHERE period_end_date > @YearStartDate
        and period_end_date <= @dtSelectedPeriodEnd
        AND   sub_branch_id = @sub_branch_id
    )
-- AMJ  BETWEEN  @YearStartPeriodID AND @SelectedPeriodID
    AND sf.transaction_type_code = 'C_CP'

-- MOVING 12 MONTHS
-- Add Premium Records - Gross
INSERT INTO #tempRSAClmPaidGrossNet
    SELECT sf.product_code,
        p.description,
        3,      -- 12 Months
        '12 Periods',
        sd.risk_type_code,
        rt.description,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC'),
        (SELECT sd.this_premium_home * -1 WHERE sd.stats_detail_type = 'NET'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home,0) <> 0
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE sf.posting_period_number IN
    (   SELECT period_id
        FROM Period
        WHERE period_end_date >= dateadd(mm, -12, @dtSelectedPeriodEnd)
        and period_end_date <= @dtSelectedPeriodEnd
        AND   sub_branch_id = @sub_branch_id
    )
-- AMJ BETWEEN  @12PeriodsAgoID AND @SelectedPeriodID
    AND sf.transaction_type_code = 'C_CP'

SET NOCOUNT OFF
Select * FROM #tempRSAClmPaidGrossNet
DROP TABLE #tempRSAClmPaidGrossNet
GO


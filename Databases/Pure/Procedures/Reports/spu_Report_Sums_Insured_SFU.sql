SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Sums_Insured_SFU'
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** 28/06/2001
** v1.0
** Underwriting Reports - Sum_Insured_Gross_To_Net.rpt
**                      - report now similar to Premium Gross To Net (rather than Premium Register)
**                      - 'NET','TTY','TAF','TAC','TAN','TAT' record values * -1
**********************************************************************************************************************************
** CHANGES
**
** 04/07/2001   Jude Killip     filters for Claims/nonClaims details
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     31/01/2002  JMK     Use new lookup parameter "Period" - user selects from list of
**                              ..current and previous period_end_dates (as a string)
***********************************************************************************************************************************/


CREATE PROCEDURE spu_Report_Sums_Insured_SFU
    @PeriodDate varchar (50)
AS

SET NOCOUNT ON
/*
-- for testing
DECLARE @PeriodDate varchar (20)
SELECT @PeriodDate = 'Oct 31 2001'
*/

CREATE TABLE #tempRSASumInsGrossNet
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
IF @12PeriodsAgoID IS NULL SELECT @12PeriodsAgoID = 0


-- Selected PERIOD
INSERT INTO #tempRSASumInsGrossNet
    SELECT sf.product_code,
        p.description,
        1,      -- Selected Period
        'Selected Period',
        sd.risk_type_code,
        rt.description,
        (SELECT sd.sum_insured_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.sum_insured_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'FAC'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'NET'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.sum_insured_home,0) <> 0
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- Selected YEAR
INSERT INTO #tempRSASumInsGrossNet
    SELECT sf.product_code,
        p.description,
        2,      -- Selected Year
        'Selected Year',
        sd.risk_type_code,
        rt.description,
        (SELECT sd.sum_insured_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.sum_insured_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'FAC'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'NET'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.sum_insured_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @YearStartPeriodID AND @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

-- MOVING 12 Periods
INSERT INTO #tempRSASumInsGrossNet
    SELECT sf.product_code,
        p.description,
        3,      -- 12 Periods
        '12 Periods',
        sd.risk_type_code,
        rt.description,
        (SELECT sd.sum_insured_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.sum_insured_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'FAC'),
        (SELECT sd.sum_insured_home * -1 WHERE sd.stats_detail_type = 'NET'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE isnull(sd.sum_insured_home,0) <> 0
    AND sf.posting_period_number BETWEEN  @12PeriodsAgoID + 1 AND @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims

SET NOCOUNT OFF
Select * FROM #tempRSASumInsGrossNet
DROP TABLE #tempRSASumInsGrossNet
GO


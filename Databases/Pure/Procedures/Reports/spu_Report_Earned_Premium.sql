SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Earned_Premium'
GO



-- $Author: Tom.brown $
-- $Revision: 20 $
-- $Modtime: 25/10/02 9:56 $
-- $Workfile: sp_Report_Earned_Premium.sql $
-- $Logfile: /Sirius For Underwriting/Crystal Reports/Stored Procedures/sp_Report_Earned_Premium.sql $
-- $History: sp_Report_Earned_Premium.sql $
--
-- *****************  Version 20  *****************
-- User: Tom.brown    Date: 25/10/02   Time: 9:58
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- Use company 1 when getting period dates
--
-- *****************  Version 19  *****************
-- User: Tom.brown    Date: 8/10/02    Time: 11:10
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- F00059638:  Fix inconsistancy between Earned and UnEarned premium
-- reports.  Ensure both work from Current Period End Date and print this
-- on the report heading
/**********************************************************************************************************************************
** Created by Jude Killip
** 17/08/2000
** RSA Reports - Earned_Premium.rpt
**
** Report calculates earned values
**********************************************************************************************************************************
** 06/11/2000 - JMK -   Update to use DB.
**                      Based on sp_Report_Unearned_Premium
**
** 07/12/2000   Jude Killip     bug 297 - stats_detail_type
**
** 10/01/2001   Jude Killip     add general date fields, Cover date fields
**                              amend DailyRate fields
**                              calculate commission from commission_percent (commission_value fields not populated)
**
** 13/03/2001   Jude Killip     div/zero causing proc to fail. Trap potential zeros
**
** 01/05/2001   Jude Killip     base dates on Current Period
**
** 11/06/2001   Jude Killip     use sf.posting_period_number, NOT policy dates
**                              No longer used by Unearned_Premium.rpt
**
** 16/06/2001   Jude Killip     12months correction if we're in the very first period
**
** 19/06/2001   Jude Killip     more refinement of date selection criteria
**
** 02/07/2001   Jude Killip     add calculation rounding checks
**                              document_ref
**                              set 'NET','TTY','TAF','TAC','TAN','TAT' record values * -1
**
** 04/07/2001   Jude Killip     filters for Claims/nonClaims details
**
** 13/09/2001   Jude Killip     set 'FAC' record values * -1
**                              comment out NET records - not needed??
**
** 13/09/2001   Jude Killip     increase decimal places for daily rate to lessen rounding problems
**
** 18/09/2001   Jude Killip     Add day to days of cover for midnight renewal products
**                              Get rid of commented out 'NET' stuff
**                              remove rounding checks
**
** 28/09/2001   Jude Killip     Filter out failed Export records
**
** 04/20/2001   Jude Killip     Add document date to check for backdated policies
**
** 28/08/2002  Tom Brown        Added Calc Fields to save calc time in crystal report
**                              Further addition of GROUP BY and SUM to save Crystal processing time
**                              Debug Timers for fine tuning the query
** TIMINGS:  Original - over 24 minutes New version (same machine) 2 minutes
**
** 26/02/2003   Jude Killip     Update Combined: rename spu, DDL
**                              Merge periodID and sourceID into 1.6 changes
***********************************************************************************************************************************/


CREATE PROCEDURE spu_Report_Earned_Premium
    @source_id int,
    @Period int = 0,
    @DetailSummary varchar(10)

AS


/*
--TESTING
DECLARE @DetailSummary varchar(10)
SELECT @DetailSummary = 'Detail'
*/



SET NOCOUNT ON

-- TB 28/08/2002 Main issue with this Report is its too slow,
-- DEBUG (1) restricts selection to Accident/Money only and prints timings
-- DEBUG (2) produces output for debugging only (no final results)
DECLARE @DEBUG INT
SELECT @DEBUG = 0    --0 OFF, 1= ON, 2=test data, 3 = restrict output
-- TIMER  - to time the various parts of it
IF @DEBUG = 1
BEGIN
    declare @TimeNow datetime
    declare @TimeInit datetime
    declare @TimePoint datetime
    select @TimeInit = getdate()
    select @TimeNow = getdate()
    select @TimePoint = @TimeNow
    print 'START Time: ' + convert(varchar(30), @TimeNow, 108)
    print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
    print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
END


-- get sub_branch_id
DECLARE @sub_branch_id int
EXECUTE spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id, @CurrentPeriodID OUTPUT, @dtCurrentPeriodEnd OUTPUT

-- get current year values
DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
EXECUTE spu_Report_GetCurrentYear @sub_branch_id, @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT

-- get current 12 month period values
DECLARE @12MonthPeriodID int, @dt12MonthPeriodEnd datetime
EXECUTE spu_Report_GetCurrent12MonthPeriod @sub_branch_id, @12MonthPeriodID OUTPUT, @dt12MonthPeriodEnd OUTPUT


--IF @DEBUG = 1
--BEGIN
--    SELECT "Current ID = " , @CurrentPeriodID
--    SELECT "Current End Date = ", @dtCurrentPeriodEND
--    SELECT "Current Year = ", @dtLAstYearPeriodEndDate
--    SELECT "12 month = ", @dt12MonthPeriodEnd
--END

CREATE TABLE #tempRSAEarndPrem
(
    StatsFolderCnt int,
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    CommissionOrPremium int NULL,           -- 0=premium, 1=commission
    PeriodRangeID int,
    PeriodRangeName varchar(30),
    RiskTypeCode varchar (10) NULL,
    RiskTypeDescription varchar (255) NULL,
    Gross decimal (19,8) NULL,              -- GRS          Daily Rate = premium/days of cover
    GrossTotal decimal (19,4) NULL,
    Coinsurance decimal (19,8) NULL,        -- COI                  "
    CoinsTotal decimal (19,4) NULL,
    Treaty decimal (19,8) NULL,             -- TTY                  "
    TreatyTotal decimal (19,4) NULL,
    Facultative decimal (19,8) NULL,        -- FAC                  "
    FacTotal decimal (19,4) NULL,
    DocumentRef varchar (25) NULL,
    FromDate datetime NULL,
    ToDate datetime NULL,
    PostingPeriodID int,
    DaysOfCoverTotal int,
    IsMidnightRenewal int,
    DocumentDate datetime,
    --17/01/2003 - PWC - Added for grouping
    ClassOfBusinessCode CHAR(10)
)


IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print 'LABEL1 Time: ' + convert(varchar(30), @TimeNow, 108)
    print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
    print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
    select @TimePoint = @TimeNow
END


-- Add Premium Records
INSERT INTO #tempRSAEarndPrem
    SELECT sd.stats_folder_cnt,
        sf.product_code,
        p.description,
        0,      -- Premium
        NULL,
        NULL,
        sd.risk_type_code,
        rt.description,
        NULL,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        NULL,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        NULL,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY')*-1,
        NULL,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC')*-1,
        sf.document_ref,
        sf.cover_start_date,
        sf.expiry_date,
        sf.posting_period_number,
        datediff(day, sf.cover_start_date, sf.expiry_date),
        isnull(p.is_midnight_renewal,0),
        sf.document_date,
        --17/01/2003 - PWC - Added for grouping
        sd.class_of_business_code
    FROM Stats_Folder sf
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home,0) <> 0
        AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC')
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE (
        datediff(month, @dt12MonthPeriodEnd, sf.expiry_date) >= 1
        AND datediff(month, @dtCurrentPeriodEnd, sf.cover_start_date) <= 0
        )
    AND datediff(day, sf.cover_start_date, sf.expiry_date)<> 0      -- in case of div/zero
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    AND(
        SELECT isnull(max(tef.accounts_export_status),'x')
        FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    ) = 'c'



IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print 'LABEL2 Time: ' + convert(varchar(30), @TimeNow, 108)
    print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
    print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
    select @TimePoint = @TimeNow
END

-- Now add Commission Records
INSERT INTO #tempRSAEarndPrem
    SELECT sd.stats_folder_cnt,
        sf.product_code,
        p.description,
        1,      -- Commission
        NULL,
        NULL,
        sd.risk_type_code,
        rt.description,
        NULL,
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0)) WHERE sd.stats_detail_type = 'GRS'),
        NULL,
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0)) WHERE sd.stats_detail_type = 'COI'),
        NULL,
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0)) WHERE sd.stats_detail_type = 'TTY')*-1,
        NULL,
        (SELECT (isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0)) WHERE sd.stats_detail_type = 'FAC')*-1,
        sf.document_ref,
        sf.cover_start_date,
        sf.expiry_date,
        sf.posting_period_number,
        datediff(day, sf.cover_start_date, sf.expiry_date),
        isnull(p.is_midnight_renewal,0),
        sf.document_date,
        --17/01/2003 - PWC - Added for grouping
        sd.class_of_business_code
    FROM Stats_Folder sf
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND (isnull(sd.lead_commission_value_home,0) + isnull(sd.sub_commission_value_home,0)) <> 0
        AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC')
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    JOIN Product p          ON sf.product_id = p.product_id
    WHERE (
        datediff(month, @dt12MonthPeriodEnd, sf.expiry_date) >= 1
        AND datediff(month, @dtCurrentPeriodEnd, sf.cover_start_date) <= 0
        )
    AND datediff(day, sf.cover_start_date, sf.expiry_date)<> 0      -- in case of div/zero
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    AND(
        SELECT isnull(max(tef.accounts_export_status),'x')
        FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    ) = 'c'


IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print 'LABEL3 Time: ' + convert(varchar(30), @TimeNow, 108)
    print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
    print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
    select @TimePoint = @TimeNow
END

CREATE TABLE #tempRSAEarndPremSplit
(
    StatsFolderCnt int,
--    CountOfStatsFolderCnt int NULL,
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    CommissionOrPremium int NULL,           -- 0=premium, 1=commission
    PeriodRangeID int,
    PeriodRangeName varchar(30),
    RiskTypeCode varchar (10) NULL,
    RiskTypeDescription varchar (255) NULL,
    Gross decimal (19,8) NULL,              -- GRS          Daily Rate = premium/days of cover
    GrossTotal decimal (19,4) NULL,
    Coinsurance decimal (19,8) NULL,        -- COI                  "
    CoinsTotal decimal (19,4) NULL,
    Treaty decimal (19,8) NULL,             -- TTY                  "
    TreatyTotal decimal (19,4) NULL,
    Facultative decimal (19,8) NULL,        -- FAC                  "
    FacTotal decimal (19,4) NULL,
    DocumentRef varchar (25) NULL,
    FromDate datetime NULL,
    ToDate datetime NULL,
    dtCurrentPeriodEnd datetime,
    dtLastYearEnd datetime,
    dt12MonthsAgo datetime,
    PostingPeriodID int,
    CurrentPeriodID int,
    DaysOfCoverTotal int,
    IsMidnightRenewal int,
    DocumentDate datetime,
    --17/01/2003 - PWC - Added for grouping
    ClassOfBusinessCode CHAR(10),
    CurrentPeriodStart datetime NULL,        -- TB 28/8/02 - Extra fields
    dt12MonthStart datetime NULL,
    dtYTDStart datetime NULL,
    BackDateExtra int NULL,
    DatesInRange int NULL,
    CalcDaysOfCoverCurrent int NULL,
    CalcDaysOfCoverYearToDate int NULL,
    CalcDaysOfCover12Month int NULL,
    GrossCoverRounded decimal (19,2) NULL,
    CoInsCoverRounded decimal (19,2) NULL,
    NetCoverRounded decimal (19,2) NULL,
    TreatyCoverRounded decimal (19,2) NULL,
    FACCoverRounded decimal (19,2) NULL,
    RetainedCoverRounded decimal (19,2) NULL
)

-- INDEXES on Temp Tables
-- TomO's idea to add StatsFolderCnt to make indexes unique
CREATE INDEX Idx_FromDate ON #TempRSAEarndPremSplit
   ( FromDate  , StatsFolderCnt)

CREATE INDEX Idx_ToDate ON #TempRSAEarndPremSplit
   ( ToDate  , StatsFolderCnt )

CREATE INDEX Idx_dtCurrentPeriodEnd ON #TempRSAEarndPremSplit
   ( dtCurrentPeriodEnd , StatsFolderCnt )

CREATE INDEX Idx_dt12MonthsAgo ON #TempRSAEarndPremSplit
   ( dt12MonthsAgo , StatsFolderCnt )

CREATE INDEX Idx_dtLastYearEnd ON #TempRSAEarndPremSplit
   ( dtLastYearEnd  , StatsFolderCnt )

CREATE INDEX Idx_DatesInRange ON #TempRSAEarndPremSplit
   ( DatesInRange , StatsFolderCnt )


CREATE INDEX Idx_PeriodRangeID ON #TempRSAEarndPremSplit
   ( PeriodRangeID,  StatsFolderCnt )

-- CURRENT PERIOD
-- Add Premium Records
INSERT INTO #TempRSAEarndPremSplit
    SELECT   StatsFolderCnt,
--        NULL,
        ProductCode,
        ProductDesc,
        CommissionOrPremium,
        1,      -- Current Period
        'Current Period',
        RiskTypeCode,
        RiskTypeDescription,
        GrossTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        GrossTotal,
        CoinsTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        CoinsTotal,
        TreatyTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        TreatyTotal,
        FacTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        FacTotal,
        DocumentRef,
        FromDate,
        ToDate,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        @dt12MonthPeriodEnd,
        PostingPeriodID,
        @CurrentPeriodID,
        DaysOfCoverTotal,
        IsMidnightRenewal,
        DocumentDate,
        ClassOfBusinessCode,
        NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
    FROM #tempRSAEarndPrem
    WHERE datediff(month, @dtCurrentPeriodEnd, ToDate) >= 0
    AND datediff(month, @dtCurrentPeriodEnd, FromDate) <= 0

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print 'LABEL4 Time: ' + convert(varchar(30), @TimeNow, 108)
    print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
    print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
    select @TimePoint = @TimeNow
END

-- CURRENT YEAR TO DATE
-- Add Premium Records
INSERT INTO #tempRSAEarndPremSplit
    SELECT   StatsFolderCnt,
--        NULL,
        ProductCode,
        ProductDesc,
        CommissionOrPremium,
        2,      -- Current Year To Date
        'Current Year To Date',
        RiskTypeCode,
        RiskTypeDescription,
        GrossTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        GrossTotal,
        CoinsTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        CoinsTotal,
        TreatyTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        TreatyTotal,
        FacTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        FacTotal,
        DocumentRef,
        FromDate,
        ToDate,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        @dt12MonthPeriodEnd,
        PostingPeriodID,
        @CurrentPeriodID,
        DaysOfCoverTotal,
        IsMidnightRenewal,
        DocumentDate,
        ClassOfBusinessCode,
        NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
    FROM #tempRSAEarndPrem
    WHERE datediff(month, @dtLastYearPeriodEndDate, ToDate) >= 1
    AND datediff(month, @dtCurrentPeriodEnd, FromDate) <= 0

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print 'LABEL5 Time: ' + convert(varchar(30), @TimeNow, 108)
    print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
    print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
    select @TimePoint = @TimeNow
END

-- MOVING 12 MONTHS
-- Add Premium Records
INSERT INTO #tempRSAEarndPremSplit
    SELECT   StatsFolderCnt,
--        NULL,
        ProductCode,
        ProductDesc,
        CommissionOrPremium,
        3,      -- 12 Months To Date
        '12 Months To Date',
        RiskTypeCode,
        RiskTypeDescription,
        GrossTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        GrossTotal,
        CoinsTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        CoinsTotal,
        TreatyTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        TreatyTotal,
        FacTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        FacTotal,
        DocumentRef,
        FromDate,
        ToDate,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        @dt12MonthPeriodEnd,
        PostingPeriodID,
        @CurrentPeriodID,
        DaysOfCoverTotal,
        IsMidnightRenewal,
        DocumentDate,
        ClassOfBusinessCode,
        NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
    FROM #tempRSAEarndPrem
    WHERE datediff(month, @dt12MonthPeriodEnd, ToDate) >= 1
    AND datediff(month, @dtCurrentPeriodEnd, FromDate) <= 0

DROP TABLE #tempRSAEarndPrem



IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print 'LABEL6 Time: ' + convert(varchar(30), @TimeNow, 108)
    print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
    print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
    select @TimePoint = @TimeNow
END



-- TB 28/8/2002 - Pre-Process temp table before passing to Crystal, otherwise the
-- sheer volume of data will slow down the report to a snails pace

--  CurrentPeriodStart
-- This is the last period end date
UPDATE #TempRSAEarndPremSplit
    SET CurrentPeriodStart = ( select period_end_Date
                               from period
                              where period_id = ( select period_id  - 1
                                                    from period
                                                   where period_end_date = dtCurrentPeriodEnd
                                                     and company_id = 1)
                              )

-- BackDateExtra
UPDATE #TempRSAEarndPremSplit
    SET BackDateExtra = 0
 WHERE PeriodRangeID = 1

UPDATE #TempRSAEarndPremSplit
    SET BackDateExtra = datediff(day, FromDate, CurrentPeriodStart)
WHERE FromDate < CurrentPeriodStart
--  AND DocumentDate >= CurrentPeriodStart        -- Original
--  AND DateDiff(day, DocumentDate, CurrentPeriodStart) <= 0  -- WRONG !!
  AND DateDiff(day,  CurrentPeriodStart, DocumentDate) >= 0
  AND PeriodRangeID = 1

-- Populate the DatesInRange Parameter
-- Note:  DatesInRange is just a flag to avoid overwriting CalcDaysOfCover fields
-- **************** Current *********************

-- Neither Dates in Range Current
UPDATE #tempRSAEarndPremSplit
    SET DatesInRange = 0,
        CalcDaysOfCoverCurrent = 0
WHERE ToDate < CurrentPeriodStart
   OR FromDate > dtCurrentPeriodEnd
  AND PeriodRangeID = 1

--BothDatesInRange Current
UPDATE #TempRSAEarndPremSplit
    SET DatesInRange = 2,
        CalcDaysOfCoverCurrent = datediff(day, FromDate, ToDate)
WHERE FromDate > CurrentPeriodStart
  AND ToDate < dtCurrentPeriodEnd
  AND DatesInRange is NULL
  AND PeriodRangeID = 1

-- BothDatesOutsideRange Current
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 1,
       CalcDaysOfCoverCurrent =
       datediff(day, CurrentPeriodStart, dtCurrentPeriodEnd) +
--       datediff(day, FromDate, dtCurrentPeriodEnd) +            -- WRONG!!
       BackDateExtra + 1
WHERE FromDate < CurrentPeriodStart
  AND ToDate > dtCurrentPeriodEnd
  AND DatesInRange is NULL
  AND PeriodRangeID = 1

-- ToDate < dtCurrentPeriodEnd
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 3,
       CalcDaysOfCoverCurrent =
       datediff(day, CurrentPeriodStart, ToDate) + BackDateExtra + 1
WHERE ToDate < dtCurrentPeriodEnd
  AND DatesInRange is NULL
  AND PeriodRangeID = 1

-- Anything Else
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 5,
       CalcDaysOfCoverCurrent =
       datediff(day, FromDate, dtCurrentPeriodEnd) + BackDateExtra + 1
WHERE DatesInRange is NULL
  AND PeriodRangeID = 1

-- Cant leave any as null
-- yes we can
/*UPDATE #TempRSAEarndPremSplit
   SET CalcDaysOfCoverCurrent = 0
 WHERE CalcDaysOfCoverCurrent is NULL
  AND PeriodRangeID = 1
*/

if @DEBUG = 2
BEGIN
    select commissionorpremium, periodrangeid, statsfoldercnt, documentref, fromdate, todate, dtcurrentperiodend,
        documentdate, productdesc, risktypedescription, gross, grosstotal,
        calcdaysofcovercurrent, datesinrange, backdateextra
    from #TempRSAEarndPremSplit
    WHERE productdesc = 'money' and risktypedescription = 'accident'
    order by periodrangeid, commissionorpremium, backdateextra, productdesc, risktypedescription
END

-- Set the fields dependent on CalcDaysOfCoverCurrent
UPDATE #TempRSAEarndPremSplit
   SET GrossCoverRounded = isnull(Gross,0) * CalcDaysOfCoverCurrent,
       CoInsCoverRounded = isnull(Coinsurance,0) * CalcDaysOfCoverCurrent,
       TreatyCoverRounded = isnull(Treaty,0) * CalcDaysOfCoverCurrent,
       FACCoverRounded = isnull(Facultative,0) * CalcDaysOfCoverCurrent
 WHERE PeriodRangeID = 1

-- Set the dependent fields
UPDATE #TempRSAEarndPremSplit
   SET NetCoverRounded = GrossCoverRounded - CoInsCoverRounded,
       RetainedCoverRounded = (GrossCoverRounded - CoInsCoverRounded) -
                              (TreatyCoverRounded + FACCoverRounded)
 WHERE PeriodRangeID = 1



-- ******************** 12 Month ***********************************
-- Crystal Reports code this replaces

-- Reset DatesInRange to NULL
-- Reset BackDateExtra to NULL
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = NULL,
       BackDateExtra = 0
 WHERE ( PeriodRangeID < 1 OR PeriodRangeID > 2 )

-- Work out the BackDateExtra quantity
UPDATE #TempRSAEarndPremSplit
   SET dt12MonthStart = dt12MonthsAgo
 WHERE ( PeriodRangeID < 1 OR PeriodRangeID > 2 )

UPDATE #TempRSAEarndPremSplit
   SET BackDateExtra = datediff(day, FromDate, dt12MonthStart)
 WHERE FromDate < dt12MonthStart
   AND DocumentDate >= dt12MonthStart
   AND ( PeriodRangeID < 1 OR PeriodRangeID > 2 )

-- NeitherDatesInRange
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 0,
       CalcDaysofCover12Month = 0
 WHERE ToDate < dt12MonthsAgo
    OR FromDate > dtCurrentPeriodEnd
   AND ( PeriodRangeID < 1 OR PeriodRangeID > 2 )

-- BothDatesInRange
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 2,
       CalcDaysofCover12Month = datediff(day, FromDate, ToDate)
 WHERE FromDate > dt12MonthsAgo
   AND ToDate < dtCurrentPeriodEnd
   AND DatesInRange is NULL
   AND ( PeriodRangeID < 1 OR PeriodRangeID > 2 )

-- BothDatesOutsideRange
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 1,
       CalcDaysofCover12Month =
       datediff(day, dt12MonthsAgo, dtCurrentPeriodEnd) + BackDateExtra
 WHERE FromDate < dt12MonthsAgo
   AND Todate > dtCurrentPeriodEnd
   AND DatesInRange is NULL
   AND ( PeriodRangeID < 1 OR PeriodRangeID > 2 )

UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 4,
       CalcDaysofCover12Month =
       datediff(day, dt12MonthsAgo, ToDate) + BackDateExtra
 WHERE ToDate < dtCurrentPeriodEnd
   AND DatesInRange is NULL
   AND ( PeriodRangeID < 1 OR PeriodRangeID > 2 )

-- ELSE - anything where DatesInRange not updated
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 5,
       CalcDaysOfCover12Month =
       datediff(day, FromDate, dtCurrentPeriodEnd) + BackDateExtra
 WHERE DatesInRange is NULL
   AND ( PeriodRangeID < 1 OR PeriodRangeID > 2 )

-- Cant leave any as null
-- Yes We can
/*UPDATE #TempRSAEarndPremSplit
   SET CalcDaysOfCover12Month = 0
 WHERE CalcDaysOfCover12Month is NULL
   AND ( PeriodRangeID < 1 OR PeriodRangeID > 2 )
*/

-- Set the fields dependent on CalcDaysOfCover12Month
UPDATE #TempRSAEarndPremSplit
   SET GrossCoverRounded = isnull(Gross,0) * CalcDaysOfCover12Month,
       CoInsCoverRounded = isnull(Coinsurance,0) * CalcDaysOfCover12Month,
       TreatyCoverRounded = isnull(Treaty,0) * CalcDaysOfCover12Month,
       FACCoverRounded = isnull(Facultative,0) * CalcDaysOfCover12Month
 WHERE PeriodRangeID <> 1
   AND PeriodRangeID <> 2

-- Set the dependent fields
UPDATE #TempRSAEarndPremSplit
   SET NetCoverRounded = GrossCoverRounded - CoInsCoverRounded,
       RetainedCoverRounded = (GrossCoverRounded - CoInsCoverRounded) -
                              (TreatyCoverRounded + FACCoverRounded)
 WHERE PeriodRangeID <> 1
   AND PeriodRangeID <> 2



-- ******************** YearToDate ***********************************
--Crystal Reports code this replaces

-- Reset DatesInRange to NULL
-- Reset BackDateExtra to NULL
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = NULL,
       BackDateExtra = 0
 WHERE PeriodRangeID = 2

-- Work out the BackDateExtra quantity
UPDATE #TempRSAEarndPremSplit
   SET dtYTDStart = dtLastYearEnd
 WHERE PeriodRangeID = 2

UPDATE #TempRSAEarndPremSplit
   SET BackDateExtra = datediff(day, FromDate, dtYTDStart)
 WHERE FromDate <= dtYTDStart
   AND DocumentDate > dtYTDStart
   AND PeriodRangeID = 2

-- NeitherDatesInRange
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 0,
       CalcDaysofCoverYearToDate = 0
 WHERE ToDate < dtLastYearEnd
    OR FromDate > dtCurrentPeriodEnd
   AND PeriodRangeID = 2

-- BothDatesInRange
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 2,
       CalcDaysofCoverYearToDate = datediff(day, FromDate, ToDate)
 WHERE FromDate > dtLastYearEnd
   AND ToDate < dtCurrentPeriodEnd
   AND DatesInRange is NULL
   AND PeriodRangeID = 2

-- BothDatesOutsideRange
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 1,
       CalcDaysofCoverYearToDate =
       datediff(day, dtLastYearEnd, dtCurrentPeriodEnd) + BackDateExtra
 WHERE FromDate < dtLastYearEnd
   AND Todate > dtCurrentPeriodEnd
   AND DatesInRange is NULL
   AND PeriodRangeID = 2

UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 4,
       CalcDaysofCoverYearToDate =
       datediff(day, dtLastYearEnd, ToDate) + BackDateExtra
 WHERE ToDate < dtCurrentPeriodEnd
   AND DatesInRange is NULL
   AND PeriodRangeID = 2

-- ELSE - anything where DatesInRange not updated
UPDATE #TempRSAEarndPremSplit
   SET DatesInRange = 5,
       CalcDaysOfCoverYearToDate =
       datediff(day, FromDate, dtCurrentPeriodEnd) + BackDateExtra
 WHERE DatesInRange is NULL
   AND PeriodRangeID = 2

-- Cant leave any as null
-- Yes we can!
/*UPDATE #TempRSAEarndPremSplit
   SET CalcDaysOfCoverYearToDate = 0
 WHERE CalcDaysOfCoverYearToDate is NULL
   AND PeriodRangeID = 2
*/

-- Set the fields dependent on CalcDaysOfCoverYearToDate
UPDATE #TempRSAEarndPremSplit
   SET GrossCoverRounded = isnull(Gross,0) * CalcDaysOfCoverYearToDate,
       CoInsCoverRounded = isnull(Coinsurance,0) * CalcDaysOfCoverYearToDate,
       TreatyCoverRounded = isnull(Treaty,0) * CalcDaysOfCoverYearToDate,
       FACCoverRounded = isnull(Facultative,0) * CalcDaysOfCoverYearToDate
 WHERE PeriodRangeID = 2

-- Set the dependent fields
UPDATE #TempRSAEarndPremSplit
   SET NetCoverRounded = GrossCoverRounded - CoInsCoverRounded,
       RetainedCoverRounded = (GrossCoverRounded - CoInsCoverRounded) -
                              (TreatyCoverRounded + FACCoverRounded)
 WHERE PeriodRangeID = 2




SET NOCOUNT OFF

--
-- Don't Select ALL the data as there is over 400Mb on RSA's database
-- SELECT * FROM #tempRSAEarndPremSplit

-- DEBUG Special - dont do the final report
IF @debug < 2
BEGIN
    IF @DetailSummary = 'SUMMARY'
    BEGIN
        SELECT   PeriodRangeID,
                 PeriodRangeName,
            'StatsFolderCnt'=COUNT(DISTINCT StatsFolderCnt),
            'CountOfStatsFolderCnt'=COUNT(DISTINCT StatsFolderCnt),
            ProductCode,
            ProductDesc,
            CommissionOrPremium,
            RiskTypeDescription,
            ClassOfBusinessCode,
            'DocumentRef'=NULL,
            'GrossCoverRounded'=SUM(GrossCoverRounded),
            'CoInsCoverRounded'=SUM(CoInsCoverRounded),
            'NetCoverRounded'=SUM(NetCoverRounded),
            'TreatyCoverRounded'=SUM(TreatyCoverRounded),
            'FACCoverRounded'=SUM(FACCoverRounded),
            'RetainedCoverRounded'=SUM(RetainedCoverRounded),
            dtCurrentPeriodEnd
        FROM #tempRSAEarndPremSplit
	WHERE PostingPeriodID = @Period OR @Period = 0
        GROUP BY  dtCurrentPeriodEnd, PeriodRangeID, PeriodRangeName, CommissionOrPRemium,  ProductDesc, RiskTypeDescription,
              ProductCode, ClassOfBusinessCode
    END
    ELSE     -- DETAIL - include Document Ref
    BEGIN
        SELECT  DISTINCT  PeriodRangeID,
                 PeriodRangeName,
            StatsFolderCnt,
            'CountOfStatsFolderCnt'=1,
            ProductCode,
            ProductDesc,
            CommissionOrPremium,
            RiskTypeDescription,
            ClassOfBusinessCode,
            DocumentRef,
            'GrossCoverRounded'=SUM(GrossCoverRounded),
            'CoInsCoverRounded'=SUM(CoInsCoverRounded),
            'NetCoverRounded'=SUM(NetCoverRounded),
            'TreatyCoverRounded'=SUM(TreatyCoverRounded),
            'FACCoverRounded'=SUM(FACCoverRounded),
            'RetainedCoverRounded'=SUM(RetainedCoverRounded),
            dtCurrentPeriodEnd
        FROM #tempRSAEarndPremSplit
	WHERE PostingPeriodID = @Period OR @Period = 0
        GROUP BY  dtCurrentPeriodEnd, PeriodRangeID, PeriodRangeName, CommissionOrPRemium,  ProductDesc, RiskTypeDescription,
              ProductCode, DocumentRef, StatsFolderCnt, ClassOfBusinessCode
    END
END
ELSE  -- Limit output to Accident\Money only
IF @debug = 3
    BEGIN
        IF @DetailSummary = 'SUMMARY'
        BEGIN
            SELECT   PeriodRangeID,
                     PeriodRangeName,
                'StatsFolderCnt'=COUNT(DISTINCT StatsFolderCnt),
                'CountOfStatsFolderCnt'=COUNT(DISTINCT StatsFolderCnt),
                ProductCode,
                ProductDesc,
                CommissionOrPremium,
                RiskTypeDescription,
                'DocumentRef'=NULL,
                'GrossCoverRounded'=SUM(GrossCoverRounded),
                'CoInsCoverRounded'=SUM(CoInsCoverRounded),
                'NetCoverRounded'=SUM(NetCoverRounded),
                'TreatyCoverRounded'=SUM(TreatyCoverRounded),
                'FACCoverRounded'=SUM(FACCoverRounded),
                'RetainedCoverRounded'=SUM(RetainedCoverRounded),
                dtCurrentPeriodEnd
            FROM #tempRSAEarndPremSplit
            WHERE productdesc = 'money' and risktypedescription = 'accident'
            GROUP BY  dtCurrentPeriodEnd, PeriodRangeID, PeriodRangeName, CommissionOrPRemium,  ProductDesc, RiskTypeDescription,
                  ProductCode
        END
        ELSE     -- DETAIL - include Document Ref
        BEGIN
            SELECT  DISTINCT  PeriodRangeID,
                PeriodRangeName,
                StatsFolderCnt,
                'CountOfStatsFolderCnt'=1,
                ProductCode,
                ProductDesc,
                CommissionOrPremium,
                RiskTypeDescription,
                DocumentRef,
                'GrossCoverRounded'=SUM(GrossCoverRounded),
                'CoInsCoverRounded'=SUM(CoInsCoverRounded),
                'NetCoverRounded'=SUM(NetCoverRounded),
                'TreatyCoverRounded'=SUM(TreatyCoverRounded),
                'FACCoverRounded'=SUM(FACCoverRounded),
                'RetainedCoverRounded'=SUM(RetainedCoverRounded),
                dtCurrentPeriodEnd
            FROM #tempRSAEarndPremSplit
            WHERE productdesc = 'money' and risktypedescription = 'accident'
            GROUP BY  dtCurrentPeriodEnd, PeriodRangeID, PeriodRangeName, CommissionOrPRemium,  ProductDesc, RiskTypeDescription,
                  ProductCode, DocumentRef, StatsFolderCnt
        END
    END

-- @debug >1 No output at all


DROP TABLE #tempRSAEarndPremSplit


IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print 'LABEL9 Time: ' + convert(varchar(30), @TimeNow, 108)
    print 'Total Run Time: ' + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + ' milliseconds'
    print 'Section Time: ' + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + ' milliseconds'
    select @TimePoint = @TimeNow
END


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO



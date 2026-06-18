SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Unearned_Premium_SFU'
GO

CREATE PROCEDURE spu_Report_Unearned_Premium_SFU
    @DetailSummary varchar(10),
    @branch_id int, 
    @End_Date NVARCHAR(50),
	@TypeOfCurrency VARCHAR(10)
AS

-- $Author: Tom.brown $
-- $Revision: 11 $
-- $Modtime: 8/10/02 11:07 $
-- $Workfile: sp_Report_Unearned_Premium.sql $
-- $Logfile: /Sirius For Underwriting/Crystal Reports/Stored Procedures/sp_Report_Unearned_Premium.sql $
-- $History: sp_Report_Unearned_Premium.sql $
--
-- *****************  Version 11  *****************
-- User: Tom.brown    Date: 8/10/02    Time: 11:10
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- F00059638:  Fix inconsistancy between Earned and UnEarned premium
-- reports.  Ensure both work from Current Period End Date and print this
-- on the report heading
/**********************************************************************************************************************************
** Created by Jude Killip
** 11/06/2001
** RSA Reports -  Unearned_Premium.rpt
**
** Reports calculates unearned values
**********************************************************************************************************************************
**
** 16/06/2001   Jude Killip     12months correction if we're in the very first period
**
** 19/06/2001   Jude Killip     Just get every transaction with future expiry date
**                              (i.e. expiry date  > current period end date)
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
** 27/08/2002  Tom Brown        Added Calc Fields to save calc time in crystal report
**                              Further addition of GROUP BY and SUM to save Crystal processing time
**                              Debug Timers for fine tuning the query
** 06/09/2002  Tom Brown        Make report type a parameter - as 'Detailed' Requires different select
**                              to 'Summary'
** 28/05/2003  Jon Kemp         branch parameter added
**
** 05/08/2003  AMB              Replace use of 'transaction_export_folder' with 'document' - TEF should not be used in SFU
**
** 03/26/2004  Jay Bishtawi		Added the As_of_date PN9098.  Changed the way calculation are made commented my changes.
***********************************************************************************************************************************/
SELECT @End_Date= CONVERT(DATETIME,@End_Date,103)
DECLARE @TypeOfRates TINYINT
DECLARE @RatesCompanyID INT
DECLARE @system_currency_id INT
DECLARE @iso_code VARCHAR(4)
DECLARE @description VARCHAR(255)

declare @ibranchid int

IF @branch_id IS NULL
    SELECT @iBranchID = 0
ELSE
    SELECT @iBranchID = @branch_id


-- TB 27/08/2002 Main issue with this Report is its too slow,
DECLARE @DEBUG INT
SELECT @DEBUG = 0     -- OFF, 1= ON
-- TIMER  - to time the various parts of it
IF @DEBUG = 1
BEGIN
    declare @TimeNow datetime
    declare @TimeInit datetime
    declare @TimePoint datetime
    select @TimeInit = getdate()
    select @TimeNow = getdate()
    select @TimePoint = @TimeNow
    print "START Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
END

--AAB-March 26, 2004, commenting this section out, we are going to use the value 
--passed in by the user instead
--Had to leave the declartions out so we can use for debug
DECLARE  @12MonthPeriodID int, @dt12MonthPeriodEnd datetime
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime

/*Is the system set up with one set of rates?*/
EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT

IF @TypeOfRates = 1
BEGIN
	SELECT @RatesCompanyID = 1
END
ELSE
BEGIN
	SELECT @RatesCompanyID = NULL
END

/*Get System Currency*/
SELECT
	@system_currency_id = currency_id
FROM PMSystem
WHERE system_id=1


IF @DEBUG = 1
BEGIN
    SELECT "Current ID = " , @CurrentPeriodID
    SELECT "Current End Date = ", @dtCurrentPeriodEND
    SELECT "12 Month ID = ",12MonthPeriodID
    SELECT "12 month = ", @dt12MonthPeriodEnd
END

CREATE TABLE #tempRSAUnEarndPrem
(
    StatsFolderCnt int,
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    CommissionOrPremium int NULL,           -- 0=premium, 1=commission
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
    PostingPeriodID int,
    CurrentPeriodID int,
    DaysOfCoverTotal int,
    IsMidnightRenewal int,
    BothDatesInRange tinyint            NULL,     -- TB 27/8/02  Added Calc Fields
    CalcDaysOfCover int                 NULL,
    GrossCoverRounded decimal (19,2)    NULL,
    CoInsCoverRounded decimal (19,2)    NULL,
    NetCoverRounded decimal (19,2)      NULL,
    TreatyCoverRounded decimal (19,2)   NULL,
    FACCoverRounded decimal (19,2)      NULL,
    RetainedCoverRounded decimal (19,2) NULL,
    EarningPatternId int
)

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL1 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END


-- MOVING 12 MONTHS
-- Add Premium Records
INSERT INTO #tempRSAUnEarndPrem
    SELECT
		sd.stats_folder_cnt,
        sf.product_code,
        p.description,
        0,      -- Premium
        sd.risk_type_code,
        rt.description,
        NULL,
		CASE @TypeOfCurrency
		WHEN 'Base' THEN
			(SELECT ROUND(sd.this_premium_home,2) WHERE sd.stats_detail_type = 'GRS')
		WHEN 'System' THEN
			(SELECT ROUND(sd.this_premium_system,2) WHERE sd.stats_detail_type = 'GRS')
		END,
        NULL,
        CASE @TypeOfCurrency
		WHEN 'Base' THEN
			(SELECT ROUND(sd.this_premium_home,2) WHERE sd.stats_detail_type = 'COI')
		WHEN 'System' THEN
			(SELECT ROUND(sd.this_premium_system,2) WHERE sd.stats_detail_type = 'COI')
		END,
        NULL,
        CASE @TypeOfCurrency
		WHEN 'Base' THEN
			(SELECT ROUND(sd.this_premium_home * -1,2) WHERE sd.stats_detail_type = 'TTY')
		WHEN 'System' THEN
			(SELECT ROUND(sd.this_premium_system * -1,2) WHERE sd.stats_detail_type = 'TTY')
		END,
        NULL,
		CASE @TypeOfCurrency
		WHEN 'Base' THEN
			(SELECT ROUND(sd.this_premium_home * -1,2) WHERE sd.stats_detail_type = 'FAC')
		WHEN 'System' THEN
			(SELECT ROUND(sd.this_premium_system * -1,2) WHERE sd.stats_detail_type = 'FAC')
		END,
        sf.document_ref,
        sf.cover_start_date,
        sf.expiry_date,
        --@dtCurrentPeriodEnd,
        --AAB, March 26, 2004 replace with the date provided by the user
        @End_Date, 
        sf.posting_period_number,
        @CurrentPeriodID,
        datediff(day, sf.cover_start_date, sf.expiry_date),
        isnull(p.is_midnight_renewal,0),
        NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
        sd.Earning_Pattern_id

    FROM Stats_Folder sf
    JOIN Stats_Detail sd
		ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND ISNULL(sd.this_premium_home,0) <> 0
        AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC')
        AND sd.earning_pattern_id<>2
    JOIN Product p
		ON sf.product_id = p.product_id
    JOIN Risk_Type rt
		ON sd.risk_type_id = rt.risk_type_id
    INNER JOIN Document AS doc 
        ON doc.document_ref = sf.document_ref
    WHERE DATEDIFF(DAY, sf.cover_start_date, sf.expiry_date)<> 0      -- in case of div/zero
    AND sf.expiry_date > @End_Date
    AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    AND (@iBranchID = 0 OR (@iBranchID <> 0 AND sf.branch_id = @iBranchID))
	--AND sf.cover_start_date <= @End_Date


IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL2 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END

-- Now add Commission Records
INSERT INTO #tempRSAUnEarndPrem
    SELECT 
		sd.stats_folder_cnt,
        sf.product_code,
        p.description,
        1,      -- Commission
        sd.risk_type_code,
        rt.description,
        NULL,
		CASE @TypeOfCurrency  
		WHEN 'Base' THEN  
			(SELECT ROUND (ISNULL(sd.lead_commission_value_home,0),2 ) WHERE  sd.stats_detail_type IN ('GRS','SUB')   )    
		WHEN 'System' THEN  
			(SELECT ROUND((ISNULL(sd.lead_commission_value_system,0) +  
			ISNULL(sd.sub_commission_value_system,0)),2) WHERE  sd.stats_detail_type IN ('GRS','SUB'))  
		END,  
        NULL,
		CASE @TypeOfCurrency
		WHEN 'Base' THEN
			(SELECT ROUND((ISNULL(sd.lead_commission_value_home,0) +
					ISNULL(sd.sub_commission_value_home,0)),2) WHERE sd.stats_detail_type = 'COI')
		WHEN 'System' THEN
			(SELECT ROUND((ISNULL(sd.lead_commission_value_system,0) +
					ISNULL(sd.sub_commission_value_system,0)),2) WHERE sd.stats_detail_type = 'COI')
		END,
        NULL,
		CASE @TypeOfCurrency
		WHEN 'Base' THEN
			(SELECT ROUND((ISNULL(sd.lead_commission_value_home,0) +
					ISNULL(sd.sub_commission_value_home,0)) * -1,2) WHERE sd.stats_detail_type = 'TTY')
		WHEN 'System' THEN
			(SELECT ROUND((ISNULL(sd.lead_commission_value_system,0) +
					ISNULL(sd.sub_commission_value_system,0)) * -1,2) WHERE sd.stats_detail_type = 'TTY')
		END,
        NULL,
		CASE @TypeOfCurrency
		WHEN 'Base' THEN
			(SELECT ROUND((ISNULL(sd.lead_commission_value_home,0) +
					ISNULL(sd.sub_commission_value_home,0)) * -1,2) WHERE sd.stats_detail_type = 'FAC')
		WHEN 'System' THEN
			(SELECT ROUND((ISNULL(sd.lead_commission_value_system,0) +
					ISNULL(sd.sub_commission_value_system,0)) * -1,2) WHERE sd.stats_detail_type = 'FAC')
		END,
        sf.document_ref,
        sf.cover_start_date,
        sf.expiry_date,
        --@dtCurrentPeriodEnd,
        --AAB, March 26, 2004 replace with the date provided by the user
        @End_Date, 
        sf.posting_period_number,
        @CurrentPeriodID,
        datediff(day, sf.cover_start_date, sf.expiry_date),
        isnull(p.is_midnight_renewal,0),
        NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
        sd.Earning_Pattern_id
    FROM Stats_Folder sf
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.lead_commission_value_home,0) +
            isnull(sd.sub_commission_value_home,0) <> 0
        AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC','SUB')
        AND sd.earning_pattern_id<>2
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    INNER JOIN Document AS doc 
        ON doc.document_ref = sf.document_ref
    WHERE datediff(day, sf.cover_start_date, sf.expiry_date) <> 0       -- in case of div/zero
    AND sf.expiry_date > @End_Date
    AND sf.transaction_type_code NOT LIKE ('C_%')                       -- all but claims
    AND (@iBranchID = 0 OR (@iBranchID <> 0 AND sf.branch_id = @iBranchID))
	AND sf.cover_start_date <= @End_Date

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL3 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END

-- update with daily rates
UPDATE #tempRSAUnEarndPrem
	--This gets us the value of each day in the policy
   SET 
       Gross = CASE EarningPatternId
                               WHEN 1 THEN GrossTotal/(DaysOfCoverTotal+IsMidnightRenewal)
                               ELSE isnull(GrossTotal,0)
                           END,
       Coinsurance = CASE EarningPatternId
                               WHEN 1 THEN CoinsTotal/(DaysOfCoverTotal+IsMidnightRenewal)
                               ELSE isnull(CoinsTotal,0)
                           END,
       Treaty = CASE EarningPatternId
                                WHEN 1 THEN TreatyTotal/(DaysOfCoverTotal+IsMidnightRenewal)
                                ELSE isnull(TreatyTotal,0)
                            END,
       Facultative = CASE EarningPatternId
                             WHEN 1 THEN FacTotal/(DaysOfCoverTotal+IsMidnightRenewal)
                             ELSE isnull(FacTotal,0)
                         END

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL4 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END

-- TB 27/8/02  Populate the new Calc Fields
UPDATE #TempRSAUnEarndPrem
	--If the effective date is greater than the As_Of_Date then we determine the difference
	--between effective and expiration date
    SET BothDatesInRange = 1,                              -- Set all to true and correct later
        CalcDaysOfCover = datediff(day, FromDate, ToDate)  -- Reset incorrect ones later
--WHERE FromDate > dtCurrentPeriodEnd
WHERE FromDate > @End_Date

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL5 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END

UPDATE #TempRSAUnEarndPrem
	--If the effective date is less than the As_Of_Date then we determine the difference
	--between effective and the As_of_date
    SET BothDatesInRange = 0,                                        -- Set all to true and correct later
        --CalcDaysOfCover = DateDiff(day, dtCurrentPeriodEnd,  ToDate) -- Reset incorrect ones later
        CalcDaysOfCover = DateDiff(day, FromDate, dtCurrentPeriodEnd) -- Reset incorrect ones later
--WHERE FromDate <= dtCurrentPeriodEnd
WHERE FromDate <= @End_Date


IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL6 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END

UPDATE #TempRSAUnEarndPrem
    SET
    --If the effective date if the policy is <= to the as_of_date
    --The calculation should be = Premium - (DayRate * number of days) 
    --Example:  if the premium is 2000 and the number of days from effective date to as_of_date = 10 then
    --calculation should be = 2000 - ((2000/365)* 10)  (2000/365) is done when we calculate the Gross etc...

       GrossCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(GrossTotal,0) - (isnull(Gross,0) * CalcDaysOfCover)
                               ELSE isnull(GrossTotal,0) - isnull(Gross,0)
                           END,
       CoInsCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(CoinsTotal,0) - (isnull(Coinsurance, 0) * CalcDaysOfCover)
                               ELSE isnull(CoinsTotal,0) - isnull(Coinsurance, 0)
                           END,
       TreatyCoverRounded = CASE EarningPatternId
                                WHEN 1 THEN isnull(TreatyTotal,0) - (isnull(Treaty,0) * CalcDaysOfCover)
                                ELSE isnull(TreatyTotal,0) - isnull(Treaty,0)
                            END,
       FACCoverRounded = CASE EarningPatternId
                             WHEN 1 THEN isnull(FacTotal, 0) - (isnull(Facultative,0) * CalcDaysOfCover)
                             ELSE isnull(FacTotal,0) - isnull(Facultative,0)
                         END

where BothDatesInRange = 0


UPDATE #TempRSAUnEarndPrem
    SET
	--If the effective date if the policy is <= to the as_of_date
    --The calculation should be = DayRate * number of days
    --Example:  if the premium is 2000 and the number of days from effective date Expriation date = 365 then
    --calculation should be = (2000/365)* 365  (2000/365) is done when we calculate the Gross etc...
       GrossCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(Gross,0) * CalcDaysOfCover
                               ELSE isnull(Gross,0)
                           END,
       CoInsCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(Coinsurance, 0) * CalcDaysOfCover
                               ELSE isnull(Coinsurance,0)
                           END,
       TreatyCoverRounded = CASE EarningPatternId
                                WHEN 1 THEN isnull(Treaty,0) * CalcDaysOfCover
                                ELSE isnull(Treaty,0)
                            END,
       FACCoverRounded = CASE EarningPatternId
                             WHEN 1 THEN isnull(Facultative,0) * CalcDaysOfCover
                             ELSE isnull(Facultative,0)
                         END
where BothDatesInRange = 1

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL7 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END


UPDATE #TempRSAUnEarndPrem
    SET
    NetCoverRounded      = isnull(GrossCoverRounded,0) - isnull(CoInsCoverRounded,0)

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL8 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END

UPDATE #TempRSAUnEarndPrem
    SET
    RetainedCoverRounded =  isnull(NetCoverRounded,0) - ( isnull(TreatyCoverRounded,0) + isnull(FACCoverRounded,0))

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "LABEL9 Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END

/*Set currency code and description for report header*/
IF @TypeOfCurrency = 'Base'
BEGIN
	SELECT
		@iso_code = c.iso_code,
		@description = c.description
	FROM currency c
	JOIN source s
		ON s.base_currency_id = c.currency_id
	WHERE s.source_id = @branch_id
END

IF @TypeOfCurrency = 'System'
BEGIN
	SELECT
		@iso_code = c.iso_code,
		@description = c.description
	FROM currency c
	WHERE c.currency_id = @system_currency_id
END

SET NOCOUNT OFF

-- The 'SELECT *' takes the longest time (38,000 records about 300Mb of Data)
-- SELECT * FROM #tempRSAUnEarndPrem
-- to reduce this, only return the records that Crystal actually needs, and
-- pre-summary them according the report requirements

IF @DetailSummary = 'SUMMARY'
BEGIN
    SELECT
    	'RowCount' = COUNT(DISTINCT StatsFolderCnt),
        'StatsFolderCnt'=COUNT(DISTINCT StatsFolderCnt),
		ProductCode,
        ProductDesc,
        CommissionOrPremium,
        RiskTypeDescription,
		'DocumentRef'=NULL,
        dtCurrentPeriodEnd,
        'GrossCoverRounded'=SUM(GrossCoverRounded),
        'CoInsCoverRounded'=SUM(CoInsCoverRounded),
        'NetCoverRounded'=SUM(NetCoverRounded),
        'TreatyCoverRounded'=SUM(TreatyCoverRounded),
        'FACCoverRounded'=SUM(FACCoverRounded),
        'RetainedCoverRounded'=SUM(RetainedCoverRounded),
		'CurrencyCode' = @iso_code,
		'CurrencyDesc' = @description
    FROM #TempRSAUnEarndPrem
    GROUP BY CommissionOrPRemium, ProductDesc, RiskTypeDescription,
          ProductCode, dtCurrentPeriodEnd
END
ELSE   -- DETAIL report (Include DocumentRef)
BEGIN
    SELECT DISTINCT
		'RowCount' = 1,
		StatsFolderCnt,
		ProductCode,
        ProductDesc,
        CommissionOrPremium,
        RiskTypeDescription,
        DocumentRef,
        dtCurrentPeriodEnd,
        'GrossCoverRounded'=SUM(GrossCoverRounded),
        'CoInsCoverRounded'=SUM(CoInsCoverRounded),
        'NetCoverRounded'=SUM(NetCoverRounded),
        'TreatyCoverRounded'=SUM(TreatyCoverRounded),
        'FACCoverRounded'=SUM(FACCoverRounded),
        'RetainedCoverRounded'=SUM(RetainedCoverRounded),
		'CurrencyCode' = @iso_code,
		'CurrencyDesc' = @description
    FROM #TempRSAUnEarndPrem
    GROUP BY CommissionOrPRemium, ProductDesc, RiskTypeDescription,
          ProductCode, StatsFolderCnt, DocumentRef, dtCurrentPeriodEnd
END


DROP TABLE #tempRSAUnEarndPrem

IF @DEBUG = 1
BEGIN
    select @TimeNow = getdate()
    print "END Time: " + convert(varchar(30), @TimeNow, 108)
    print "Total Run Time: " + convert(varchar(20),datediff(millisecond,@TimeInit, @TimeNow)) + " milliseconds"
    print "Section Time: " + convert(varchar(20),datediff(millisecond,@TimePoint, @TimeNow)) + " milliseconds"
    select @TimePoint = @TimeNow
END
GO

-- End of $Workfile: sp_Report_Unearned_Premium.sql $

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Earned_Premium_SFU'
go

CREATE PROCEDURE spu_Report_Earned_Premium_SFU
    @DetailSummary varchar(10),
    @period_end_date datetime,    
    @branch_id int,
	@TypeOfCurrency VARCHAR(10)
    
AS

SET NOCOUNT ON

DECLARE @TypeOfRates TINYINT
DECLARE @RatesCompanyID INT
DECLARE @system_currency_id INT
DECLARE @iso_code VARCHAR(4)
DECLARE @description VARCHAR(255)
 
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

DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
DECLARE  @12MonthPeriodID int, @dt12MonthPeriodEnd datetime
DECLARE @12MonthPeriodIDPlusOne int, @dtLastYearPeriodEndDate datetime,@CurrentYearStartPeriodID int

select @dtCurrentPeriodEnd = @period_end_date
select @dt12MonthPeriodEnd  = max(period_end_date) from period where period_end_date < ( select dateadd(year, -1 , @dtCurrentPeriodEnd))


SELECT @CurrentPeriodID = period_id
FROM period
WHERE period_end_Date  = @dtCurrentPeriodEnd

IF @CurrentPeriodID IS NULL
BEGIN
	SELECT @CurrentPeriodID = period_id + 1
	FROM period
	WHERE period_end_Date  <= @dtCurrentPeriodEnd
END

SELECT @CurrentYearStartPeriodID = min(period_id)
FROM period
WHERE year_name =
    (SELECT year_name
        FROM period
        WHERE period_id = @CurrentPeriodID)
SELECT @dtLastYearPeriodEndDate = period_end_date
FROM period
WHERE period_id = @CurrentYearStartPeriodID -1

IF @12MonthPeriodID = @CurrentPeriodID
    BEGIN
        SELECT  @12MonthPeriodIDPlusOne = @12MonthPeriodID
    END
ELSE
    BEGIN
        SELECT @12MonthPeriodIDPlusOne = @12MonthPeriodID + 1
    END


--IF @DEBUG = 1
--BEGIN
--    SELECT "Current ID = " , @CurrentPeriodID
--    SELECT "Current End Date = ", @dtCurrentPeriodEND
--    SELECT "Current Year = ", @dtLAstYearPeriodEndDate
--    SELECT "12 month = ", @dt12MonthPeriodEnd
--END

--AAB, March 26, 2004
declare @ibranchid int
--Set the branch to 1 if none is selected
IF @branch_id IS NULL
    SELECT @iBranchID = 0
ELSE
    SELECT @iBranchID = @branch_id

CREATE TABLE #tempRSAEarndPrem
(
    StatsFolderCnt		int,
    ProductCode			varchar (10) NULL,
    ProductDesc			varchar (255) NULL,
    CommissionOrPremium int NULL,           -- 0=premium, 1=commission
    PeriodRangeID		int,
    PeriodRangeName		varchar(30),
    RiskTypeCode		varchar (10) NULL,
    RiskTypeDescription varchar (255) NULL,
    Gross				FLOAT NULL,         -- GRS          Daily Rate = premium/days of cover
    GrossTotal			FLOAT NULL,
    Coinsurance			FLOAT NULL,         -- COI                  "
    CoinsTotal			FLOAT NULL,
    Treaty				FLOAT NULL,         -- TTY                  "
    TreatyTotal			FLOAT NULL,
    Facultative			FLOAT NULL,         -- FAC                  "
    FacTotal			FLOAT NULL,
    DocumentRef			varchar (25) NULL,
    FromDate			datetime NULL,
    ToDate				datetime NULL,
    PostingPeriodID		int,
    DaysOfCoverTotal	int,
    IsMidnightRenewal	int,
    DocumentDate		datetime,
    EarningPatternId    int
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
    SELECT	
		sd.stats_folder_cnt,
		sf.product_code,
		p.description,
		0,      -- Premium
		NULL,   -- PeriodRangeID
		NULL,   -- PeriodRangeName
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
		sf.posting_period_number,
		DATEDIFF(DAY, sf.cover_start_date, sf.expiry_date),
		ISNULL(p.is_midnight_renewal,0),
		sf.document_date,
                sd.Earning_Pattern_id
    FROM Stats_Folder sf
	JOIN Stats_Detail sd
		ON sf.stats_folder_cnt = sd.stats_folder_cnt
		AND ISNULL(sd.this_premium_home,0) <> 0
		AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC')
	JOIN Product p
		ON sf.product_id = p.product_id
	JOIN Risk_Type rt
		ON sd.risk_type_id = rt.risk_type_id
	WHERE
	( 
		DATEDIFF(MONTH, @dt12MonthPeriodEnd, sf.expiry_date) >= 1 
		AND 
		DATEDIFF(MONTH, @dtCurrentPeriodEnd, sf.cover_start_date) <= 0
	)
	AND DATEDIFF(DAY, sf.cover_start_date, sf.expiry_date) <> 0      -- in case of div/zero
	AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
	AND
	(
		SELECT ISNULL(MAX(tef.accounts_export_status),'x')
		FROM transaction_export_folder tef
		WHERE sf.document_ref = tef.document_ref 
	) = 'c'
	AND ( @iBranchID= 0 OR (@iBranchID <> 0 and sf.branch_id = @iBranchID))


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
    SELECT	
		sd.stats_folder_cnt,
		sf.product_code,
		p.description,
		1,      -- Commission
		NULL,   -- PeriodRangeID
		NULL,   -- PeriodRangeName
		sd.risk_type_code,
		rt.description,
		NULL,
		CASE @TypeOfCurrency
		WHEN 'Base' THEN  
			(SELECT ROUND(ISNULL(sd.lead_commission_value_home,0),2 ) WHERE  sd.stats_detail_type IN ('GRS','SUB')   )    
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
		sf.posting_period_number,
		DATEDIFF(DAY, sf.cover_start_date, sf.expiry_date),
		ISNULL(p.is_midnight_renewal,0),
		sf.document_date,
                sd.earning_pattern_id
    FROM Stats_Folder sf
	JOIN Stats_Detail sd
		ON sf.stats_folder_cnt = sd.stats_folder_cnt
		AND (ISNULL(sd.lead_commission_value_home,0) + ISNULL(sd.sub_commission_value_home,0)) <> 0
		AND sd.stats_detail_type IN ('GRS', 'COI', 'TTY', 'FAC')
	JOIN Risk_Type rt
		ON sd.risk_type_id = rt.risk_type_id
	JOIN Product p
		ON sf.product_id = p.product_id
	WHERE 
	(
		DATEDIFF(MONTH, @dt12MonthPeriodEnd, sf.expiry_date) >= 1
		AND 
		DATEDIFF(MONTH, @dtCurrentPeriodEnd, sf.cover_start_date) <= 0
	)
	AND DATEDIFF(DAY, sf.cover_start_date, sf.expiry_date)<> 0      -- in case of div/zero
	AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
	AND 
	(
		SELECT ISNULL(MAX(tef.accounts_export_status),'x')
		FROM transaction_export_folder tef 
		WHERE sf.document_ref = tef.document_ref
	) = 'c'
	AND (@iBranchID= 0 OR (@iBranchID <> 0 and sf.branch_id = @iBranchID ))

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
    StatsFolderCnt            int,
--    CountOfStatsFolderCnt int NULL,
    ProductCode				  varchar (10) NULL,
    ProductDesc			      varchar (255) NULL,
    CommissionOrPremium       int NULL,           -- 0=premium, 1=commission
    PeriodRangeID		      int,
    PeriodRangeName		      varchar(30),
    RiskTypeCode		      varchar (10) NULL,
    RiskTypeDescription       varchar (255) NULL,
    Gross				      FLOAT NULL,         -- GRS          Daily Rate = premium/days of cover
    GrossTotal			      FLOAT NULL,
    Coinsurance			      FLOAT NULL,        -- COI                  "
    CoinsTotal			      FLOAT NULL,
    Treaty					  FLOAT NULL,             -- TTY                  "
    TreatyTotal			      FLOAT NULL,
    Facultative			      FLOAT NULL,        -- FAC                  "
    FacTotal				  FLOAT NULL,
    DocumentRef			      varchar (25) NULL,
    FromDate			      datetime NULL,
    ToDate				      datetime NULL,
    dtCurrentPeriodEnd	      datetime,
    dtLastYearEnd		      datetime,
    dt12MonthsAgo		      datetime,
    PostingPeriodID		      int,
    CurrentPeriodID		      int,
    DaysOfCoverTotal	      int,
    IsMidnightRenewal	      int,
    DocumentDate		      datetime,
    CurrentPeriodStart	      datetime NULL,        -- TB 28/8/02 - Extra fields
    dt12MonthStart		      datetime NULL,
    dtYTDStart			      datetime NULL,
    BackDateExtra		      int NULL,
    DatesInRange			  int NULL,
    CalcDaysOfCoverCurrent	  int NULL,
    CalcDaysOfCoverYearToDate int NULL,
    CalcDaysOfCover12Month	  int NULL,
    GrossCoverRounded		  decimal (19,2) NULL,
    CoInsCoverRounded		  decimal (19,2) NULL,
    NetCoverRounded           decimal (19,2) NULL,
    TreatyCoverRounded		  decimal (19,2) NULL,
    FACCoverRounded			  decimal (19,2) NULL,
    RetainedCoverRounded      decimal (19,2) NULL,
    EarningPatternId          int
)

-- INDEXES on Temp Tables
-- TomO's idea to add StatsFolderCnt to make indexes unique
CREATE INDEX Idx_FromDate ON #TempRSAEarndPremSplit (FromDate, StatsFolderCnt)

CREATE INDEX Idx_ToDate ON #TempRSAEarndPremSplit (ToDate, StatsFolderCnt )

CREATE INDEX Idx_dtCurrentPeriodEnd ON #TempRSAEarndPremSplit (dtCurrentPeriodEnd, StatsFolderCnt )

CREATE INDEX Idx_dt12MonthsAgo ON #TempRSAEarndPremSplit (dt12MonthsAgo, StatsFolderCnt )

CREATE INDEX Idx_dtLastYearEnd ON #TempRSAEarndPremSplit (dtLastYearEnd, StatsFolderCnt )

CREATE INDEX Idx_DatesInRange ON #TempRSAEarndPremSplit (DatesInRange, StatsFolderCnt )

CREATE INDEX Idx_PeriodRangeID ON #TempRSAEarndPremSplit (PeriodRangeID,  StatsFolderCnt )

-- CURRENT PERIOD
-- Add Premium Records
INSERT INTO #TempRSAEarndPremSplit
    SELECT	StatsFolderCnt,
--			NULL,
			ProductCode,
			ProductDesc,
			CommissionOrPremium,
			1,      -- Current Period
			'Current Period',
			RiskTypeCode,
			RiskTypeDescription,
			CASE EarningPatternId
			    WHEN 1 THEN GrossTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE GrossTotal
                        END,
			GrossTotal,

			CASE EarningPatternId
			    WHEN 1 THEN CoinsTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE CoinsTotal
                        END,
			CoinsTotal,

			CASE EarningPatternId
			    WHEN 1 THEN TreatyTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE TreatyTotal
                        END,
			TreatyTotal,

			CASE EarningPatternId
			    WHEN 1 THEN FacTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE FacTotal
                        END,
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
			NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
                        EarningPatternId
    FROM	#tempRSAEarndPrem
    WHERE	datediff(month, @dtCurrentPeriodEnd, ToDate) >= 0 AND 
			datediff(month, @dtCurrentPeriodEnd, FromDate) <= 0

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
    SELECT	StatsFolderCnt,
--			NULL,
			ProductCode,
			ProductDesc,
			CommissionOrPremium,
			2,      -- Current Year To Date
			'Current Year To Date',
			RiskTypeCode,
			RiskTypeDescription,
			CASE EarningPatternId
			    WHEN 1 THEN GrossTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE GrossTotal
                        END,
			GrossTotal,

			CASE EarningPatternId
			    WHEN 1 THEN CoinsTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE CoinsTotal
                        END,
			CoinsTotal,

			CASE EarningPatternId
			    WHEN 1 THEN TreatyTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE TreatyTotal
                        END,
			TreatyTotal,

			CASE EarningPatternId
			    WHEN 1 THEN FacTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE FacTotal
                        END,
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
			NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
                        EarningPatternId
    FROM	#tempRSAEarndPrem
    WHERE	datediff(month, @dtLastYearPeriodEndDate, ToDate) >= 1 AND 
			datediff(month, @dtCurrentPeriodEnd, FromDate) <= 0

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
    SELECT	StatsFolderCnt,
--			NULL,
			ProductCode,
			ProductDesc,
			CommissionOrPremium,
			3,      -- 12 Months To Date
			'12 Months To Date',
			RiskTypeCode,
			RiskTypeDescription,
			CASE EarningPatternId
			    WHEN 1 THEN GrossTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE GrossTotal
                        END,
			GrossTotal,

			CASE EarningPatternId
			    WHEN 1 THEN CoinsTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE CoinsTotal
                        END,
			CoinsTotal,

			CASE EarningPatternId
			    WHEN 1 THEN TreatyTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE TreatyTotal
                        END,
			TreatyTotal,

			CASE EarningPatternId
			    WHEN 1 THEN FacTotal/(DaysOfCoverTotal+IsMidnightRenewal)
			    ELSE FacTotal
                        END,
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
			NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
                        EarningPatternId
    FROM	#tempRSAEarndPrem
    WHERE	datediff(month, @dt12MonthPeriodEnd, ToDate) >= 1 AND 
			datediff(month, @dtCurrentPeriodEnd, FromDate) <= 0

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

	IF @iBranchID = 0 
	BEGIN
                      UPDATE #TempRSAEarndPremSplit
        SET CurrentPeriodStart = (SELECT period_end_Date
		     						FROM	 period
									WHERE  period_id = (SELECT TOP 1 period_id  - 1
														FROM   period
 														WHERE  period_end_date = dtCurrentPeriodEnd
														ORDER BY period_id DESC))
    END
	ELSE
	BEGIN 
                       UPDATE #TempRSAEarndPremSplit
		SET CurrentPeriodStart = (SELECT period_end_Date
									FROM	 period
									WHERE  period_id = (SELECT period_id  - 1
														FROM   period
														WHERE  period_end_date = dtCurrentPeriodEnd 
																AND company_id = @iBranchID))
	END 
-- BackDateExtra
UPDATE	#TempRSAEarndPremSplit
	SET		BackDateExtra = 0
	WHERE	PeriodRangeID = 1

UPDATE #TempRSAEarndPremSplit
    SET		BackDateExtra = datediff(day, FromDate, CurrentPeriodStart)
	WHERE	FromDate < CurrentPeriodStart
--  AND DocumentDate >= CurrentPeriodStart        -- Original
--  AND DateDiff(day, DocumentDate, CurrentPeriodStart) <= 0  -- WRONG !!
  AND DateDiff(day,  CurrentPeriodStart, DocumentDate) >= 0
  AND PeriodRangeID = 1

-- Populate the DatesInRange Parameter
-- Note:  DatesInRange is just a flag to avoid overwriting CalcDaysOfCover fields
-- **************** Current *********************
-- Neither Dates in Range Current
UPDATE #tempRSAEarndPremSplit
    SET		DatesInRange = 0,
			CalcDaysOfCoverCurrent = 0
	WHERE	ToDate < CurrentPeriodStart OR 
			FromDate > dtCurrentPeriodEnd AND 
			PeriodRangeID = 1

--BothDatesInRange Current
UPDATE #TempRSAEarndPremSplit
    SET		DatesInRange = 2,
			CalcDaysOfCoverCurrent = datediff(day, FromDate, ToDate)
	WHERE	FromDate > CurrentPeriodStart AND 
			ToDate < dtCurrentPeriodEnd AND 
			DatesInRange is NULL AND 
			PeriodRangeID = 1

-- BothDatesOutsideRange Current
UPDATE #TempRSAEarndPremSplit
   SET	DatesInRange = 1,
		CalcDaysOfCoverCurrent = datediff(day, CurrentPeriodStart, dtCurrentPeriodEnd)+ BackDateExtra + 1
--       datediff(day, FromDate, dtCurrentPeriodEnd) +            -- WRONG!!
WHERE	FromDate < CurrentPeriodStart AND 
		ToDate > dtCurrentPeriodEnd AND 
		DatesInRange is NULL AND 
		PeriodRangeID = 1

-- ToDate < dtCurrentPeriodEnd
UPDATE #TempRSAEarndPremSplit
	SET		DatesInRange = 3,
			CalcDaysOfCoverCurrent = datediff(day, CurrentPeriodStart, ToDate) + BackDateExtra + 1
	WHERE	ToDate < dtCurrentPeriodEnd AND 
			DatesInRange is NULL AND 
			PeriodRangeID = 1

-- Anything Else
UPDATE #TempRSAEarndPremSplit
	SET		DatesInRange = 5,
			CalcDaysOfCoverCurrent = Datediff(day, FromDate, dtCurrentPeriodEnd) + BackDateExtra + 1
 WHERE		DatesInRange is NULL AND 
			PeriodRangeID = 1

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
   SET 
       GrossCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(Gross,0) * CalcDaysOfCoverCurrent
                               ELSE isnull(Gross,0)
                           END,
       CoInsCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(Coinsurance,0) * CalcDaysOfCoverCurrent
                               ELSE isnull(Coinsurance,0)
                           END,
       TreatyCoverRounded = CASE EarningPatternId
                                WHEN 1 THEN isnull(Treaty,0) * CalcDaysOfCoverCurrent
                                ELSE isnull(Treaty,0)
                            END,
       FACCoverRounded = CASE EarningPatternId
                             WHEN 1 THEN isnull(Facultative,0) * CalcDaysOfCoverCurrent
                             ELSE isnull(Facultative,0)
                         END
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
    SET
       GrossCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(Gross,0) * CalcDaysOfCover12Month
                               ELSE isnull(Gross,0)
                           END,
       CoInsCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(Coinsurance,0) * CalcDaysOfCover12Month
                               ELSE isnull(Coinsurance,0)
                           END,
       TreatyCoverRounded = CASE EarningPatternId
                                WHEN 1 THEN isnull(Treaty,0) * CalcDaysOfCover12Month
                                ELSE isnull(Treaty,0)
                            END,
       FACCoverRounded = CASE EarningPatternId
                             WHEN 1 THEN isnull(Facultative,0) * CalcDaysOfCover12Month
                             ELSE isnull(Facultative,0)
                         END
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
    SET
       GrossCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(Gross,0) * CalcDaysOfCoverYearToDate
                               ELSE isnull(Gross,0)
                           END,
       CoInsCoverRounded = CASE EarningPatternId
                               WHEN 1 THEN isnull(Coinsurance,0) * CalcDaysOfCoverYearToDate
                               ELSE isnull(Coinsurance,0)
                           END,
       TreatyCoverRounded = CASE EarningPatternId
                                WHEN 1 THEN isnull(Treaty,0) * CalcDaysOfCoverYearToDate
                                ELSE isnull(Treaty,0)
                            END,
       FACCoverRounded = CASE EarningPatternId
                             WHEN 1 THEN isnull(Facultative,0) * CalcDaysOfCoverYearToDate
                             ELSE isnull(Facultative,0)
                         END
 WHERE PeriodRangeID = 2

-- Set the dependent fields
UPDATE #TempRSAEarndPremSplit
   SET NetCoverRounded = GrossCoverRounded - CoInsCoverRounded,
       RetainedCoverRounded = (GrossCoverRounded - CoInsCoverRounded) -
                              (TreatyCoverRounded + FACCoverRounded)
 WHERE PeriodRangeID = 2


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
            'DocumentRef'=NULL,
            'GrossCoverRounded'=SUM(GrossCoverRounded),
            'CoInsCoverRounded'=SUM(CoInsCoverRounded),
            'NetCoverRounded'=SUM(NetCoverRounded),
            'TreatyCoverRounded'=SUM(TreatyCoverRounded),
            'FACCoverRounded'=SUM(FACCoverRounded),
            'RetainedCoverRounded'=SUM(RetainedCoverRounded),
            dtCurrentPeriodEnd,
			'CurrencyCode' = @iso_code,
			'CurrencyDesc' = @description
        FROM #tempRSAEarndPremSplit
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
            dtCurrentPeriodEnd,
			'CurrencyCode' = @iso_code,
			'CurrencyDesc' = @description
        FROM #tempRSAEarndPremSplit
        GROUP BY  dtCurrentPeriodEnd, PeriodRangeID, PeriodRangeName, CommissionOrPRemium,  ProductDesc, RiskTypeDescription,
              ProductCode, DocumentRef, StatsFolderCnt
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
                dtCurrentPeriodEnd,
				'CurrencyCode' = @iso_code,
				'CurrencyDesc' = @description
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
                dtCurrentPeriodEnd,
				'CurrencyCode' = @iso_code,
				'CurrencyDesc' = @description
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
-- End of $Workfile: sp_Report_Earned_Premium.sql $

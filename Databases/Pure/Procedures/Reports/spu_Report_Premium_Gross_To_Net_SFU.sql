SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Premium_Gross_To_Net_SFU'
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** 17/08/2000
** Premium_Gross_To_Net.rpt
**
**********************************************************************************************************************************
** 06/08/2004	JT		Added MultiCurrency Feature
** 28/04/2003   JMK     remove double quotes
** 05/08/2003   AMB     Replace use of 'transaction_export_folder' with 'document' - TEF should not be used in SFU
** 15/11/02 Tom.brown	fixed:  Basis and Branch params
** 20/11/02	Tom.brown	Standardise parameters as Branch, Period End Date, <others> and Basis (Date/Period)
** 27/11/02	Tom.brown	Fix 12 periods ago, use minus 11 months instead of minus 1 year
** 26/09/2003	JMK		PN 3170 - retrieve class of business and product for grouping (to follow PMU format)
** 03/10/2003	JMK		PN 7157 - rework period dates and ids
**								- also roll sBasis option into one section to avoid duplicated statements
** 06/10/2003	JMK		PN 6915 - Add @ReportSection to allow printing each section separately
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Premium_Gross_To_Net_SFU
                @branch_id int,
                @PeriodDate varchar (20),
                @sBasis varchar(50),
                @ReportSection varchar(30),
                @TypeOfCurrency	Varchar(30), --Added for MultiCurency-Jitendra
                @GroupByCode	Varchar(255)
                
AS

SET NOCOUNT ON

/*-- @ReportSection = 'ALL'; 'Selected Period'; 'Selected Year'; '12 Months'
DECLARE @branch_id int,
        @PeriodDate varchar (20),
        @sBasis varchar(50),
        @ReportSection varchar(30)

SELECT  @branch_id = 0, @PeriodDate = 'Jul 31 2003',
@sBasis = 'Transaction Date',
--'Transaction Period'
@ReportSection = 'Selected Year'
--'ALL'
--'Selected Period'
--'Selected Year'
--'12 Months'
*/
SELECT @PeriodDate = @PeriodDate + ' 23:59:59'


DECLARE @period_end_date datetime, @dtSelectedPeriodEnd datetime, @dtPrevPeriodEnd datetime, 
		@dtYearStart datetime, @dtPrevYearEnd datetime, 
		@dt12PeriodsAgo datetime, @dt13PeriodsAgo datetime
DECLARE @SelectedPeriodID int, @YearStartPeriodID int, @12PeriodsAgoID int, @iBranchPeriod int

-- Always use Branch 1 period table
-- if anyone sets up different periods for different branches this will
-- need to be revisited and worked out for the branch
SELECT @iBranchPeriod = 1
-- convert string parameter to datetime
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)

-- Selected period values
SELECT @dtSelectedPeriodEnd = max(period_end_date)
FROM period
WHERE period_end_Date <= @period_end_date
AND company_id = @iBranchPeriod

SELECT @SelectedPeriodID = period_id 
FROM period
WHERE period_end_Date  = @dtSelectedPeriodEnd
AND company_id = @iBranchPeriod

-- Previous period values
SELECT @dtPrevPeriodEnd = max(period_end_date)
FROM Period
WHERE period_end_date < @period_end_date
AND company_id = @iBranchPeriod

-- *If current period is the first period set up
IF @dtPrevPeriodEnd IS NULL
BEGIN
    SELECT  @dtPrevPeriodEnd = dateadd(month, -1, @dtSelectedPeriodEnd)

END

-- year start period values
SELECT @dtYearStart = min(period_end_date)
FROM period
WHERE year_name = (SELECT year_name
                   FROM period
                   WHERE period_id = @SelectedPeriodID
                   AND company_id = @iBranchPeriod)
AND company_id = @iBranchPeriod 

SELECT @YearStartPeriodID = period_id
FROM Period
WHERE period_end_date = @dtYearStart
AND company_id = @iBranchPeriod 

-- previous year end values
SELECT @dtPrevYearEnd = max(period_end_date)
FROM period
WHERE period_end_date < @dtYearStart
AND company_id = @iBranchPeriod

-- *If there are no periods set up for the previous year
IF @dtPrevYearEnd IS NULL
BEGIN
    SELECT  @dtPrevYearEnd = dateadd(month, -1, @dtYearStart)

END

-- 12 period values
SELECT @12PeriodsAgoID = period_id, @dt12PeriodsAgo = period_end_date
FROM period
WHERE datediff(month, period_end_date, @period_end_date)= 11
AND company_id = @iBranchPeriod

SELECT @dt13PeriodsAgo = max(period_end_date)
FROM period
WHERE period_end_date < @dt12PeriodsAgo
AND company_id = @iBranchPeriod


-- *If there are less than 12 months of periods set up prior to the selected period
IF @12PeriodsAgoID IS NULL
BEGIN
    SELECT  @dt12PeriodsAgo = min(period_end_date)
	FROM period

	SELECT @12PeriodsAgoID = period_id
	FROM period
	WHERE period_end_date = @dt12PeriodsAgo
END

IF @dt13PeriodsAgo IS NULL
BEGIN
	SELECT @dt13PeriodsAgo = dateadd(month, -1, @dt12PeriodsAgo)
END
/*
SELECT @period_end_date '@period_end_date', 
@dtSelectedPeriodEnd '@dtSelectedPeriodEnd', 
@dtPrevPeriodEnd '@dtPrevPeriodEnd', 
@dtYearStart '@dtYearStart', 
@dtPrevYearEnd '@dtPrevYearEnd',
@dt12PeriodsAgo '@dt12PeriodsAgo',
@dt13PeriodsAgo '@dt13PeriodsAgo'

SELECT @SelectedPeriodID '@SelectedPeriodID', 
@YearStartPeriodID '@YearStartPeriodID', 
@12PeriodsAgoID '@12PeriodsAgoID'
*/

/*Get System Currency Details--jitendra*/
	declare @SystemCurrencyCode varchar(10)
	declare @SystemCurrencyDesc varchar(255)
    SELECT
    	@SystemCurrencyCode = c.iso_code,
    	@SystemCurrencyDesc = c.description
    FROM PMSystem pms
    JOIN currency c
    	ON c.currency_id = pms.currency_id
    WHERE pms.system_id = 1
/*end  Get System Currency*/

CREATE TABLE #tempPremGrossNet
(
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    CommissionOrPremium int NULL,           -- 0=premium, 1=commission
    PeriodRangeID int,                      -- 1=Current, 2=YTD, 3=12Months
    PeriodRangeName varchar(30),
    Agent varchar (100) NULL,
    COBCode varchar (10) NULL,
    COBDesc varchar (255) NULL,
    Gross decimal (19,4) NULL,
    Coinsurance decimal (19,4) NULL,
    Treaty decimal (19,4) NULL,
    Facultative decimal (19,4) NULL,
    TransDate datetime NULL,
    dtSelectedPeriodEnd datetime,
    PostingPeriodID int,
    SelectedPeriodID int,
    Source_id	int --Added for Currency-jitendra
  
)

-- ************* SELECTED PERIOD *****************

IF @ReportSection = 'ALL' OR @ReportSection = 'Selected Period'
BEGIN

-- SELECTED PERIOD PREMIUM
INSERT INTO #tempPremGrossNet
	SELECT 	sf.product_code,
		p.description,
		0,      -- Premium
		1,      -- Selected Period
		'Selected Period',
		(SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
		sd.class_of_business_code,
		cob.description,
		--**Added for Multicurrency***
		(SELECT 
			Case @TypeofCurrency 
			When 'System' THEN sd.this_premium_system
			When 'Base' THEN sd.this_premium_home 
			END
			WHERE sd.stats_detail_type = 'GRS'),
		(SELECT Case @TypeofCurrency 
			When 'System' THEN sd.this_premium_system
			When 'Base' THEN sd.this_premium_home 
			END WHERE sd.stats_detail_type = 'COI'),
		(SELECT Case @TypeofCurrency 
			When 'System' THEN sd.this_premium_system * -1 
			When 'Base' THEN sd.this_premium_home * -1  
			END WHERE sd.stats_detail_type = 'TTY'),
		(SELECT Case @TypeofCurrency 
			When 'System' THEN sd.this_premium_system * -1 
			When 'Base' THEN sd.this_premium_home * -1  
			END WHERE sd.stats_detail_type = 'FAC'),
		--**Added for Multicurrency END***
		sf.transaction_date,
		@dtSelectedPeriodEnd,
		sf.posting_period_number,
		@SelectedPeriodID,sf.source_id
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
        JOIN Class_Of_Business cob       ON sd.class_of_business_id = cob.class_of_business_id
        -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
        INNER JOIN Document AS doc 
            ON doc.document_ref = sf.document_ref
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND (
			@sBasis = 'Transaction Date' AND  (sf.document_date > @dtPrevPeriodEnd AND sf.document_date <= @dtSelectedPeriodEnd)
			OR
	    	@sBasis = 'Transaction Period' AND (sf.posting_period_number = @SelectedPeriodID)
			)
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
		AND (
			@branch_id = 0
			OR    
			(@branch_id <> 0 AND sf.source_id = @branch_id )
			)

-- SELECTED PERIOD COMMISSION
INSERT INTO #tempPremGrossNet
	SELECT sf.product_code,
		p.description,
		1,      -- Commission
		1,      -- Selected Period
		'Selected Period',
		(SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
		sd.class_of_business_code,
		cob.description,
		--**Added for Multicurrency***
		(SELECT CASE @Typeofcurrency 
					WHEN 'Base'  THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System'  THEN isnull(sd.lead_commission_value_system,0) 
				END
					+
				CASE @Typeofcurrency 
					WHEN 'Base'  THEN isnull(sd.sub_commission_value_home,0) 
					WHEN 'System'  THEN isnull(sd.sub_commission_value_system,0)
				END
		 WHERE sd.stats_detail_type = 'GRS' OR sd.stats_detail_type = 'SUB'),
		(SELECT CASE @Typeofcurrency 
					WHEN 'Base'  THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System'  THEN isnull(sd.lead_commission_value_system,0) 
				END
				+
				CASE @Typeofcurrency 
					WHEN 'Base'  THEN isnull(sd.sub_commission_value_home,0) 
					WHEN 'System'  THEN isnull(sd.sub_commission_value_system,0)
				END
		 WHERE sd.stats_detail_type = 'COI'),
	
	(SELECT CASE @Typeofcurrency 
				WHEN 'Base'  THEN isnull(sd.lead_commission_value_home,0) 
				WHEN 'System'  THEN isnull(sd.lead_commission_value_system,0) 
			END
				+
			CASE @Typeofcurrency 
				WHEN 'Base'  THEN isnull(sd.sub_commission_value_home,0) 
				WHEN 'System'  THEN isnull(sd.sub_commission_value_system,0) 
			END
		 WHERE sd.stats_detail_type = 'TTY')*-1,
		(SELECT CASE @Typeofcurrency 
					WHEN 'Base'  THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System'  THEN isnull(sd.lead_commission_value_system,0) 
				END
				+
				CASE @Typeofcurrency 
					WHEN 'Base'  THEN isnull(sd.sub_commission_value_home,0)  
					WHEN 'System'  THEN isnull(sd.sub_commission_value_system,0) 
				END
		 WHERE sd.stats_detail_type = 'FAC')*-1 ,
		 --**Added for Multicurrency END ***
		sf.transaction_date,
		@dtSelectedPeriodEnd,
		sf.posting_period_number,
		@SelectedPeriodID,sf.source_id
	FROM Stats_Folder sf
	JOIN Product p          ON sf.product_id = p.product_id
	JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
		AND sd.stats_detail_type IN ('SUB','GRS','COI','TTY','FAC')
	JOIN Class_Of_Business cob       ON sd.class_of_business_id = cob.class_of_business_id
	-- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
	INNER JOIN Document AS doc 
		ON doc.document_ref = sf.document_ref
	WHERE isnull(sd.lead_commission_value_home,0) +
		isnull(sd.sub_commission_value_home,0) <> 0
	AND (
		@sBasis = 'Transaction Date' AND  (sf.document_date > @dtPrevPeriodEnd AND sf.document_date <= @dtSelectedPeriodEnd)
		OR
		@sBasis = 'Transaction Period' AND (sf.posting_period_number = @SelectedPeriodID)
		)
	AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
	AND (
		@branch_id = 0
		OR    
		(@branch_id <> 0 AND sf.source_id = @branch_id )
		)
END
-- END SELECTED PERIOD


-- ************* SELECTED YEAR *****************

IF @ReportSection = 'ALL' OR @ReportSection = 'Selected Year'
BEGIN

-- SELECTED YEAR PREMIUM
INSERT INTO #tempPremGrossNet
	SELECT sf.product_code,
		p.description,
		0,      -- Premium
		2,      -- Selected Year
		'Selected Year',
		(SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
		sd.class_of_business_code,
		cob.description,
		--**Added for Multicurrency***
		(SELECT CASE @TypeOfCurrency  
					WHEN 'Base' THEN sd.this_premium_home 
					WHEN 'System' THEN sd.this_premium_System
				END
					WHERE sd.stats_detail_type = 'GRS'),
		(SELECT CASE @TypeOfCurrency  
					WHEN 'Base' THEN sd.this_premium_home 
					WHEN 'System' THEN sd.this_premium_System
				END
				WHERE sd.stats_detail_type = 'COI'),
		(SELECT CASE @TypeOfCurrency  
					WHEN 'Base' THEN sd.this_premium_home * -1 
					WHEN 'System' THEN sd.this_premium_System * -1 
				END WHERE sd.stats_detail_type = 'TTY'),
				
		(SELECT CASE @TypeOfCurrency  
					WHEN 'Base' THEN sd.this_premium_home * -1 
					WHEN 'System' THEN sd.this_premium_System * -1 
				END
			WHERE sd.stats_detail_type = 'FAC'),
			--**Added for Multicurrency END***
		sf.transaction_date,
		@dtSelectedPeriodEnd,
		sf.posting_period_number,
		@SelectedPeriodID,sf.source_id
	FROM Stats_Folder sf
	JOIN Product p          ON sf.product_id = p.product_id
	JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
		AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
	JOIN Class_Of_Business cob       ON sd.class_of_business_id = cob.class_of_business_id
	-- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
	INNER JOIN Document AS doc 
		ON doc.document_ref = sf.document_ref
	WHERE isnull(sd.this_premium_home,0) <> 0
	AND (
		@sBasis = 'Transaction Date' AND  (sf.document_date > @dtPrevYearEnd AND sf.document_date <= @dtSelectedPeriodEnd)
		OR
		@sBasis = 'Transaction Period' AND (sf.posting_period_number BETWEEN  @YearStartPeriodID AND @SelectedPeriodID)
		)
	AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
	AND (
		@branch_id = 0
		OR    
		(@branch_id <> 0 AND sf.source_id = @branch_id )
		)


-- SELECTED YEAR COMMISSION
INSERT INTO #tempPremGrossNet
	SELECT sf.product_code,
		p.description,
		1,      -- Commission
		2,      -- Selected Year
		'Selected Year',
		(SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
		sd.class_of_business_code,
		cob.description,
		--**Added for Multicurrency***
		(SELECT CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
				END
			+
				CASE @TypeOfCurrency 
					WHEN 'Base' THEN 	isnull(sd.sub_commission_value_home,0)
					WHEN 'System' THEN 	isnull(sd.sub_commission_value_system,0)
				END
		 WHERE sd.stats_detail_type = 'GRS' or sd.stats_detail_type = 'SUB'),
		 
		(SELECT CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
				END
			+
				CASE @TypeOfCurrency 
					WHEN 'Base' THEN 	isnull(sd.sub_commission_value_home,0)
					WHEN 'System' THEN 	isnull(sd.sub_commission_value_system,0)
				END
		 WHERE sd.stats_detail_type = 'COI'),
		 
		(SELECT CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
				END
				+
				CASE @TypeOfCurrency 
					When 'Base' THEN isnull(sd.sub_commission_value_home,0) 
					WHEN 'System' THEN	isnull(sd.sub_commission_value_system,0) 
				END
		 WHERE sd.stats_detail_type = 'TTY')* -1,
		 
		(SELECT CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
				END
				+
				CASE @TypeOfCurrency 
					When 'Base' THEN isnull(sd.sub_commission_value_home,0) 
					WHEN 'System' THEN	isnull(sd.sub_commission_value_system,0) 
				END
		 WHERE sd.stats_detail_type = 'FAC')* -1,
		 --**Added for Multicurrency**END*
		sf.transaction_date,
		@dtSelectedPeriodEnd,
		sf.posting_period_number,
		@SelectedPeriodID,sf.source_id
	FROM Stats_Folder sf
	JOIN Product p          ON sf.product_id = p.product_id
	JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
		AND sd.stats_detail_type IN ('SUB','GRS','COI','TTY','FAC')
	JOIN Class_Of_Business cob       ON sd.class_of_business_id = cob.class_of_business_id
	-- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
	INNER JOIN Document AS doc 
		ON doc.document_ref = sf.document_ref
	WHERE isnull(sd.lead_commission_value_home,0) +
		isnull(sd.sub_commission_value_home,0) <> 0
	AND (
		@sBasis = 'Transaction Date' AND  (sf.document_date > @dtPrevYearEnd AND sf.document_date <= @dtSelectedPeriodEnd)
		OR
		@sBasis = 'Transaction Period' AND (sf.posting_period_number BETWEEN  @YearStartPeriodID AND @SelectedPeriodID)
		)
	AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
	AND (
		@branch_id = 0
		OR    
		(@branch_id <> 0 AND sf.source_id = @branch_id )
		)

END
-- END SELECTED YEAR


-- ************* 12 MONTHS *****************
IF @ReportSection = 'ALL' OR @ReportSection = '12 Months'
BEGIN
-- 12 MONTHS PREMIUM
INSERT INTO #tempPremGrossNet
	SELECT sf.product_code,
		p.description,
		0,      -- Premium
		3,      -- 12 Periods
		'12 Periods',
		(SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
		sd.class_of_business_code,
		cob.description,
		--**Added for Multicurrency***
	(SELECT CASE @TypeOfCurrency  
					WHEN 'Base' THEN sd.this_premium_home 
					WHEN 'System' THEN sd.this_premium_System
				END
					WHERE sd.stats_detail_type = 'GRS'),
		(SELECT CASE @TypeOfCurrency  
					WHEN 'Base' THEN sd.this_premium_home 
					WHEN 'System' THEN sd.this_premium_System
				END
				WHERE sd.stats_detail_type = 'COI'),
		(SELECT CASE @TypeOfCurrency  
					WHEN 'Base' THEN sd.this_premium_home * -1 
					WHEN 'System' THEN sd.this_premium_System * -1 
				END WHERE sd.stats_detail_type = 'TTY'),
				
		(SELECT CASE @TypeOfCurrency  
					WHEN 'Base' THEN sd.this_premium_home * -1 
					WHEN 'System' THEN sd.this_premium_System * -1 
				END
			WHERE sd.stats_detail_type = 'FAC'),
			--**Added for Multicurrency**END*
		sf.transaction_date,
		@dtSelectedPeriodEnd,
		sf.posting_period_number,
		@SelectedPeriodID,sf.source_id
	FROM Stats_Folder sf
	JOIN Product p          ON sf.product_id = p.product_id
	JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
		AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
	JOIN Class_Of_Business cob       ON sd.class_of_business_id = cob.class_of_business_id
	-- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
	INNER JOIN Document AS doc 
		ON doc.document_ref = sf.document_ref
	WHERE isnull(sd.this_premium_home,0) <> 0
	AND (
		@sBasis = 'Transaction Date' AND  (sf.document_date > @dt12PeriodsAgo AND sf.document_date <= @dtSelectedPeriodEnd)
		OR
		@sBasis = 'Transaction Period' AND (sf.posting_period_number BETWEEN @12PeriodsAgoID AND @SelectedPeriodID)
		)
	AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
	AND (
		@branch_id = 0
		OR    
		(@branch_id <> 0 AND sf.source_id = @branch_id )
		)


-- 12 MONTHS COMMISSION
INSERT INTO #tempPremGrossNet
	SELECT sf.product_code,
		p.description,
		1,      -- Commission
		3,      -- 12 Periods
		'12 Periods',
		(SELECT isnull(resolved_name, ' Direct') FROM Party WHERE shortname = sf.agent_shortname),
		sd.class_of_business_code,
		cob.description,
		--**Added for Multicurrency***
	(SELECT CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
				END
			+
				CASE @TypeOfCurrency 
					WHEN 'Base' THEN 	isnull(sd.sub_commission_value_home,0)
					WHEN 'System' THEN 	isnull(sd.sub_commission_value_system,0)
				END
		 WHERE sd.stats_detail_type = 'GRS' OR sd.stats_detail_type = 'SUB'),
		 
		(SELECT CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
				END
			+
				CASE @TypeOfCurrency 
					WHEN 'Base' THEN 	isnull(sd.sub_commission_value_home,0)
					WHEN 'System' THEN 	isnull(sd.sub_commission_value_system,0)
				END
		 WHERE sd.stats_detail_type = 'COI'),
		 
		(SELECT CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
				END
				+
				CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.sub_commission_value_home,0) 
					WHEN 'System' THEN	isnull(sd.sub_commission_value_system,0) 
				END
		 WHERE sd.stats_detail_type = 'TTY')* -1,
		 
		(SELECT CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.lead_commission_value_home,0) 
					WHEN 'System' THEN isnull(sd.lead_commission_value_system,0) 
				END
				+
				CASE @TypeOfCurrency 
					WHEN 'Base' THEN isnull(sd.sub_commission_value_home,0) 
					WHEN 'System' THEN	isnull(sd.sub_commission_value_system,0) 
				END
		 WHERE sd.stats_detail_type = 'FAC')* -1,
		 --**Added for Multicurrency**END*
		sf.transaction_date,
		@dtSelectedPeriodEnd,
		sf.posting_period_number,
		@SelectedPeriodID,sf.source_id
	FROM Stats_Folder sf
	JOIN Product p          ON sf.product_id = p.product_id
	JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
		AND sd.stats_detail_type IN ('SUB','GRS','COI','TTY','FAC')
	JOIN Class_Of_Business cob       ON sd.class_of_business_id = cob.class_of_business_id
	-- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
	INNER JOIN Document AS doc 
		ON doc.document_ref = sf.document_ref
	WHERE isnull(sd.lead_commission_value_home,0) +
		isnull(sd.sub_commission_value_home,0) <> 0
	AND (
		@sBasis = 'Transaction Date' AND  (sf.document_date > @dt12PeriodsAgo AND sf.document_date <= @dtSelectedPeriodEnd)
		OR
		@sBasis = 'Transaction Period' AND (sf.posting_period_number BETWEEN @12PeriodsAgoID AND @SelectedPeriodID)
		)
	AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
	AND (
		@branch_id = 0
		OR    
		(@branch_id <> 0 AND sf.source_id = @branch_id )
		)

END
-- END 12 MONTHS

UPDATE #tempPremGrossNet
	SET Agent = ' Direct'
	FROM Party
	WHERE isnull(Agent,'') = ''

SET NOCOUNT OFF

SELECT ProductCode,
    ProductDesc ,
    CommissionOrPremium,           -- 0=premium, 1=commission
    PeriodRangeID,                      -- 1=Current, 2=YTD, 3=12Months
    PeriodRangeName,
    Agent,
    COBCode,
    COBDesc,
    sum(isnull(Gross,0)) as Gross,
    sum(isnull(Coinsurance,0))as Coinsurance,
    Sum(isnull(Treaty,0)) as Treaty,
    Sum(isnull(Facultative,0)) as Facultative,
    TransDate,
    dtSelectedPeriodEnd,
    PostingPeriodID,
    SelectedPeriodID,
    Source_id	 
  INTO #tempPremGrossNet1 	
  FROM #tempPremGrossNet 
  GROUP BY   Source_id,PeriodRangeID,PeriodRangeName,SelectedPeriodID,CommissionOrPremium,Agent,COBCode,COBDesc,ProductCode,ProductDesc,
    dtSelectedPeriodEnd,PostingPeriodID,TransDate
--  HAVING sum(isnull(Gross,0))>0 or sum(isnull(Coinsurance,0))>0 or Sum(isnull(Treaty,0))>0 or Sum(isnull(Facultative,0))>0

DROP TABLE #tempPremGrossNet


SELECT ProductCode,
    ProductDesc,
    CommissionOrPremium,
    PeriodRangeID,
    PeriodRangeName,
    Agent,
    COBCode,
    COBDesc,
    Gross,
    Coinsurance,
    Treaty,
    Facultative,
    TransDate,
    dtSelectedPeriodEnd,
    PostingPeriodID,
    --**Added for Multicurrency***
    SelectedPeriodID,S.Code CompanyCode,S.description CompanyDesc
    ,Case @typeOfCurrency 
    	WHEN 'Base'  THEN CB.Code 
    	WHEN 'System'  THEN @SystemCurrencycode END CurrencyCode,
    Case @TypeOfCurrency 
    	WHEN 'Base' THEN CB.description 
    	WHEN 'System' THEN @systemCurrencyDesc
    END CurrecnyDesc,
    Case @GroupByCode 
    	WHEN 'Branch' THEN S.Code
    	WHEN 'Branch And Currency' THEN S.Code
    else ''
    END GroupBycode
    --**Added for Multicurrency**END*
FROM #tempPremGrossNet1 TP
INNER JOIN Source S ON S.source_id = TP.Source_id
INNER JOIN Currency CB ON CB.Currency_id = S.base_Currency_id

DROP TABLE #tempPremGrossNet1

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Summary_of_Premium_SFU'
GO

/****** Object:  Stored Procedure dbo.sp_Report_Summary_of_Premium    Script Date: 16/10/00 12:26:05 ******/
/**********************************************************************************************************************************
** Created by Jude Killip
** 04/08/2000
** RSA Reports - Summary_Of_Premium.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 24/10/2000 JMK       - Update to use DB
** 25/10/2000 JMK       - silly date condition error in last month details
** 15/01/2001 JMK       - use document_date, amend date criteria (match Net Premium)
** 05/05/2001 JMK       - base on Period dates **make sure Orion_For_Broking named correctly**
** 12/06/2001 JMK       - adjust Period
** 03/08/2001 JMK       - rewrite: use Stats NOT Orion
** 05/09/2001 JMK       - add Stats description
** 28/05/2003 Jon Kemp  - Branch parameter added
** 10/06/2003 Jon Kemp	- changed description of report to work on risk rather than product.
** 06/10/2004	JT 		- for display the base curreny
** 15/01/2008	RP 		- Use parameter for period end

***********************************************************************************************************************************/

CREATE  PROCEDURE spu_Report_Summary_of_Premium_SFU
	@PeriodDate VARCHAR(255),
	@branch_id INT,
	@TypeOfCurrency VARCHAR(15)
AS

SET NOCOUNT ON

DECLARE @ibranchid INT
DECLARE @SelectedPeriodID INT
DECLARE @SelectedPeriodYearName VARCHAR(20)
DECLARE @SelectedPeriodEndDate DATETIME
DECLARE @SelectedYearStartPeriodID INT
DECLARE @LastYearPeriodEndDate DATETIME

DECLARE @RatesCompanyID INT
DECLARE @system_currency_id INT
DECLARE @TypeOfRates TINYINT
DECLARE @CurrencyCode VARCHAR(10)
DECLARE @CurrencyDesc VARCHAR(255)

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
WHERE system_id = 1

IF @TypeOfCurrency = 'System'
BEGIN
	SELECT
		@CurrencyCode = c.iso_code,
		@CurrencyDesc = c.description
	FROM currency c
	WHERE c.currency_id = @system_currency_id
END

IF @branch_id IS NULL
BEGIN
	SELECT @iBranchID = 0
END
ELSE
BEGIN
	SELECT @iBranchID = @branch_id
END

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'

SELECT @SelectedPeriodEndDate = CONVERT (DATETIME, @PeriodDate)

SELECT 
	@SelectedPeriodID = period_id,
	@SelectedPeriodYearName = year_name
FROM Period
WHERE period_end_date = @SelectedPeriodEndDate
--AND (company_id = @ibranchID OR (@iBranchID = 0 AND company_id = 1))

SELECT 
	@SelectedYearStartPeriodID = MIN(period_id)
FROM
	Period
WHERE 	(company_id = @ibranchID OR (@iBranchID = 0 AND company_id = 1))
AND	year_name = @SelectedPeriodYearName
	
SELECT
	@LastYearPeriodEndDate = MAX(period_end_date)
FROM
	Period
WHERE 	(company_id = @ibranchID OR (@iBranchID = 0 AND company_id = 1))
AND	period_id < @SelectedYearStartPeriodID

CREATE TABLE #tempRSAPremSum
(
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    SectionID int NULL,
    SectionName varchar (15) NULL,
    StatsType varchar (3) NULL,
    StatsTypeDesc varchar (30) NULL,
    ShortName varchar (30) NULL,
    TPAmount decimal (19,4) NULL,
    BFAmount decimal (19,4) NULL,
    TransDate datetime NULL,
    dtCurrentPeriodEnd datetime,
    dtLastYearEnd datetime,
    PostingPeriodID int,
    CurrentPeriodID int,
    risktypecode varchar(10) NULL,
    risktypedescription  varchar (255) NULL,
    CurrencyCode VARCHAR(10),
    CurrencyDesc VARCHAR(255)
)


INSERT INTO #tempRSAPremSum
SELECT
	sf.product_code,
	p.description,
	0,
	'Premium',
	sd.stats_detail_type,
	CASE sd.stats_detail_type
		WHEN 'GRS' THEN 'Gross'
		WHEN 'FAC' THEN 'Facultative'
		WHEN 'TTY' THEN 'Treaty'
		WHEN 'COI' THEN 'Coinsurance'
	END,
	ISNULL(agent_shortname, insurance_holder_shortname),
	CASE @TypeOfCurrency
	    WHEN 'Base' THEN ISNULL(ROUND(sd.this_premium_home,2),0)
		WHEN 'System' THEN ISNULL(ROUND(sd.this_premium_system,2),0)
	END,
	NULL,
	sf.transaction_date,
	@SelectedPeriodEndDate,
	@LastYearPeriodEndDate,
	sf.posting_period_number,
	@SelectedPeriodID,
	sd.risk_type_code,
	rt.description,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Code
		WHEN 'System' THEN @CurrencyCode
	END,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Description
		WHEN 'System' THEN @CurrencyDesc
	END
FROM Stats_Folder sf
JOIN Product p
	ON sf.product_id = p.product_id
JOIN Stats_Detail sd
	ON sf.stats_folder_cnt = sd.stats_folder_cnt
	AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
JOIN Risk_Type rt
	ON sd.risk_type_id = rt.risk_type_id
JOIN Currency CB
	ON sd.Currency_Code= CB.Code
WHERE ISNULL(sd.this_premium_home,0) <> 0
AND sf.transaction_type_code NOT LIKE ('C_%')
AND sf.posting_period_number = @SelectedPeriodID
AND (@iBranchID = 0 OR (@iBranchID <> 0 AND sf.source_id = @iBranchID))

INSERT INTO #tempRSAPremSum
SELECT
	sf.product_code,
	p.description,
	1,
	'Commission',
	sd.stats_detail_type,
	CASE sd.stats_detail_type
		WHEN 'GRS' THEN 'Gross'
		WHEN 'FAC' THEN 'Facultative'
		WHEN 'TTY' THEN 'Treaty'
		WHEN 'COI' THEN 'Coinsurance'
	END,
	ISNULL(agent_shortname, insurance_holder_shortname),
	CASE @TypeOfCurrency
		WHEN 'Base' THEN ROUND(ISNULL(sd.lead_commission_value_home,0),2) + ROUND(ISNULL(sd.sub_commission_value_home,0),2)
		WHEN 'System' THEN ROUND(ISNULL(sd.lead_commission_value_system,0),2) + ROUND(ISNULL(sd.sub_commission_value_system,0),2)
	END,
	NULL,
	sf.transaction_date,
	@SelectedPeriodEndDate,
	@LastYearPeriodEndDate,
	sf.posting_period_number,
	@SelectedPeriodID,
	sd.risk_type_code,
	rt.description,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Code
		WHEN 'System' THEN @CurrencyCode
	END,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Description
		WHEN 'System' THEN @CurrencyDesc
	END
FROM Stats_Folder sf
JOIN Product p
	ON sf.product_id = p.product_id
JOIN Stats_Detail sd
	ON sf.stats_folder_cnt = sd.stats_folder_cnt
	AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
JOIN Risk_Type rt
	ON sd.risk_type_id = rt.risk_type_id
JOIN insurance_file i
	ON i.insurance_file_cnt = sf.insurance_file_cnt
JOIN Currency CB
	ON sd.Currency_Code= CB.Code
WHERE ISNULL(sd.lead_commission_value_home,0) + ISNULL(sd.sub_commission_value_home,0) <> 0
AND sf.posting_period_number = @SelectedPeriodID
AND sf.transaction_type_code NOT LIKE ('C_%')

INSERT INTO #tempRSAPremSum
SELECT
	sf.product_code,
	p.description,
	3,
	'Claims Paid',
	sd.stats_detail_type,
	CASE sd.stats_detail_type
		WHEN 'GRS' THEN 'Gross'
		WHEN 'FAC' THEN 'Facultative'
		WHEN 'TTY' THEN 'Treaty'
		WHEN 'COI' THEN 'Coinsurance'
	END,
	ISNULL(agent_shortname, insurance_holder_shortname),
	CASE @TypeOfCurrency
	    WHEN 'Base' THEN ISNULL(ROUND(sd.this_premium_home,2),0)
		WHEN 'System' THEN ISNULL(ROUND(sd.this_premium_system,2),0)
	END,
	NULL,
	sf.transaction_date,
	@SelectedPeriodEndDate,
	@LastYearPeriodEndDate,
	sf.posting_period_number,
	@SelectedPeriodID,
	sd.risk_type_code,
	rt.description,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Code
		WHEN 'System' THEN @CurrencyCode
	END,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Description
		WHEN 'System' THEN @CurrencyDesc
	END
FROM Stats_Folder sf
JOIN Product p
	ON sf.product_id = p.product_id
JOIN Stats_Detail sd
	ON sf.stats_folder_cnt = sd.stats_folder_cnt
	AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
JOIN Risk_Type rt
	ON sd.risk_type_id = rt.risk_type_id
JOIN insurance_file i
	ON i.insurance_file_cnt = sf.insurance_file_cnt
JOIN Currency CB
	ON sd.Currency_Code= CB.Code
WHERE ISNULL(sd.this_premium_home,0) <> 0
AND sf.posting_period_number = @SelectedPeriodID
AND sf.transaction_type_code = ('C_CP')

INSERT INTO #tempRSAPremSum
SELECT
	sf.product_code,
	p.description,
	0,
	'Premium',
	sd.stats_detail_type,
	CASE sd.stats_detail_type
		WHEN 'GRS' THEN 'Gross'
		WHEN 'FAC' THEN 'Facultative'
		WHEN 'TTY' THEN 'Treaty'
		WHEN 'COI' THEN 'Coinsurance'
	END,
	ISNULL(agent_shortname, insurance_holder_shortname),
	NULL,
	CASE @TypeOfCurrency
	   WHEN 'Base' THEN ISNULL(ROUND(sd.this_premium_home,2),0)
		WHEN 'System' THEN ISNULL(ROUND(sd.this_premium_system,2),0)
	END,
	sf.transaction_date,
	@SelectedPeriodEndDate,
	@LastYearPeriodEndDate,
	sf.posting_period_number,
	@SelectedPeriodID,
	sd.risk_type_code,
	rt.description,
	Case @TypeOfCurrency
			WHEN 'Base' THEN CB.Code
			WHEN 'System' THEN @CurrencyCode
	END,
	Case @TypeOfCurrency
			WHEN 'Base' THEN CB.Description
			WHEN 'System' THEN @CurrencyDesc
	END
FROM Stats_Folder sf
JOIN Product p
	ON sf.product_id = p.product_id
JOIN Stats_Detail sd
	ON sf.stats_folder_cnt = sd.stats_folder_cnt
	AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
JOIN Risk_Type rt
	ON sd.risk_type_id = rt.risk_type_id
JOIN insurance_file i
	ON i.insurance_file_cnt = sf.insurance_file_cnt
JOIN Currency CB
	ON sd.Currency_Code= CB.Code
WHERE ISNULL(sd.this_premium_home,0) <> 0
AND sf.posting_period_number BETWEEN  @SelectedYearStartPeriodID AND @SelectedPeriodID -1
AND sf.transaction_type_code NOT LIKE ('C_%')

INSERT INTO #tempRSAPremSum
SELECT
	sf.product_code,
	p.description,
	1,
	'Commission',
	sd.stats_detail_type,
	CASE sd.stats_detail_type
		WHEN 'GRS' THEN 'Gross'
		WHEN 'FAC' THEN 'Facultative'
		WHEN 'TTY' THEN 'Treaty'
		WHEN 'COI' THEN 'Coinsurance'
	END,
	ISNULL(agent_shortname, insurance_holder_shortname),
	NULL,
	CASE @TypeOfCurrency
		WHEN 'Base' THEN ROUND(ISNULL(sd.lead_commission_value_home,0),2) + ROUND(ISNULL(sd.sub_commission_value_home,0),2)
		WHEN 'System' THEN ROUND(ISNULL(sd.lead_commission_value_system,0),2) + ROUND(ISNULL(sd.sub_commission_value_system,0),2)
	END,
	sf.transaction_date,
	@SelectedPeriodEndDate,
	@LastYearPeriodEndDate,
	sf.posting_period_number,
	@SelectedPeriodID,
	sd.risk_type_code,
	rt.description,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Code
		WHEN 'System' THEN @CurrencyCode
	END,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Description
		WHEN 'System' THEN @CurrencyDesc
	END
FROM Stats_Folder sf
JOIN Product p
	ON sf.product_id = p.product_id
JOIN Stats_Detail sd
	ON sf.stats_folder_cnt = sd.stats_folder_cnt
	AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
JOIN Risk_Type rt
	ON sd.risk_type_id = rt.risk_type_id
JOIN insurance_file i
	ON i.insurance_file_cnt = sf.insurance_file_cnt
JOIN Currency CB
	ON CB.code= SD.Currency_code

WHERE ISNULL(sd.lead_commission_value_home,0) +	ISNULL(sd.sub_commission_value_home,0) <> 0
AND sf.posting_period_number BETWEEN  @SelectedYearStartPeriodID AND @SelectedPeriodID -1
AND sf.transaction_type_code NOT LIKE ('C_%')

INSERT INTO #tempRSAPremSum
SELECT
	sf.product_code,
	p.description,
	3,
	'Claims Paid',
	sd.stats_detail_type,
	CASE sd.stats_detail_type
		WHEN 'GRS' THEN 'Gross'
		WHEN 'FAC' THEN 'Facultative'
		WHEN 'TTY' THEN 'Treaty'
		WHEN 'COI' THEN 'Coinsurance'
	END,
	ISNULL(agent_shortname, insurance_holder_shortname),
	NULL,
	CASE @TypeOfCurrency
		WHEN 'Base' THEN ISNULL(ROUND(sd.this_premium_home,2),0)
		WHEN 'System' THEN ISNULL(ROUND(sd.this_premium_system,2),0)
	END,
	sf.transaction_date,
	@SelectedPeriodEndDate,
	@LastYearPeriodEndDate,
	sf.posting_period_number,
	@SelectedPeriodID,
	sd.risk_type_code,
	rt.description,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Code
		WHEN 'System' THEN @CurrencyCode
	END,
	Case @TypeOfCurrency
		WHEN 'Base' THEN CB.Description
		WHEN 'System' THEN @CurrencyDesc
	END
FROM Stats_Folder sf
JOIN Product p
	ON sf.product_id = p.product_id
JOIN Stats_Detail sd
	ON sf.stats_folder_cnt = sd.stats_folder_cnt
	AND sd.stats_detail_type IN ('GRS','COI','TTY','FAC')
JOIN Risk_Type rt
	ON sd.risk_type_id = rt.risk_type_id
JOIN insurance_file i
	ON i.insurance_file_cnt = sf.insurance_file_cnt
JOIN Currency CB
	ON sd.Currency_Code= CB.Code
WHERE ISNULL(sd.this_premium_home,0) <> 0
AND sf.posting_period_number BETWEEN  @SelectedYearStartPeriodID AND @SelectedPeriodID -1
AND sf.transaction_type_code = 'C_CP'

SET NOCOUNT OFF

UPDATE #tempRSAPremSum    
  SET TPAmount= ISNULL(TPAmount,0),  
    BFAmount= ISNULL(BFAmount,0) 
SELECT *
FROM #tempRSAPremSum

DROP TABLE #tempRSAPremSum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


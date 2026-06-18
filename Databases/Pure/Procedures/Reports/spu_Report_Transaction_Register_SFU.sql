/***
Modification JT	25-10-2004	Changes merged from 1.8.6 SR20.2
*/
EXECUTE DDLDropProcedure 'spu_Report_Transaction_Register_SFU'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_Report_Transaction_Register_SFU
    @PeriodDate VARCHAR(20),
    @branch_id INT,
    @TypeOfCurrency VARCHAR(15),
    @GroupBy VARCHAR(20)
AS

SET NOCOUNT ON

DECLARE @iBranchid INT
DECLARE @CompanyCode VARCHAR(10)
DECLARE @CompanyDesc VARCHAR(255)
DECLARE @SystemCurrencyCode VARCHAR(10)
DECLARE @SystemCurrencyDesc VARCHAR(255)
DECLARE @BaseCurrencyCode VARCHAR(10)
DECLARE @BaseCurrencyDesc VARCHAR(255)

IF @branch_id IS NULL
	SELECT @iBranchID = 0
ELSE
	SELECT @iBranchID = @branch_id

/*Get System Currency Details*/
SELECT
	@SystemCurrencyCode = c.iso_code,
	@SystemCurrencyDesc = c.description
FROM PMSystem pms
JOIN currency c
	ON c.currency_id = pms.currency_id
WHERE pms.system_id = 1

CREATE TABLE #tempTransReg
    (
    TransactionID            int,
    DocumentID               int,
    AccountDate              datetime NULL,
    InsuranceRef             varchar(30) NULL,
    GrossAmount              decimal(19,4) NULL,
    CommissionAmount  decimal(19,4) NULL,
    Amount                   decimal(19,4) NULL,
    DocumentDate             datetime NULL,
    DocumentRef              varchar(25) NULL,
    DocumentType             varchar(255) NULL,
    AccountCode              varchar(30) NULL,
    AccountName              varchar(60) NULL,
    LedgerCode               varchar(2) NULL,
    LedgerName               varchar(30) NULL,
    dtThisPeriodEnd          datetime NULL,
    InsFileCnt               int NULL,
    Product                  varchar(255) NULL,
    ProductCode              varchar(10) NULL,
    BusType                  varchar(10) NULL,
    ExportStatus             char(1) NULL,
    ClientName               varchar(30) NULL,
    UWType                   char(1),
    TaxAmount		     decimal (19,4) NULL,
	CompanyCode VARCHAR(10),
	CompanyDesc VARCHAR(255),
	CurrencyCode VARCHAR(10),
	CurrencyDesc VARCHAR(255)
    )

-- Get underwriting type
DECLARE @UWType char(1)
EXECUTE spu_Report_GetUnderwritingType_SFU @UWType OUTPUT

-- Get period date 
DECLARE @dtPeriodEndDate DATETIME

SELECT 	@PeriodDate = @PeriodDate + ' 23:59:59'       --PN71314

SELECT 	@dtPeriodEndDate = CONVERT (datetime, @PeriodDate)

-- Get data
INSERT INTO #tempTransReg
SELECT
	td.transdetail_id,
	td.document_id,
	td.accounting_date,
	td.insurance_ref,
	CASE @TypeOfCurrency
		WHEN 'Base' THEN (SELECT td.amount WHERE td.spare = 'GROSS')
		WHEN 'System' THEN (SELECT td.system_amount WHERE td.spare = 'GROSS')
		WHEN 'Transaction' THEN (SELECT td.currency_amount WHERE td.spare = 'GROSS')
	END,
	CASE @TypeOfCurrency
		WHEN 'Base' THEN (SELECT td.amount WHERE td.spare = 'COMM' )
		WHEN 'System' THEN (SELECT td.system_amount WHERE td.spare = 'COMM')
		WHEN 'Transaction' THEN (SELECT td.currency_amount WHERE td.spare = 'COMM')
	END,
	CASE @TypeOfCurrency
		WHEN 'Base' THEN td.amount
		WHEN 'System' THEN td.system_amount
		WHEN 'Transaction' THEN td.currency_amount
	END,
	d.document_date,
	d.document_ref,
	dt.description,
	a.short_code,
	a.account_name,
	l.ledger_short_name,
	l.ledger_name,
	@dtPeriodEndDate,
	iff.insurance_file_cnt,
	p.description,
	p.code,
	bt.code,
	'c',
	pa.shortname,
	@UWType,
	CASE @TypeOfCurrency
		WHEN 'Base' THEN (SELECT td.amount WHERE td.spare Like 'TAX%' )
		WHEN 'System' THEN (SELECT td.system_amount WHERE td.spare Like 'TAX%')
		WHEN 'Transaction' THEN (SELECT td.currency_amount WHERE td.spare Like 'TAX%')
	END,
	s.code,
	s.description,
	CASE @TypeOfCurrency
		WHEN 'Base' THEN cs.iso_code
		WHEN 'System' THEN @SystemCurrencyCode
		WHEN 'Transaction' THEN ctd.iso_code
	END,
	CASE @TypeOfCurrency
		WHEN 'Base' THEN cs.description
		WHEN 'System' THEN @SystemCurrencyDesc
		WHEN 'Transaction' THEN ctd.description
	END
FROM Account a
JOIN Ledger l
	ON a.ledger_id = l.ledger_id
JOIN TransDetail td 
	ON a.account_id = td.account_id
	AND td.period_id = 
	(
		SELECT TOP 1 period_id         --PN71314
		FROM Period
		WHERE period_end_date = @dtPeriodEndDate
		ORDER BY Period_id DESC        --PN71314
		--AND company_id = td.company_id
	)
JOIN Document d
	ON d.document_id = td.document_id
JOIN DocumentType dt
	ON d.documenttype_id = dt.documenttype_id
JOIN insurance_file iff 
	ON d.insurance_file_cnt = iff.insurance_file_cnt
JOIN product p 
	ON p.product_id = iff.product_id
JOIN business_type bt 
	ON bt.business_type_id = iff.business_type_id
JOIN party pa 
	ON pa.party_cnt = iff.insured_cnt
JOIN source s
	ON s.source_id = td.company_id
JOIN currency cs
	ON cs.currency_id = s.base_currency_id
JOIN currency ctd
	ON ctd.currency_id = td.currency_id

WHERE   
	dt.from_sirius = 1 AND 
	(
		@iBranchID= 0
		OR 
		(
			@iBranchID <> 0
			AND
			td.company_id = @iBranchID 
		)
	)
UPDATE #tempTransReg    
   SET GrossAmount= ISNULL(GrossAmount,0),  
	CommissionAmount= ISNULL(CommissionAmount,0),  
    Amount= ISNULL(Amount,0), 
	TaxAmount= ISNULL(TaxAmount,0) 
      	
SET NOCOUNT OFF

-- Only returns records with an amount <> 0
SELECT
	*,
	CASE @GroupBy
		WHEN 'Branch' THEN CompanyCode
		WHEN 'Branch and Currency' THEN CompanyCode
		WHEN 'Currency' THEN CurrencyCode
		ELSE ''
	END 'GroupByCode1',
	CASE @GroupBy
		WHEN 'Branch' THEN CompanyDesc
		WHEN 'Branch and Currency' THEN CompanyDesc
		WHEN 'Currency' THEN CurrencyDesc
		ELSE ''
	END 'GroupByDesc1'
FROM #tempTransReg
WHERE ISNULL(GrossAmount,0) <> 0
OR ISNULL(Amount,0) <> 0
OR ISNULL(CommissionAmount,0) <> 0
OR ISNULL(TaxAmount,0) <> 0

DROP TABLE #tempTransReg


GO



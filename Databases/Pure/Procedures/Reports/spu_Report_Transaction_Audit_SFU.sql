SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Transaction_Audit_SFU'
GO


CREATE PROCEDURE spu_Report_Transaction_Audit_SFU
	@start_date datetime, 
	@end_date datetime, 
	@branch_id int,
	@TypeOfCurrency VARCHAR(15),
	@GroupBy VARCHAR(20)
AS


/*
-- FOR TESTING
DECLARE @start_date datetime, 
        @end_date datetime, 
        @branch_id int

SELECT 	@start_date = dateadd(day, -15, getdate()),
       	@end_date = getdate(), 
       	@branch_id = 0
*/
SET NOCOUNT ON

DECLARE @ibranchid INT
DECLARE @SystemCurrencyCode VARCHAR(10)
DECLARE @SystemCurrencyDesc VARCHAR(255)

SELECT @iBranchID = isnull(@branch_id,0)

/*Get System Currency Details*/
SELECT
	@SystemCurrencyCode = c.iso_code,
	@SystemCurrencyDesc = c.description
FROM PMSystem pms
JOIN currency c
	ON c.currency_id = pms.currency_id
WHERE pms.system_id = 1

CREATE TABLE #tempRSATransReg
(
	TransactionID int,
	DocumentID int,
	AccountDate datetime NULL,
	InsuranceRef varchar (30) NULL,
	GrossAmount decimal (19,4) NULL,
	CommissionAndTaxAmounts decimal (19,4) NULL,
	Amount decimal (19,4) NULL,
	DocumentDate datetime NULL,
	DocumentRef varchar (25) NULL,
	DocumentType varchar (255) NULL,
	AccountID int NULL,
	ElementName varchar(30) NULL,
	AccountCode varchar (30) NULL,
	AccountName varchar (60) NULL,
	LedgerCode varchar (2) NULL,
	LedgerName varchar (30) NULL,
	Startdate datetime NULL,
	Enddate datetime NULL,
	InsFileCnt int NULL,
	Product varchar (255) NULL,
	ProductCode varchar (10) NULL,
	BusType varchar (10) NULL,
	ExportStatus char (1) NULL,
	ClientName varchar (30) NULL,
	CoverStartDate datetime NULL,
	ExpiryDate datetime NULL,
	CompanyCode VARCHAR(10),
	CompanyDesc VARCHAR(255),
	CurrencyCode VARCHAR(10),
	CurrencyDesc VARCHAR(255),
	UserName char (12) NULL
)


INSERT INTO #tempRSATransReg
	SELECT  td.transdetail_id,
			td.document_id,
			td.accounting_date,
			td.insurance_ref,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN (SELECT td.amount WHERE td.spare = 'GROSS')
				WHEN 'System' THEN (SELECT td.system_amount WHERE td.spare = 'GROSS')
				WHEN 'Transaction' THEN (SELECT td.currency_amount WHERE td.spare = 'GROSS')
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN (SELECT td.amount WHERE td.spare = 'COMM' OR td.spare LIKE ('TAX%'))
				WHEN 'System' THEN (SELECT td.system_amount WHERE td.spare = 'COMM' OR td.spare LIKE ('TAX%'))
				WHEN 'Transaction' THEN (SELECT td.currency_amount WHERE td.spare = 'COMM' OR td.spare LIKE ('TAX%'))
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN td.amount
				WHEN 'System' THEN td.system_amount 
				WHEN 'Transaction' THEN td.currency_amount 
			END,
			d.document_date,
			d.document_ref,
			dt.description,
			a.account_id,
			NULL,
			a.short_code,
			a.account_name,
			l.ledger_short_name,
			l.ledger_name,
			@start_date,
			@end_date,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			s.code,
			s.description,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN cb.iso_code
				WHEN 'System' THEN @SystemCurrencyCode
				WHEN 'Transaction' THEN ct.iso_code
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN cb.description
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN 'Transaction' THEN ct.description
			END,
			td.operator_id
	FROM Account a
	JOIN Ledger l
		ON a.ledger_id = l.ledger_id
	JOIN TransDetail td
		ON a.account_id = td.account_id
	JOIN Document d
		ON d.document_id = td.document_id
	JOIN DocumentType dt
		ON d.documenttype_id = dt.documenttype_id
	JOIN Source s
		ON s.source_id = td.company_id
	JOIN Currency cb /*Base Currency*/
		ON cb.currency_id = s.base_currency_id
	JOIN Currency ct /*Transaction Currency*/
		ON ct.currency_id = td.currency_id	
	WHERE dt.from_sirius = 1
	AND ( @iBranchID = 0 OR  ( @iBranchID <> 0 AND td.company_id = @iBranchID ))
	AND d.document_date >= @start_date
	AND d.document_Date <= @end_date

UPDATE #tempRSATransReg
    SET InsFileCnt = tef.insurance_file_cnt,
        Product = (SELECT description FROM product WHERE product_id = tef.product_id),
        ProductCode = tef.product_code,
        BusType = tef.business_type_code,
        ExportStatus = tef.accounts_export_status,
        UserName = tef.created_by_username
    FROM transaction_export_folder tef
    WHERE tef.document_ref = DocumentRef
 
UPDATE #tempRSATransReg
    SET ClientName = p.shortname,
    	CoverStartDate = ifi.cover_start_date,
    	ExpiryDate = ifi.expiry_date
    FROM Insurance_File ifi
    JOIN Insurance_Folder ifo 	ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt
    JOIN Party p				ON p.party_cnt = ifo.insurance_holder_cnt
    WHERE ifi.insurance_file_cnt = InsFileCnt
       

UPDATE #tempRSATransReg
	SET ElementName = e.element_name
	FROM StructureTree SAccount
	JOIN StructureTree SElement ON SElement.node_id = SAccount.parent_node_id
	JOIN Element e 				ON e.element_id = SElement.element_id
	WHERE SAccount.account_id = AccountID

SET NOCOUNT OFF

SELECT
	DocumentID,
	InsuranceRef, 
	Amount,
	DocumentDate, 
	DocumentRef,
	ElementName,
	AccountCode, 
	ProductCode,
	BusType,
	ClientName,
	CoverStartDate,
	ExpiryDate,
	CompanyCode,
	CompanyDesc,
	CurrencyCode,
	CurrencyDesc,
	CASE @GroupBy
		WHEN 'Branch' THEN CompanyCode
		WHEN 'Branch and Currency' THEN CompanyCode
		WHEN 'Currency' THEN CurrencyCode
		ELSE ''
	END 'GroupByCode',
	UserName
FROM #tempRSATransReg
WHERE (
    Isnull(GrossAmount,0) <> 0
    OR Isnull(Amount,0) <> 0
    OR Isnull(CommissionAndTaxAmounts,0) <> 0
    )
AND ExportStatus = 'c'

DROP TABLE #tempRSATransReg
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_report_daily_audit6'
GO
CREATE PROCEDURE spu_report_daily_audit6
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @date_type VARCHAR(11),
    @fee_type VARCHAR(20)
AS

DECLARE @transdetail_id INT
DECLARE @Client_premium MONEY
DECLARE @Client_code VARCHAR(10)
DECLARE @Client_name VARCHAR(100)
DECLARE @Fees_code VARCHAR(10)
DECLARE @Fees_name VARCHAR(30)
DECLARE @Fees_amount MONEY
DECLARE @VAT_code VARCHAR(10)
DECLARE @VAT_amount MONEY
DECLARE @NZConfig INT

IF EXISTS (SELECT value FROM hidden_options WHERE option_number = 86) 
	SELECT @NZConfig=ISNULL(value,0) FROM hidden_options WHERE option_number = 86
ELSE
SET @NZConfig = 0


IF @branch_id = 0
BEGIN
	SELECT @branch_id = NULL
END


SELECT DISTINCT
	td.transdetail_id,
	td.accounting_date,
	td.insurance_ref,
	d.comment,
	a.short_code,
	d.document_ref,
	dt.description 'document_type',

	ISNULL(
		( /*Get client code for client fees*/
			SELECT MAX(a.short_code)
			FROM transdetail td
			JOIN account a
				ON a.account_id = td.account_id
			JOIN ledger l
				ON l.ledger_id = a.ledger_id
			WHERE td.document_id = d.document_id
			AND l.ledger_short_name = 'SA'
			AND td.document_sequence =
				(
					SELECT MIN(tdx.document_sequence)
					FROM transdetail tdx
					JOIN account ax
						ON ax.account_id = tdx.account_id
					JOIN ledger lx
						ON lx.ledger_id = ax.ledger_id 
					WHERE tdx.document_id = d.document_id
					AND lx.ledger_short_name = 'SA'
				)
		),'') 'client_code',
	ISNULL(
		( /*Get client name for client fees*/
			SELECT MAX(a.account_name)
			FROM transdetail td
			JOIN account a
				ON a.account_id = td.account_id
			JOIN ledger l
				ON l.ledger_id = a.ledger_id
			WHERE td.document_id = d.document_id
			AND l.ledger_short_name = 'SA'
			AND td.document_sequence =
				(
					SELECT MIN(tdx.document_sequence)
					FROM transdetail tdx
					JOIN account ax
						ON ax.account_id = tdx.account_id
					JOIN ledger lx
						ON lx.ledger_id = ax.ledger_id 
					WHERE tdx.document_id = d.document_id
					AND lx.ledger_short_name = 'SA'
				)
		),'') 'client_name',
	ISNULL(
		( /*Get client name for client fees*/
			SELECT SUM(td.amount)
			FROM transdetail td
			JOIN account a
				ON a.account_id = td.account_id
			JOIN ledger l
				ON l.ledger_id = a.ledger_id
			WHERE td.document_id = d.document_id
			AND l.ledger_short_name = 'SA'
		),0) 'client_premium',
		
	a.short_code 'fee_code',
	a.account_name 'fee_name',
	td.amount 'fee_amount',

	ISNULL(
		( /*Get VAT code for client fees*/
			SELECT MAX(a.short_code)
			FROM transdetail td
			JOIN account a
				ON a.account_id = td.account_id
			JOIN ledger l
				ON l.ledger_id = a.ledger_id
			WHERE td.document_id = d.document_id
			AND l.ledger_short_name = 'NO'
			AND d.documenttype_id = 30
		),'') 'vat_code',
	CASE d.documenttype_id 
	WHEN 30 THEN 
		ISNULL(
			( /*Get VAT amount for client fees*/
					SELECT SUM(td.amount)
					FROM transdetail td
					JOIN account a
						ON a.account_id = td.account_id
					JOIN ledger l
						ON l.ledger_id = a.ledger_id
					WHERE td.document_id = d.document_id
					AND l.ledger_short_name = 'NO'
					AND d.documenttype_id = 30
			),0)
	ELSE 

		ISNULL(
		        (SELECT  
			CASE WHEN SIGN(td.amount)<> SIGN(SUM(PF.tax_amount))THEN
				SUM(PF.tax_amount)  * -1
			ELSE
				SUM(PF.tax_amount)
			END
			FROM  
	 		policy_fee PF  
	 		INNER JOIN party P ON PF.party_cnt=P.party_cnt  
	 		INNER JOIN party_type PT ON pt.party_type_id=P.party_type_id  
			LEFT OUTER JOIN tax_calculation TC ON TC.policy_fee_id=PF.policy_fee_id  
			LEFT OUTER JOIN tax_group TG ON TC.tax_group_id=TG.tax_group_id  
			WHERE   PF.insurance_file_cnt=d.insurance_file_cnt  
			AND PT.code  ='FE'
			),0)
					
	END 'vat_amount',

	
	d.document_date,
	a.ledger_id,
	s.code 'company_code',
	s.description 'company_desc',
	ISNULL (bt.code, 'NBT') 'business_type_code',
	ISNULL (bt.description, 'No Business Type') 'business_type_desc',
    @NZConfig 'nz_config'
FROM document d
JOIN transdetail td
	ON td.document_id = d.document_id 
JOIN account a
	ON a.account_id = td.account_id
JOIN documenttype dt
	ON dt.documenttype_id = d.documenttype_id
JOIN source s
	ON s.source_id = d.company_id
LEFT JOIN insurance_file i
		JOIN business_type bt
			ON bt.business_type_id = i.business_type_id
	ON i.insurance_file_cnt = d.insurance_file_cnt
WHERE d.company_id = ISNULL(@branch_id,D.company_id)
AND a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE')
AND 
(
	(
		@fee_type = 'All'
		AND
		D.documenttype_id IN (2, 3, 4, 5, 15, 16, 17, 18, 30, 31, 32, 35, 36)
	)
	OR
	(
		@fee_type = 'Policy'
		AND
		D.documenttype_id IN (2, 3, 4, 5, 15, 16, 17, 18, 31, 32, 35, 36)
	)
	OR
	(
		@fee_type  = 'Client'
		AND
		D.documenttype_id IN (30)
	)
)
AND
(
	(
		@date_type = 'Transaction'
		AND
		d.document_date >= @start_date
		AND 
		d.document_date <= @end_date		
	)
	OR
	(
		@date_type = 'Effective'
		AND
		td.accounting_date >= @start_date
		AND 
		td.accounting_date <= @end_date
	)
) 


GO


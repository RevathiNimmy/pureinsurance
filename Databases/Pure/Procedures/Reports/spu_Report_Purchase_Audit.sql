SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Purchase_Audit'
GO
/*PN5172 Not returning any data for multi company */
/*PN14466 Not bringing data through for transactions not on company 1 */

CREATE PROCEDURE spu_Report_Purchase_Audit
    --TF030500
    @branch_id int,
    @start_date datetime,
    @end_date datetime
AS

DECLARE @NZConfig INT

SELECT @NZConfig = ISNULL(value, 0) FROM hidden_options WHERE option_number = 86

--TF030500
DECLARE @iBranchID int
SELECT @iBranchID = ISNULL(@branch_id, 0)

DECLARE    @document_ref VARCHAR(30),
    @document_date DATETIME,
    @document_sequence INT,
    @liab_code VARCHAR(30),
    @liab_name VARCHAR(50),
    @liab_transdetail_id INT,
    @liab_amount NUMERIC(19,4),
    @liab_branch_id INT,
    @liab_branch VARCHAR(50),
    @liab_invoice_dt DATETIME,
    @liab_order_no VARCHAR(30),
    @liab_desc VARCHAR(50),
    @exp_code VARCHAR(30),
    @exp_name VARCHAR(50),
    @exp_transdetail_id INT,
    @exp_amount NUMERIC(19,4),
    @exp_branch_id INT,
    @exp_branch VARCHAR(50),
    @exp_invoice_dt DATETIME,
    @exp_order_no VARCHAR(30),
    @exp_desc VARCHAR(50),
    @tax_code VARCHAR(30),
    @tax_name VARCHAR(50),
    @tax_amount NUMERIC(19,4),
    @nz_config INT,
    @check_doc_ref VARCHAR(30)

CREATE TABLE #temp_tab
(
    temp_document_ref VARCHAR(30),
    temp_document_date DATETIME,
    temp_document_sequence INT,
    temp_liab_code VARCHAR(30),
    temp_liab_name VARCHAR(50),
    temp_liab_transdetail_id INT,
    temp_liab_amount NUMERIC(19,4),
    temp_liab_branch_id INT,
    temp_liab_branch VARCHAR(50),
    temp_liab_invoice_dt DATETIME,
    temp_liab_order_no VARCHAR(30),
    temp_liab_desc VARCHAR(50),
    temp_exp_code VARCHAR(30),
    temp_exp_name VARCHAR(50),
    temp_exp_transdetail_id INT,
    temp_exp_amount NUMERIC(19,4),
    temp_exp_branch_id INT,
    temp_exp_branch VARCHAR(50),
    temp_exp_invoice_dt DATETIME,
    temp_exp_order_no VARCHAR(30),
    temp_exp_desc VARCHAR(50),
    temp_tax_code VARCHAR(30),
    temp_tax_name VARCHAR(50),
    temp_tax_amount NUMERIC(19,4),
    temp_nz_config INT
)

SELECT @check_doc_ref = ''

--Select 1st 2 transactions in document for 1st line
--Debit input first

DECLARE c_cursor CURSOR FAST_FORWARD FOR
SELECT
    D.document_ref,
    D.document_date,
    TD1.document_sequence,
    A1.short_code liab_code,
    A1.account_name liab_name,
    TD1.transdetail_id liab_transdetail_id,
    TD1.amount liab_amount,
    TD1.company_id liab_branch_id,
    C1.description liab_branch,
    I.invoice_date liab_invoice_dt,
    I.order_no liab_order_no,
    I.description liab_desc,
    A2.short_code exp_code,
    A2.account_name exp_name,
    TD2.transdetail_id exp_transdetail_id,
    TD2.amount exp_amount,
    TD2.company_id exp_branch_id,
    C2.description exp_branch,
    I.invoice_date exp_invoice_dt,
    I.order_no exp_order_no,
    I.description exp_desc,
    A3.short_code tax_code,
    A3.account_name tax_name,
    TD3.amount tax_amount,
    @NZConfig nz_config
FROM TransDetail TD1
JOIN TransDetail TD2 ON TD1.document_id = TD2.document_id
JOIN Transdetail TD3 ON TD2.document_id = TD3.document_id
JOIN Document D ON TD1.document_id = D.document_id
JOIN Account A1 ON TD1.account_id = A1.account_id
JOIN Account A2 ON TD2.account_id = A2.account_id
JOIN Account A3 ON TD3.account_id = A3.account_id
JOIN Company C1 ON TD1.company_id = C1.company_id
JOIN Company C2 ON TD2.company_id = C2.company_id
JOIN Invoice I ON A1.account_id = I.account_id
WHERE D.documenttype_id IN (13, 25)
AND
(
    D.document_date >= @start_date
    AND D.document_date <= @end_date
)
AND TD1.purchase_invoice_no = I.invoice_number
/** DC170901 to prevent duplicates **/
AND TD1.purchase_order_no = I.order_no
--PN5172
--AND A1.ledger_id = 3
--AND A2.ledger_id <> 3
AND A1.ledger_id in (SELECT ledger_id from ledger where ledger_short_name = 'PU')
AND A2.ledger_id not in (SELECT ledger_id from ledger where ledger_short_name = 'PU')
AND ISNULL(TD2.transdetail_type_id, 0) <> 8
AND A3.ledger_id not in (SELECT ledger_id from ledger where ledger_short_name = 'PU')
AND ISNULL(TD3.transdetail_type_id, 0) = 8
AND (
        @iBranchID = 0
        OR
        (
            @iBranchID <> 0
            AND
            TD1.company_id = @iBranchID
            AND
            TD2.company_id = @iBranchID
        )
    )

ORDER BY
    D.document_ref,
    TD1.document_sequence

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO @document_ref, @document_date, @document_sequence,
	@liab_code, @liab_name, @liab_transdetail_id, @liab_amount, @liab_branch_id,
    	@liab_branch, @liab_invoice_dt, @liab_order_no, @liab_desc, 
	@exp_code, @exp_name, @exp_transdetail_id, @exp_amount, @exp_branch_id,
    	@exp_branch, @exp_invoice_dt, @exp_order_no, @exp_desc, 
	@tax_code, @tax_name, @tax_amount, @nz_config

WHILE @@FETCH_STATUS = 0 
BEGIN
	IF @check_doc_ref = @document_ref
	BEGIN
		SELECT @tax_code = ''
		SELECT @tax_name = ''
		SELECT @tax_amount = 0
	END
	ELSE
	BEGIN
		SELECT @check_doc_ref = @document_ref
	END

	INSERT INTO #temp_tab 
		(temp_document_ref, temp_document_date, temp_document_sequence, temp_liab_code, temp_liab_name,
    		temp_liab_transdetail_id, temp_liab_amount, temp_liab_branch_id, temp_liab_branch, temp_liab_invoice_dt,
    		temp_liab_order_no, temp_liab_desc, temp_exp_code, temp_exp_name, temp_exp_transdetail_id,
    		temp_exp_amount, temp_exp_branch_id, temp_exp_branch, temp_exp_invoice_dt, temp_exp_order_no,
    		temp_exp_desc, temp_tax_code, temp_tax_name, temp_tax_amount, temp_nz_config)
	VALUES (@document_ref, @document_date, @document_sequence,
		@liab_code, @liab_name, @liab_transdetail_id, @liab_amount, @liab_branch_id,
    		@liab_branch, @liab_invoice_dt, @liab_order_no, @liab_desc, 
		@exp_code, @exp_name, @exp_transdetail_id, @exp_amount, @exp_branch_id,
    		@exp_branch, @exp_invoice_dt, @exp_order_no, @exp_desc, 
		@tax_code, @tax_name, @tax_amount, @nz_config)

	FETCH NEXT FROM c_cursor INTO @document_ref, @document_date, @document_sequence,
		@liab_code, @liab_name, @liab_transdetail_id, @liab_amount, @liab_branch_id,
    		@liab_branch, @liab_invoice_dt, @liab_order_no, @liab_desc, 
		@exp_code, @exp_name, @exp_transdetail_id, @exp_amount, @exp_branch_id,
    		@exp_branch, @exp_invoice_dt, @exp_order_no, @exp_desc, 
		@tax_code, @tax_name, @tax_amount, @nz_config
END

CLOSE c_cursor
DEALLOCATE c_cursor

SELECT * FROM #temp_tab

DROP TABLE #temp_tab

SET NOCOUNT OFF

GO


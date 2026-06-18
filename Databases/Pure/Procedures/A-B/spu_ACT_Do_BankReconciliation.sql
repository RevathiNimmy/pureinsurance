SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_BankReconciliation'
GO

CREATE PROCEDURE spu_ACT_Do_BankReconciliation
    @account_id INT,
    @date_to DATETIME,
    @marked_status INT,
    @month INT,
    @total_reconciled MONEY OUTPUT,
    @total_unreconciled MONEY OUTPUT
AS

DECLARE @settlement_period SMALLINT
DECLARE @document_ref VARCHAR(25)
DECLARE @short_code VARCHAR(50)
DECLARE @gross_transdetail_id INT
DECLARE @banktranstemp_id INT
DECLARE @company_id INT	

/*Get the settlement period*/
SELECT @settlement_period = settlement_period
FROM account
WHERE account_id = @account_id


CREATE TABLE #BankTransTemp 
(
	source_id INT,
	period VARCHAR(15),
	short_code VARCHAR(50),
	cheque_no VARCHAR(30),
	document_ref VARCHAR(25),
	accounting_date DATETIME,
	gross_transdetail_id INT,
	gross_amount MONEY,
	currency_id INT,
	currency VARCHAR(4),
	marked_status TINYINT,
	month SMALLINT,
	spare CHAR(20),
	account_id INT,
	document_id INT,
	period_end_date DATETIME
)

INSERT INTO #BankTransTemp 
(
	source_id,
	period,
	short_code,
	cheque_no,
	document_ref,
	accounting_date,
	gross_transdetail_id,
	gross_amount,
	currency_id,
	currency,
	marked_status,
	month,
	spare,
	account_id,
	document_id,
	period_end_date
) 
SELECT
	d.company_id,
	p.period_name,
	'***Multiple***',
	'Not Available',
	d.document_ref,
	d.document_date,
	td.transdetail_id,
	td.currency_amount,
	td.currency_id,
	c.iso_code,
	0,
	DATEPART(mm, DATEADD(dd, @settlement_period, td.accounting_date)),
	td.spare,
	td.account_id,
	td.document_id,
	p.period_end_date
FROM transdetail td
JOIN document d
	ON d.document_id = td.document_id
JOIN period p
	ON p.period_id = td.period_id
JOIN currency c
	ON c.currency_id = td.currency_id
WHERE td.account_id = @account_id
AND (td.accounting_date <= @date_to OR @date_to IS NULL)


/* Remove ones that dont comply with the month, if needed */
IF @month IS NOT NULL BEGIN
	DELETE 
	FROM #BankTransTemp
	WHERE month <> @month
END

/* Update the marked status. They should all be 0 at the moment.*/
/* marked for reconciliation */
UPDATE #BankTransTemp
SET marked_status = 2
WHERE gross_transdetail_id IN 
	(
		SELECT transdetail_id
		FROM transmatch
		WHERE allocationdetail_id IS NULL
	)

/* fully reconciled */
UPDATE #BankTransTemp
SET marked_status = 1
WHERE spare like '%RECONCILED%'

/*Get totals*/
SELECT @total_reconciled = SUM(gross_amount)
FROM #BankTransTemp
WHERE marked_status = 1

SELECT @total_unreconciled = SUM(gross_amount)
FROM #BankTransTemp
WHERE marked_status <> 1

/* Remove the marked ones now if needed */
IF @marked_status IS NOT NULL BEGIN
	DELETE 
	FROM #BankTransTemp
	WHERE marked_status = (1 - (@marked_status))
END

UPDATE btt
SET	short_code = 
		(
			SELECT ax.short_code
			FROM document dx
			JOIN transdetail tdx
				ON tdx.document_id = dx.document_id
			JOIN account ax
				ON ax.account_id = tdx.account_id
			WHERE dx.document_id = btt.document_id
			AND tdx.transdetail_id <> btt.gross_transdetail_id
			AND ax.account_id <> btt.account_id
			AND 
				(
					SELECT SUM(1)
					FROM transdetail
					WHERE document_id = btt.document_id
					AND transdetail_id <> btt.gross_transdetail_id
					AND account_id <> btt.account_id
				) = 1
		),
	cheque_no = 
		(
			SELECT media_ref
			FROM cashlistitem
			WHERE transdetail_id = 
				(
					SELECT tdx.transdetail_id
					FROM document dx
					JOIN transdetail tdx
						ON tdx.document_id = dx.document_id
					WHERE dx.document_id = btt.document_id
					AND tdx.transdetail_id <> btt.gross_transdetail_id
					AND tdx.account_id <> btt.account_id
					AND 
						(
							SELECT SUM(1)
							FROM transdetail
							WHERE document_id = btt.document_id
							AND transdetail_id <> btt.gross_transdetail_id
							AND account_id <> btt.account_id
						) = 1
				)
		)
FROM #BankTransTemp btt

/*Select all except the hidden primary key.*/
SELECT 
	source_id,
	period,
	short_code,
	cheque_no,
	document_ref,
	accounting_date,
	gross_transdetail_id,
	gross_amount,
	currency_id,
	currency,
	marked_status,
	month,
	spare,
	period_end_date
FROM #BankTransTemp
ORDER BY source_id, document_ref

/*Remove the temp table*/
DROP TABLE #BankTransTemp


GO


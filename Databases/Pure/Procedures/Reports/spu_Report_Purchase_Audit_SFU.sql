SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Purchase_Audit_SFU'
GO


CREATE PROCEDURE spu_Report_Purchase_Audit_SFU
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @TypeOfCurrency VARCHAR(50),
    @GroupByCode VARCHAR(50)
AS

DECLARE @iBranchID INT
SELECT @iBranchID = ISNULL(@branch_id, 0)

/*Get System Currency Details*/
DECLARE @SystemCurrencyCode VARCHAR(10)
DECLARE @SystemCurrencyDesc VARCHAR(255)
SELECT
    @SystemCurrencyCode = c.iso_code,
    @SystemCurrencyDesc = c.description
FROM PMSystem pms
JOIN currency c
    ON c.currency_id = pms.currency_id
WHERE pms.system_id = 1
/*end  Get System Currency*/

--Select 1st 2 transactions in document for 1st line
--Debit input first
SELECT
    D.document_ref,
    D.document_date,
    TD1.document_sequence,
    A1.short_code liab_code,
    A1.account_name liab_name,
    TD1.transdetail_id liab_transdetail_id,
    CASE @TypeOfCurrency
        WHEN 'System' THEN TD1.system_amount
        WHEN 'Base' THEN  TD1.amount 
        WHEN 'Transaction' THEN  TD1.currency_amount 
    END liab_amount,
    TD1.company_id liab_branch_id,
    C1.code liab_branch_code,
    C1.description liab_branch,
    I.invoice_date liab_invoice_dt,
    I.order_no liab_order_no,
    I.description liab_desc,
    A2.short_code exp_code,
    A2.account_name exp_name,
    TD2.transdetail_id exp_transdetail_id,
    CASE @TypeOfCurrency 
        WHEN 'System' THEN TD2.system_amount 
        WHEN 'Base' THEN TD2.amount 
        WHEN 'Transaction' THEN  TD2.currency_amount 
    END exp_amount,
    TD2.company_id exp_branch_id,
    C2.description exp_branch,
    I.invoice_date exp_invoice_dt,
    I.order_no exp_order_no,
    I.description exp_desc,
    CASE @TypeOfCurrency
        WHEN 'System' THEN @SystemCurrencycode
        WHEN 'Base' THEN CB.Code 
        WHEN 'Transaction' THEN CT.Code 
    END CurrencyCode,
    CASE @TypeOfCurrency
        WHEN 'System' THEN @SystemCurrencyDesc
        WHEN 'Base' THEN CB.Description
        WHEN 'Transaction' THEN CT.Description
    End CurrencyDesc,
    CASE @GroupByCode 
        WHEN 'Branch' THEN C1.Code
        WHEN 'Branch and Currency' THEN C1.Code
        ELSE ''
    END GroupByCode
FROM TransDetail TD1
JOIN Document D
    ON D.document_id = TD1.document_id
JOIN Account A1
    ON A1.Account_id = TD1.Account_id
JOIN Company C1
    ON C1.company_id = TD1.company_id
JOIN Currency CB
    ON CB.Currency_id = C1.Base_Currency
JOIN Currency CT
    ON CT.Currency_id = TD1.Currency_id
JOIN TransDetail TD2
    ON TD2.document_id = TD1.document_id
JOIN Account A2
    ON A2.Account_id = TD2.Account_id
JOIN Company C2
    ON C2.company_id = TD2.company_id
JOIN Invoice I
    ON I.account_id = A1.account_id 
    AND I.invoice_number = TD1.purchase_invoice_no
    AND I.order_no = TD1.purchase_order_no
WHERE D.documenttype_id IN (13, 25)
AND D.document_date BETWEEN @start_date AND @end_date
AND A1.ledger_id = 3
AND A2.ledger_id <> 3
AND (
        @iBranchID = 0
        OR
        (
            @iBranchID <> 0
            AND
            A1.company_id = @iBranchID
            AND
            A2.company_id = @iBranchID
        )
    )
ORDER BY
    D.document_ref,
    TD1.document_sequence

GO


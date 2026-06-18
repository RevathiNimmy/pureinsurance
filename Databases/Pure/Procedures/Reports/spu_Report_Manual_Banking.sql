SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Manual_Banking'
GO

CREATE PROCEDURE spu_Report_Manual_Banking
    @document_ref VARCHAR(25),
    @branch_id INT
AS

DECLARE @cashlist_id INT

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

SELECT @cashlist_id = cashlist_id
FROM cashlistitem cli
JOIN transdetail td
ON cli.transdetail_id = td.transdetail_id
JOIN document d
ON d.document_id =  td.document_id
AND d.document_ref = @document_ref
AND d.company_id = ISNULL(@branch_id, d.company_id)

CREATE TABLE #temp_banking
(
code VARCHAR(10),
account_name VARCHAR(60),
short_code VARCHAR(30),
document_ref VARCHAR(25),
media_ref VARCHAR(100),
media_type_desc VARCHAR(255),
[date] DATETIME,
amount MONEY,
receipt_type_desc VARCHAR(50),
currency_code VARCHAR(4),
currency_desc VARCHAR(255),
susp_bank_account VARCHAR(60),
bank_account VARCHAR(60),
bank_date DATETIME,
bank_media_ref VARCHAR(100),
bank_amount MONEY,
bank_doc_ref VARCHAR(25)
)

INSERT #temp_banking
SELECT
    c.code,
    a.account_name,
    a.short_code,
    d.document_ref,
    cli.media_ref,
    mt.description, 
    td.accounting_date,
    ABS(td1.amount),
    clrt.description,
    cur.iso_code,
    cur.description,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL
FROM Cashlistitem cli
JOIN cashlistitem_receipt_type clrt
     ON clrt.cashlistitem_receipt_type_id = cli.cashlistitem_receipt_type_id
JOIN mediatype mt 
     ON mt.mediatype_id = cli.mediatype_id 
JOIN transmatch tm
    ON cli.transdetail_id = tm.transdetail_id 
JOIN transmatch tm1
    ON tm1.match_id = tm.match_id 
    AND cli.transdetail_id <> tm1.transdetail_id
JOIN transdetail td 
    ON td.transdetail_id = tm1.transdetail_id 
JOIN document d
    ON d.document_id = td.document_id
JOIN transdetail td1 
    ON td1.document_id = td.document_id
    AND td1.transdetail_id <> td.transdetail_id 
JOIN account a
    ON a.account_id = td1.account_id  
JOIN company c
    ON c.company_id = td1.company_id
JOIN currency cur
    ON cur.currency_id=td1.currency_id
WHERE cli.cashlist_id=@cashlist_id
ORDER BY c.company_id

UPDATE #temp_banking 
SET 
susp_bank_account = a.account_name,
bank_date =td.accounting_date,
bank_media_ref =cli.media_ref,
bank_amount =ABS(td.amount),
bank_doc_ref = d.document_ref
FROM Cashlistitem cli
JOIN transdetail td 
    ON cli.transdetail_id = td.transdetail_id 
JOIN document d
    ON d.document_id = td.document_id
JOIN account a
    ON a.account_id =  td.account_id  
WHERE cli.Cashlist_ID=@cashlist_id

UPDATE #temp_banking 
SET bank_account = a.account_name 
FROM Cashlistitem cli
JOIN transdetail td 
    ON cli.transdetail_id = td.transdetail_id 
JOIN transdetail td1 
    ON td1.document_id = td.document_id
    AND td1.transdetail_id <> td.transdetail_id 
JOIN account a
    ON a.account_id= td1.account_id
WHERE Cashlist_ID=@cashlist_id

SELECT * FROM #temp_banking

DROP TABLE #temp_banking

GO

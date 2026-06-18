SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Audit_Fees'
GO
CREATE PROCEDURE spu_Report_Audit_Fees
    @Source_id int
AS

DECLARE @transdetail_id INT
DECLARE @Client_premium numeric(19, 3)
DECLARE @Client_code varchar (10)
DECLARE @Client_name varchar (100)
DECLARE @Fees_code varchar (10)
DECLARE @Fees_name varchar (30)
DECLARE @Fees_amount numeric(19, 3)
DECLARE @VAT_code varchar (10)
DECLARE @VAT_amount numeric(19, 3)
DECLARE @sub_branch_id int

-- get default sub-branch for supplied source_id
EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

SELECT DISTINCT
    TD.transdetail_id,
    TD.accounting_date,
    TD.insurance_ref,
    TD.comment,
    A.short_code,
    D.Document_ref,
    DT.description document_type,
    Client_premium = CONVERT(numeric(19, 3), 0),
    A.short_code Client_code,
    A.account_name Client_name,
    Fees_code = A.short_code,
    Fees_name = A.account_name,
    Fees_amount = CONVERT(numeric(19, 3), 0),
    VAT_code = A.short_code,
    VAT_amount = CONVERT(numeric(19, 3), 0),
    D.document_date
INTO #TempTable
FROM TransDetail TD,
    Period P, -- Just an observation, this table is not restricted in any way. 
              -- If this procedure is broken it is most likely here.
    Document D,
    DocumentType DT,
    Account A
WHERE TD.document_id = D.document_id
AND D.documenttype_id = DT.documenttype_id
AND TD.account_id = A.account_id
AND D.documenttype_id = 30
AND A.sub_branch_id = @sub_branch_id
ORDER BY insurance_ref

DECLARE c_Cursor CURSOR FAST_FORWARD FOR
    SELECT transdetail_id FROM #TempTable

OPEN c_Cursor

FETCH NEXT FROM c_Cursor INTO @transdetail_id

WHILE @@FETCH_STATUS = 0 BEGIN
    /* Get the correct value for Client_premium */
    SELECT @Client_premium = TD.amount
                FROM TransDetail TD,
                Account A, Ledger L,
                Document D
                WHERE D.document_id = TD.document_id
                AND TD.Account_id = A.Account_id
                AND A.Ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id
                AND TD.transdetail_id = @transdetail_id

    /* Get the correct value for Client_code & Fees_code*/
    SELECT @Client_code = A.short_code,
            @Client_name = A.account_name
    FROM TransDetail TD,
            TransDetail TD2,
            Account A,
            Ledger L,
            Document D
    WHERE D.document_id = TD.document_id
    AND TD.document_id = TD2.document_id
    AND TD.Account_id = A.Account_id
    AND A.Ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id
    AND TD2.transdetail_id = @transdetail_id
    AND A.sub_branch_id = @sub_branch_id

    /* Get the correct value for Fees_amount */
    SELECT @Fees_code = A.short_code,
            @Fees_name = A.account_name,
            @Fees_amount = TD.amount
    FROM TransDetail TD,
            TransDetail TD2,
            Account A,
            Ledger L,
            Document D
    WHERE D.document_id = TD.document_id
    AND TD.document_id = TD2.document_id
    AND TD.Account_id = A.Account_id
    AND A.Ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id
    AND TD2.transdetail_id = @transdetail_id
    AND A.sub_branch_id = @sub_branch_id

    /* Get the correct value for VAT_code & amount */
    SELECT @VAT_code = A.short_code,
            @VAT_amount = TD.amount
    FROM TransDetail TD,
            TransDetail TD2,
            Account A,
            Ledger L,
            Document D
    WHERE D.document_id = TD.document_id
    AND TD.document_id = TD2.document_id
    AND TD.Account_id = A.Account_id
    AND A.Ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'NO') --PSL 16/06/2003 Iss4707 Remove Hard coded ledger_id
    AND TD2.transdetail_id = @transdetail_id
    AND A.sub_branch_id = @sub_branch_id

    /* Update the temporary table */
    UPDATE #TempTable
    SET Client_premium = ISNULL(@Client_premium, 0.0),
        Client_code = ISNULL(@Client_code, ''),
        Client_name = ISNULL(@Client_name, ''),
        Fees_code = ISNULL(@Fees_code, ''),
        Fees_name = ISNULL(@Fees_name, ''),
        Fees_amount = ISNULL(@Fees_amount, 0.0),
        VAT_code = ISNULL(@VAT_code, ''),
        VAT_amount = ISNULL(@VAT_amount, 0.0)
    WHERE transdetail_id = @transdetail_id

    /* Move to the next row */
    FETCH NEXT FROM c_Cursor INTO @transdetail_id
END

/* Clear up our cursor */

CLOSE c_Cursor
DEALLOCATE c_Cursor

/* Select everything now */
SELECT * FROM #TempTable

DELETE #TempTable
DROP TABLE #TempTable

GO


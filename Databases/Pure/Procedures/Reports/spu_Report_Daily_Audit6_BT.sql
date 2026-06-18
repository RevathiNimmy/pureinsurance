SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_report_daily_audit6_BT'
GO
CREATE PROCEDURE spu_report_daily_audit6_BT
    @branch_id int,
    @start_date datetime,
    @end_date datetime,
    @date_type varchar(11)
AS

DECLARE @branchID int
DECLARE @transdetail_id INT
DECLARE @Client_premium numeric(19, 3)
DECLARE @Client_code varchar (10)
DECLARE @Client_name varchar (100)
DECLARE @Fees_code varchar (10)
DECLARE @Fees_name varchar (30)
DECLARE @Fees_amount numeric(19, 3)
DECLARE @VAT_code varchar (10)
DECLARE @VAT_amount numeric(19, 3)

SELECT @branchID = ISNULL(@branch_id, 0)

DELETE FROM report_audit_debit_table6

if @branchID = 0
BEGIN
    INSERT INTO report_audit_debit_table6
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
        D.document_date,
        A.ledger_id
    FROM TransDetail TD,
        Period P,
        Document D,
        DocumentType DT,
        Account A
    WHERE TD.document_id = D.document_id
    AND D.documenttype_id = DT.documenttype_id
    AND (
        D.document_date >= @start_date
        AND D.document_date <= @end_date
        )
    AND TD.account_id = A.account_id
    AND D.documenttype_id = 30
--eck310502 date selection based on date type
    AND
        (
            (
            D.document_date >= @start_date
            AND D.document_date <= @end_date
            AND @date_type = 'Transaction'
            )
            OR
            (
            TD.accounting_date >= @start_date
            AND TD.accounting_date <= @end_date
            AND @date_type = 'Effective'
            )
        ) ORDER BY insurance_ref
END
ELSE
BEGIN
    INSERT INTO report_audit_debit_table6
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
        D.document_date,
        A.ledger_id
    FROM TransDetail TD,
        Period P,
        Document D,
        DocumentType DT,
        Account A
    WHERE TD.document_id = D.document_id
    AND D.documenttype_id = DT.documenttype_id
--eck100601 checked for earlier
-- AND (
-- D.document_date >= @start_date
-- AND D.document_date <= @end_date
-- )
    AND TD.account_id = A.account_id
    AND D.documenttype_id = 30
    --DC031201 use company on transdetail not the account
    --AND A.company_id = @branchID
    AND TD.company_id = @branchID
--eck310502 date selection based on date type
    AND
        (
            (
            D.document_date >= @start_date
            AND D.document_date <= @end_date
            AND @date_type = 'Transaction'
            )
            OR
            (
            TD.accounting_date >= @start_date
            AND TD.accounting_date <= @end_date
            AND @date_type = 'Effective'
            )
        ) ORDER BY insurance_ref
END

DECLARE c_Cursor CURSOR FAST_FORWARD FOR
    SELECT transdetail_id FROM report_audit_debit_table6

OPEN c_Cursor

FETCH NEXT FROM c_Cursor INTO @transdetail_id

WHILE @@FETCH_STATUS = 0 BEGIN
    /* Get the correct value for Client_premium */
    SELECT @Client_premium = TD.amount
                FROM TransDetail TD,
                Account A,
                Ledger L,
                Document D
                WHERE D.document_id = TD.document_id
                AND (
                    D.document_date >= @start_date
                    AND D.document_date <= @end_date
                    )
                AND TD.Account_id = A.Account_id
                AND A.Ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
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
                AND (
                    D.document_date >= @start_date
                    AND D.document_date <= @end_date
                    )
                AND TD.document_id = TD2.document_id
                AND TD.Account_id = A.Account_id
                AND A.Ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
                AND TD2.transdetail_id = @transdetail_id

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
                AND (
                    D.document_date >= @start_date
                    AND D.document_date <= @end_date
                    )
                AND TD.document_id = TD2.document_id
                AND TD.Account_id = A.Account_id
                AND A.Ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE') --PSL16/06/2003 Remove Hard coded ledger_id
                AND TD2.transdetail_id = @transdetail_id

    /* Get the correct value for VAT_code & amount */
    SELECT @VAT_code = A.short_code,
        @VAT_amount = TD.amount
                FROM TransDetail TD,
                TransDetail TD2,
                Account A,
                Ledger L,
                Document D
                WHERE D.document_id = TD.document_id
--eck100602 not relevant anymore
-- AND (
-- D.document_date >= @start_date
-- AND D.document_date <= @end_date
-- )
                AND TD.document_id = TD2.document_id
                AND TD.Account_id = A.Account_id
                AND A.Ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'NO') --PSL16/06/2003 Remove Hard coded ledger_id
                AND TD2.transdetail_id = @transdetail_id

    /* Update the temporary table */
    UPDATE report_audit_debit_table6
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
SELECT rt6.*, s.description branch,
ISNULL (bt.description, '') business_type
FROM report_audit_debit_table6 rt6
LEFT OUTER JOIN insurance_file ifi
ON rt6.insurance_ref = ifi.insurance_ref
LEFT OUTER JOIN business_type bt
ON bt.business_type_id = ifi.business_type_id
JOIN .transdetail td
ON td.transdetail_id = rt6.transdetail_id
JOIN source s
ON s.source_id = td.company_id
WHERE rt6.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Commission_Outstand'
GO

CREATE PROCEDURE spu_Report_Commission_Outstand
    @end_date varchar(10),
    @branch_id int
AS
    DECLARE
        @dEndDate datetime,
        @iBranchID int,
        @sStatementType char(2),
        @iAccountID int,
        @nAccountTotal numeric(19,4),
        @nMatchTotal numeric(19,4),
        @nUnallTD numeric(19,4),
        @nUnallTM numeric(19,4),
        @nUnallocated numeric(19,4),
        @account_name varchar(60),
        @document_id int,
        @account_code varchar(30),
        @transdetail_id int

    SET NOCOUNT ON

    IF @end_date IS NULL OR @end_date = ''
        SELECT @dEndDate = GETDATE()
    ELSE
        SELECT @dEndDate = CONVERT(datetime, @end_date)

    IF @branch_id IS NULL
        SELECT @iBranchID = ISNULL(@branch_id, 0)

    -- Empty the transaction table
    DELETE FROM Report_Transaction

    -- Get the required transactions
    IF @end_date IS NULL OR @end_date = ''
        SELECT @dEndDate = GETDATE()
    ELSE
        SELECT @dEndDate = @end_date

    INSERT INTO Report_Transaction(
        transdetail_id, /* TransDetail.transdetail_id */
        account_code, /* Account.short_code */
        account_name, /* Account.account_name */
        document_ref, /* Document.document_ref */
        document_date, /* Document.document_date */
        extra_datetime1, /* TransDetail.accounting_date */
        ledger_type, /* Ledger.ledger_name */
        policy_number, /* TransDetail.insurance_ref */
        documenttype_id, /* Document.documenttype_id */
        extra_char1, /* Risk_Code.code */
        extra_char2, /* Risk_Code.description */
        amount, /* TransDetail.amount */
        branch_id, /* Account.company_id */
        extra_int1, /* Match indicator */
        extra_numeric1, /* Total match amount for transaction */
        extra_int2, /* Document.document_id */
        record_type /* Account.ledger_id */
    ) SELECT
        TransDetail.transdetail_id,
        Account.short_code,
        Account.account_name,
        Document.document_ref,
        Document.document_date,
        TransDetail.accounting_date,
        ISNULL(Ledger.ledger_name, ''),
        ISNULL(TransDetail.insurance_ref, ''),
        ISNULL(Document.documenttype_id, 0),
        ISNULL(Risk_Code.code, ''),
        ISNULL(Risk_Code.description, ''),
        TransDetail.Amount,
        Account.company_id,
        0,
        ISNULL(AllocationDetail.alloc_base_amount, 0.0),
        Document.document_id,
        Account.ledger_id
        FROM Account
        INNER JOIN Ledger
            ON Account.ledger_id = Ledger.ledger_id
        INNER JOIN TransDetail
            ON Account.account_id = TransDetail.account_id
        INNER JOIN Document
            ON TransDetail.document_id = Document.document_id
            AND Document.document_date <= @dEndDate
        LEFT OUTER JOIN AllocationDetail
            ON TransDetail.transdetail_id = AllocationDetail.transdetail_id
            AND AllocationDetail.allocationdetail_id IS NOT NULL
        --sj 31/07/2002 - Start
        LEFT OUTER JOIN Insurance_file
            ON Document.insurance_file_cnt = Insurance_File.insurance_file_cnt
        --LEFT OUTER JOIN Transaction_Export_Folder
        --    ON Document.document_ref = Transaction_Export_Folder.document_ref
        --LEFT OUTER JOIN Insurance_File
        --    ON Transaction_Export_Folder.insurance_file_cnt = Insurance_File.insurance_file_cnt
        --    AND Transaction_Export_Folder.accounts_export_status = 'c'
        --sj 31/07/2002 - End
        LEFT OUTER JOIN Risk_Code
            ON Insurance_File.risk_code_id = Risk_Code.risk_code_id
        WHERE Account.ledger_id IN (2, 4, 9, 7, 5, 10)
        AND (@iBranchID = 0 OR (@iBranchID > 0 AND Account.company_id = @iBranchID))
        ORDER BY TransDetail.transdetail_id

    /* Delete insurer premium */
    DELETE FROM Report_Transaction
        WHERE transdetail_id IN (
            SELECT MIN(transdetail_id)
            FROM Report_Transaction
            WHERE record_type = 4
            GROUP BY document_ref
        )

    /* Set account names to client name */
    DECLARE c_cursor CURSOR FAST_FORWARD FOR
        SELECT account_code,
        account_name,
        extra_int2,
        transdetail_id
        FROM Report_Transaction
        WHERE record_type = 2

    OPEN c_cursor
    FETCH NEXT FROM c_cursor INTO
        @account_code,
        @account_name,
        @document_id,
        @transdetail_id

    WHILE @@FETCH_STATUS = 0 BEGIN
        UPDATE Report_Transaction
            SET account_code = @account_code,
            account_name = @account_name
            WHERE extra_int2 = @document_id

        /* Get matched totals for each transdetail */
        UPDATE Report_Transaction
            SET extra_numeric1 = (
                SELECT SUM(ISNULL(AllocationDetail.alloc_base_amount, 0.0))
                FROM AllocationDetail AllocationDetail
                WHERE AllocationDetail.transdetail_id = @transdetail_id
            )
            WHERE transdetail_id = @transdetail_id

        FETCH NEXT FROM c_cursor INTO
            @account_code,
            @account_name,
            @document_id,
            @transdetail_id
    END

    CLOSE c_cursor
    DEALLOCATE c_cursor

    /* Delete fully matched documents */
    DELETE FROM Report_Transaction
        WHERE extra_int2 IN (
            SELECT DISTINCT extra_int2
            FROM Report_Transaction
            WHERE extra_numeric1 = amount
            AND record_type = 2
        )

    /* Delete non-client documents */
    DELETE FROM Report_Transaction
        WHERE extra_int2 IN (
            SELECT DISTINCT extra_int2
            FROM Party P,
            Account A,
            Report_Transaction R
            WHERE R.account_code = A.short_code
 --PN62169  AND P.party_id = A.account_key
	    AND P.party_cnt = A.account_key
            AND P.party_type_id NOT IN(1, 2, 4)
        )

    /* Return required data */
    SELECT DISTINCT
        transdetail_id,
        account_code,
        account_name,
        document_ref,
        document_date,
        extra_datetime1,
        ledger_type,
        policy_number,
        documenttype_id,
        extra_char1,
        extra_char2,
        amount,
        branch_id,
        extra_int1,
        extra_numeric1,
        record_type
    FROM Report_Transaction
    ORDER BY account_code, document_ref, transdetail_id

GO


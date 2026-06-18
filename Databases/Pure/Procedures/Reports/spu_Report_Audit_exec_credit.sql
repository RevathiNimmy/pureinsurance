SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Audit_exec_Credits'
GO
CREATE PROCEDURE spu_Report_Audit_exec_Credits
    -- TF030500
    @branch_id int,
    @start_date datetime,
    @end_date datetime
AS

DECLARE @transdetail_id INT
DECLARE @document_id INT
DECLARE @main_key INT
DECLARE @old_main_key INT
DECLARE @account_id INT
DECLARE @amount numeric(19, 4)
DECLARE @iBranchID int
DECLARE @OrigDocRef varchar(11)
DECLARE @iCompanyID int

DELETE FROM report_audit_debit_table1
DELETE FROM report_audit_debit_table2
DELETE FROM report_audit_debit_table3
DBCC CHECKIDENT (report_audit_debit_table3, RESEED, 0)

SELECT @iBranchID = ISNULL(@branch_id, 0)

-- Select Client Credit Transaction(s) into Table 1
INSERT INTO report_audit_debit_table1
SELECT DISTINCT
    P.year_name,
    P.period_name,
    D.document_id,
    TD.accounting_date,
    TD.insurance_ref,
    D.comment,
    A.short_code,
    D.Document_ref,
    DT.description document_type,
    PT.code party_type,
    PT.description type_description,
    RC.code risk_code,
    RC.description risk_description,
    TEF.cover_start_date,
    TD.amount,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    IFI.commission_percentage,
    PY.resolved_name resolved_name,
    D.document_date,
    (
        SELECT ISNULL (resolved_name, '')
        FROM Party
        WHERE party_cnt = PY.consultant_cnt
    ) account_handler
FROM TransDetail TD,
    Transaction_Export_Folder TEF,
    Period P,
    Document D,
    DocumentType DT,
    Account A,
    party PY,
    party_type PT,
    insurance_file IFI,
    risk_code RC
WHERE
    TD.insurance_ref = TEF.insurance_ref
AND TD.period_id = P.period_id
AND TD.document_id = D.document_id
AND D.documenttype_id = DT.documenttype_id
-- Select Document date within supplied date range
AND (
    D.document_date >= @start_date
    AND D.document_date <= @end_date
    )
AND TD.account_id = A.account_id
AND A.short_code = PY.shortname
AND PY.party_type_id = PT.party_type_id
AND TEF.insurance_file_cnt = IFI.insurance_file_cnt
AND TEF.document_ref = D.document_ref
AND TEF.accounts_export_status = 'c'
AND TEF.source_id = D.company_id
AND IFI.risk_code_id = RC.risk_code_id
AND PT.code IN ('PC', 'CC', 'GC')
AND D.documenttype_id IN (5, 16, 18, 32, 36)
-- Select on supplied branch parameter
AND (
    @iBranchID = 0
    OR
        (
        @iBranchID <> 0
        AND
        A.company_id = @iBranchID
        )
    )
-- Select only Credit transactions
AND TD.amount <= 0
ORDER BY
    D.document_id

-- Select Reversals
INSERT INTO report_audit_debit_table1
SELECT DISTINCT
    P.year_name,
    P.period_name,
    D.document_id,
    TD.accounting_date,
    TD.insurance_ref,
    D.comment,
    A.short_code,
    D.Document_ref,
    DT.description document_type,
    PT.code party_type,
    PT.description type_description,
    '',
    '',
    D.document_date,
    TD.amount,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    PY.resolved_name resolved_name,
    D.document_date,
    ''
FROM TransDetail TD,
    Period P,
    Document D,
    DocumentType DT,
    Account A,
    party PY,
    party_type PT
WHERE TD.period_id = P.period_id
AND TD.document_id = D.document_id
AND D.documenttype_id = DT.documenttype_id
AND (
    D.document_date >= @start_date
    AND D.document_date <= @end_date
    )
AND TD.account_id = A.account_id
AND A.short_code = PY.shortname
AND PY.party_type_id = PT.party_type_id
AND PT.code IN ('PC', 'CC', 'GC')
AND D.documenttype_id IN (5, 16, 18, 32, 36)

--AND TD.spare = 'Reversal'		-- RAG 2004-08-27
AND TD.spare like 'Reversal%'

AND TD.amount >= 0
AND (
    @iBranchID = 0
    OR
        (
        @iBranchID <> 0
        AND
        A.company_id = @iBranchID
        )
    )
ORDER BY
    D.document_id

-- Update Reversals with Policy details where applicable
DECLARE c_Cursor CURSOR FAST_FORWARD FOR
    SELECT DISTINCT
        R.transdetail_id,
        SUBSTRING(D.comment, 22, 11),
        D.company_id
    FROM report_audit_debit_table1 R,
        Document D
    WHERE D.document_id = R.transdetail_id
    AND D.comment like 'Reversal of Document%'
    ORDER BY R.transdetail_id

OPEN c_Cursor

FETCH NEXT FROM c_Cursor INTO @document_id,
                @OrigDocRef,
                @iCompanyID

WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE report_audit_debit_table1
    SET risk_code = RC.code,
        risk_description = RC.description,
        commission_percentage = IFI.commission_percentage,
        account_handler =
        (
            SELECT ISNULL (resolved_name, '')
            FROM Party
            WHERE party_cnt = PY.consultant_cnt
        )
    FROM TransDetail TD,
        Transaction_Export_Folder TEF,
        Document D,
        Account A,
        party PY,
        insurance_file IFI,
        risk_code RC
    WHERE D.document_id = @document_id
    AND TEF.document_ref = D.document_ref
    AND TEF.source_id = @iCompanyID
    AND TEF.accounts_export_status = 'c'
    AND IFI.insurance_file_cnt = TEF.insurance_file_cnt
    AND RC.risk_code_id = IFI.risk_code_id

    FETCH NEXT FROM c_Cursor INTO @document_id,
                    @OrigDocRef,
                    @iCompanyID
END

CLOSE c_cursor
DEALLOCATE c_cursor

-- Process each Client Transaction
DECLARE c_Cursor CURSOR FAST_FORWARD FOR
    SELECT DISTINCT
        transdetail_id
    FROM report_audit_debit_table1
    ORDER BY transdetail_id

OPEN c_Cursor

FETCH NEXT FROM c_Cursor INTO @document_id

WHILE @@FETCH_STATUS = 0
BEGIN
    DELETE FROM report_audit_debit_table2

    -- Set counter
    SELECT @old_main_key = ISNULL(MAX(main_key), 0)
    FROM report_audit_debit_table3

    -- Select Whole Document into Table 2
    INSERT INTO report_audit_debit_table2
    SELECT TD.transdetail_id,
        A.account_id,
        A.Ledger_id,
        TD.amount,
        ISNULL(RTRIM(TD.spare), '')
    FROM TransDetail TD,
        Account A
    WHERE
        TD.document_id = @document_id
    AND A.Account_id = TD.Account_id

    -- Remove Comm Adjustments (not part of orig. debit)
    /* Delete contra entries for any COMM ADJ items */
    DELETE
    FROM report_audit_debit_table2
    WHERE transdetail_id IN
    (
        SELECT transdetail_id + 1
        FROM report_audit_debit_table2
        WHERE (
                comment = 'COMM ADJ'
                OR
                comment = 'AGENT ADJ'
            )
    )
    /* Delete primary entries for any COMM ADJ items */
    DELETE
    FROM report_audit_debit_table2
    WHERE (
            comment = 'COMM ADJ'
            OR
            comment = 'AGENT ADJ'
        )

    -- Select Client Transactions into Table 3
    INSERT INTO report_audit_debit_table3
    ( year_name,
        period_name,
        transdetail_id,
        accounting_date,
        insurance_ref,
        comment,
        short_code,
        Document_ref,
        document_type,
        party_type,
        type_description,
        risk_code,
        risk_description,
        cover_start_date,
        this_premium,
        insurer_premium,
        net_commission,
        fees,
        agent_fees,
        commission_amount,
        disc,
        sub_agent_fees,
        commission_percentage,
        resolved_name,
        document_date,
        account_id,
        insurer_premium_short_code,
        insurer_commission_short_code,
        agent_fees_short_code,
        sub_agent_short_code,
        fees_short_code,
        disc_short_code,
        net_commission_short_code,
        account_handler)
    SELECT DISTINCT
        T1.year_name,
        T1.period_name,
        T1.transdetail_id,
        T1.accounting_date,
        T1.insurance_ref,
        T1.comment,
        T1.short_code,
        T1.Document_ref,
        T1.document_type,
        T1.party_type,
        T1.type_description,
        T1.risk_code,
        T1.risk_description,
        T1.cover_start_date,
        (
        SELECT SUM(amount)
        FROM report_audit_debit_table2
        WHERE account_id = A.account_id
        ) this_premium,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        T1.resolved_name,
        T1.document_date,
        T2.account_id,
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        T1.account_handler
        FROM report_audit_debit_table1 T1,
            report_audit_debit_table2 T2,
            Account A
        WHERE
            T1.transdetail_id = @document_id
        AND T2.ledger_id = 2
        AND T2.account_id = A.account_id
        AND T1.short_code = A.short_code

    -- Select 1st Insurer Transactions into Table 3
    UPDATE report_audit_debit_table3
    SET insurer_premium = T2.amount,
        account_id = A.account_id,
        insurer_premium_short_code = A.short_code
    FROM report_audit_debit_table1 T1,
        report_audit_debit_table2 T2,
        Account A
    WHERE main_key = @old_main_key + 1
    AND T1.transdetail_id = @document_id
    AND T2.ledger_id = 4
    AND T2.account_id = A.account_id
    AND (
            (
            T2.amount > 0
            --AND ISNULL(T2.comment, '') <> 'Reversal'      -- RAG 2004-08-27
            AND ISNULL(T2.comment, '') NOT LIKE 'Reversal%'
            )
            OR
            (
            T2.amount < 0
            --AND ISNULL(T2.comment, '') = 'Reversal'		-- RAG 2004-08-27
            AND ISNULL(T2.comment, '') LIKE 'Reversal%'	
            )
        )
    AND A.short_code =
        (
            SELECT MIN(DISTINCT A.short_code)
            FROM report_audit_debit_table2 R,
                .Account A
            WHERE A.account_id = R.account_id
            AND A.ledger_id = 4
        )

    -- Add Additional Insurers to Table 3
    IF
    (
        SELECT COUNT(DISTINCT account_id)
        FROM report_audit_debit_table2
        WHERE ledger_id = 4
    ) > 1
    BEGIN
        INSERT INTO report_audit_debit_table3
        ( year_name,
            period_name,
            transdetail_id,
            accounting_date,
            insurance_ref,
            comment,
            short_code,
            Document_ref,
            document_type,
            party_type,
            type_description,
            risk_code,
            risk_description,
            cover_start_date,
            this_premium,
            insurer_premium,
            net_commission,
            fees,
            agent_fees,
            commission_amount,
            disc,
            sub_agent_fees,
            commission_percentage,
            resolved_name,
            document_date,
            account_id,
            insurer_premium_short_code,
            insurer_commission_short_code,
            agent_fees_short_code,
            sub_agent_short_code,
            fees_short_code,
            disc_short_code,
            net_commission_short_code,
            account_handler)
        SELECT
            T1.year_name,
            T1.period_name,
            T1.transdetail_id,
            T1.accounting_date,
            T1.insurance_ref,
            T1.comment,
            T1.short_code,
            T1.Document_ref,
            T1.document_type,
            T1.party_type,
            T1.type_description,
            T1.risk_code,
            T1.risk_description,
            T1.cover_start_date,
            0,
            T2.amount insurer_premium,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            T1.resolved_name,
            T1.document_date,
            T2.account_id,
            A.short_code insurer_premium_short_code,
            "",
            "",
            "",
            "",
            "",
            "",
            T1.account_handler
        FROM report_audit_debit_table1 T1,
            report_audit_debit_table2 T2,
            Account A
        WHERE
            T1.transdetail_id = @document_id
        AND T2.ledger_id = 4
        AND T2.account_id = A.account_id
        AND (
                (
                T2.amount > 0
                --AND ISNULL(T2.comment, '') <> 'Reversal'      -- RAG 2004-08-27
                AND ISNULL(T2.comment, '') NOT LIKE 'Reversal%'
                )
                OR
                (
                T2.amount < 0
                --AND ISNULL(T2.comment, '') = 'Reversal'      -- RAG 2004-08-27
                AND ISNULL(T2.comment, '') LIKE 'Reversal%'
                )
            )
        AND A.short_code <>
            (
                SELECT MIN(DISTINCT A.short_code)
                FROM report_audit_debit_table2 R,
                    .Account A
                WHERE A.account_id = R.account_id
                AND A.ledger_id = 4
            )

        ORDER BY T2.transdetail_id
    END

    -- Delete Insurer Premiums from table 2
    DELETE
    FROM report_audit_debit_table2
    WHERE ledger_id = 4
    AND (
            (
            amount > 0
            --AND ISNULL(comment, '') <> 'Reversal'      -- RAG 2004-08-27
            AND ISNULL(comment, '') NOT LIKE 'Reversal%'
            )
            OR
            (
            amount < 0
            --AND ISNULL(comment, '') = 'Reversal'      -- RAG 2004-08-27
            AND ISNULL(comment, '') LIKE 'Reversal%'
            )
        )

    -- Set Commission Amounts
    UPDATE report_audit_debit_table3
    SET commission_amount = -R2.amount,
        insurer_commission_short_code = A.short_code
    FROM report_audit_debit_table3 R3,
        report_audit_debit_table2 R2,
        .Account A
    WHERE R3.account_id = R2.account_id
    AND R3.main_key > @old_main_key
    AND A.account_id = R2.account_id
    AND R2.ledger_id = 4

    -- Delete Commission Amounts from table 2
    DELETE FROM report_audit_debit_table2
    WHERE ledger_id = 4

    -- Set Net Commissions
    UPDATE report_audit_debit_table3
    SET net_commission = amount,
        net_commission_short_code = A.short_code
    FROM report_audit_debit_table2 R2,
        .Account A
    WHERE main_key = @old_main_key + 1
    AND A.account_id = R2.account_id
    AND R2.ledger_id = 9

    -- Delete Net Commissions from table 2
    DELETE FROM report_audit_debit_table2
    WHERE ledger_id = 9

    -- Set Agent
    UPDATE report_audit_debit_table3
    SET agent_fees = amount,
        agent_fees_short_code = A.short_code
    FROM report_audit_debit_table2 R2,
        .Account A
    WHERE main_key = @old_main_key + 1
    AND A.account_id = R2.account_id
    AND R2.ledger_id = 5

    -- Delete Agents from table 2
    DELETE FROM report_audit_debit_table2
    WHERE ledger_id = 5

    -- Set Sub-Agent
    UPDATE report_audit_debit_table3
    SET sub_agent_fees = amount,
        sub_agent_short_code = A.short_code
    FROM report_audit_debit_table2 R2,
        .Account A
    WHERE main_key = @old_main_key + 1
    AND A.account_id = R2.account_id
    AND R2.ledger_id = 10

    -- Delete Sub-Agents from table 2
    DELETE FROM report_audit_debit_table2
    WHERE ledger_id = 10

    -- Select 1st Fee into Table 3
    UPDATE report_audit_debit_table3
    SET fees = T2.amount,
        fees_short_code = A.short_code
    FROM report_audit_debit_table1 T1,
        report_audit_debit_table2 T2,
        Account A
    WHERE main_key = @old_main_key + 1
    AND T1.transdetail_id = @document_id
    AND T2.ledger_id = 7
    AND T2.account_id = A.account_id
    AND A.short_code =
        (
            SELECT MIN(DISTINCT A.short_code)
            FROM report_audit_debit_table2 R,
                .Account A
            WHERE A.account_id = R.account_id
            AND A.ledger_id = 7
        )

    -- Add Additional Fees to Table 3
    IF
    (
        SELECT COUNT(DISTINCT account_id)
        FROM report_audit_debit_table2
        WHERE ledger_id = 7
    ) > 1
    BEGIN
        INSERT INTO report_audit_debit_table3
        ( year_name,
            period_name,
            transdetail_id,
            accounting_date,
            insurance_ref,
            comment,
            short_code,
            Document_ref,
            document_type,
            party_type,
            type_description,
            risk_code,
            risk_description,
            cover_start_date,
            this_premium,
            insurer_premium,
            net_commission,
            fees,
            agent_fees,
            commission_amount,
            disc,
            sub_agent_fees,
            commission_percentage,
            resolved_name,
            document_date,
            account_id,
            insurer_premium_short_code,
            insurer_commission_short_code,
            agent_fees_short_code,
            sub_agent_short_code,
            fees_short_code,
            disc_short_code,
            net_commission_short_code,
            account_handler)
        SELECT
            T1.year_name,
            T1.period_name,
            T1.transdetail_id,
            T1.accounting_date,
            T1.insurance_ref,
            T1.comment,
            T1.short_code,
            T1.Document_ref,
            T1.document_type,
            T1.party_type,
            T1.type_description,
            T1.risk_code,
            T1.risk_description,
            T1.cover_start_date,
            0,
            0,
            0,
            T2.amount fees,
            0,
            0,
            0,
            0,
            0,
            T1.resolved_name,
            T1.document_date,
            T2.account_id,
            "",
            "",
            "",
            "",
            A.short_code fees_short_code,
            "",
            "",
            T1.account_handler
        FROM report_audit_debit_table1 T1,
            report_audit_debit_table2 T2,
            Account A
        WHERE
            T1.transdetail_id = @document_id
        AND T2.ledger_id = 7
        AND T2.account_id = A.account_id
        AND A.short_code <>
            (
                SELECT MIN(DISTINCT A.short_code)
                FROM report_audit_debit_table2 R,
                    .Account A
                WHERE A.account_id = R.account_id
                AND A.ledger_id = 7
            )

        ORDER BY T2.transdetail_id
    END

    -- Delete Fees from table 2
    DELETE FROM report_audit_debit_table2
    WHERE ledger_id = 7

    -- Select 1st Discount into Table 3
    UPDATE report_audit_debit_table3
    SET disc = T2.amount,
        disc_short_code = A.short_code
    FROM report_audit_debit_table1 T1,
        report_audit_debit_table2 T2,
        Account A
    WHERE main_key = @old_main_key + 1
    AND T1.transdetail_id = @document_id
    AND T2.ledger_id = 8
    AND T2.account_id = A.account_id
    AND A.short_code =
        (
            SELECT MIN(DISTINCT A.short_code)
            FROM report_audit_debit_table2 R,
                .Account A
            WHERE A.account_id = R.account_id
            AND A.ledger_id = 8
        )

    -- Add Additional Discounts to Table 3
    IF
    (
        SELECT COUNT(DISTINCT account_id)
        FROM report_audit_debit_table2
        WHERE ledger_id = 8
    ) > 1
    BEGIN
        INSERT INTO report_audit_debit_table3
        ( year_name,
            period_name,
            transdetail_id,
            accounting_date,
            insurance_ref,
            comment,
            short_code,
            Document_ref,
            document_type,
            party_type,
            type_description,
            risk_code,
            risk_description,
            cover_start_date,
            this_premium,
            insurer_premium,
            net_commission,
            fees,
            agent_fees,
            commission_amount,
            disc,
            sub_agent_fees,
            commission_percentage,
            resolved_name,
            document_date,
            account_id,
            insurer_premium_short_code,
            insurer_commission_short_code,
            agent_fees_short_code,
            sub_agent_short_code,
            fees_short_code,
            disc_short_code,
            net_commission_short_code,
            account_handler)
        SELECT
            T1.year_name,
            T1.period_name,
            T1.transdetail_id,
            T1.accounting_date,
            T1.insurance_ref,
            T1.comment,
            T1.short_code,
            T1.Document_ref,
            T1.document_type,
            T1.party_type,
            T1.type_description,
            T1.risk_code,
            T1.risk_description,
            T1.cover_start_date,
            0,
            0,
            0,
            0,
            0,
            0,
            T2.amount disc,
            0,
            0,
            T1.resolved_name,
            T1.document_date,
            T2.account_id,
            "",
            "",
            "",
            "",
            "",
            A.short_code disc_short_code,
            "",
            T1.account_handler
        FROM report_audit_debit_table1 T1,
            report_audit_debit_table2 T2,
            Account A
        WHERE
            T1.transdetail_id = @document_id
        AND T2.ledger_id = 8
        AND T2.account_id = A.account_id
        AND A.short_code <>
            (
                SELECT MIN(DISTINCT A.short_code)
                FROM report_audit_debit_table2 R,
                    .Account A
                WHERE A.account_id = R.account_id
                AND A.ledger_id = 8
            )

        ORDER BY T2.transdetail_id
    END

    -- Delete Discounts from table 2
    DELETE FROM report_audit_debit_table2
    WHERE ledger_id = 8

    FETCH NEXT FROM c_Cursor INTO @document_id
END

CLOSE c_cursor
DEALLOCATE c_cursor

/* TF300600 - remove NULL values */
UPDATE report_audit_debit_table3
SET this_premium = ISNULL(this_premium, 0),
    insurer_premium = ISNULL(insurer_premium, 0),
    commission_amount = ISNULL(commission_amount, 0),
    net_commission = ISNULL(net_commission, 0),
    fees = ISNULL(fees, 0),
    agent_fees = ISNULL(agent_fees, 0),
    sub_agent_fees = ISNULL(sub_agent_fees, 0),
    disc = ISNULL(disc, 0)

/* Select everything now */
SELECT * FROM report_audit_debit_table3
ORDER by main_key DESC

GO


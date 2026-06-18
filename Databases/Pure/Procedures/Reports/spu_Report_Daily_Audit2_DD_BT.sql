SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Daily_Audit2_DD_BT'
GO

/*eck 270603 PN5069 Show Cover Start Date */
CREATE PROCEDURE spu_Report_Daily_Audit2_DD_BT
    @branch_id int,
    @start_date datetime,
    @end_date datetime,
    @date_type varchar(11)
AS

DECLARE @transdetail_id INT
DECLARE @main_key INT
DECLARE @old_main_key INT

DECLARE @account_id INT
DECLARE @amount numeric(19, 4)

DECLARE @iBranchID int

DELETE FROM report_audit_debit_table1
DELETE FROM report_audit_debit_table2
DELETE FROM report_audit_debit_table3

SELECT @main_key = 0
SELECT @old_main_key = 0

SELECT @iBranchID = ISNULL(@branch_id, 0)

-- Select Client Transaction into Table 1
IF @iBranchID = 0
BEGIN
    INSERT INTO report_audit_debit_table1
    SELECT DISTINCT
        P.year_name,
        P.period_name,
        TD.transdetail_id,
        TD.accounting_date,
        TD.insurance_ref,
        D.comment,
        A.short_code,
        D.Document_ref,
        DT.description document_type,
        null,
        null,
        null,
        null,
        I.cover_start_date,			--PN5069
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        null,
        null,
        D.document_date,
        BT.description
    FROM TransDetail TD,
        Period P,
        Document D,
        DocumentType DT,
        Account A,
        insurance_file IFI,
	party PA,				-- 1.6.9
        business_type BT,
	Insurance_File I			--PN5069
    WHERE
        TD.period_id = P.period_id
    AND TD.document_id = D.document_id
    AND D.insurance_file_cnt = I.insurance_file_cnt	--PN5069
    AND D.documenttype_id = DT.documenttype_id
--eck100602 extra selection based on date type
    AND (
        (
        D.document_date >= @start_date
        AND D.document_date <= @end_date
        AND @date_type = 'Transaction'
        )
    OR (
        TD.accounting_date >= @start_date
        AND TD.accounting_date <= @end_date
        AND @date_type = 'Effective'
        )
    )
    AND TD.account_id = A.account_id
    AND D.documenttype_id = 34
    AND A.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
    AND IFI.insurance_ref = TD.insurance_ref
    AND BT.business_type_id = IFI.business_type_id
    AND	IFI.insured_cnt = PA.party_cnt			--1.6.9
    AND	a.account_key = PA.party_cnt			--1.6.9
--eck100602 extra check to only get one client record for the policy holder
    AND TD.document_sequence = (SELECT min(document_sequence) from transdetail t2,
                    account a2
                    where t2.document_Id = D.document_Id
                    and t2.account_id = a2.account_id
                    and a2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')) --PSL16/06/2003 Remove Hard coded ledger_id)
    ORDER BY transdetail_id
END
ELSE
BEGIN
    INSERT INTO report_audit_debit_table1
    SELECT DISTINCT
        P.year_name,
        P.period_name,
        TD.transdetail_id,
        TD.accounting_date,
        TD.insurance_ref,
        D.comment,
        A.short_code,
        D.Document_ref,
        DT.description document_type,
        null,
        null,
        null,
        null,
        I.cover_start_date,			--PN5069
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        null,
        null,
        D.document_date,
        BT.description
    FROM TransDetail TD,
        Period P,
        Document D,
        DocumentType DT,
        Account A,
        insurance_file IFI,
	party PA,				-- 1.6.9
        business_type BT,
	Insurance_File I			--PN5069
    WHERE
        TD.period_id = P.period_id
    AND TD.document_id = D.document_id
    AND D.insurance_file_cnt = I.insurance_file_cnt	--PN5069
    AND D.documenttype_id = DT.documenttype_id
--eck310502 extra selection based on date type
    AND (
        (
        D.document_date >= @start_date
        AND D.document_date <= @end_date
        AND @date_type = 'Transaction'
        )
    OR (
        TD.accounting_date >= @start_date
        AND TD.accounting_date <= @end_date
        AND @date_type = 'Effective'
        )
        )
    AND TD.account_id = A.account_id
    AND D.documenttype_id = 34
    AND A.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
    --DC031201 changed to use company on transaction, not account
    --AND A.company_id = @iBranchID
    AND TD.company_id = @iBranchId
    AND IFI.insurance_ref = TD.insurance_ref
    AND	IFI.insured_cnt = PA.party_cnt
    AND	a.account_key = PA.party_cnt
    AND BT.business_type_id = IFI.business_type_id
--eck210802 end    
--eck100602 extra check to only get one client record for the policy holder
    AND TD.document_sequence = (SELECT min(document_sequence) from transdetail
                    where document_Id = D.document_Id
                    and ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')) --PSL16/06/2003 Remove Hard coded ledger_id)
    ORDER BY transdetail_id
END

-- Process each Client Transaction
DECLARE c_Cursor CURSOR FAST_FORWARD FOR
    SELECT transdetail_id
    FROM report_audit_debit_table1
    ORDER BY transdetail_id DESC

OPEN c_Cursor

FETCH NEXT FROM c_Cursor INTO @transdetail_id

WHILE @@FETCH_STATUS = 0 BEGIN
    /* Get the correct value for this_premium */

    DELETE FROM report_audit_debit_table2

    -- Select Whole Document into Table 2
    INSERT INTO report_audit_debit_table2
    SELECT
        TD2.transdetail_id,
        A.account_id,
        A.Ledger_id,
        TD2.amount,
        ISNULL(RTRIM(TD2.spare), '')
    FROM TransDetail TD,
        TransDetail TD2,
        Account A
    WHERE
        TD2.document_id = TD.document_id
    AND TD2.Account_id = A.Account_id
    AND TD.transdetail_id = @transdetail_id

    -- Remove Comm Adjustments (not part of orig. debit)
    /* Delete contra entries for any COMM ADJ items */
    DELETE
    FROM report_audit_debit_table2
    WHERE transdetail_id IN
    (
        SELECT transdetail_id + 1
        FROM report_audit_debit_table2
        WHERE comment = 'COMM ADJ'
    )
    /* Delete primary entries for any COMM ADJ items */
    DELETE
    FROM report_audit_debit_table2
    WHERE comment = 'COMM ADJ'

    -- Select Insurer Transactions into Table 3
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
        T1.this_premium,
        T2.amount,
        T1.net_commission,
        T1.fees,
        T1.agent_fees,
        T1.commission_amount,
        T1.disc,
        T1.sub_agent_fees,
        T1.commission_percentage,
        T1.resolved_name,
        T1.document_date,
        T2.account_id,
        A.short_code,
        '',
        '',
        '',
        '',
        '',
        '',
        T1.account_handler
        FROM report_audit_debit_table1 T1,
            report_audit_debit_table2 T2,
            Account A
        WHERE
            T1.transdetail_id = @transdetail_id
        AND (
            (
            T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
            AND T2.amount < 0
            AND T2.comment <> 'Reversal'
            )
        OR
            (
            T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
            AND T2.amount > 0
            AND T2.comment = 'Reversal'
            )
        )
        AND T2.account_id = A.account_id
        ORDER BY T2.amount

    -- Set pointer to last record just entered
    SELECT @main_key = max(main_key)
    FROM report_audit_debit_table3

    -- TF210201 - Fix for 1st item
    IF (@old_main_key = 0)
        SELECT @old_main_key = @main_key - 1

    -- Update Insurer record with gross premium from Client record
--eck100602 need to total client amounts
    UPDATE report_audit_debit_table3
    SET this_premium = (SELECT sum(amount)
                FROM report_audit_debit_table2
                WHERE
                (
                    (
                    ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
                    AND amount > 0
                    AND comment <> 'Reversal'
                    )
                OR
                    (
                    ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
                    AND amount < 0
                    AND comment = 'Reversal'
                    )
                ))

    WHERE main_key = @old_main_key + 1

    -- Delete Client debits from Table 2
    DELETE FROM report_audit_debit_table2
    WHERE
    (
        (
        ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
        AND amount > 0
        AND comment <> 'Reversal'
        )
    OR
        (
        ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
        AND amount < 0
        AND comment = 'Reversal'
        )
    )

    -- Select any Client credits into Table 3
    IF EXISTS (SELECT * FROM report_audit_debit_table2 WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')) --PSL16/06/2003 Remove Hard coded ledger_id)
    BEGIN
        INSERT INTO report_audit_debit_table3
        (
            year_name,
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
            T3.year_name,
            T3.period_name,
            T3.transdetail_id,
            T3.accounting_date,
            T3.insurance_ref,
            T3.comment,
            T3.short_code,
            T3.Document_ref,
            T3.document_type,
            T3.party_type,
            T3.type_description,
            T3.risk_code,
            T3.risk_description,
            T3.cover_start_date,
            - T3.this_premium,
            - T3.insurer_premium,
            T3.net_commission,
            T3.fees,
            T3.agent_fees,
            T3.commission_amount,
            T3.disc,
            T3.sub_agent_fees,
            T3.commission_percentage,
            T3.resolved_name,
            T3.document_date,
            T3.account_id,
            T3.insurer_premium_short_code,
            T3.insurer_commission_short_code,
            T3.agent_fees_short_code,
            T3.sub_agent_short_code,
            T3.fees_short_code,
            T3.disc_short_code,
            T3.net_commission_short_code,
            T3.account_handler
            FROM report_audit_debit_table3 T3
        WHERE
            T3.transdetail_id = @transdetail_id
        ORDER BY main_key

    END

    DELETE FROM report_audit_debit_table2
    WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id

    -- Update Pointer
    SELECT @main_key = max(main_key)
    FROM report_audit_debit_table3

    -- Select Insurer amounts
    DECLARE c_Cursor2 CURSOR FAST_FORWARD FOR
        SELECT amount
        FROM report_audit_debit_table2
        WHERE
        (
            (
            ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
            AND amount < 0
            AND comment <> 'Reversal'
            )
        OR
            (
            ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
            AND amount > 0
            AND comment = 'Reversal'
            )
        )
        ORDER BY amount

    OPEN c_Cursor2

    FETCH NEXT FROM c_Cursor2 INTO @amount

    WHILE @@FETCH_STATUS = 0 BEGIN
        DELETE FROM report_audit_debit_table2
        WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
        AND amount = -@amount

        FETCH NEXT FROM c_Cursor2 INTO @amount
    END

    /* Clear up our cursor */
    CLOSE c_Cursor2
    DEALLOCATE c_Cursor2

    DELETE FROM report_audit_debit_table2
    WHERE
    (
        (
        ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
        AND amount < 0
        AND comment <> 'Reversal'
        )
    OR
        (
        ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
        AND amount > 0
        AND comment = 'Reversal'
    ))

    -- Update Net Commissions
    UPDATE report_audit_debit_table3
    SET net_commission = amount,
        net_commission_short_code = A.short_code
    FROM Account A,
        report_audit_debit_table2 T2
    WHERE
        T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'CO') --PSL16/06/2003 Remove Hard coded ledger_id
    AND T2.account_id = A.account_id
    AND main_key = @old_main_key + 1
--eck100602 sum all amounts and select single code as there could be more than one
    UPDATE report_audit_debit_table3
    SET agent_fees = (SELECT sum(amount)
                FROM report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG')), --PSL16/06/2003 Remove Hard coded ledger_id
        agent_fees_short_code = (SELECT max(A.short_code)
                        FROM Account A,
                            report_audit_debit_table2 T2
                        WHERE T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG') --PSL16/06/2003 Remove Hard coded ledger_id
                        AND T2.account_id = A.account_id),
        fees = (SELECT sum(amount)
                FROM report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE')), --PSL16/06/2003 Remove Hard coded ledger_id
        fees_short_code = (SELECT max(A.short_code)
                        FROM Account A,
                            report_audit_debit_table2 T2
                        WHERE T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE') --PSL16/06/2003 Remove Hard coded ledger_id
                        AND T2.account_id = A.account_id),
        disc = (SELECT sum(amount)
                FROM report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'DI')), --PSL16/06/2003 Remove Hard coded ledger_id
        disc_short_code = (SELECT max(A.short_code)

                        FROM Account A,
                            report_audit_debit_table2 T2
                        WHERE T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'DI') --PSL16/06/2003 Remove Hard coded ledger_id
                        AND T2.account_id = A.account_id),
        sub_agent_fees = (SELECT sum(amount)
                FROM report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')), --PSL16/06/2003 Remove Hard coded ledger_id

        sub_agent_short_code = (SELECT max(A.short_code)
                        FROM Account A,
                            report_audit_debit_table2 T2
                        WHERE T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB') --PSL16/06/2003 Remove Hard coded ledger_id
                        AND T2.account_id = A.account_id)
    WHERE main_key > @old_main_key

    --PSL30/06/2003 Remove Hard coded ledger_id
    DELETE FROM report_audit_debit_table2
    FROM report_audit_debit_table2 r2, Ledger l
    WHERE r2.ledger_id =l.ledger_id
    AND l.ledger_short_name IN ('AG', 'FE', 'DI', 'CO', 'UB')

    DECLARE c_Cursor3 CURSOR FAST_FORWARD FOR
        SELECT account_id, amount
        FROM report_audit_debit_table2

    OPEN c_Cursor3

    FETCH NEXT FROM c_Cursor3 INTO @account_id, @amount

    WHILE @@FETCH_STATUS = 0 BEGIN
        UPDATE report_audit_debit_table3
        SET commission_amount = -@amount,
                insurer_commission_short_code = A.short_code
        FROM report_audit_debit_table3 T,
                Account A
        WHERE T.main_key > @old_main_key
        AND A.account_id = T.account_id
        AND A.account_id = @account_id

        FETCH NEXT FROM c_Cursor3 INTO @account_id, @amount
    END

    /* Clear up our cursor */
    CLOSE c_Cursor3
    DEALLOCATE c_Cursor3

    SELECT @old_main_key = @main_key

    /* Move to the next row */
    FETCH NEXT FROM c_Cursor INTO @transdetail_id
END

/* Clear up our cursor */
CLOSE c_Cursor
DEALLOCATE c_Cursor

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
SELECT radt.*, branch = c.description
FROM report_audit_debit_table3 radt
JOIN .transdetail td
ON radt.transdetail_id = td.transdetail_id
JOIN .company c
ON td.company_id = c.company_id
ORDER by main_key DESC

GO

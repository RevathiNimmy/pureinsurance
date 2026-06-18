SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Audit_Direct_To_Insurer'
GO

CREATE PROCEDURE spu_Report_Audit_Direct_To_Insurer 
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME
AS

DECLARE @transdetail_id INT
DECLARE @main_key INT
DECLARE @old_main_key INT

DECLARE @account_id INT
DECLARE @amount MONEY

DECLARE @iBranchID INT

SELECT @main_key = 0
SELECT @old_main_key = 0

SELECT @iBranchID = ISNULL(@branch_id, 0)

CREATE TABLE #report_audit_debit_table1
 (
	year_name VARCHAR (20),
	period_name VARCHAR (15),
	transdetail_id INT,
	accounting_date DATETIME ,
	insurance_ref VARCHAR (30),
	comment VARCHAR (60) ,
	short_code VARCHAR (30),
	Document_ref VARCHAR (25),
	document_type VARCHAR (255),
	party_type VARCHAR (10),
	type_description VARCHAR (255),
	risk_code VARCHAR (10),
	risk_description VARCHAR (255),
	cover_start_date DATETIME,
	this_premium NUMERIC(19, 4),
	insurer_premium NUMERIC(19, 4) ,
	net_commission NUMERIC(19, 4),
	fees NUMERIC(19, 4),
	agent_fees NUMERIC(19, 4),
	commission_amount NUMERIC(19, 4),
	disc NUMERIC(19, 4),
	sub_agent_fees NUMERIC(19, 4),
	commission_percentage NUMERIC(7, 4),
	resolved_name VARCHAR (100),
	document_date DATETIME,
	account_handler VARCHAR (100),
	documenttype_id INT
) 

CREATE TABLE #report_audit_debit_table2
(
	transdetail_id INT,
	account_id INT,
	ledger_id INT,
	amount NUMERIC(19, 4),
	comment VARCHAR (20)
) 

CREATE TABLE #report_audit_debit_table3
(
	main_key INT IDENTITY (1, 1),
	year_name VARCHAR (20),
	period_name VARCHAR (15),
	transdetail_id INT,
	accounting_date DATETIME,
	insurance_ref VARCHAR (30),
	comment VARCHAR (60),
	short_code VARCHAR (30),
	Document_ref VARCHAR (25),
	document_type VARCHAR (255),
	party_type VARCHAR (10),
	type_description VARCHAR (255),
	risk_code VARCHAR (10),
	risk_description VARCHAR (255),
	cover_start_date DATETIME ,
	this_premium NUMERIC(19, 4),
	insurer_premium NUMERIC(19, 4),
	net_commission NUMERIC(19, 4),
	fees NUMERIC(19, 4),
	agent_fees NUMERIC(19, 4),
	commission_amount NUMERIC(19, 4),
	disc NUMERIC(19, 4),
	sub_agent_fees NUMERIC(19, 4),
	commission_percentage NUMERIC(7, 4),
	resolved_name VARCHAR (100),
	document_date DATETIME ,
	account_id INT,
	insurer_premium_short_code VARCHAR (20),
	insurer_commission_short_code VARCHAR (20),
	agent_fees_short_code VARCHAR (20),
	sub_agent_short_code VARCHAR (20),
	fees_short_code VARCHAR (20),
	disc_short_code VARCHAR (20),
	net_commission_short_code VARCHAR (20),
	account_handler VARCHAR (100),
	company_id INT,
	company_desc VARCHAR (255),
	documenttype_id INT 
) 

-- Select Client Transaction INTo Table 1
IF @iBranchID = 0
BEGIN
    INSERT INTO #report_audit_debit_table1
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
        I.cover_start_date,			
        I.this_premium,                        
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
        '',
	D.documenttype_id
    FROM TransDetail TD,
        Period P,
        Document D,
        DocumentType DT,
        Account A,
	Insurance_File I			
    WHERE
        TD.period_id = P.period_id
    AND TD.document_id = D.document_id
    AND D.insurance_file_cnt = I.insurance_file_cnt	
    AND D.documenttype_id = DT.documenttype_id
    AND (
        D.document_date >= @start_date
        AND D.document_date <= @end_date
        )
    AND TD.account_id = A.account_id
    AND D.documenttype_id IN (33,34)
    AND A.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') 
--eck200802 extra check to only get one client record for the policy holder
	AND	TD.document_sequence = (SELECT min(document_sequence) from transdetail t2,
					account a2
					where t2.document_Id = D.document_Id
					and t2.account_id = a2.account_id
					and a2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA'))	ORDER BY transdetail_id 
END
ELSE
BEGIN
    INSERT INTO #report_audit_debit_table1
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
        I.cover_start_date,			
        I.this_premium,                         
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
        '',
	D.documenttype_id
    FROM TransDetail TD,
        Period P,
        Document D,
        DocumentType DT,
        Account A,
	Insurance_File I			
    WHERE
        TD.period_id = P.period_id
    AND TD.document_id = D.document_id    
    AND D.insurance_file_cnt = I.insurance_file_cnt	
    AND D.documenttype_id = DT.documenttype_id
    AND (
        D.document_date >= @start_date
        AND D.document_date <= @end_date
        )
    AND TD.account_id = A.account_id
    AND D.documenttype_id IN (33,34)
    AND A.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') 
    --DC031201 changed to use company on transaction, not account
    AND TD.company_id = @iBranchId
--eck200802 extra check to only get one client record for the policy holder
    AND	TD.document_sequence = (SELECT min(document_sequence) from transdetail
					where document_Id = D.document_Id
					and ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')) 
    ORDER BY transdetail_id
END

-- Process each Client Transaction
DECLARE c_Cursor CURSOR FAST_FORWARD FOR
    SELECT transdetail_id
    FROM #report_audit_debit_table1
    ORDER BY transdetail_id DESC

OPEN c_Cursor

FETCH NEXT FROM c_Cursor INTO @transdetail_id

WHILE @@FETCH_STATUS = 0
BEGIN /* Get the correct value for this_premium */

    DELETE FROM #report_audit_debit_table2

    -- Select Whole Document INTo Table 2
    INSERT INTO #report_audit_debit_table2
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
    FROM #report_audit_debit_table2
    WHERE transdetail_id IN
    (
        SELECT transdetail_id + 1
        FROM #report_audit_debit_table2
        WHERE comment = 'COMM ADJ'
    )
    /* Delete primary entries for any COMM ADJ items */
    DELETE
    FROM #report_audit_debit_table2
    WHERE comment = 'COMM ADJ'

    -- Select Insurer Transactions INTo Table 3
    INSERT INTO #report_audit_debit_table3
       (year_name,
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
        account_handler,
	documenttype_id)
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
        T1.account_handler,
	t1.documenttype_id
        FROM #report_audit_debit_table1 T1,
            #report_audit_debit_table2 T2,
            Account A
        WHERE
            T1.transdetail_id = @transdetail_id
            AND T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN')
        AND T2.account_id = A.account_id
        ORDER BY T2.amount

    -- Set poINTer to last record just entered
    SELECT @main_key = max(main_key)
    FROM #report_audit_debit_table3

    -- TF210201 - Fix for 1st item
    IF (@old_main_key = 0)
        SELECT @old_main_key = @main_key - 1

    -- Update Insurer record with gross premium from Client record
    UPDATE #report_audit_debit_table3
--eck010802 Could be more than one client posting so sum the amount
	SET	this_premium = (SELECT SUM(amount)
                FROM #report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA'))

    WHERE main_key = @old_main_key + 1

    -- Delete Client debits from Table 2
    DELETE FROM #report_audit_debit_table2
    WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')

    -- Update Pointer
    SELECT @main_key = max(main_key)
    FROM #report_audit_debit_table3

    -- Select Insurer amounts
    DECLARE c_Cursor2 CURSOR FAST_FORWARD FOR
        SELECT amount
        FROM #report_audit_debit_table2
        WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN')
        ORDER BY amount

    OPEN c_Cursor2

    FETCH NEXT FROM c_Cursor2 INTO @amount

    WHILE @@FETCH_STATUS = 0 BEGIN
        DELETE FROM #report_audit_debit_table2
        WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
        AND amount = -@amount

        FETCH NEXT FROM c_Cursor2 INTO @amount
    END

    /* Clear up our cursor */
    CLOSE c_Cursor2
    DEALLOCATE c_Cursor2

    DELETE FROM #report_audit_debit_table2
    WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN')

    -- Update Net Commissions
    UPDATE #report_audit_debit_table3
    SET net_commission = amount,
        net_commission_short_code = A.short_code
    FROM Account A,
        #report_audit_debit_table2 T2
    WHERE
        T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'CO') --PSL30/06/2003 Remove Hard coded ledger_id
    AND T2.account_id = A.account_id
    AND main_key = @old_main_key + 1

    UPDATE #report_audit_debit_table3
    SET agent_fees = (SELECT amount
                FROM #report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG')), 
        agent_fees_short_code = (SELECT A.short_code
                        FROM Account A,
                            #report_audit_debit_table2 T2
                        WHERE T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG') 
                        AND T2.account_id = A.account_id),
        fees = (SELECT amount
                FROM #report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE')), 
        fees_short_code = (SELECT A.short_code
                        FROM Account A,
                            #report_audit_debit_table2 T2
                        WHERE T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE') 
                        AND T2.account_id = A.account_id),
        disc = (SELECT amount
                FROM #report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'DI')), 
        disc_short_code = (SELECT A.short_code

                        FROM Account A,
                            #report_audit_debit_table2 T2
                        WHERE T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'DI') 
                        AND T2.account_id = A.account_id),
        sub_agent_fees = (SELECT amount
                FROM #report_audit_debit_table2
                WHERE ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')), 

        sub_agent_short_code = (SELECT A.short_code
                        FROM Account A,
                            #report_audit_debit_table2 T2
                        WHERE T2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'DI') 
                        AND T2.account_id = A.account_id)
    WHERE main_key > @old_main_key

    --PSL30/06/2003 Remove Hard coded ledger_id
    DELETE FROM #report_audit_debit_table2
    FROM #report_audit_debit_table2 r2, Ledger l
    WHERE r2.ledger_id =l.ledger_id 
    AND l.ledger_short_name IN ('AG', 'FE', 'DI', 'CO', 'UB')

    DECLARE c_Cursor3 CURSOR FAST_FORWARD FOR
        SELECT account_id, amount
        FROM #report_audit_debit_table2

    OPEN c_Cursor3

    FETCH NEXT FROM c_Cursor3 INTO @account_id, @amount

    WHILE @@FETCH_STATUS = 0 BEGIN
        UPDATE #report_audit_debit_table3
        SET commission_amount = -@amount,
                insurer_commission_short_code = A.short_code
        FROM #report_audit_debit_table3 T,
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
UPDATE #report_audit_debit_table3
SET this_premium = ISNULL(this_premium, 0),
        insurer_premium = ISNULL(insurer_premium, 0),
        commission_amount = ISNULL(commission_amount, 0),
        net_commission = ISNULL(net_commission, 0),
        fees = ISNULL(fees, 0),
        agent_fees = ISNULL(agent_fees, 0),
        sub_agent_fees = ISNULL(sub_agent_fees, 0),
        disc = ISNULL(disc, 0)

/* Select everything now */
SELECT * FROM #report_audit_debit_table3 ORDER by main_key DESC

DROP TABLE #report_audit_debit_table1
DROP TABLE #report_audit_debit_table2
DROP TABLE #report_audit_debit_table3

GO


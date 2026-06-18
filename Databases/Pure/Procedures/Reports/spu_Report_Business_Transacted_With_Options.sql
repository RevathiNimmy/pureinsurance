SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_Report_Business_Transacted_With_Options'
GO

CREATE PROCEDURE spu_Report_Business_Transacted_With_Options
    @branch_id INT, 
    @date_type VARCHAR(20),
    @start_date DATETIME,
    @end_date DATETIME,
    @group_by VARCHAR(20),
    @then_by VARCHAR(20),
    @unique_report_name VARCHAR(300)

AS

DECLARE 
    @document_id INT,
    @account_id INT,
    @line_no INT
   
IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

SELECT @date_type = RTRIM(@date_type)
SELECT @end_date = ISNULL(@end_date, GETDATE())


CREATE TABLE #transactions
(
    /*ID fields, unique to identify each line*/
    document_id INT,
    line_no INT,
    
    /*Extra infomation fields*/
    insurance_file_cnt INT,
    client_cnt INT,
    insurer_cnt INT,

    /*Columns on the report*/    
    client_code VARCHAR(20),
    document_ref VARCHAR(30),
    transaction_date DATETIME,
    effective_date DATETIME,
    insurance_ref VARCHAR(30),
    insurer_code VARCHAR(20),    
    gross_premium_inc_ipt MONEY, 
    gross_premium_exc_ipt MONEY, 
    extra_premium MONEY,
    fees MONEY,
    disc MONEY,
    gross_commission MONEY,
    extra_commission MONEY,
    tp_commission MONEY,
    net_commission MONEY,
    
    /*Group fields*/
    group_by_code VARCHAR(50),
    group_by_desc VARCHAR(255),
    then_by_code VARCHAR(50),
    then_by_desc VARCHAR(255)

)
CREATE INDEX I_#transactions_document_id_line_no ON #transactions (document_id, line_no)


INSERT INTO #transactions
(
    document_id,
    line_no,
    insurance_file_cnt,
    document_ref,
    transaction_date,
    effective_date,
    insurance_ref,
    gross_premium_inc_ipt, 
    gross_premium_exc_ipt, 
    gross_commission
)
SELECT 
    d.document_id,
    1,
    d.insurance_file_cnt,
    CASE 
        WHEN td.spare LIKE 'Revers%' THEN
            RTRIM(d.document_ref) + ' (R)'
        ELSE
            d.document_ref
    END,
    d.document_date,
    td.ref_date,
    ISNULL(i.insurance_ref, ''),
    0,
    0,
    0
FROM document d
JOIN documenttype dt 
    ON dt.documenttype_id = d.documenttype_id 
JOIN transdetail td
    ON td.document_id = d.document_id
    AND td.document_sequence = 1  
LEFT JOIN insurance_file i
    ON i.insurance_file_cnt = d.insurance_file_cnt
WHERE d.company_id = ISNULL(@branch_id, d.company_id)
AND dt.code IN ('SND', 'SNC', 'SED', 'SEC', 'SRD', 'SRC', 'TRD', 'TRC', 'SHD', 'SHC', 'FEE')
AND 
(
    (
        @date_Type = 'Transaction Date' 
        AND
        d.document_date BETWEEN @start_date AND @end_date
    ) 
    OR
    (
        @date_type = 'Effective Date' 
        AND 
        td.ref_date BETWEEN @start_date AND @end_date
    )
)


/*Update the non-insurer amounts*/
UPDATE t 
SET t.extra_premium = 
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0) * -1
            FROM transdetail td 
            JOIN transdetail_type tt 
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code = 'GROSS'
            JOIN account a 
                ON a.account_id = td.account_id 
            JOIN party p 
                ON p.party_cnt = a.account_key 
            JOIN party_type pt 
                ON pt.party_type_id = p.party_type_id
                AND pt.code = 'EX'
            WHERE td.document_id = t.document_id
        ),  
    t.extra_commission =
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0)
            FROM transdetail td 
            JOIN transdetail_type tt 
                ON tt.transdetail_type_id = td.transdetail_type_id
                AND tt.code = 'COMM'
            JOIN account a 
                ON a.account_id = td.account_id 
            JOIN party p 
                ON p.party_cnt = a.account_key 
            JOIN party_type pt 
                ON pt.party_type_id = p.party_type_id
                AND pt.code = 'EX'
            WHERE td.document_id = t.document_id
        ),    
    t.fees =
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0) * -1
            FROM transdetail td 
            JOIN account a 
                ON a.account_id = td.account_id 
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name  = 'FE'
            WHERE td.document_id = t.document_id
        ),    
    t.disc =
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0) * -1
            FROM transdetail td 
            JOIN account a 
                ON a.account_id = td.account_id 
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name  = 'DI'
            WHERE td.document_id = t.document_id
        ), 
    t.net_commission = 
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0) * -1
            FROM transdetail td 
            JOIN account a 
                ON a.account_id = td.account_id 
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name  = 'CO'
            WHERE td.document_id = t.document_id
        ), 
    
    t.TP_Commission = 
        /*Agent Amount : 0 if no agent*/
        (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0) * -1
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name  = 'AG'
            WHERE td.document_id = t.document_id
            AND ISNULL(td.spare, '') <> 'AGENT ADJ'
        )
        +
        /*Sub Agent Amount (DD) : 0 if no sub agent or isn't a direct debit transaction*/
       (
            SELECT 
                ISNULL(SUM(ROUND(td.amount,2)),0) * -1
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name  = 'UB'
            WHERE td.document_id = t.Document_id
            AND ISNULL(td.spare, '') <> 'AGENT ADJ'
            AND td.document_sequence =
                (
                    SELECT 
                        MIN(td2.document_sequence)
                    FROM transdetail td2
                    WHERE td2.document_id = td.document_id
                    AND td2.account_id = td.account_id
                )
            AND 0 <>
                (
                    SELECT 
                        ISNULL(SUM(ROUND(td.amount,2)),0)
                    FROM transdetail td
                    JOIN account a
                        ON a.account_id = td.account_id
                    JOIN ledger l
                        ON l.ledger_id = a.ledger_id
                        AND l.ledger_short_name  = 'SA'
                    WHERE td.document_id = t.Document_id
                )
        )
        /*Sub Agent Amount (Not DD) : 0 if no sub agent or is a direct debit transaction*/
        -
        (
            (
                SELECT 
                    ISNULL(SUM(ROUND(td.amount,2)),0) * -1
                FROM transdetail td
                JOIN account a
                    ON a.account_id = td.account_id
                JOIN ledger l
                    ON l.ledger_id = a.ledger_id
                    AND l.ledger_short_name  = 'SA'
                WHERE td.document_id = t.Document_id
                AND td.document_sequence =
                    (
                        SELECT 
                            MIN(document_sequence)
                        FROM transdetail td2
                        WHERE td2.document_id = td.document_id
                    )
                AND EXISTS
                    (
                        SELECT NULL
                        FROM transdetail td
                        JOIN account a
                            ON a.account_id = td.account_id
                        JOIN ledger l
                            ON l.ledger_id = a.ledger_id
                            AND l.ledger_short_name  = 'UB'
                        WHERE td.document_id = t.Document_id
                        AND ISNULL(td.spare, '') <> 'AGENT ADJ'
                    )
                AND 0 =
                    (
                        SELECT 
                            ISNULL(SUM(ROUND(td.amount,2)),0)
                        FROM transdetail td
                        JOIN account a
                            ON a.account_id = td.account_id
                        JOIN ledger l
                            ON l.ledger_id = a.ledger_id
                            AND l.ledger_short_name  = 'SA'
                        WHERE td.document_id = t.Document_id
                    )
            )
            -
            (
                SELECT 
                    ISNULL(SUM(ROUND(td.amount,2)),0) * -1
                FROM transdetail td
                JOIN account a
                    ON a.account_id = td.account_id
                JOIN ledger l
                    ON l.ledger_id = a.ledger_id
                    AND l.ledger_short_name  = 'UB'
                WHERE td.document_id = t.Document_id
                AND ISNULL(td.spare, '') <> 'AGENT ADJ'
                AND 0 =
                    (
                        SELECT 
                            ISNULL(SUM(ROUND(td.amount,2)),0)
                        FROM transdetail td
                        JOIN account a
                            ON a.account_id = td.account_id
                        JOIN ledger l
                            ON l.ledger_id = a.ledger_id
                            AND l.ledger_short_name  = 'SA'
                        WHERE td.document_id = t.Document_id        
                    )
            )
        )
FROM #transactions t 




DECLARE c_transactions CURSOR FAST_FORWARD FOR
    SELECT 
        t.document_id
    FROM #transactions t

OPEN c_transactions

FETCH NEXT FROM c_transactions INTO 
    @document_id

WHILE @@FETCH_STATUS = 0
BEGIN

    /*Add client code to the table and include an extra line for each share*/
    SELECT @line_no = 1

    DECLARE c_clients CURSOR FAST_FORWARD FOR
        SELECT 
            a.account_id
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id    
            AND l.ledger_short_name = 'SA'
        WHERE td.document_id = @document_id
        GROUP BY a.account_id
        ORDER BY MIN(document_sequence)

    OPEN c_clients

    FETCH NEXT FROM c_clients INTO 
        @account_id

    WHILE @@FETCH_STATUS = 0
    BEGIN
        
    
        IF EXISTS
            (
                SELECT
                    NULL
                FROM #transactions
                WHERE document_id = @document_id
                AND line_no = @line_no
            )
        BEGIN
            /*Line already exists so update it*/
            UPDATE t
            SET t.client_cnt = @account_id,
                t.client_code = 
                    (
                        SELECT
                            short_code
                        FROM account
                        WHERE account_id = @account_id
                    )
            FROM #transactions t
            WHERE t.document_id = @document_id
            AND t.line_no = @line_no
        END
        ELSE
        BEGIN
            /*Line does not exists so create it*/
            INSERT INTO #transactions
            (
                document_id,
                line_no,
                client_cnt,
                client_code
            )
            SELECT
                @document_id,
                @line_no,
                @account_id,
                (
                    SELECT
                        short_code
                    FROM account
                    WHERE account_id = @account_id
                )
        END
    
        SELECT @line_no = @line_no + 1
    
        FETCH NEXT FROM c_clients INTO 
            @account_id

    END

    CLOSE c_clients
    DEALLOCATE c_clients   
    
    /*Add insurer code and amounts to the table and include an extra line for each coinsurer*/
    SELECT @line_no = 1
    
    IF EXISTS
        (
            SELECT 
                NULL
            FROM transdetail td
            JOIN account a
                ON a.account_id = td.account_id
            JOIN party p 
                ON p.party_cnt = a.account_key 
            JOIN party_type pt 
                ON pt.party_type_id = p.party_type_id
                AND pt.code = 'IN'
            WHERE td.document_id = @document_id
            GROUP BY td.document_id
            HAVING MIN(a.account_id) <> MAX(a.account_id)
        )
    BEGIN
    
        UPDATE t
        SET t.insurer_cnt = NULL,
            t.insurer_code = '-',
            t.gross_premium_inc_ipt = 0, 
            t.gross_premium_exc_ipt = 0, 
            t.gross_commission = 0
        FROM #transactions t
        WHERE t.document_id = @document_id
        AND t.line_no = @line_no
    
        SELECT @line_no = @line_no + 1
        
    END
    

    DECLARE c_insurers CURSOR FAST_FORWARD FOR
        SELECT 
            a.account_id
        FROM transdetail td
        JOIN account a
            ON a.account_id = td.account_id
        JOIN party p 
            ON p.party_cnt = a.account_key 
        JOIN party_type pt 
            ON pt.party_type_id = p.party_type_id
            AND pt.code = 'IN'
        WHERE td.document_id = @document_id
        GROUP BY a.account_id
        ORDER BY MIN(document_sequence)

    OPEN c_insurers

    FETCH NEXT FROM c_insurers INTO 
        @account_id

    WHILE @@FETCH_STATUS = 0
    BEGIN
        
        IF EXISTS
            (
                SELECT
                    NULL
                FROM #transactions
                WHERE document_id = @document_id
                AND line_no = @line_no
            )
        BEGIN
            /*Line already exists so update it*/
            UPDATE t
            SET t.insurer_cnt = @account_id,
                t.insurer_code = 
                    (
                        SELECT
                            short_code
                        FROM account
                        WHERE account_id = @account_id
                    ),
                t.gross_premium_inc_ipt =
                    (
                        SELECT 
                            ISNULL(SUM(ROUND(td.amount,2)),0) * -1
                        FROM transdetail td 
                        JOIN transdetail_type tt 
                            ON tt.transdetail_type_id = td.transdetail_type_id
                            AND tt.code IN ('GROSS', 'IFEE')
                        JOIN account a 
                            ON a.account_id = td.account_id 
                        WHERE td.document_id = @document_id
                        AND a.account_id = @account_id
                    ), 
                t.gross_premium_exc_ipt =
                    (
                        SELECT 
                            ((ISNULL(ABS(SUM(ROUND(td.amount,2))),0) - ISNULL(ABS(SUM(ROUND(td.ref_amount,2))),0))* SIGN(SUM(td.amount))) * -1
                        FROM transdetail td 
                        JOIN transdetail_type tt 
                            ON tt.transdetail_type_id = td.transdetail_type_id
                            AND tt.code IN ('GROSS', 'IFEE')
                        JOIN account a 
                            ON a.account_id = td.account_id 
                        WHERE td.document_id = @document_id
                        AND a.account_id = @account_id
                    ), 
                t.gross_commission =                     
                    ((
                    	SELECT 
                        ISNULL(SUM(ROUND(td.amount,2)),0)
                    	FROM transdetail td 
                    	JOIN transdetail_type tt 
                        	ON tt.transdetail_type_id = td.transdetail_type_id
                        	AND tt.code IN ('COMM')
                    	JOIN account a 
                        	ON a.account_id = td.account_id 
                    	WHERE td.document_id = @document_id
                    	AND a.account_id = @account_id
                	)
				+
		    		(
					 	SELECT 
                            ISNULL(SUM(ROUND(td.amount,2)),0)
                        FROM transdetail td 
                        JOIN transdetail_type tt 
                            ON tt.transdetail_type_id = td.transdetail_type_id
                            AND tt.code IN ('COMMADJ')
                        JOIN account a 
                            ON a.account_id = td.account_id 
                        WHERE td.document_id = @document_id
                        AND a.account_id = @account_id
					))
            FROM #transactions t
            WHERE t.document_id = @document_id
            AND t.line_no = @line_no
        END
        ELSE
        BEGIN
            /*Line does not exists so create it*/
            INSERT INTO #transactions
            (
                document_id,
                line_no,
                insurer_cnt,
                insurer_code,
                gross_premium_inc_ipt,
                gross_premium_exc_ipt,
                gross_commission
            )
            SELECT
                @document_id,
                @line_no,
                @account_id,
                (
                    SELECT
                        short_code
                    FROM account
                    WHERE account_id = @account_id
                ),
                (
                    SELECT 
                        ISNULL(SUM(ROUND(td.amount,2)),0) * -1
                    FROM transdetail td 
                    JOIN transdetail_type tt 
                        ON tt.transdetail_type_id = td.transdetail_type_id
                        AND tt.code IN ('GROSS', 'IFEE')
                    JOIN account a 
                        ON a.account_id = td.account_id 
                    WHERE td.document_id = @document_id
                    AND a.account_id = @account_id
                ), 
                (
                    SELECT 
                        ((ISNULL(ABS(SUM(ROUND(td.amount,2))),0) - ISNULL(ABS(SUM(ROUND(td.ref_amount,2))),0))* SIGN(SUM(td.amount))) * -1
                    FROM transdetail td 
                    JOIN transdetail_type tt 
                        ON tt.transdetail_type_id = td.transdetail_type_id
                        AND tt.code IN ('GROSS', 'IFEE')
                    JOIN account a 
                        ON a.account_id = td.account_id 
                    WHERE td.document_id = @document_id
                    AND a.account_id = @account_id
                ), 
                ((
                    	SELECT 
                        ISNULL(SUM(ROUND(td.amount,2)),0)
                    	FROM transdetail td 
                    	JOIN transdetail_type tt 
                        	ON tt.transdetail_type_id = td.transdetail_type_id
                        	AND tt.code IN ('COMM')
                    	JOIN account a 
                        	ON a.account_id = td.account_id 
                    	WHERE td.document_id = @document_id
                    	AND a.account_id = @account_id
                	)
				+
		    		(
					 	SELECT 
                            ISNULL(SUM(ROUND(td.amount,2)),0)
                        FROM transdetail td 
                        JOIN transdetail_type tt 
                            ON tt.transdetail_type_id = td.transdetail_type_id
                            AND tt.code IN ('COMMADJ')
                        JOIN account a 
                            ON a.account_id = td.account_id 
                        WHERE td.document_id = @document_id
                        AND a.account_id = @account_id
					))
        END
    
        SELECT @line_no = @line_no + 1
    
        FETCH NEXT FROM c_insurers INTO 
            @account_id

    END

    CLOSE c_insurers
    DEALLOCATE c_insurers   

        
    FETCH NEXT FROM c_transactions INTO 
        @document_id

END

CLOSE c_transactions
DEALLOCATE c_transactions

        

/*Exclude any transactions with account executives that were selected for exclusion*/
IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'AE'
        AND id <> 0
    )
BEGIN
    DELETE 
    FROM #transactions 
    WHERE document_id IN
        (
            SELECT
                t.document_id
            FROM #transactions t
            JOIN account a
                ON a.account_id = t.client_cnt
            JOIN party p
                ON p.party_cnt = a.account_key
            JOIN temp_report_exclude tre
                ON tre.id = p.consultant_cnt
                AND tre.type IN ('AE')
            WHERE tre.unique_report_name = @unique_report_name
            
        )
END

IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'AE'
        AND id = 0
    )
BEGIN
    DELETE FROM #transactions
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.client_cnt
            JOIN party p
                ON p.party_cnt = a.account_key
            JOIN party pae
                ON pae.party_cnt = p.consultant_cnt
            WHERE tt.document_id = t.document_id
        )
END

/*Exclude any transactions with account handlers that were selected for exclusion*/
IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'AH'
    )
BEGIN
    DELETE 
    FROM #transactions 
    WHERE document_id IN
        (
            SELECT
                t.document_id
            FROM #transactions t
            JOIN insurance_file i
                ON i.insurance_file_cnt = t.insurance_file_cnt
            JOIN temp_report_exclude tre
                ON tre.id = i.account_handler_cnt 
                AND tre.type IN ('AH')
            WHERE tre.unique_report_name = @unique_report_name
            
        )
END

IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'AH'
        AND id = 0
    )
BEGIN
    DELETE FROM #transactions
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN insurance_file i
                ON i.insurance_file_cnt = tt.insurance_file_cnt
            JOIN party p
                ON p.party_cnt = i.account_handler_cnt
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
END

/*Exclude any transactions with risk codes that were selected for exclusion*/
IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'RC'
    )
BEGIN
    DELETE 
    FROM #transactions 
    WHERE document_id IN
        (
            SELECT
                t.document_id
            FROM #transactions t
            JOIN insurance_file i
                ON i.insurance_file_cnt = t.insurance_file_cnt
            JOIN temp_report_exclude tre
                ON tre.id = i.risk_code_id
                AND tre.type IN ('RC')
            WHERE tre.unique_report_name = @unique_report_name
        )
END

IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'RC'
        AND id = 0
    )
BEGIN
    DELETE FROM #transactions
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN insurance_file i
                ON i.insurance_file_cnt = tt.insurance_file_cnt
            JOIN risk_code rc
                ON rc.risk_code_id = i.risk_code_id
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
END

/*Exclude any transactions with insurers that were selected for exclusion*/
IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'IN'
    )
BEGIN
    DELETE 
    FROM #transactions 
    WHERE document_id IN
        (
            SELECT
                t.document_id
            FROM #transactions t
            JOIN account a
                ON a.account_id = t.insurer_cnt
            JOIN temp_report_exclude tre
                ON tre.id = a.account_key
                AND tre.type IN ('IN')
            WHERE tre.unique_report_name = @unique_report_name
        )
END

IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'IN'
        AND id = 0
    )
BEGIN
    DELETE FROM #transactions
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.insurer_cnt
            WHERE tt.document_id = t.document_id
        )
END

/*Exclude any transactions with third parties that were selected for exclusion*/
IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'TP'
    )
BEGIN
    DELETE 
    FROM #transactions 
    WHERE document_id IN
        (
            SELECT
                t.document_id
            FROM #transactions t
            JOIN transdetail td
                ON td.document_id = t.document_id
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name IN ('AG', 'UB')
            JOIN temp_report_exclude tre
                ON tre.id = a.account_key
                AND tre.type IN ('TP')
            WHERE tre.unique_report_name = @unique_report_name
        )
END

IF EXISTS 
    (
        SELECT 
            NULL
        FROM temp_report_exclude 
        WHERE unique_report_name = @unique_report_name 
        AND type = 'TP'
        AND id = 0
    )
BEGIN
    DELETE FROM #transactions
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN transdetail td
                ON td.document_id = t.document_id
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name IN ('AG', 'UB')
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
END
        

/*Work out all of the groupings*/
IF @group_by = 'Account Executive'
BEGIN

    UPDATE t
    SET t.group_by_code = 'No Account Executive',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.client_cnt
            JOIN party p
                ON p.party_cnt = a.account_key
            JOIN party pae
                ON pae.party_cnt = p.consultant_cnt
            WHERE tt.document_id = t.document_id
        )
        
    UPDATE t
    SET t.group_by_code = 'Multiple Account Executives',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.client_cnt
            JOIN party p
                ON p.party_cnt = a.account_key
            LEFT JOIN party pae
                ON pae.party_cnt = p.consultant_cnt
            WHERE tt.document_id = t.document_id
            GROUP BY tt.document_id
            HAVING MIN(ISNULL(pae.party_cnt, 0)) <> MAX(ISNULL(pae.party_cnt, 0))
        )
        
    UPDATE t
    SET t.group_by_code = pae.shortname,
        t.group_by_desc = pae.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN account a
        ON a.account_id = tt.client_cnt
    JOIN party p
        ON p.party_cnt = a.account_key
    JOIN party pae
        ON pae.party_cnt = p.consultant_cnt
    WHERE t.group_by_code IS NULL

END

IF @then_by = 'Account Executive'
BEGIN

    UPDATE t
    SET t.then_by_code = 'No Account Executive',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.client_cnt
            JOIN party p
                ON p.party_cnt = a.account_key
            JOIN party pae
                ON pae.party_cnt = p.consultant_cnt
            WHERE tt.document_id = t.document_id
        )
        
    UPDATE t
    SET t.then_by_code = 'Multiple Account Executives',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.client_cnt
            JOIN party p
                ON p.party_cnt = a.account_key
            LEFT JOIN party pae
                ON pae.party_cnt = p.consultant_cnt
            WHERE tt.document_id = t.document_id
            GROUP BY tt.document_id
            HAVING MIN(ISNULL(pae.party_cnt, 0)) <> MAX(ISNULL(pae.party_cnt, 0))
        )
        
    UPDATE t
    SET t.then_by_code = pae.shortname,
        t.then_by_desc = pae.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN account a
        ON a.account_id = tt.client_cnt
    JOIN party p
        ON p.party_cnt = a.account_key
    JOIN party pae
        ON pae.party_cnt = p.consultant_cnt
    WHERE t.then_by_code IS NULL

END


IF @group_by = 'Account Handler'
BEGIN

    UPDATE t
    SET t.group_by_code = 'No Account Handler',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN insurance_file i
                ON i.insurance_file_cnt = tt.insurance_file_cnt
            JOIN party p
                ON p.party_cnt = i.account_handler_cnt
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
        
    UPDATE t
    SET t.group_by_code = p.shortname,
        t.group_by_desc = p.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = tt.insurance_file_cnt
    JOIN party p
        ON p.party_cnt = i.account_handler_cnt
    WHERE t.group_by_code IS NULL

END

IF @then_by = 'Account Handler'
BEGIN

    UPDATE t
    SET t.then_by_code = 'No Account Handler',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN insurance_file i
                ON i.insurance_file_cnt = tt.insurance_file_cnt
            JOIN party p
                ON p.party_cnt = i.account_handler_cnt
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
        
    UPDATE t
    SET t.then_by_code = p.shortname,
        t.then_by_desc = p.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = tt.insurance_file_cnt
    JOIN party p
        ON p.party_cnt = i.account_handler_cnt
    WHERE t.then_by_code IS NULL

END


IF @group_by = 'Branch'
BEGIN
        
    UPDATE t
    SET t.group_by_code = s.code,
        t.group_by_desc = s.description
    FROM #transactions t
    JOIN document d
        ON d.document_id = t.document_id
    JOIN source s
        ON s.source_id = d.company_id
    WHERE t.group_by_code IS NULL

END

IF @then_by = 'Branch'
BEGIN
        
    UPDATE t
    SET t.then_by_code = s.code,
        t.then_by_desc = s.description
    FROM #transactions t
    JOIN document d
        ON d.document_id = t.document_id
    JOIN source s
        ON s.source_id = d.company_id
    WHERE t.then_by_code IS NULL

END

IF @group_by = 'Business Type'
BEGIN
        
    UPDATE t
    SET t.group_by_code = 'No Business Type',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN insurance_file i
                ON i.insurance_file_cnt = tt.insurance_file_cnt
            JOIN business_type bt
                ON bt.business_type_id = i.business_type_id
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
        
    UPDATE t
    SET t.group_by_code = bt.code,
        t.group_by_desc = bt.description
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = tt.insurance_file_cnt
    JOIN business_type bt
        ON bt.business_type_id = i.business_type_id
    WHERE t.group_by_code IS NULL

END

IF @then_by = 'Business Type'
BEGIN
        
    UPDATE t
    SET t.then_by_code = 'No Business Type',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN insurance_file i
                ON i.insurance_file_cnt = tt.insurance_file_cnt
            JOIN business_type bt
                ON bt.business_type_id = i.business_type_id
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
        
    UPDATE t
    SET t.then_by_code = bt.code,
        t.then_by_desc = bt.description
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = tt.insurance_file_cnt
    JOIN business_type bt
        ON bt.business_type_id = i.business_type_id
    WHERE t.then_by_code IS NULL

END


IF @group_by = 'Client'
BEGIN
        
    UPDATE t
    SET t.group_by_code = 'Multiple Clients',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.client_cnt
            WHERE tt.document_id = t.document_id
            GROUP BY tt.document_id
            HAVING MIN(a.account_id) <> MAX(a.account_id)
        )
        
    UPDATE t
    SET t.group_by_code = p.shortname,
        t.group_by_desc = p.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN account a
        ON a.account_id = tt.client_cnt
    JOIN party p
        ON p.party_cnt = a.account_key
    WHERE t.group_by_code IS NULL

END

IF @then_by = 'Client'
BEGIN
        
    UPDATE t
    SET t.then_by_code = 'Multiple Clients',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.client_cnt
            WHERE tt.document_id = t.document_id
            GROUP BY tt.document_id
            HAVING MIN(a.account_id) <> MAX(a.account_id)
        )
        
    UPDATE t
    SET t.then_by_code = p.shortname,
        t.then_by_desc = p.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN account a
        ON a.account_id = tt.client_cnt
    JOIN party p
        ON p.party_cnt = a.account_key
    WHERE t.then_by_code IS NULL

END


IF @group_by = 'Insurer'
BEGIN

    UPDATE t
    SET t.group_by_code = 'No Insurer',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.insurer_cnt
            WHERE tt.document_id = t.document_id
        )
        
    UPDATE t
    SET t.group_by_code = 'Multiple Insurers',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.insurer_cnt
            WHERE tt.document_id = t.document_id
            GROUP BY tt.document_id
            HAVING MIN(a.account_id) <> MAX(a.account_id)
        )
        
    UPDATE t
    SET t.group_by_code = p.shortname,
        t.group_by_desc = p.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN account a
        ON a.account_id = tt.insurer_cnt
    JOIN party p
        ON p.party_cnt = a.account_key
    WHERE t.group_by_code IS NULL

END

IF @then_by = 'Insurer'
BEGIN

    UPDATE t
    SET t.then_by_code = 'No Insurer',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.insurer_cnt
            WHERE tt.document_id = t.document_id
        )
        
    UPDATE t
    SET t.then_by_code = 'Multiple Insurers',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN account a
                ON a.account_id = tt.insurer_cnt
            WHERE tt.document_id = t.document_id
            GROUP BY tt.document_id
            HAVING MIN(a.account_id) <> MAX(a.account_id)
        )
        
    UPDATE t
    SET t.then_by_code = p.shortname,
        t.then_by_desc = p.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN account a
        ON a.account_id = tt.insurer_cnt
    JOIN party p
        ON p.party_cnt = a.account_key
    WHERE t.then_by_code IS NULL

END


IF @group_by = 'Risk'
BEGIN
        
    UPDATE t
    SET t.group_by_code = 'No Risk',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN insurance_file i
                ON i.insurance_file_cnt = tt.insurance_file_cnt
            JOIN risk_code rc
                ON rc.risk_code_id = i.risk_code_id
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
        
    UPDATE t
    SET t.group_by_code = rc.code,
        t.group_by_desc = rc.description
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = tt.insurance_file_cnt
    JOIN risk_code rc
        ON rc.risk_code_id = i.risk_code_id
    WHERE t.group_by_code IS NULL

END

IF @then_by = 'Risk'
BEGIN
        
    UPDATE t
    SET t.then_by_code = 'No Risk',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN insurance_file i
                ON i.insurance_file_cnt = tt.insurance_file_cnt
            JOIN risk_code rc
                ON rc.risk_code_id = i.risk_code_id
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
        
    UPDATE t
    SET t.then_by_code = rc.code,
        t.then_by_desc = rc.description
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = tt.insurance_file_cnt
    JOIN risk_code rc
        ON rc.risk_code_id = i.risk_code_id
    WHERE t.then_by_code IS NULL

END


IF @group_by = 'Third Party'
BEGIN

    UPDATE t
    SET t.group_by_code = 'No Third Party',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN transdetail td
                ON td.document_id = t.document_id
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name IN ('AG', 'UB')
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
        
    UPDATE t
    SET t.group_by_code = 'Multiple Third Parties',
        t.group_by_desc = NULL
    FROM #transactions t
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN transdetail td
                ON td.document_id = t.document_id
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name IN ('AG', 'UB')
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
            GROUP BY tt.document_id
            HAVING MIN(a.account_id) <> MAX(a.account_id)
        )
        
    UPDATE t
    SET t.group_by_code = p.shortname,
        t.group_by_desc = p.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN transdetail td
        ON td.document_id = t.document_id
    JOIN account a
        ON a.account_id = td.account_id
    JOIN ledger l
        ON l.ledger_id = a.ledger_id
        AND l.ledger_short_name IN ('AG', 'UB')
    JOIN party p
        ON p.party_cnt = a.account_key
    WHERE t.group_by_code IS NULL
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )

END

IF @then_by = 'Third Party'
BEGIN

    UPDATE t
    SET t.then_by_code = 'No Third Party',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN transdetail td
                ON td.document_id = t.document_id
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name IN ('AG', 'UB')
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
        )
        
    UPDATE t
    SET t.then_by_code = 'Multiple Third Parties',
        t.then_by_desc = NULL
    FROM #transactions t
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #transactions tt
            JOIN transdetail td
                ON td.document_id = t.document_id
            JOIN account a
                ON a.account_id = td.account_id
            JOIN ledger l
                ON l.ledger_id = a.ledger_id
                AND l.ledger_short_name IN ('AG', 'UB')
            WHERE tt.document_id = t.document_id
            AND tt.line_no = 1
            GROUP BY tt.document_id
            HAVING MIN(a.account_id) <> MAX(a.account_id)
        )
        
    UPDATE t
    SET t.then_by_code = p.shortname,
        t.then_by_desc = p.resolved_name
    FROM #transactions t
    JOIN #transactions tt
        ON tt.document_id = t.document_id
        AND tt.line_no = 1
    JOIN transdetail td
        ON td.document_id = t.document_id
    JOIN account a
        ON a.account_id = td.account_id
    JOIN ledger l
        ON l.ledger_id = a.ledger_id
        AND l.ledger_short_name IN ('AG', 'UB')
    JOIN party p
        ON p.party_cnt = a.account_key
    WHERE t.then_by_code IS NULL
    AND td.document_sequence =
        (
            SELECT
                MIN(document_sequence)
            FROM transdetail
            WHERE document_id = td.document_id
            AND account_id = td.account_id
        )

END


IF @then_by = 'None'
BEGIN

    UPDATE t
    SET t.then_by_code = NULL,
        t.then_by_desc = NULL
    FROM #transactions t
    
END


/*Return all of the transactions in the appropriate order*/
SELECT 
    * 
FROM #transactions
ORDER BY 
    group_by_code, 
    then_by_code, 
    document_id, 
    line_no

DROP TABLE #transactions 

GO



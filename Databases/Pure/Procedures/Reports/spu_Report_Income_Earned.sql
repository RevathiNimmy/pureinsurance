/*
This stored procedure is used by the following reports:

Income_Earned.rpt
*/

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Income_Earned'
GO

CREATE PROCEDURE spu_Report_Income_Earned

    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @date_type VARCHAR(20),
    @report_type VARCHAR(20)

AS

DECLARE 
    @Transdetail_id INT,
    @spare VARCHAR(20),
    @document_id INT,
    @document_ref VARCHAR(25),
    @document_date DATETIME,
    @short_code CHAR(30),
    @account_name VARCHAR(60),
    @company_id INT,
    @amount MONEY,
    @orig_short_code CHAR(30),
    @orig_account_name VARCHAR(60),
    @client_name VARCHAR(30),
    @brought_forward MONEY,
    @client_code CHAR(30), 
    @file_code VARCHAR(8), 
    @account_exec_code CHAR(20), 
    @account_exec VARCHAR(255), 
    @account_handler_code CHAR(20), 
    @risk VARCHAR(255), 
    @insurer VARCHAR(20), 
    @node_id INT,
    @session_id INT,
    @ins_file_acc_exec CHAR(1),
    @effective_date DATETIME,
    @original_document_id INT,
    @orig_document_ref VARCHAR(25),
    @orig_document_type VARCHAR(10),
    @policy_ref VARCHAR(30),
    @business_type VARCHAR(50),
    @account_handler VARCHAR(255),
    @branch VARCHAR(255),
    @income_order INT,
    @ledger_short_name VARCHAR(2),
    @multiplier INT,
    @insurer_account_id INT,
    @comm_total MONEY,
    @agent_total MONEY,
    @orig_transdetail_id INT,
    @ins_comm_total MONEY

SET NOCOUNT ON


IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @report_type = 'Commission'
BEGIN
    SELECT @ledger_short_name = 'CO'
    SELECT @multiplier = 1
END
ELSE
BEGIN
    SELECT @ledger_short_name = 'FE'
    SELECT @multiplier = -1
END

--Create Temporary Tables
CREATE TABLE #Report_Earned
(
    id INT IDENTITY(1,1),
    document_id INT,
    document_ref VARCHAR(25),
    document_date DATETIME,
    income_acc_code CHAR(20),
    income_acc_name VARCHAR(60),
    income_acc_amount MONEY,
    income_acc_amount_total MONEY,
    branch_id INT,
    account_name VARCHAR(60),
    account_code CHAR(30),
    file_code VARCHAR(8),
    account_exec_code CHAR(20), 
    account_exec VARCHAR(255),
    account_handler_code CHAR(20), 
    risk VARCHAR(255),
    insurer CHAR(20),
    extra CHAR(20),
    fee CHAR(20),
    effective_date DATETIME,
    original_document_id INT,
    original_document_ref VARCHAR(25),
    original_document_type VARCHAR(10),
    policy_no VARCHAR(30),
    business_type VARCHAR(50),
    account_handler VARCHAR(255),
    branch VARCHAR(255),
    income_order INT
)

CREATE TABLE #Report_Earned_Accounts
(
    account_id INT
)

/*Retrieve acc exec location from hidden options*/
SELECT @ins_file_acc_exec = '0'

SELECT 
    @ins_file_acc_exec = value
FROM hidden_options
WHERE option_number = 40


/*Get earned accounts*/
INSERT INTO #Report_Earned_Accounts
(
    account_id
)
SELECT
    A.account_id
FROM account A
JOIN ledger L
    ON L.ledger_id = A.ledger_id
JOIN structuretree ST
    ON ST.account_id = A.account_id
JOIN structuretree STM
    ON STM.node_id = ST.parent_node_id
JOIN mapping M
    ON M.mapping_id = STM.mapping_id
WHERE L.ledger_short_name = 'NO'
AND M.description = 
    CASE @ledger_short_name
    WHEN 'CO' THEN
        'Comm Inc Earned'
    WHEN 'FE' THEN
        'Fees Earned'
    END
GROUP BY A.account_id
    
    
/*Search through all transactions for the earned accounts.*/
DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT
        T.transdetail_id,
        D.document_id,
        D.document_ref,
        D.document_date,
        A.short_code,
        A.account_name,
        T.company_id,
        ROUND(ISNULL(T.amount,0),2)
    FROM Transdetail T
    JOIN Document D
        ON D.document_id = T.document_id
    JOIN Account A
        ON A.account_id = T.account_id
    JOIN #Report_Earned_Accounts EA
        ON EA.account_id = A.account_id
    WHERE T.company_id = ISNULL(@branch_id, T.company_id)
    AND D.comment <> 'Year End Retained Profit'
    AND 
    (
        (
            @date_type = 'Transaction Date'
            AND
            (
                (
                    D.document_date BETWEEN @start_date AND @end_date
                    AND 
                    T.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                )
                OR
                (
                    T.ref_date BETWEEN @start_date AND @end_date
                    AND 
                    T.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                )
            )
        )
        OR
        (
            @date_type = 'Effective Date'
            AND 
            NOT EXISTS
            (
                SELECT 
                    NULL
                FROM document d
                JOIN transdetail td
                    ON td.document_id = d.document_id
                    AND td.document_sequence = 1
                WHERE d.document_ref = RIGHT(RTRIM(T.spare), 11)
                AND d.company_id = T.company_id
                AND T.spare NOT IN ('', 'AGENT ADJ', 'COMM ADJ', 'BROK ADJ')
                AND (
                        td.ref_date < @start_date
                        OR
                        td.ref_date > @end_date
                    )
            )
            AND NOT EXISTS
            (
                SELECT 
                    NULL
                FROM transdetail td
                WHERE td.document_id = d.document_id
                AND td.document_sequence = 1
                AND T.spare IN ('', 'AGENT ADJ', 'COMM ADJ', 'BROK ADJ')
                AND (
                        td.ref_date < @start_date
                        OR
                        td.ref_date > @end_date
                    )
            )
        )
        OR
        (
            @date_type = 'Trans/Effective Date'
            AND 
            NOT EXISTS
            (
                SELECT 
                    NULL
                FROM document dx
                JOIN transdetail tdx
                    ON tdx.document_id = dx.document_id
                    AND tdx.document_sequence = 1
                WHERE dx.document_ref = RIGHT(RTRIM(T.spare), 11)
                AND dx.company_id = T.company_id
                AND T.spare NOT IN ('', 'AGENT ADJ', 'COMM ADJ', 'BROK ADJ')
                AND 
                (
                    (
                        D.document_date > @end_date
                        OR
                        tdx.ref_date > @end_date
                    )
                    OR
                    (
                        D.document_date < @start_date 
                        AND 
                        tdx.ref_date < @start_date
                    )
                )
            )
            AND NOT EXISTS
            (
                SELECT 
                    NULL
                FROM transdetail tdx
                WHERE tdx.document_id = D.document_id
                AND tdx.document_sequence = 1
                AND T.spare IN ('', 'AGENT ADJ', 'COMM ADJ', 'BROK ADJ')
                AND 
                (
                    (
                        T.ref_date > @end_date
                        OR
                        tdx.ref_date > @end_date
                    )
                    OR
                    (
                        T.ref_date < @start_date 
                        AND 
                        tdx.ref_date < @start_date
                    )
                )
            )
        )
    )
       
    

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO 
    @transdetail_id,
    @document_id,
    @document_ref,
    @document_date,
    @short_code,
    @account_name,
    @company_id,
    @amount

WHILE @@FETCH_STATUS = 0
BEGIN
    
    /*Get corresponding transaction account*/
    SELECT 
        @spare = ISNULL(T1.spare, ''),
        @orig_short_code = A.short_code,
        @orig_account_name = A.account_name,
	@orig_transdetail_id = T2.transdetail_id
    FROM Transdetail T1
    JOIN Transdetail T2
        ON T2.document_id = T1.document_id
        AND T2.transdetail_id <> T1.transdetail_id
    JOIN Account A
        ON A.account_id = T2.account_id
    WHERE T1.transdetail_id = @transdetail_id
    AND (
            T2.spare = T1.spare
            OR 
            (
                SELECT SUM(1)
                FROM Transdetail 
                WHERE document_id = T1.document_id
            ) = 2 
            OR
            (
                T2.spare = 'COMM ADJ'
                AND
                T2.transdetail_id = T1.transdetail_id - 1
            )
        )

    /*Set defaults*/
    SELECT
        @client_name = @orig_account_name,
        @client_code = @orig_short_code,
        @file_code = '',        
        @account_exec_code = 'NO EXEC',
        @account_exec = 'NO EXEC',          
        @account_handler_code = 'NO HANDLER',
        @risk = '',                
        @insurer = '',
        @effective_date  = NULL,
        @orig_document_ref = '',
        @orig_document_type = '',
        @policy_ref = '',
        @business_type = 'NO BUSINESS TYPE',
        @account_handler = 'NO HANDLER'

    /*If its not an adjustment then get the original document_id.*/
    SELECT @original_document_id = @document_id
    IF RTRIM(@spare) NOT IN ('', 'AGENT ADJ', 'COMM ADJ', 'BROK ADJ')
    BEGIN
        SELECT 
            @original_document_id = document_id
        FROM document
        WHERE document_ref = RIGHT(RTRIM(@spare), 11)
        AND company_id = @company_id
    END
    
    IF LEFT(@spare, 8) = 'Reversal'
    BEGIN
        SELECT 
            @original_document_id = d3.document_id
        FROM transmatch tm
        JOIN transmatch tm2
            ON tm2.match_id = tm.match_id
            AND tm2.transdetail_id <> tm.transdetail_id
            AND tm.allocationdetail_id IS NOT NULL
        JOIN transdetail td2
            ON td2.transdetail_id = tm2.transdetail_id
        JOIN document d3
            ON d3.document_ref = RIGHT(RTRIM(td2.spare), 11)
            AND d3.company_id = td2.company_id
        WHERE tm.transdetail_id = @transdetail_id
    END
    

    
    SELECT
        @client_name = A.Account_Name, 
        @client_code = A.short_code,             
        @file_code = P.file_Code,            
        @account_handler_code = ISNULL(PAH.shortname,'NO HANDLER'),
        @account_exec_code = (CASE @ins_file_acc_exec WHEN '1' THEN ISNULL(PAE2.shortname,'NO EXEC') ELSE ISNULL(PAE.shortname,'NO EXEC') END), 
        @account_exec = (CASE @ins_file_acc_exec WHEN '1' THEN ISNULL(PAE2.resolved_name,'') ELSE ISNULL(PAE.resolved_name,'') END),            
        @risk = R.description,              
        @insurer = INS.shortname,
        @effective_date = TEF.cover_start_date,
        @orig_document_ref = TEF.document_ref,
        @orig_document_type = ISNULL(TEF.transaction_type_code, ''),
        @policy_ref = TEF.insurance_ref,
        @business_type = ISNULL(BT.description, 'NO BUSINESS TYPE'),
        @account_handler = ISNULL(PAH.resolved_name,'')
    FROM document D
    JOIN transdetail T
        ON T.document_id = D.document_id
    JOIN account A
        ON A.account_id = T.account_id
    LEFT OUTER JOIN transaction_export_folder TEF
        ON TEF.document_ref = D.document_ref
        AND TEF.source_id = D.company_id
    LEFT OUTER JOIN insurance_file  I
        ON I.insurance_ref = T.insurance_ref
        AND I.policy_version =  
            (
                SELECT MAX(policy_version)
                FROM Insurance_file
                WHERE insurance_ref = T.insurance_ref
                AND lead_insurer_cnt IS NOT NULL
            )       
    LEFT OUTER JOIN risk_code R
        ON R.risk_code_id = I.risk_code_id               
    LEFT OUTER JOIN party INS
        ON INS.party_cnt = I.lead_insurer_cnt
    LEFT OUTER JOIN party P
        ON P.party_cnt = I.insured_cnt
    LEFT OUTER JOIN party PAH
        ON PAH.party_cnt = TEF.account_handler_cnt
    LEFT OUTER JOIN party PAE
        ON PAE.party_cnt = P.consultant_cnt
    LEFT OUTER JOIN party PAE2
        ON PAE2.party_cnt = I.account_executive_cnt
    LEFT OUTER JOIN business_type BT
        ON I.business_type_id = BT.business_type_id
    WHERE D.document_id = @original_document_id
    AND 
    (
        (
            (
                SELECT ISNULL(SUM(1),0)
                FROM transdetail TCli
                JOIN account ACli
                    ON ACli.account_id = TCli.account_id
                WHERE TCli.document_id = D.document_id
                AND ACli.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            ) > 0
            AND T.transdetail_id IN
            (
                SELECT MIN(transdetail_id)
                FROM transdetail TCli
                JOIN account ACli
                    ON ACli.account_id = TCli.account_id
                WHERE TCli.document_id = D.document_id
                AND ACli.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            )
        )
        OR
        (
            (
                SELECT ISNULL(SUM(1),0)
                FROM transdetail TCli
                JOIN account ACli
                    ON ACli.account_id = TCli.account_id
                WHERE TCli.document_id = D.document_id
                AND ACli.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
            ) = 0
            AND T.transdetail_id IN
            (
                SELECT MIN(transdetail_id)
                FROM transdetail TCli
                JOIN account ACli
                    ON ACli.account_id = TCli.account_id
                WHERE TCli.document_id = D.document_id
                AND ACli.short_code <> @short_code      
            )
        )
    )
        
    SELECT @branch = description
    FROM company
    WHERE company_id = @company_id
  

    IF @effective_date IS NULL
    BEGIN
        SELECT @effective_date = @document_date
    END

    
    SELECT @insurer_account_id = NULL  
    
    IF @report_type = 'Commission'
    BEGIN
    
        SELECT
            @insurer_account_id = a.account_id
        FROM transdetail td
        JOIN released_accounts_transactions rat
            ON rat.destination_transdetail_id = td.transdetail_id
        JOIN allocation al
            ON al.allocation_id = rat.allocation_id
        JOIN account a
            ON a.account_id = al.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
        WHERE td.document_id = @document_id
        AND l.ledger_short_name IN ('IN', 'EX')

        IF @insurer_account_id IS NULL
        BEGIN
            SELECT @insurer_account_id = td2.account_id 
            FROM transmatch tm
            JOIN matchgroup mg  
                ON mg.match_id = tm.match_id  
            JOIN transmatch tm2  
                ON tm2.match_id = mg.match_id  
                AND tm2.is_reversed IS NULL  
                AND tm2.allocationdetail_id IS NOT NULL 
            JOIN transdetail td
                ON tm2.transdetail_id = td.transdetail_id 
            JOIN transdetail td2 
                ON td.document_id = td2.document_id
            WHERE tm.transdetail_id = @orig_transdetail_id
            AND tm2.transmatch_id <> tm.transmatch_id
            AND td.spare = 'BROK ADJ'
            AND td2.document_sequence = td.document_sequence - 1
            AND 2 = (SELECT COUNT(transmatch_id) FROM transmatch WHERE match_id = mg.match_id) 
        END

        SELECT
            @comm_total = SUM(TD.amount)
        FROM transdetail TD
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN party P
            ON P.party_cnt = A.account_key 
        JOIN party_type PT
            ON PT.party_type_id = P.party_type_id
        WHERE TD.document_id = @original_document_id
        AND A.account_id = ISNULL(@insurer_account_id, A.account_id)
        AND TD.spare = 'COMM' 
        AND PT.code IN ('IN', 'EX')
        
        SELECT
            @ins_comm_total = SUM(TD.amount)
        FROM transdetail TD
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN party P
            ON P.party_cnt = A.account_key
        JOIN party_type PT
            ON PT.party_type_id = P.party_type_id
        WHERE TD.document_id = @original_document_id
        AND A.account_id = ISNULL(@insurer_account_id, A.account_id)
        AND TD.spare = 'COMM'
        AND PT.code IN ('IN')

        SELECT
            @agent_total = SUM(TD.amount)
        FROM transdetail TD
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN party P
            ON P.party_cnt = A.account_key
        JOIN party_type PT
            ON PT.party_type_id = P.party_type_id
        WHERE TD.document_id = @original_document_id
        AND A.account_id = ISNULL(@insurer_account_id, A.account_id)
        AND TD.spare = 'AGENT'
        AND PT.code='AG'

        SET @comm_total = ISNULL(@comm_total,0) + ISNULL(@agent_total,0)
    
    END
    
    IF @report_type = 'Fee'
    BEGIN

        SELECT
            @comm_total = SUM(TD.amount)
        FROM transdetail TD
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN party P
            ON P.party_cnt = A.account_key 
        JOIN party_type PT
            ON PT.party_type_id = P.party_type_id
        WHERE TD.document_id = @original_document_id
        AND PT.code IN ('FE')

    END
        
    IF 
    (
        @date_type = 'Transaction Date'
        OR
        (
            @date_type = 'Effective Date'
            AND
            @effective_date BETWEEN @start_date AND @end_date
        )
        OR
        (
            @date_type = 'Trans/Effective Date'
            AND
            (
                (
                    @document_date BETWEEN @start_date AND @end_date
                    AND
                    @effective_date < @document_date    
                )
                OR
                (
                    @effective_date BETWEEN @start_date AND @end_date
                    AND
                    @document_date < @effective_date
                )
            )
        )                
    )
    BEGIN

        IF NOT EXISTS
            (
                SELECT
                    NULL
                FROM transdetail TD
                JOIN account A
                    ON A.account_id = TD.account_id
                JOIN party P
                    ON P.party_cnt = A.account_key 
                JOIN party_type PT
                    ON PT.party_type_id = P.party_type_id
                WHERE TD.document_id = @original_document_id
                AND A.account_id = ISNULL(@insurer_account_id, A.account_id)
                AND (
                        (
                            @report_type = 'Commission'
                            AND 
                            TD.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
                            AND 
                            PT.code IN ('IN', 'EX')
                        )
                        OR
                        (
                            @report_type = 'Fee'
                            AND 
                            PT.code IN ('FE')    
                            AND
                            P.shortname = @orig_short_code
                        )
                    )
            )
        BEGIN
            INSERT INTO #Report_Earned
            (
                document_id,
                document_ref,
                document_date,
                income_acc_code,
                income_acc_name,
                income_acc_amount,
                income_acc_amount_total,
                branch_id,
                account_name,
                account_code,
                file_code,
                account_exec_code, 
                account_exec,
                account_handler_code, 
                risk,
                insurer,
                extra,
                fee,
                effective_date,
                original_document_id,
                original_document_ref,
                original_document_type,
                policy_no,
                business_type,
                account_handler,
                branch,
                income_order
            )
            SELECT  
                @document_id,
                @document_ref,
                @document_date,
                @short_code,
                @account_name,
                @amount,
                @amount,
                @company_id,
                @client_name,       
                @client_code,       
                @file_code,         
                @account_exec_code,
                @account_exec,
                @account_handler_code,
                @risk,
                '',
                '',
                '',
                @effective_date,
                @original_document_id,
                @orig_document_ref,
                @orig_document_type,
                @policy_ref,
                @business_type,
                @account_handler,
                @branch,
                NULL
            FROM transdetail TD
            WHERE TD.transdetail_id = @transdetail_id
            
        END
        ELSE
        BEGIN

            INSERT INTO #Report_Earned
            (
                document_id,
                document_ref,
                document_date,
                income_acc_code,
                income_acc_name,
                income_acc_amount,
                income_acc_amount_total,
                branch_id,
                account_name,
                account_code,
                file_code,
                account_exec_code, 
                account_exec,
                account_handler_code, 
                risk,
                insurer,
                extra,
                fee,
                effective_date,
                original_document_id,
                original_document_ref,
                original_document_type,
                policy_no,
                business_type,
                account_handler,
                branch,
                income_order
            )
            SELECT  
                @document_id,
                @document_ref,
                @document_date,
                @short_code,
                @account_name,
                (
                    SELECT
                        CASE WHEN @comm_total<>0 THEN
                            CASE WHEN PT.code='IN' THEN
				CASE WHEN @ins_comm_total<>0 THEN
                                    ROUND(((@amount * (ISNULL(SUM(TD.amount),0) + ((ISNULL(SUM(TD.amount),0) * ISNULL(@agent_total,0)) / @ins_comm_total))) / @comm_total) * @multiplier, 2)
				ELSE
				    '0.00'
				END
                            ELSE
                                ROUND(((@amount * ISNULL(SUM(TD.amount),0)) / @comm_total) * @multiplier, 2)
                            END
                        ELSE
                            '0.00'
                        END
                    FROM transdetail TD
                    WHERE TD.document_id = @original_document_id
                    AND TD.account_id = A.account_id
                    AND TD.spare = 'COMM'
                ),
                @amount,
                @company_id,
                @client_name,       
                @client_code,       
                @file_code,         
                @account_exec_code,
                @account_exec,
                @account_handler_code,
                @risk,
                CASE PT.code
                    WHEN 'IN' THEN P.shortname
                    ELSE ''
                END,
                CASE PT.code
                    WHEN 'EX' THEN P.shortname
                    ELSE ''
                END,
                CASE PT.code
                    WHEN 'FE' THEN P.shortname
                    ELSE ''
                END,
                @effective_date,
                @original_document_id,
                @orig_document_ref,
                @orig_document_type,
                @policy_ref,
                @business_type,
                @account_handler,
                @branch,
                CASE PT.code
                    WHEN 'IN' THEN 1
                    WHEN 'EX' THEN 2
                    WHEN 'FE' THEN 3
                END
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN party P
                ON P.party_cnt = A.account_key 
            JOIN party_type PT
                ON PT.party_type_id = P.party_type_id
            WHERE TD.document_id = @original_document_id
            AND A.account_id = ISNULL(@insurer_account_id, A.account_id)
            AND (
                    (
                        @report_type = 'Commission'
                        AND 
                        TD.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
                        AND 
                        PT.code IN ('IN', 'EX')
                    )
                    OR
                    (
                        @report_type = 'Fee'
                        AND 
                        PT.code IN ('FE')    
                        AND
                        P.shortname = @orig_short_code
                    )
                )

            /*If the calculations are out due to roundings modify the first value to get the total correct*/
            UPDATE #Report_Earned
            SET income_acc_amount = income_acc_amount + 
                (
                    income_acc_amount_total 
                    - 
                    (
                        SELECT
                            SUM(income_acc_amount)
                        FROM #Report_Earned
                        WHERE document_id = @document_id
                    )
                )
            WHERE document_id = @document_id
            AND id =
                (
                    SELECT
                        MIN(id)
                    FROM #Report_Earned
                    WHERE document_id = @document_id
                )
            AND income_acc_amount_total <> 
                (
                    SELECT
                        SUM(income_acc_amount)
                    FROM #Report_Earned
                    WHERE document_id = @document_id
                )
        END
    END

    FETCH NEXT FROM c_cursor INTO 
        @transdetail_id,
        @document_id,
        @document_ref,
        @document_date,
        @short_code,
        @account_name,
        @company_id,
        @amount
END

CLOSE c_cursor
DEALLOCATE c_cursor


/*Calculate brought forward figure*/
SELECT 
    @brought_forward = SUM(ROUND(ISNULL(TD.amount,0.00),2))
FROM #Report_Earned_Accounts EA
JOIN Transdetail TD
    ON TD.account_id = EA.account_id
JOIN Document D
    ON D.document_id = TD.document_id
WHERE TD.company_id = ISNULL(@branch_id, TD.company_id)
AND 
    (
        (
            @date_type = 'Transaction Date'
            AND
            (
                (
                    D.document_date < @start_date
                    AND 
                    TD.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                )
                OR
                (
                    TD.ref_date < @start_date
                    AND 
                    TD.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                )
            )
        )   
        OR
        (
            @date_type = 'Effective Date'
            AND
            (
                SELECT
                    TD.ref_date
                FROM transdetail TD
                WHERE TD.document_id = D.document_id
                AND TD.document_sequence = 1
            ) < @start_date
        )
        OR
        (
            @date_type = 'Trans/Effective Date'
            AND
            (
                (
                    D.document_date < @start_date
                    AND 
                    TD.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                )
                OR
                (
                    TD.ref_date < @start_date
                    AND 
                    TD.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
                )
            )
            AND
            (
                SELECT
                    TD.ref_date
                FROM transdetail TD
                WHERE TD.document_id = D.document_id
                AND TD.document_sequence = 1
            ) < @start_date
        )
    )

SET NOCOUNT OFF

-- Return data from Temporary Table
SELECT 
    @brought_forward 'Brought_Forward',
    document_id,
    document_ref,
    document_date,
    income_acc_code,
    income_acc_name,
    income_acc_amount,
    income_acc_amount_total,
    branch_id,
    account_name,
    account_code,
    file_code,
    account_exec_code, 
    account_exec,
    account_handler_code, 
    risk,
    insurer,
    extra,
    fee,
    effective_date,
    original_document_id,
    original_document_ref,
    original_document_type,
    policy_no,
    business_type,
    account_handler,
    branch,
    income_order
FROM #Report_Earned RT
ORDER BY
    income_acc_code,
    document_id,
    income_order
    
DROP TABLE #Report_Earned
DROP TABLE #Report_Earned_Accounts

GO


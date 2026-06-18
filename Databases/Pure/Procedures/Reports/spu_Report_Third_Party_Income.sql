SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Third_Party_Income'
GO

CREATE PROCEDURE spu_Report_Third_Party_Income
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @third_party_type VARCHAR(255)
AS

DECLARE 
    @transdetail_id INT,
    @document_ref VARCHAR(25),
    @document_id INT,
    @account_id INT,
    @account_code VARCHAR(30),
    @account_name VARCHAR(60),
    @company_id INT,
    @effective_date DATETIME,
    @amount MONEY,
    @agencyorunderwriting VARCHAR(1),
    @agenttypeid INT

SET NOCOUNT ON

IF @third_party_type = 'ALL' 
BEGIN
    SELECT @agenttypeid = NULL

END
ELSE
BEGIN
    SELECT @agenttypeid =   (
                SELECT party_agent_type_id 
                FROM party_agent_type
                WHERE rtrim([description]) = rtrim(@third_party_type)
                )
END

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

/* Clear the table */
CREATE TABLE #IncomeTrans
(
    transdetail_id INT,
    document_id INT,
    document_ref VARCHAR(25),
    gross_premium MONEY,
    ipt_amount MONEY,
    gross_commission MONEY,
    agent_commission MONEY,
    document_date DATETIME,
    branch_id INT,
    branch_code VARCHAR(20),
    commission_code VARCHAR(20),
    commission_name VARCHAR(255),
    commission_account_id INT,
    client_code VARCHAR(20),
    client_name VARCHAR(255),
    document_type_desc VARCHAR(255),
    effective_date DATETIME,
    fee_amount MONEY,
    discount_amount MONEY,
    extra_premium MONEY,
    extra_commission MONEY,
    insurance_ref VARCHAR(30),
    transaction_type_desc VARCHAR(255),
    business_type_desc VARCHAR(255),
    ipt_rate FLOAT,
    comm_opening_balance MONEY,
    adjustment BIT,
    cover_start_date DATETIME,
    expiry_date DATETIME,
    renewal_date DATETIME,
    this_premium MONEY,
    commission_amount MONEY,
    commission_percentage FLOAT,
    annual_premium MONEY,
    net_premium MONEY,
    agent_code VARCHAR(20),
    agent_name VARCHAR(255),
    agent_type_id INT
)
CREATE INDEX IncomeTrans__document_id ON #IncomeTrans(document_id)
  
  
/*Get all of the applicable documents and insert them into the working table*/
INSERT INTO #IncomeTrans 
(
    transdetail_id,
    document_id,
    document_date,
    adjustment
)
SELECT 
    TD.transdetail_id,
    D.document_id,
    D.document_date,
    0
FROM    Transdetail TD
JOIN Document D ON TD.document_id = D.document_id
WHERE D.document_date BETWEEN @start_date AND @end_date
AND D.company_id = ISNULL(@branch_id,D.company_id)
AND D.documenttype_id IN (4, 5, 15, 16, 17, 18, 31, 32, 35, 36)

/*Get all of the applicable adjustments and insert them into the working table*/
INSERT INTO #IncomeTrans 
(
    transdetail_id,
    document_id,
    document_date,
    adjustment
)  
SELECT  
    T.transdetail_id,   
    D.document_id,
    T.ref_date,
    1
FROM document D 
JOIN Transdetail T
    ON D.document_id = T.document_id
WHERE T.spare = 'BROK ADJ'
AND T.ref_date BETWEEN @start_date AND @end_date
AND D.company_id = ISNULL(@branch_id,D.company_id)
AND D.documenttype_id IN (4, 5, 15, 16, 17, 18, 31, 32, 35, 36)


/*Update all of the non-amount fields for each document_id in the table*/
UPDATE IT 
SET IT.document_ref = D.document_ref,
    IT.branch_id = D.company_id,
    IT.branch_code = S.code,
    IT.commission_code =    
        (
            SELECT ISNULL(A.short_code,'No Account')
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
            WHERE TD.document_id = D.document_id
            AND L.ledger_short_name = 'CO'
            GROUP BY A.short_code
        ),
    IT.commission_name = 
        (
            SELECT ISNULL(A.account_name,'No Commission Transacted')
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
            WHERE TD.document_id = D.document_id
            AND L.ledger_short_name = 'CO'
            GROUP BY A.account_name
        ),
    IT.commission_account_id = 
        (
            SELECT ISNULL(A.account_id,0)
            FROM transdetail TD
            JOIN account A
                ON A.account_id = TD.account_id
            JOIN ledger L
                ON L.ledger_id = A.ledger_id
            WHERE TD.document_id = D.document_id
            AND L.ledger_short_name = 'CO'
            GROUP BY A.account_id
        ),
    IT.client_code =    
        (
            SELECT ISNULL(P.shortname, '')
            FROM Transdetail T
            JOIN Account A
                ON A.account_id = T.account_id 
            JOIN Party P
                ON P.party_cnt = A.account_key
            WHERE T.document_id = D.document_id
            AND T.document_sequence = 1
        ),
    IT.client_name =    
        (
            SELECT ISNULL(P.resolved_name, '')
            FROM Transdetail T
            JOIN Account A
                ON A.account_id = T.account_id 
            JOIN Party P
                ON P.party_cnt = A.account_key
            WHERE T.document_id = D.document_id
            AND T.document_sequence = 1
        ),

    IT.document_type_desc = DT.description,
    IT.effective_date = 
        (
            SELECT ref_date
            FROM transdetail
            WHERE document_id = D.document_id
            AND document_sequence = 1
        ),
    IT.agent_code =         
            (   
            SELECT ISNULL(PA.shortname,'')
            FROM party PA
            JOIN account A ON PA.party_cnt = A.account_key
            JOIN TransDetail TD ON A.account_id = TD.account_id
            JOIN ledger L ON L.ledger_id = A.ledger_id
            WHERE   TD.transdetail_id = IT.transdetail_id
            AND TD.document_id = D.document_id
            AND TD.spare = 'AGENT ADJ' OR TD.spare = 'AGENT'
            AND     L.ledger_short_name IN ('NO','CO') /*Could be comm or comm earnt account*/
        ),
    IT.agent_name =
            (   
            SELECT ISNULL(PA.resolved_name,'')
            FROM party PA
            JOIN account A ON PA.party_cnt = A.account_key
            JOIN TransDetail TD ON A.account_id = TD.account_id
            JOIN ledger L ON L.ledger_id = A.ledger_id
            WHERE   TD.transdetail_id = IT.transdetail_id
            AND TD.document_id = D.document_id
            AND TD.spare = 'AGENT ADJ' OR TD.spare = 'AGENT'
            AND     L.ledger_short_name IN ('NO','CO') /*Could be comm or comm earnt account*/
        ),
    IT.agent_type_id =
            (   
            SELECT ISNULL(PAT.party_agent_type_id, 0)
            FROM party P
            JOIN Account A ON P.party_cnt = A.account_key
            JOIN Party_Agent PAT ON PAT.party_cnt = P.party_cnt
            JOIN TransDetail TD ON A.account_id = TD.account_id
            JOIN Ledger L ON L.ledger_id = A.ledger_id
            WHERE   TD.transdetail_id = IT.transdetail_id
            AND TD.document_id = D.document_id
            AND TD.spare = 'AGENT ADJ' OR TD.spare = 'AGENT'
            AND     L.ledger_short_name IN ('NO','CO') /*Could be comm or comm earnt account*/
        )
FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id
JOIN DocumentType DT
    ON DT.documenttype_id = D.documenttype_id 
JOIN Source S
    ON S.source_id = D.company_id

/*Update the amount fields for transactions*/
UPDATE IT 
SET IT.gross_premium = /*Gross Premium is the amount given to the insurer(s).*/
        (
            SELECT ISNULL(SUM(TDI.amount),0) * -1
            FROM transdetail TDI
            JOIN account AI
                ON AI.account_id = TDI.account_id           
            JOIN party P
                ON P.party_cnt = AI.account_key
                AND P.party_type_id = 7 /*Insurer*/
            WHERE TDI.document_id = D.document_id
            AND 
            ( 
                TDI.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
                OR
                (
                    TDI.spare = 'Reversal'
                    AND
                    (
                        (
                            D.documenttype_id IN (4, 15, 17, 31, 35) /*Debit*/
                            AND
                            TDI.amount > 0
                        )
                        OR
                        (
                            D.documenttype_id IN (5, 16, 18, 32, 36) /*Credit*/
                            AND
                            TDI.amount < 0
                        )
                    )
                )
            )
        ), 
    IT.ipt_amount =
        (
            SELECT 
                CASE
                    WHEN SUM(TDI.amount) < 0 THEN ISNULL(SUM(TDI.ref_amount),0)
                    WHEN SUM(TDI.amount) > 0 THEN ISNULL(SUM(TDI.ref_amount),0) * -1
                    ELSE 0
                END
            FROM transdetail TDI
            JOIN account AI
                ON AI.account_id = TDI.account_id           
            JOIN party P
                ON P.party_cnt = AI.account_key
                AND P.party_type_id = 7 /*Insurer*/
            WHERE TDI.document_id = D.document_id
            AND 
            ( 
                TDI.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
                OR
                (
                    TDI.spare = 'Reversal'
                    AND
                    (
                        (
                            D.documenttype_id IN (4, 15, 17, 31, 35) /*Debit*/
                            AND
                            TDI.amount > 0
                        )
                        OR
                        (
                            D.documenttype_id IN (5, 16, 18, 32, 36) /*Credit*/
                            AND
                            TDI.amount < 0
                        )
                    )
                )
            )
        ),
    IT.gross_commission = /*Gross Commission is the amount given back by the insurer(s).*/
        (
            SELECT ISNULL(SUM(TDI.amount),0)
            FROM transdetail TDI
            JOIN account AI
                ON AI.account_id = TDI.account_id           
            JOIN party P
                ON P.party_cnt = AI.account_key
                AND P.party_type_id = 7 /*Insurer*/
            WHERE TDI.document_id = D.document_id
            AND 
            ( 
                TDI.spare IN ('COMM', 'Reversed COMM', 'Reversal COMM')
                OR
                (
                    TDI.spare = 'Reversal'
                    AND
                    (
                        (
                            D.documenttype_id IN (4, 15, 17, 31, 35) /*Debit*/
                            AND
                            TDI.amount < 0
                        )
                        OR
                        (
                            D.documenttype_id IN (5, 16, 18, 32, 36) /*Credit*/
                            AND
                            TDI.amount > 0
                        )
                    )
                )
            )
        ),
    IT.agent_commission = 
        /*Agent Amount : 0 if no agent*/
        (
            SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)   
            FROM Transdetail    TD1
            JOIN Account A2
                ON A2.account_id = TD1.account_id
            WHERE TD1.document_id = D.Document_id
            AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name IN ('AG', 'TR') )
            AND TD1.spare <> 'AGENT ADJ'
        )
        +
        /*Sub Agent Amount (DD) : 0 if no sub agent or isn't a direct debit transaction*/
        (
            SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
            FROM Transdetail TD1
            JOIN Account A2
                ON A2.account_id = TD1.account_id
            WHERE TD1.document_id = D.Document_id
            AND A2.ledger_id in (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
            AND TD1.spare <> 'AGENT ADJ'
            AND TD1.document_sequence = 
                (
                    SELECT MIN(TD2.document_sequence)
                    FROM Transdetail TD2
                    WHERE TD2.document_id = TD1.document_id
                    AND  TD2.account_id = TD1.account_id
                )
            AND 0 <>
                (
                    SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
                    FROM Transdetail TD1
                    JOIN Account A2
                        ON A2.account_id = TD1.account_id
                    WHERE TD1.document_id = D.Document_id
                    AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                ) 
        )
        /*Sub Agent Amount (Not DD) : 0 if no sub agent or is a direct debit transaction*/
        -
        (
            (
                SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)       
                FROM Transdetail TD1
                JOIN Account A1
                    ON A1.account_id = TD1.account_id
                WHERE TD1.document_id = D.Document_id
                AND a1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                AND td1.document_sequence = 
                    (   
                        SELECT MIN(document_sequence)
                        FROM Transdetail        
                        WHERE document_id = td1.document_id 
                        AND a1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
                AND EXISTS
                    (
                        SELECT NULL
                        FROM Transdetail TD2
                        JOIN Account A2
                            ON A2.account_id = TD2.account_id
                        WHERE TD2.document_id = D.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
                        AND TD2.spare <> 'AGENT ADJ'
                    )
                AND 0 = 
                    (
                        SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
                        FROM Transdetail TD1
                        JOIN Account A2
                            ON A2.account_id = TD1.account_id
                        WHERE TD1.document_id = D.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
            )
            -   
            (   
                SELECT ISNULL(SUM(ROUND(amount,2)),0)
                FROM Transdetail TD1
                JOIN Account A1
                    ON A1.account_id = TD1.account_id
                WHERE TD1.document_id = D.Document_id
                AND A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
                AND TD1.spare <> 'AGENT ADJ'        
                AND 0 =
                    (
                        SELECT ISNULL(SUM(ROUND(TD1.amount,2)),0)
                        FROM Transdetail TD1
                        JOIN Account A2
                            ON A2.account_id = TD1.account_id
                        WHERE TD1.document_id = D.Document_id
                        AND A2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                    )
            )
        ),
    IT.fee_amount = 
        (
            SELECT ISNULL(SUM(TDF.amount), 0.0) * -1
            FROM Transdetail TDF
            JOIN account AF
                ON AF.account_id = TDF.account_id           
            JOIN party P
                ON P.party_cnt = AF.account_key
            WHERE TDF.document_id = D.document_id
            AND P.party_type_id = 9 /*Fee*/
        ),
    IT.discount_amount = 
        (
            SELECT ISNULL(SUM(TDD.amount), 0.0)
            FROM Transdetail TDD
            JOIN account AD
                ON AD.account_id = TDD.account_id           
            JOIN party P
                ON P.party_cnt = AD.account_key
            WHERE TDD.document_id = D.document_id
            AND P.party_type_id = 11 /*Discount*/
        ),
    IT.extra_premium = /*Extra Premium is the amount given to the extra(s).*/
        (
            SELECT ISNULL(SUM(TDE.amount), 0.0)  * -1
            FROM Transdetail TDE
            JOIN account AE
                ON AE.account_id = TDE.account_id           
            JOIN party P
                ON P.party_cnt = AE.account_key
            WHERE TDE.document_id = D.document_id
            AND P.party_type_id = 10 /*Extra*/
            AND 
            ( 
                TDE.spare IN ('GROSS', 'Reversed GROSS', 'Reversal GROSS')
                OR
                (
                    TDE.spare = 'Reversal'
                    AND
                    (
                        (
                            D.documenttype_id IN (4, 15, 17, 31, 35) /*Debit*/
                            AND
                            TDE.amount > 0
                        )
                        OR
                        (
                            D.documenttype_id IN (5, 16, 18, 32, 36) /*Credit*/
                            AND
                            TDE.amount < 0
                        )
                    )
                )
            )   
        ),
    IT.extra_commission = /*Extra Commission is the amount given back by the extra(s).*/
        (
            SELECT ISNULL(SUM(TDE.amount), 0.0)
            FROM Transdetail TDE
            JOIN account AE
                ON AE.account_id = TDE.account_id           
            JOIN party P
                ON P.party_cnt = AE.account_key
            WHERE TDE.document_id = D.document_id
            AND P.party_type_id = 10 /*Extra*/
            AND 
            ( 
                TDE.spare IN ('COMM', 'Reversed COMM', 'Reversal COMM')
                OR
                (
                    TDE.spare = 'Reversal'
                    AND
                    (
                        (
                            D.documenttype_id IN (4, 15, 17, 31, 35) /*Debit*/
                            AND
                            TDE.amount < 0
                        )
                        OR
                        (
                            D.documenttype_id IN (5, 16, 18, 32, 36) /*Credit*/
                            AND
                            TDE.amount > 0
                        )
                    )
                )
            )
        )
FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id

/*Update the amount fields for adjustments*/
UPDATE IT
SET IT.gross_premium = 0,
    IT.ipt_amount = 0,
    IT.fee_amount = 0,
    IT.discount_amount = 0,
    IT.extra_premium = 0,
    IT.extra_commission = 0,
    IT.gross_commission = 
        (
            SELECT ISNULL(SUM(ROUND(T.amount,2)),0)
            FROM Transdetail T
            WHERE T.document_id = IT.document_id
            AND T.ref_date = IT.document_date
            AND T.spare = 'COMM ADJ'
        ), /*Calculate Insurer Adjustment*/
    IT.agent_commission = 
        (
            SELECT ISNULL(SUM(ROUND(T.amount,2)),0)
            FROM Transdetail T
            WHERE T.document_id = IT.document_id
            AND T.ref_date = IT.document_date
            AND T.spare = 'AGENT ADJ' 
        ) /*Calculate Agent Adjustment*/
FROM #IncomeTrans IT
WHERE IT.adjustment = 1

/*Update values that require a transaction export record.*/
UPDATE IT
SET IT.insurance_ref = TEF.insurance_ref,
    IT.transaction_type_desc = TT.description,
    IT.business_type_desc = BT.description
FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id
JOIN Transaction_Export_Folder TEF
    ON TEF.document_ref = D.document_ref
    AND TEF.source_id = D.company_id
    AND TEF.accounts_export_status = 'c'
JOIN Transaction_Type TT
    ON TT.transaction_type_id = TEF.transaction_type_id
JOIN Business_Type BT
    ON BT.business_type_id = TEF.business_type_id

    
/*Update values that require a insurance file record.*/
UPDATE IT
SET IT.insurance_ref = ISNULL(IT.insurance_ref,I.insurance_ref),
    IT.ipt_rate = IPT.rate,
    IT.cover_start_date = I.cover_start_date,
    IT.expiry_date = I.expiry_date,
    IT.renewal_date = I.renewal_date,
    IT.this_premium = ISNULL(I.this_premium, 0),
    IT.commission_amount =
        (
            ISNULL(I.commission_amount, 0)
            + 
            (
                SELECT ISNULL(SUM(coinsurer_commission_amount),0) 
                FROM policy_coinsurers 
                WHERE insurance_file_cnt = I.insurance_file_cnt
            ) 
        ),
    IT.commission_percentage = ISNULL(I.commission_percentage,0),
    IT.annual_premium = ISNULL(I.annual_premium,0),
    IT.net_premium = ISNULL(I.net_premium, 0)
FROM #IncomeTrans IT
JOIN Document D
    ON D.document_id = IT.document_id
JOIN Insurance_File I
    ON I.insurance_file_cnt = D.insurance_file_cnt
JOIN Insurance_File_Type IFT
    ON IFT.insurance_file_type_id = I.insurance_file_type_id
LEFT JOIN Insurance_File_Status IFS
    ON IFS.insurance_file_status_id = I.insurance_file_status_id    
JOIN Party P
    ON P.party_cnt = I.lead_insurer_cnt
JOIN Risk_Code RC
    ON RC.risk_code_id = I.risk_code_id
LEFT JOIN party PA
    ON PA.party_cnt = I.account_handler_cnt
LEFT JOIN IPT
    ON IPT.risk_code_id = RC.risk_code_id
    AND IPT.effective_date =
        (
            SELECT MAX(effective_date)
            FROM IPT
            WHERE effective_date <= D.document_date
            AND risk_code_id = RC.risk_code_id
        )
LEFT JOIN Analysis_Code AC
    ON AC.analysis_code_id = I.analysis_code_id
    
/*Update rows with the opening balance of the commission accounts.*/
DECLARE c_comm_account CURSOR FAST_FORWARD FOR
    SELECT commission_account_id
    FROM #IncomeTrans IT
    GROUP BY commission_account_id

OPEN c_comm_account

FETCH NEXT FROM c_comm_account INTO @account_id

WHILE @@FETCH_STATUS = 0
BEGIN

    SELECT @amount = 
        (
            SELECT ISNULL(SUM(ROUND(TD.amount,2)),0)
            FROM Transdetail TD
            JOIN Document D
                ON D.document_id = TD.document_id
            WHERE D.document_date < @start_date
            AND TD.account_id = @account_id
        )
    
    UPDATE #IncomeTrans
    SET comm_opening_balance = @amount
    WHERE commission_account_id = @account_id

    FETCH NEXT FROM c_comm_account INTO @account_id
END 

CLOSE c_comm_account
DEALLOCATE c_comm_account

SET NOCOUNT OFF

/* Return required data */
SELECT DISTINCT 
    document_id,
    document_ref,
    gross_premium,
    ipt_amount,
    gross_commission,
    agent_commission,
    document_date,
    branch_id,
    branch_code,
    commission_code,
    commission_name,
    commission_account_id,
    client_code,
    client_name,
    document_type_desc,
    effective_date,
    fee_amount,
    discount_amount,
    extra_premium,
    extra_commission,
    insurance_ref,
    transaction_type_desc,
    business_type_desc,
    ipt_rate,
    comm_opening_balance,
    adjustment,
    cover_start_date,
    expiry_date,
    renewal_date,
    this_premium,
    commission_amount,
    commission_percentage,
    annual_premium,
    net_premium,
    agent_code,
    agent_name,
    agent_type_id
FROM    #IncomeTrans 
WHERE   ((@AgentTypeId <> 0 AND agent_type_id = @AgentTypeId) OR (@AgentTypeId IS NULL AND agent_type_id IS NOT NULL)) 
ORDER BY
    agent_code,
    commission_code,
    document_ref,
    document_date

DROP TABLE #IncomeTrans

GO

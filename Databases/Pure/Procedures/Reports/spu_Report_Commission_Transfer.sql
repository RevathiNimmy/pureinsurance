SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Commission_Transfer'
GO

CREATE PROCEDURE spu_Report_Commission_Transfer 

    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME

AS

DECLARE @agencyorunderwriting VARCHAR(1)

IF @branch_id = 0 
BEGIN
    SELECT @branch_id = NULL
END 

SELECT @agencyorunderwriting = (SELECT value FROM hidden_options WHERE option_number = 1 )


CREATE TABLE #Report_Temp
(
    Insurer VARCHAR(60),
    Insurer_Code VARCHAR(30),
    Document_Ref VARCHAR(20),
    Policy_Doc_Date DATETIME,
    Document_Type VARCHAR(255),
    Client VARCHAR(60),
    Client_Code CHAR(30),
    Policy_Ref VARCHAR(30),
    Gross_Premium MONEY,
    Gross_Commission MONEY,
    IPT MONEY,
    Agent_Commission MONEY,
    SubAgent_Commission MONEY
)

INSERT INTO #Report_Temp
SELECT DISTINCT
    ISNULL(P.name, '') 'Insurer',
    ISNULL(P.shortname, '') 'Insurer_Code',
    PaidD.document_ref 'Document_Ref',
    MIN(PaidD.document_date) 'Policy_Doc_Date',
    DT.description 'Document_Type',
    (
        SELECT  ISNULL(A1.account_name, '')
        FROM    Transdetail     TD1
        JOIN    Account         A1
        ON      A1.account_id = TD1.account_id
        WHERE   TD1.document_id = PaidD.document_id
        AND     TD1.document_sequence = 
        (   
            SELECT  MIN(document_sequence)
            FROM    transdetail     
            WHERE   document_id = td1.document_id 
            AND ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA' )--PN4707
        )
    ) 'Client',
    (
        SELECT  ISNULL(A1.short_code, '')
        FROM    Transdetail TD1
        JOIN    Account     A1
        ON      A1.account_id = TD1.account_id
        WHERE   TD1.document_id = PaidD.document_id
        AND     TD1.document_sequence = 
        (   
            SELECT  MIN(document_sequence)
            FROM    transdetail     
            WHERE   document_id = td1.document_id 
            AND ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA' )--PN4707
        )
    ) 'Client_Code',
    (
        SELECT  ISNULL(MAX(insurance_ref), '')
        FROM    transdetail
        WHERE   document_id = PaidD.document_id 
    ) 'Policy_Ref',
    (   
        SELECT  SUM(-1 * ISNULL(ROUND(currency_amount,2), 0))
        FROM    transdetail     td2
        WHERE   td2.document_id = PaidD.document_id
        AND     td2.document_sequence = 
        (
            SELECT  MIN(document_sequence)
            FROM    transdetail     
            WHERE   document_id = td2.document_id 
            AND     account_id = A.account_id
        )
    ) 'Gross_Premium',
    (   
        SELECT  ISNULL(SUM(ROUND(ISNULL(currency_amount,0),2)),0) 
        FROM    transdetail TDComm
        JOIN    document DComm
        ON      DComm.document_id = TDComm.document_id
        WHERE   DComm.document_id = PaidD.document_id
        AND     (
                    (   
                        TDComm.spare LIKE 'COMM'
                        OR
                        TDComm.spare LIKE 'COMM ADJ'                    
                    )
                    OR
                    (   
                        Dcomm.documenttype_id IN (8, 10, 11, 12, 20, 21)  
                    )
                )
        AND     TDComm.account_id = A.account_id
    ) 'Gross_Commission',
    (   
        SELECT  ISNULL(SUM(ROUND(ISNULL(round(TD1.ref_amount,2) * (round(TD1.amount,2)/abs(round(TD1.amount,2))), 0),2)),0)
        FROM    TransDetail     TD1
        WHERE   TD1.document_id = PaidD.document_id
        AND     TD1.amount <> 0
        AND     TD1.ref_amount <> 0 
        AND     TD1.account_id = A.account_id
    ) 'IPT',
    (   
        SELECT  ISNULL(SUM(ROUND(ISNULL(currency_amount,0),2)),0) * -1
        FROM    transdetail TDComm
        JOIN    document DComm
        ON      DComm.document_id = TDComm.document_id
        JOIN    account AComm
        ON      AComm.account_id = TDComm.account_id
        JOIN    ledger LComm
        ON      LComm.ledger_id = AComm.ledger_id
        WHERE   TDComm.document_id = PaidD.document_id
        AND     (
                    (   
                        TDComm.spare LIKE 'AGENT'
                        OR
                        TDComm.spare like 'AGENT ADJ'
                    )
                    AND
                    (   
                        Dcomm.documenttype_id NOT IN (8, 10, 11, 12, 20, 21)  
                    )
                )
        AND     LComm.ledger_short_name IN ( 'AG', 'TR' )
    ) 'Agent_Commission',
    (
        (
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(amount,0),2)),0)
                FROM transdetail TDCli
                JOIN account ACli
                ON ACli.account_id = TDCli.account_id
                WHERE TDCli.document_id = PaidD.Document_id
                AND ACli.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') 
                AND TDCli.document_sequence = 
                (   
                    SELECT MIN(document_sequence)
                    FROM transdetail TDCli2
                    JOIN account ACli2
                    ON ACli2.account_id = TDCli2.account_id
                    WHERE TDCli2.document_id = TDCli.document_id 
                    AND ACli2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
                )
                AND EXISTS
                (   
                    SELECT NULL
                    FROM Transdetail TDSA
                    JOIN Account ASA
                    ON ASA.account_id = TDSA.account_id
                    WHERE TDSA.document_id = PaidD.Document_id
                    AND ASA.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB')
                )
            )
        )
        -   
        (   
            (
                SELECT ISNULL(SUM(ROUND(ISNULL(amount,0),2)),0)
                FROM Transdetail TDSA
                JOIN Account ASA
                ON ASA.account_id = TDSA.account_id
                WHERE   TDSA.document_id = PaidD.Document_id
                AND ASA.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB') 
            )                   
        )
    ) 'SubAgent_Commission'
FROM CashList C
JOIN CashListItem I
    ON C.cashlist_id = I.cashlist_id 
JOIN TransDetail TD
    ON I.transdetail_id = TD.transdetail_id
JOIN Document D
    ON D.document_id = TD.document_id
JOIN Account A
    ON TD.account_id = A.account_id
JOIN Ledger L
    ON L.Ledger_Id = A.Ledger_Id
JOIN Party P
    ON A.account_key =   P.party_cnt
JOIN AllocationDetail AL
    ON AL.cashlistitem_id = I.cashlistitem_id
JOIN TransDetail PaidTD
    ON AL.transdetail_id = PaidTD.transdetail_id
JOIN Document PaidD
    ON PaidTD.document_id = PaidD.document_id
JOIN DocumentType DT
    ON DT.documenttype_id = PaidD.documenttype_id
WHERE D.company_id = ISNULL(@branch_id, D.company_id)
AND I.transaction_date BETWEEN @start_date AND @end_date
AND L.ledger_short_name = 'IN'
AND (
        SELECT SUM(ROUND(td.amount,2))
        FROM transdetail td
        WHERE td.document_id = PaidD.document_id
        AND td.account_id = A.account_id
    )
    =
    (
        SELECT SUM(ROUND(tm.base_match_amount,2))
        FROM transmatch tm
        JOIN transdetail td
            ON td.transdetail_id = tm.transdetail_id
        WHERE td.document_id = PaidD.document_id
        AND td.account_id = A.account_id
        AND tm.allocationdetail_id IS NOT NULL
    )
GROUP BY
    A.account_id,           
    P.name,
    P.shortname,
    PaidD.document_ref,
    PaidD.document_id,
    DT.description,
    L.ledger_short_name
ORDER BY
    Client_Code

SELECT  
    Insurer,
    Insurer_Code,
    Document_Ref,
    Policy_Doc_Date,
    Document_Type,
    Client,
    Client_Code,
    Policy_Ref,
    Gross_Premium,
    Gross_Commission,
    IPT,
    Agent_Commission,
    SubAgent_Commission,
    @agencyorunderwriting agencyorunderwriting
FROM #Report_Temp

DROP TABLE #Report_Temp

GO

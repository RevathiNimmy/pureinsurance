SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Projected_Income'
GO

CREATE PROCEDURE spu_Report_Projected_Income
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME
AS

/*Validate input parameters*/
IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #Income
(
    commission_code VARCHAR(20),
    commission_name VARCHAR(255),
    client_code VARCHAR(20),
    client_name VARCHAR(255),
    insurer_code VARCHAR(20),
    insurer_name VARCHAR(255),
    document_ref VARCHAR(25),
    insurance_ref VARCHAR(30),
    effective_date DATETIME,
    risk_description VARCHAR(255),
    commission_amount MONEY,
    fsa_disabled BIT,
    transacted_amount MONEY,
    amount_settled MONEY,
    date_settled DATETIME,
    reversed BIT
)

IF NOT EXISTS
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    
    INSERT INTO #Income
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT 
        *
    FROM #Income
    
    DROP TABLE #Income
        
    RETURN  
END

INSERT INTO #income
SELECT 
    PComm.shortname,
    PComm.resolved_name,
    PClient.shortname,  
    PClient.resolved_name,
    PInsurer.shortname, 
    PInsurer.resolved_name, 
    D.document_ref,
    I.insurance_ref,
    TD.ref_date ,
    RC.description, 
    (
        SELECT 
            ISNULL(SUM(TD.amount),0)
        FROM transdetail TD
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN ledger L
            ON L.ledger_id = A.ledger_id
            AND L.ledger_short_name = 'CO'
        WHERE TD.document_id = D.document_id
    ),
    NULL,
    (
        SELECT 
            ISNULL(SUM(TD.amount),0)
        FROM transdetail TD
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN ledger L
            ON L.ledger_id = A.ledger_id
            AND L.ledger_short_name = 'SA'
        WHERE TD.document_id = D.document_id
    ),
    (
        SELECT 
            ISNULL(SUM(TM.base_match_amount),0)
        FROM transmatch TM
        JOIN transdetail TD
            ON TD.transdetail_id = TM.transdetail_id
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN ledger L
            ON L.ledger_id = A.ledger_id
            AND L.ledger_short_name = 'SA'
        WHERE TD.document_id = D.document_id
    ),
    (
        SELECT 
            MAX(MG.match_date)
        FROM matchgroup MG
        JOIN transmatch TM
            ON TM.match_id = MG.match_id
        JOIN transdetail TD
            ON TD.transdetail_id = TM.transdetail_id
        JOIN account A
            ON A.account_id = TD.account_id
        JOIN ledger L
            ON L.ledger_id = A.ledger_id
            AND L.ledger_short_name = 'SA'
        WHERE TD.document_id = D.document_id
    ),
    CASE 
        WHEN D.comment LIKE '%Revers%' THEN 1
        ELSE 0
    END
FROM document D
JOIN transdetail TD
    ON TD.document_id = D.document_id 
    AND TD.document_sequence = 1
JOIN insurance_file I
    ON I.insurance_file_cnt = D.insurance_file_cnt
JOIN party PClient
    ON PClient.party_cnt = I.insured_cnt
JOIN party PInsurer
    ON PInsurer.party_cnt = I.lead_insurer_cnt
JOIN party PComm
    ON PComm.party_cnt = I.broker_cnt
JOIN risk_code RC
    ON RC.risk_code_id = I.risk_code_id
WHERE D.documenttype_id IN (4,5,15,16,17,18,31,32,35,36) /*SND,SNC,SRD,SRC,SED,SEC,SHD,SHC,TRD,TRC*/
AND D.company_id = ISNULL(@branch_id, D.company_id)
AND TD.ref_date BETWEEN @start_date AND @end_date


SELECT 
    *
FROM #Income
ORDER BY 
    effective_date,
    document_ref

DROP TABLE #Income

GO
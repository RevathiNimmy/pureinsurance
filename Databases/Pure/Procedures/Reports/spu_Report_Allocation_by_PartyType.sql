SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON  
GO

EXECUTE DDLDropProcedure 'spu_Report_Allocation_by_PartyType' 
GO

CREATE PROCEDURE spu_Report_Allocation_by_PartyType
    @branch_id INT ,
    @start_date DATETIME,
    @end_date DATETIME,
    @date_type VARCHAR(20),
    @party_type VARCHAR(30) 
AS

DECLARE @ledger_short_name VARCHAR(2)

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END
 
 
CREATE TABLE #Allocations
(
    ledger_id INT,
    account_id INT,
    account_type_count INT,
    document_id INT,
    document_ref VARCHAR(30),
    client_code VARCHAR(20),
    client_name VARCHAR(255),
    transdetail_id INT,
    insurance_ref VARCHAR(30),
    insured_cnt INT,
    posting_date DATETIME,
    effective_date DATETIME,
    risk_type VARCHAR(255),
    amount_settled MONEY,
    settled_date DATETIME,
    subagent_transdetail_id INT,
    instalmentplan_transdetail_id INT,
    fsa_disabled BIT
)

IF NOT EXISTS
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    
    INSERT INTO #Allocations
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT 
        *
    FROM #Allocations
    
    DROP TABLE #Allocations
        
    RETURN  
END

SELECT @ledger_short_name = 
    CASE @party_type
        WHEN 'Clients' THEN 'SA'
        WHEN 'Insurers' THEN 'IN'
        WHEN 'Agents' THEN 'AG'
        WHEN 'Sub Agents' THEN 'UB' 
        WHEN 'Discounts' THEN 'DI'
        WHEN 'Fees' THEN  'FE'
        WHEN 'Finance Providers' THEN  'RF'
        WHEN 'Extras' THEN 'IN'
    END

If @party_type <> 'Finance Providers'
BEGIN 

    INSERT INTO #Allocations
    SELECT  
        L.ledger_id,
        A.account_id,
        (
            SELECT  
                ISNULL(SUM(1),1) 
            FROM Transdetail T2   
            JOIN Account A2 
                ON A2.account_id = T2.account_id 
            JOIN Ledger L2 
                ON L2.ledger_id = A2.ledger_id
            WHERE L2.ledger_short_name = @ledger_short_name
            AND T2.document_id = D.document_id
            AND T2.document_sequence = 
                (
                    SELECT 
                        MIN(document_sequence) 
                    FROM transdetail
                    WHERE document_id = T2.document_id
                    AND account_id = T2.account_id
                )
        ), 
        D.document_id,
        D.document_ref,
        P.shortname,
        CASE @ledger_short_name
            WHEN 'SA' THEN P.resolved_name
            ELSE P.name
        END,
        T.transdetail_id,
        T.insurance_ref,
        I.insured_cnt,
        D.document_date,
        T.ref_date,
        RC.description,
        T.amount,
        (
            SELECT 
                MAX(MG.match_date) 
            FROM matchgroup MG
            JOIN transmatch TM 
                ON TM.match_id = MG.match_id
            WHERE TM.transdetail_id = T.transdetail_id
        ),
        (
            SELECT  
                ISNULL(MIN(T2.transdetail_id),0)  
            FROM transdetail T2 
            JOIN account A2 
                ON A2.account_id = T2.account_id
            JOIN ledger L2 
                ON L2.ledger_id = A2.ledger_id
            WHERE L2.ledger_short_name = 'UB'
            AND T2.Document_id = D.document_id
        ),
        (
            SELECT
                ISNULL(MIN(PFPF.PlanTransaction_id),0) 
            FROM PFTransaction_Id PFT
            JOIN PFPremiumFinance PFPF 
                ON PFPF.pfprem_finance_cnt = PFT.pfprem_finance_cnt
            JOIN PFScheme PFS 
                ON PFS.CompanyNo = PFPF.CompanyNo
                AND PFS.SchemeNo = PFPF.SchemeNo
                AND PFS.SchemeVersion = PFPF.SchemeVersion
            JOIN PFScheme_type PFST 
                ON PFST.PFSCheme_type_id = PFS.PFScheme_type_id
                AND PFPF.pfprem_finance_version = PFT.pfprem_finance_version
            WHERE PFT.pftransaction_id = T.transdetail_id
            AND PFST.code = 'IH'
        ),
        NULL
    FROM Document D
    JOIN Transdetail T 
        ON T.document_id = D.document_id
    JOIN Account A 
        ON A.account_id = T.account_id
    JOIN Ledger L 
        ON L.ledger_id = A.ledger_id
    LEFT JOIN Insurance_file I 
        ON I.insurance_file_cnt = D.insurance_file_cnt
    LEFT JOIN Risk_code RC 
        ON RC.risk_code_id = I.risk_code_id
    JOIN Party P 
        ON P.party_cnt = A.account_key
    JOIN Party_Type PT 
        ON PT.party_type_id = P.party_type_id
    WHERE D.comment NOT LIKE '%Revers%'
    AND (
            D.documenttype_id IN (4,5,15,16,17,18,30,31,32,35,36) /*SND,SNC,SRD,SRC,SED,SEC,SHD,SHC,TRD,TRC, FEE */
            OR 
            (
                D.documenttype_id = 1 
                AND 
                RTRIM(D.comment) = 'Premium Finance Transfer'
            )
        )
    AND T.outstanding_amount = 0
    AND T.transdetail_id = 
        (
            SELECT
                MIN(T2.transdetail_id) 
            FROM Transdetail T2
            WHERE T2.Document_id = D.document_id
            AND T2.Account_id = A.account_id
        )
    AND D.company_id = ISNULL(@branch_id, D.company_id) 
    AND (
            (
                L.ledger_short_name <> 'IN'
                AND
                L.ledger_short_name = @ledger_short_name
            )
            OR    
            (
                L.ledger_short_name = 'IN'
                AND
                @party_type = 'Insurers' 
                AND 
                PT.code = 'IN'
            )
            OR
            (
                L.ledger_short_name = 'EX'
                AND
                @party_type = 'Extras' 
                AND 
                PT.code = 'EX'
            )
        ) 
END
ELSE
BEGIN
    INSERT INTO #Allocations
    SELECT  
        L.ledger_id, 
        A.account_id,
        0,
        D.document_id,
        D.document_ref,
        P.shortname,
        P.name,
        T.transdetail_id,
        TC.insurance_ref,
        I.insured_cnt,
        D.document_date,
        T.ref_date,
        RC.description,
        T.amount,
        (
            SELECT 
                MAX(MG.match_date) 
            FROM matchgroup MG
            JOIN transmatch TM 
                ON TM.match_id = MG.match_id
            WHERE TM.transdetail_id = T.transdetail_id
        ),
        0, 
        0,
        NULL
    FROM Document D
    JOIN Transdetail TC 
        ON TC.document_id = D.document_id
    JOIN PFTransaction_id PFT 
        ON PFT.pftransaction_id = TC.transdetail_id
    JOIN PFPremiumFinance PFPF 
        ON PFPF.pfprem_finance_cnt = PFT.pfprem_finance_cnt
        AND PFPF.pfprem_finance_version = PFT.pfprem_finance_version
    JOIN Transdetail T 
        ON T.transdetail_id = PFPF.plantransaction_id
    JOIN Account A 
        ON A.account_id = T.account_id
    JOIN Ledger L 
        ON L.ledger_id = A.ledger_id
    JOIN Insurance_file  I 
        ON I.insurance_file_cnt = D.insurance_file_cnt
    JOIN Risk_code RC 
        ON RC.risk_code_id = I.risk_code_id
    JOIN Party PC 
        ON PC.party_cnt = I.insured_cnt
    JOIN Party P 
        ON P.party_cnt = A.account_key
    WHERE TC.document_sequence = 1
    AND D.comment NOT LIKE '%Revers%'
    AND T.outstanding_amount = 0
    AND L.ledger_short_name = 'RF'
    AND d.company_id = ISNULL(@branch_id, d.company_id) 

END

/*Delete records not in the date range */
 
IF @date_type = 'Transaction Date'
BEGIN
    DELETE 
    FROM #Allocations 
    WHERE posting_date < @start_date
    OR posting_date > @end_date
END

IF @date_type = 'Effective Date'
BEGIN
    DELETE 
    FROM #Allocations 
    WHERE effective_date < @start_date
    OR effective_date > @end_date 
END

IF @date_type = 'Settled Date'
BEGIN
    DELETE 
    FROM #Allocations 
    WHERE settled_date < @start_date
    OR settled_date > @end_date  
END
 
/* Delete Client Paid transaction if Sub Agent & Instalment Plans present
and these transactions haven't been settled */
IF @party_type = 'Clients'
BEGIN

    DELETE #Allocations 
    FROM #Allocations AL
    WHERE AL.subagent_transdetail_id <> 0
    AND AL.subagent_transdetail_id IN 
        (
            SELECT 
                transdetail_id 
            FROM transdetail
            WHERE transdetail_id = AL.subagent_transdetail_id
            AND outstanding_amount <> 0
        )
        
    DELETE #Allocations 
    FROM #Allocations AL
    WHERE AL.subagent_transdetail_id <> 0
    AND AL.instalmentplan_transdetail_id IN 
        (
            SELECT 
                transdetail_id 
            FROM transdetail
            WHERE transdetail_id = AL.instalmentplan_transdetail_id
            AND outstanding_amount <> 0
        )
END  

/* Update gross premium for Insurers*/
If @party_type in ('Insurers','Extras')
BEGIN

    UPDATE AL 
    SET AL.amount_settled = 
        (
            SELECT 
                SUM(T2.amount) 
            FROM Transdetail T2                     
            JOIN Account A 
                ON A.account_id = T2.account_id
            WHERE T2.account_id = AL.account_id
            AND T2.document_id = AL.document_id
        )
    FROM #Allocations AL  
END

/* Update gross premium for Clients with shares*/                      
If @party_type = 'Clients'
BEGIN
    UPDATE AL
    SET AL.amount_settled = AL.amount_settled + ISNULL(T2.amount,0) 
    FROM #Allocations AL 
    JOIN Party P
        ON P.party_cnt = AL.insured_cnt
    JOIN Account A
        ON A.account_key = p.party_cnt
        AND A.account_id = AL.account_id
    JOIN Transdetail T2                     
        ON T2.account_id = AL.account_id
        AND T2.document_id = AL.document_id
    JOIN Account A2 
        ON A2.account_id = T2.account_id
    WHERE AL.account_type_count > 1
    AND T2.transdetail_id =
        (
            SELECT 
                MIN(transdetail_id) 
            FROM transdetail
            WHERE account_id = AL.account_id
            AND document_id = AL.document_id
            AND transdetail_id <> AL.transdetail_id
        )      

    DELETE 
    FROM #Allocations 
    WHERE amount_settled = 0

    UPDATE AL
    SET AL.settled_date = 
        (
            SELECT 
                MAX(MG.match_date) 
            FROM transmatch TM 
            JOIN matchgroup MG
                ON MG.match_id = TM.match_id
            WHERE TM.transdetail_id = AL.subagent_transdetail_id
        )
    FROM #Allocations AL    
    WHERE AL.subagent_transdetail_id <> 0  
    
    UPDATE AL
    SET AL.settled_date = 
        (
            SELECT 
                MAX(MG.match_date) 
            FROM transmatch TM 
            JOIN matchgroup MG
                ON MG.match_id = TM.match_id
            WHERE TM.transdetail_id = AL.instalmentplan_transdetail_id
        )
    FROM #Allocations AL    
    WHERE AL.instalmentplan_transdetail_id <> 0  

END
 
UPDATE AL
SET AL.amount_settled = AL.amount_settled * -1
FROM #Allocations AL
WHERE AL.amount_settled < 0  

SELECT 
    * 
FROM #Allocations
ORDER BY settled_date

DROP TABLE #Allocations
   
GO
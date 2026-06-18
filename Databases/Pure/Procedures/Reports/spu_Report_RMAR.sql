SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_report_RMAR'
GO

CREATE PROCEDURE spu_report_RMAR
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @IncludeQuotes VARCHAR(10)

AS

DECLARE @total_non_investment_premium MONEY
DECLARE @total_net_premium MONEY
DECLARE @total_net_commission MONEY
DECLARE @claims_count INT

SET NOCOUNT ON

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #TempRetailBusiness
(
    /*FSA Details*/
    FSA_product_id INT,
    FSA_product_description VARCHAR(255),

    /*Document Details*/
    document_id INT,
    document_ref VARCHAR(50),
    document_date DATETIME,

    /*Policy Details*/
    insurance_file_cnt INT,
    insurance_ref VARCHAR(255),
    insured_name VARCHAR(255),
    
    /*Values*/
    client_gross_premium MONEY,
    total_non_investment_premium MONEY, 
    claims_count INT,
    
    /*Selection Details*/
    is_sub_agent BIT,
    is_chain BIT,
    is_delegated_authority BIT,
    
    /*FSA Disabled Flag*/
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
    INSERT INTO #TempRetailBusiness
    (
        fsa_disabled
    )
    VALUES
    (
        1
    )
    
    SELECT *
    FROM #TempRetailBusiness
    
    DROP TABLE #TempRetailBusiness
    
    RETURN
END


/*Add lines for all valid transactions not including the extras*/
INSERT INTO #TempRetailBusiness
SELECT
    /*FSA Details*/
    FP.FSA_product_id,
    FP.description,
    
    /*Document Details*/
    D.document_id,
    D.document_ref,
    D.document_date,
    
    /*Policy Details*/
    I.insurance_file_cnt,
    I.insurance_ref,
    I.insured_name,
    
    /*Values*/
    (
        SELECT 
            SUM(td.amount) -
                (
                    SELECT
                        ISNULL(SUM(td.amount), 0) * -1
                    FROM transdetail td
                    JOIN transdetail_type tt
                        ON tt.transdetail_type_id = td.transdetail_type_id
                    JOIN account a 
                        ON a.account_id = td.account_id
                    JOIN party p
                        ON p.party_cnt = a.account_key
                    JOIN party_type pt
                        ON pt.party_type_id = p.party_type_id
                        AND pt.code = 'EX'
                    WHERE td.document_id = d.document_id
                    AND tt.code = 'GROSS'
                )
        FROM transdetail td
        WHERE td.document_id = d.document_id
        AND td.document_sequence = 1
    ), 
    0,
    0,
    
    /*Selection Details*/
    (
        SELECT 
            ISNULL(MAX(1),0)
        FROM transdetail td
        JOIN account a 
            ON a.account_id = td.account_id
        JOIN ledger l 
            ON l.ledger_id = a.ledger_id
        WHERE td.document_id = d.document_id
        AND l.ledger_short_name = 'UB'
    ),    
    (
        SELECT 
            ISNULL(MAX(1),0)
        FROM insurance_file i
        WHERE i.insurance_file_cnt = d.insurance_file_cnt
        AND NOT EXISTS 
            (
                SELECT 
                    NULL
                FROM transdetail td
                JOIN account a 
                    ON a.account_id = td.account_id
                JOIN ledger l 
                    ON l.ledger_id = a.ledger_id
                WHERE td.document_id = d.document_id
                AND l.ledger_short_name = 'IN'
                AND a.account_key = i.fsa_underwriter_cnt
            )
    ),        
    CASE
        WHEN EXISTS
            (
                SELECT 
                    NULL
                FROM party p
                WHERE p.party_cnt = i.lead_insurer_cnt
                AND p.shortname LIKE 'MULTI%'
            ) THEN
            
            CASE
                WHEN EXISTS
                    (
                        SELECT
                            NULL
                        FROM document dx
                        JOIN insurance_file i
                            ON i.insurance_file_cnt = dx.insurance_file_cnt    
                        JOIN risk_code rc
                            ON rc.risk_code_id = i.risk_code_id    
                        JOIN transdetail td
                            ON td.document_id = d.document_id
                        JOIN account a 
                            ON a.account_id = td.account_id
                        JOIN party p
                            ON p.party_cnt = a.account_key
                        JOIN party_type pt
                            ON pt.party_type_id = p.party_type_id
                            AND pt.code = 'IN'
                        LEFT JOIN party_insurer_risk pir
                            ON pir.risk_code_id = rc.risk_code_id
                            AND pir.party_cnt = p.party_cnt
                        WHERE dx.document_id = d.document_id
                        HAVING MIN(CAST(ISNULL(pir.delegated_authority, rc.is_delegated_authority) AS INT)) = MAX(CAST(ISNULL(pir.delegated_authority, rc.is_delegated_authority) AS INT)) 
                    ) THEN

                    (
                        SELECT
                            ISNULL(pir.delegated_authority, rc.is_delegated_authority)
                        FROM document dx
                        JOIN insurance_file i
                            ON i.insurance_file_cnt = dx.insurance_file_cnt    
                        JOIN risk_code rc
                            ON rc.risk_code_id = i.risk_code_id    
                        JOIN transdetail td
                            ON td.document_id = dx.document_id
                        JOIN account a 
                            ON a.account_id = td.account_id
                        JOIN party p
                            ON p.party_cnt = a.account_key
                        JOIN party_type pt
                            ON pt.party_type_id = p.party_type_id
                            AND pt.code = 'IN'
                        LEFT JOIN party_insurer_risk pir
                            ON pir.risk_code_id = rc.risk_code_id
                            AND pir.party_cnt = p.party_cnt
                        WHERE dx.document_id = d.document_id
                        AND EXISTS
                            (
                                SELECT 
                                    NULL
                                FROM transdetail td2
                                JOIN account a2 
                                    ON a2.account_id = td2.account_id
                                JOIN party p2
                                    ON p2.party_cnt = a2.account_key
                                JOIN party_type pt2
                                    ON pt2.party_type_id = p2.party_type_id
                                    AND pt2.code = 'IN'
                                WHERE td2.document_id = dx.document_id
                                HAVING MIN(td2.transdetail_id) = td.transdetail_id
                            )
                    ) 
                    
                ELSE
                    rc.is_delegated_authority
            END
            
        ELSE
            (
                SELECT
                    ISNULL(pir.delegated_authority, rc.is_delegated_authority)
                FROM risk_code rc
                LEFT JOIN party_insurer_risk pir
                    ON pir.risk_code_id = rc.risk_code_id
                    AND pir.party_cnt = i.lead_insurer_cnt
                WHERE rc.risk_code_id = i.risk_code_id
            )
    END,
    
    /*FSA Disabled Flag*/
    0
    
FROM document d
JOIN insurance_file i
    ON i.insurance_file_cnt = d.insurance_file_cnt    
JOIN risk_code rc
    ON rc.risk_code_id = i.risk_code_id    
JOIN risk_group rg
    ON rg.risk_group_id = rc.risk_group_id
JOIN fsa_product fp
    ON fp.fsa_product_id = rg.fsa_product_id
WHERE d.documenttype_id IN (4,5,35,36) /*SND,SNC,TRD,TRC*/
AND d.document_date BETWEEN @start_date AND @end_date
AND d.company_id = ISNULL(@branch_id,d.company_id)
AND i.fsa_customer_category_id = 1
AND (
        @IncludeQuotes = 'True' 
        OR 
        i.insurance_file_type_id <> 1
    )

/*Add lines for all valid transactions only including the extras*/
INSERT INTO #TempRetailBusiness
SELECT
    /*FSA Details*/
    FP.FSA_product_id,
    FP.description,
    
    /*Document Details*/
    D.document_id,
    D.document_ref,
    D.document_date,
    
    /*Policy Details*/
    I.insurance_file_cnt,
    I.insurance_ref,
    I.insured_name,
    
    /*Values*/
    td.amount * -1, 
    0,
    0,
    
    /*Selection Details*/
    (
        SELECT 
            ISNULL(MAX(1),0)
        FROM transdetail td
        JOIN account a 
            ON a.account_id = td.account_id
        JOIN ledger l 
            ON l.ledger_id = a.ledger_id
        WHERE td.document_id = d.document_id
        AND l.ledger_short_name = 'UB'
    ),    
    (
        SELECT 
            ISNULL(MAX(1),0)
        FROM insurance_file i
        WHERE i.insurance_file_cnt = d.insurance_file_cnt
        AND NOT EXISTS 
            (
                SELECT 
                    NULL
                FROM transdetail td
                JOIN account a 
                    ON a.account_id = td.account_id
                JOIN ledger l 
                    ON l.ledger_id = a.ledger_id
                WHERE td.document_id = d.document_id
                AND l.ledger_short_name = 'IN'
                AND a.account_key = i.fsa_underwriter_cnt
            )
    ),        
    pe.delegated_authority,

    /*FSA Disabled Flag*/
    0
    
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
JOIN document d
    ON d.document_id = td.document_id
JOIN insurance_file i
    ON i.insurance_file_cnt = d.insurance_file_cnt    
JOIN party_extra pe
    ON pe.party_cnt = p.party_cnt
JOIN fsa_product fp
    ON fp.fsa_product_id = pe.fsa_product_id
WHERE d.documenttype_id IN (4,5,35,36) /*SND,SNC,TRD,TRC*/
AND d.document_date BETWEEN @start_date AND @end_date
AND d.company_id = ISNULL(@branch_id,d.company_id)
AND i.fsa_customer_category_id = 1
AND (
        @IncludeQuotes = 'True' 
        OR 
        i.insurance_file_type_id <> 1
    )


/*Get the total of non-investment premium and the count of claims*/
UPDATE #TempRetailBusiness
SET total_non_investment_premium = 
        (
            SELECT
                ISNULL(SUM(TD.amount),0)
            FROM Document D
            JOIN Transdetail TD
                ON TD.document_id = D.document_id
                AND TD.document_sequence = 1
            JOIN Insurance_File I
                ON I.insurance_file_cnt = D.insurance_file_cnt
            WHERE D.documenttype_id IN (4,5,35,36) /*SND,SNC,TRD,TRC*/
            AND D.document_date BETWEEN @start_date AND @end_date
            AND D.company_id = ISNULL(@branch_id,D.company_id)
            AND I.FSA_Customer_Category_Id = 1
            AND (
                    @IncludeQuotes = 'True' 
                    OR 
                    I.insurance_file_type_id <> 1
                )
        ),
    claims_count = 
        (
            SELECT
                ISNULL(SUM(1),0)
            FROM Claim CLM
            JOIN Insurance_file I 
                ON I.insurance_file_cnt = CLM.policy_id
            JOIN Risk_Code RC 
                ON RC.risk_code_id = I.risk_code_id
            JOIN Risk_Group RG 
                ON RG.risk_group_id = RC.risk_group_id
            JOIN FSA_Product FP 
                ON FP.FSA_product_id = RG.FSA_product_id
            WHERE I.source_id = ISNULL(@branch_id,I.source_id)
            AND CLM.reported_date BETWEEN @start_date AND @end_date
            AND CLM.claim_handled = 2
        )

/*Select all of the records to be reported on.*/
SELECT
    *
FROM #TempRetailBusiness TRB
ORDER BY 
    TRB.FSA_product_description,
    TRB.document_id

DROP TABLE #TempRetailBusiness

SET NOCOUNT OFF

GO

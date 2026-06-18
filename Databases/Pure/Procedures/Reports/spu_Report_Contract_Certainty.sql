SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Contract_Certainty'
GO

CREATE PROCEDURE spu_Report_Contract_Certainty
    @branch_id INT,
    @CalenderYear VARCHAR(4),
    @ReportOn VARCHAR(20)
AS 

DECLARE @GROUP1 DATETIME
DECLARE @GROUP2 DATETIME
DECLARE @GROUP3 DATETIME
DECLARE @GROUP4 DATETIME
DECLARE @GROUP4END DATETIME


IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END


IF @ReportOn ='Quarter 1' 
BEGIN
    SET @GROUP1 = CAST(@CalenderYear + '-01-01' AS DATETIME)
    SET @GROUP2 = CAST(@CalenderYear + '-02-01' AS DATETIME)
    SET @GROUP3 = CAST(@CalenderYear + '-03-01' AS DATETIME)
    SET @GROUP4 = CAST(@CalenderYear + '-04-01' AS DATETIME)
END

IF @ReportOn ='Quarter 2' 
BEGIN
    SET @GROUP1 = CAST(@CalenderYear + '-04-01' AS DATETIME)
    SET @GROUP2 = CAST(@CalenderYear + '-05-01' AS DATETIME)
    SET @GROUP3 = CAST(@CalenderYear + '-06-01' AS DATETIME)
    SET @GROUP4 = CAST(@CalenderYear + '-07-01' AS DATETIME)
END

IF @ReportOn ='Quarter 3' 
BEGIN
    SET @GROUP1 = CAST(@CalenderYear + '-07-01' AS DATETIME)
    SET @GROUP2 = CAST(@CalenderYear + '-08-01' AS DATETIME)
    SET @GROUP3 = CAST(@CalenderYear + '-09-01' AS DATETIME)
    SET @GROUP4 = CAST(@CalenderYear + '-10-01' AS DATETIME)
END

IF @ReportOn ='Quarter 4' 
BEGIN
    SET @GROUP1 = CAST(@CalenderYear + '-10-01' AS DATETIME)
    SET @GROUP2 = CAST(@CalenderYear + '-11-01' AS DATETIME)
    SET @GROUP3 = CAST(@CalenderYear + '-12-01' AS DATETIME)
    SET @GROUP4 = CAST(@CalenderYear + '-12-31' AS DATETIME) + 1
END

IF @ReportOn ='Quarter Summary' 
BEGIN
    SET @GROUP1 = CAST(@CalenderYear + '-01-01' AS DATETIME)
    SET @GROUP2 = CAST(@CalenderYear + '-04-01' AS DATETIME)
    SET @GROUP3 = CAST(@CalenderYear + '-07-01' AS DATETIME)
    SET @GROUP4 = CAST(@CalenderYear + '-10-01' AS DATETIME)
    SET @GROUP4END = CAST(@CalenderYear + '-12-31' AS DATETIME) + 1
END

CREATE TABLE #contract_certainty
(
    insurer_name  VARCHAR(255),
    g1_retail_total INT,
    g1_commer_total INT,
    g1_retail_correct INT,
    g1_commer_correct INT,
    g1_retail_appropriate_evidence INT,
    g1_commer_appropriate_evidence INT,
    g1_retail_not_issued INT,
    g1_commer_not_issued INT,

    g2_retail_total INT,
    g2_commer_total INT,
    g2_retail_correct INT,
    g2_commer_correct INT,
    g2_retail_appropriate_evidence INT,
    g2_commer_appropriate_evidence INT,
    g2_retail_not_issued INT,
    g2_commer_not_issued INT,

    g3_retail_total INT,
    g3_commer_total INT,
    g3_retail_correct INT,
    g3_commer_correct INT,
    g3_retail_appropriate_evidence INT,
    g3_commer_appropriate_evidence INT,
    g3_retail_not_issued INT,
    g3_commer_not_issued INT,

    g4_retail_total INT,
    g4_commer_total INT,
    g4_retail_correct INT,
    g4_commer_correct INT,
    g4_retail_appropriate_evidence INT,
    g4_commer_appropriate_evidence INT,
    g4_retail_not_issued INT,
    g4_commer_not_issued INT,

    fsa_disabled BIT
)

IF NOT EXISTS  
    (  
        SELECT 
            NULL  
        FROM hidden_options  
        WHERE option_number = 61  
        AND value = '1'  
    )  
BEGIN  
    INSERT INTO #contract_certainty
    (  
        fsa_disabled  
    )  
    VALUES  
    (  
        1  
    )  


    SELECT
        insurer_name,
        g1_retail_total,
        g1_commer_total,
        g1_retail_correct,
        g1_commer_correct,
        g1_retail_appropriate_evidence,
        g1_commer_appropriate_evidence,
        g1_retail_not_issued,
        g1_commer_not_issued,

        g2_retail_total,
        g2_commer_total,
        g2_retail_correct,
        g2_commer_correct,
        g2_retail_appropriate_evidence,
        g2_commer_appropriate_evidence,
        g2_retail_not_issued,
        g2_commer_not_issued,

        g3_retail_total,
        g3_commer_total,
        g3_retail_correct,
        g3_commer_correct,
        g3_retail_appropriate_evidence,
        g3_commer_appropriate_evidence,
        g3_retail_not_issued,
        g3_commer_not_issued,

        g4_retail_total,
        g4_commer_total,
        g4_retail_correct,
        g4_commer_correct,
        g4_retail_appropriate_evidence,
        g4_commer_appropriate_evidence,
        g4_retail_not_issued,
        g4_commer_not_issued,

        fsa_disabled

    FROM #contract_certainty

    DROP TABLE #contract_certainty

    RETURN 

END

INSERT INTO #contract_certainty
SELECT 
    p.name 'InsurerName',
    -------------------------------GROUP 1--------------
    CASE 
        WHEN 
            inf.fsa_customer_category_id = 1 /*Retail*/
            AND 
            (
                inf.inception_Date >= @GROUP1 
                AND 
                inf.inception_Date < @GROUP2
            ) 
        THEN
        1
    END 'TotalRatailG1',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP1 
                AND 
                inf.inception_Date < @GROUP2
            ) 
        THEN
        1
    END 'TotalCommG1',

    CASE 
        WHEN 
            terms_agreed_date <= inception_date
            AND 
            terms_agreed=1 
            AND 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP1 
                AND 
                inf.inception_Date < @GROUP2
            ) 
        THEN 
        1
    END 'CorrectRetailG1',

    CASE 
        WHEN 
            terms_agreed_date <= inception_date 
            AND 
            terms_agreed=1 
            AND 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP1 
                AND 
                inf.inception_Date < @GROUP2
            ) 
        THEN 
        1
    END 'CorrectCommG1',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            DATEDIFF(d,inception_date,policy_documents_issued_date)<7 
            AND 
            (
                inf.inception_Date >= @GROUP1 
                AND inf.inception_Date < @GROUP2
            ) 
        THEN
        1
    END 'AppropriateEvidenceRetailG1',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            DATEDIFF(d,inception_date,policy_documents_issued_date)<30 
            AND 
            (
                inf.inception_Date >= @GROUP1 
                AND 
                inf.inception_Date < @GROUP2
            ) 
        THEN
        1
    END 'AppropriateEvidenceCommG1',

    CASE 
        WHEN 
            policy_documents_issued_date IS NULL 
            AND 
            terms_agreed=0 
            AND 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP1 
                AND 
                inf.inception_Date < @GROUP2
            ) 
        THEN 
        1
    END 'NotIssuedRetailG1',

    CASE 
        WHEN 
            policy_documents_issued_date IS NULL 
            AND 
            terms_agreed=0 
            AND 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP1 
                AND 
                inf.inception_Date < @GROUP2
            ) 
        THEN 
        1
    END 'NotIssuedCommG1',
    --------------------------------------------------------
    -------------------------------GROUP 2 -----------------
    CASE 
        WHEN 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP2 
                AND 
                inf.inception_Date < @GROUP3
            ) 
        THEN
        1
    END 'TotalRatailG2',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP2 
                AND 
                inf.inception_Date < @GROUP3
            ) 
        THEN
        1
    END 'TotalCommG2',

    CASE 
        WHEN 
            terms_agreed_date <= inception_date
            AND 
            terms_agreed=1 
            AND 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP2 
                AND 
                inf.inception_Date < @GROUP3
            ) 
        THEN 
        1
    END 'CorrectRetailG2',

    CASE 
        WHEN 
            terms_agreed_date <= inception_date
            AND 
            terms_agreed=1 
            AND 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP2 
                AND 
                inf.inception_Date < @GROUP3
            ) 
        THEN 
        1
    END 'CorrectCommG2',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            DATEDIFF(d,inception_date,policy_documents_issued_date)<7 
            AND 
            (
                inf.inception_Date >= @GROUP2 
                AND 
                inf.inception_Date < @GROUP3
            ) 
        THEN
        1
    END 'AppropriateEvidenceRetailG2',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            DATEDIFF(d,inception_date,policy_documents_issued_date)<30 
            AND 
            (
                inf.inception_Date >= @GROUP2 
                AND 
                inf.inception_Date < @GROUP3
            ) 
        THEN
        1
    END 'AppropriateEvidenceCommG2',

    CASE 
        WHEN 
            policy_documents_issued_date IS NULL 
            AND 
            terms_agreed=0 
            AND 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP2 
                AND 
                inf.inception_Date < @GROUP3
            ) 
        THEN 
        1
    END 'NotIssuedRetailG2',

    CASE 
        WHEN 
            policy_documents_issued_date IS NULL 
            AND 
            terms_agreed=0 
            AND 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP2 
                AND 
                inf.inception_Date < @GROUP3
            ) 
        THEN 
        1
    END 'NotIssuedCommG2',
    -------------------------------------------------------
    -------------------------------GROUP 3-----------------
    CASE 
        WHEN 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP3 
                AND 
                inf.inception_Date < @GROUP4
            ) 
        THEN
        1
    END 'TotalRatailG3',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP3 
                AND 
                inf.inception_Date < @GROUP4
            ) 
        THEN
        1
    END 'TotalCommG3',

    CASE 
        WHEN 
            terms_agreed_date <= inception_date 
            AND 
            terms_agreed=1 
            AND 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP3 
                AND 
                inf.inception_Date < @GROUP4
            ) 
        THEN 
        1
    END 'CorrectRetailG3',

    CASE 
        WHEN 
            terms_agreed_date <= inception_date
            AND 
            terms_agreed=1 
            AND 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP3 
                AND 
                inf.inception_Date < @GROUP4
            ) 
        THEN 
        1
    END 'CorrectCommG3',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            DATEDIFF(d,inception_date,policy_documents_issued_date)<7 
            AND 
            (
                inf.inception_Date >= @GROUP3 
                AND 
                inf.inception_Date < @GROUP4
            ) 
        THEN
        1
    END 'AppropriateEvidenceRetailG3',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            DATEDIFF(d,inception_date,policy_documents_issued_date)<30 
            AND 
            (
                inf.inception_Date >= @GROUP3 
                AND 
                inf.inception_Date < @GROUP4
            ) 
        THEN
        1
    END 'AppropriateEvidenceCommG3',

    CASE 
        WHEN 
            policy_documents_issued_date IS NULL 
            AND 
            terms_agreed=0 
            AND 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP3 
                AND 
                inf.inception_Date < @GROUP4
            ) 
        THEN 
        1
    END 'NotIssuedRetailG3',

    CASE 
        WHEN 
            policy_documents_issued_date IS NULL 
            AND 
            terms_agreed=0 
            AND 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP3 
                AND 
                inf.inception_Date < @GROUP4
            ) 
        THEN 
        1
    END 'NotIssuedCommG3',
    -------------------------------------------------------
    -------------------------------GROUP 4-----------------
    CASE 
        WHEN 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP4 
                AND 
                inf.inception_Date < @GROUP4END
            ) 
            AND 
            @ReportOn ='Quarter Summary' 
        THEN
        1
    END 'TotalRatailG4',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP4 
                AND 
                inf.inception_Date < @GROUP4END
            ) 
            AND 
            @ReportOn ='Quarter Summary' 
        THEN
        1
    END 'TotalCommG4',

    CASE 
        WHEN 
            terms_agreed_date <= inception_date
            AND 
            terms_agreed=1 
            AND 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP4 
                AND 
                inf.inception_Date < @GROUP4END
            ) 
            AND 
            @ReportOn ='Quarter Summary' 
        THEN 
        1
    END 'CorrectRetailG4',

    CASE 
        WHEN 
            terms_agreed_date <= inception_date 
            AND 
            terms_agreed=1 
            AND 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP4 
                AND 
                inf.inception_Date < @GROUP4END
            ) 
            AND 
            @ReportOn ='Quarter Summary' 
        THEN 
        1
    END 'CorrectCommG4',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 1 /*Retail*/
            AND 
            DATEDIFF(d,inception_date,policy_documents_issued_date)<7 
            AND 
            (
                inf.inception_Date >= @GROUP4 
                AND 
                inf.inception_Date < @GROUP4END
            ) 
            AND 
            @ReportOn ='Quarter Summary' 
        THEN
        1
    END 'AppropriateEvidenceRetailG4',

    CASE 
        WHEN 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            DATEDIFF(d,inception_date,policy_documents_issued_date)<30 
            AND 
            (
                inf.inception_Date >= @GROUP4 
                AND 
                inf.inception_Date < @GROUP4END
            ) 
            AND 
            @ReportOn ='Quarter Summary' 
        THEN
        1
    END 'AppropriateEvidenceCommG4',

    CASE 
        WHEN 
            policy_documents_issued_date IS NULL 
            AND 
            terms_agreed=0 
            AND 
            inf.fsa_customer_category_id = 1 /*Retail*/ 
            AND 
            (
                inf.inception_Date >= @GROUP4 
                AND 
                inf.inception_Date < @GROUP4END
            ) 
            AND 
            @ReportOn ='Quarter Summary' 
        THEN 
        1
    END 'NotIssuedRetailG4',

    CASE 
        WHEN 
            policy_documents_issued_date IS NULL 
            AND 
            terms_agreed=0 
            AND 
            inf.fsa_customer_category_id = 0 /*Commercial*/ 
            AND 
            (
                inf.inception_Date >= @GROUP4 
                AND 
                inf.inception_Date < @GROUP4END
            ) 
            AND 
            @ReportOn ='Quarter Summary' 
        THEN 
        1
    END 'NotIssuedCommG4',
    NULL
    -------------------------------------------------------
FROM insurance_file inf
JOIN party p
    ON inf.lead_insurer_cnt=p.party_cnt
JOIN party pa
    ON pa.party_cnt=inf.insured_cnt
WHERE inf.policy_version =
    (
        SELECT 
            MAX(i2.policy_version)
        FROM insurance_file i2
        JOIN insurance_file_type ift2
            ON ift2.insurance_file_type_id = i2.insurance_file_type_id
        WHERE i2.insurance_folder_cnt = inf.insurance_folder_cnt
        AND i2.insurance_file_status_id IS NULL
        AND ift2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN')
    )
AND inf.source_id = ISNULL(@branch_id,inf.source_id)
AND (    
        (
            @ReportOn ='Quarter 1'
            AND
            inf.inception_Date >= @GROUP1
            AND 
            inf.inception_Date < @GROUP4
        )
        OR
        (
            @ReportOn ='Quarter 2'
            AND
            inf.inception_Date >= @GROUP1
            AND 
            inf.inception_Date < @GROUP4
        )
        OR
        (
            @ReportOn ='Quarter 3'
            AND
            inf.inception_Date >= @GROUP1
            AND 
            inf.inception_Date < @GROUP4
        )
        OR
        (
            @ReportOn ='Quarter 4'
            AND
            inf.inception_Date >= @GROUP1
            AND 
            inf.inception_Date <= @GROUP4
        )
        OR
        (
            @ReportOn ='Quarter Summary'
            AND
            inf.inception_Date >= @GROUP1
            AND 
            inf.inception_Date < CAST(@CalenderYear + '-12-31' AS DATETIME)
        )
    )


SELECT 
    insurer_name,
    ISNULL(SUM(g1_retail_total),0) 'g1_retail_total',
    ISNULL(SUM(g1_commer_total),0) 'g1_commer_total',
    ISNULL(SUM(g1_retail_correct),0) 'g1_retail_correct',
    ISNULL(SUM(g1_commer_correct),0) 'g1_commer_correct',
    ISNULL(SUM(g1_retail_appropriate_evidence),0) 'g1_retail_appropriate_evidence',
    ISNULL(SUM(g1_commer_appropriate_evidence),0) 'g1_commer_appropriate_evidence',
    ISNULL(SUM(g1_retail_not_issued),0) 'g1_retail_not_issued',
    ISNULL(SUM(g1_commer_not_issued),0) 'g1_commer_not_issued',

    ISNULL(SUM(g2_retail_total),0) 'g2_retail_total',
    ISNULL(SUM(g2_commer_total),0) 'g2_commer_total',
    ISNULL(SUM(g2_retail_correct),0) 'g2_retail_correct',
    ISNULL(SUM(g2_commer_correct),0) 'g2_commer_correct',
    ISNULL(SUM(g2_retail_appropriate_evidence),0) 'g2_retail_appropriate_evidence',
    ISNULL(SUM(g2_commer_appropriate_evidence),0) 'g2_commer_appropriate_evidence',
    ISNULL(SUM(g2_retail_not_issued),0) 'g2_retail_not_issued',
    ISNULL(SUM(g2_commer_not_issued),0) 'g2_commer_not_issued',

    ISNULL(SUM(g3_retail_total),0) 'g3_retail_total',
    ISNULL(SUM(g3_commer_total),0) 'g3_commer_total',
    ISNULL(SUM(g3_retail_correct),0) 'g3_retail_correct',
    ISNULL(SUM(g3_commer_correct),0) 'g3_commer_correct',
    ISNULL(SUM(g3_retail_appropriate_evidence),0) 'g3_retail_appropriate_evidence',
    ISNULL(SUM(g3_commer_appropriate_evidence),0) 'g3_commer_appropriate_evidence',
    ISNULL(SUM(g3_retail_not_issued),0) 'g3_retail_not_issued',
    ISNULL(SUM(g3_commer_not_issued),0) 'g3_commer_not_issued',

    ISNULL(SUM(g4_retail_total),0) 'g4_retail_total',
    ISNULL(SUM(g4_commer_total),0) 'g4_commer_total',
    ISNULL(SUM(g4_retail_correct),0) 'g4_retail_correct',
    ISNULL(SUM(g4_commer_correct),0) 'g4_commer_correct',
    ISNULL(SUM(g4_retail_appropriate_evidence),0) 'g4_retail_appropriate_evidence',
    ISNULL(SUM(g4_commer_appropriate_evidence),0) 'g4_commer_appropriate_evidence',
    ISNULL(SUM(g4_retail_not_issued),0) 'g4_retail_not_issued',
    ISNULL(SUM(g4_commer_not_issued),0) 'g4_commer_not_issued',
    fsa_disabled
FROM #contract_certainty
GROUP BY insurer_name, fsa_disabled
ORDER BY insurer_name


DROP TABLE #contract_certainty



GO

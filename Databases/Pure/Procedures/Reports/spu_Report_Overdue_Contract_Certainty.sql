SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Overdue_Contract_Certainty'
GO

CREATE PROCEDURE spu_Report_Overdue_Contract_Certainty
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @client_type VARCHAR(50),
    @group_by VARCHAR(50),
    @sort_by VARCHAR(50)

AS 

DECLARE @insurance_file_cnt INT
DECLARE @group_by_code2 VARCHAR(100)
DECLARE @line_no INT 
DECLARE @insurer_code VARCHAR(20)
DECLARE @insurer_name VARCHAR(255)

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END


CREATE TABLE #overdue_contract_certainty
(
    /*Policy Details*/
    insurance_file_cnt INT,
    line_no INT,
    policy_no VARCHAR(30),
    terms_agreed VARCHAR(3),
    terms_agreed_date DATETIME,
    inception_date DATETIME,
    policy_documents_correct VARCHAR(3),
    error_notification_date DATETIME,
    client_type VARCHAR(50),
    days_overdue INT,
    has_multiple_clients BIT,
    has_multiple_insurers BIT,

    /*Client Details*/
    client_code VARCHAR(20),
    client_name VARCHAR(255),

    /*Insurer Details*/
    insurer_code VARCHAR(20),
    insurer_name VARCHAR(255),

    /*Risk Details*/
    risk_code VARCHAR(10),
    risk_desc VARCHAR(255),

    /*Group Details*/
    group_by_code1 VARCHAR(100),
    group_by_desc1 VARCHAR(255),
    group_by_code2 VARCHAR(100),
    group_by_desc2 VARCHAR(255),
    
    /*Sort Details*/
    sort_by1a VARCHAR(20), /*client_code*/
    sort_by1b INT, /*days_overdue*/
    sort_by2 DATETIME, /*inception_date*/
    
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

    INSERT INTO #overdue_contract_certainty
    (  
        fsa_disabled  
    )  
    VALUES  
    (  
        1  
    ) 

    SELECT
        *
    FROM #overdue_contract_certainty

    DROP TABLE #overdue_contract_certainty

    RETURN 

END

/*Policy Documents Outstanding*/
INSERT INTO #overdue_contract_certainty
(
    insurance_file_cnt,
    line_no,
    days_overdue,
    group_by_code2,
    group_by_desc2
)
SELECT 
    i.insurance_file_cnt,
    1,
    CASE
        WHEN i.fsa_customer_category_id = 1 THEN/*Retail*/
            CASE /*five working days*/
                WHEN @@DATEFIRST + DATEPART(dw, i.inception_date) = 8 THEN /*Sunday*/
                    DATEDIFF(d, DATEADD(d, 5, i.inception_date), @end_date)
                WHEN @@DATEFIRST + DATEPART(dw, i.inception_date) IN (7, 14) THEN /*Saturday*/
                    DATEDIFF(d, DATEADD(d, 6, i.inception_date), @end_date)
                ELSE /*Monday-Friday*/
                    DATEDIFF(d, DATEADD(d, 7, i.inception_date), @end_date)
            END
        ELSE /*Commercial*/
            DATEDIFF(d, DATEADD(d, 30, i.inception_date), @end_date)
    END,
    '1',
    'Policy Documents Outstanding'
FROM insurance_file i
WHERE i.policy_version =
    (
        SELECT 
            MAX(i2.policy_version)
        FROM insurance_file i2
        JOIN insurance_file_type ift2
            ON ift2.insurance_file_type_id = i2.insurance_file_type_id
        WHERE i2.insurance_folder_cnt = i.insurance_folder_cnt
        AND i2.insurance_file_status_id IS NULL
        AND ift2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN')
    )
AND i.source_id = ISNULL(@branch_id, i.source_id)
AND i.inception_date BETWEEN @start_date AND @end_date
AND (
        @client_type = 'Both' 
        OR
        (
            @client_type = 'Retail' 
            AND 
            i.fsa_customer_category_id = 1
        )
        OR
        (
            @client_type='Commercial' 
            AND 
            i.fsa_customer_category_id = 0
        )
    )
AND (
        i.policy_documents_issued_date IS NULL
        OR
        i.policy_documents_issued_date > @end_date
    )
AND (
        (
            i.fsa_customer_category_id = 0 /*Commercial*/
            AND
            DATEDIFF(d, i.inception_Date, @end_date) > 30
        )
        OR
        (
            i.fsa_customer_category_id = 1 /*Retail*/
            AND
            (
                (
                    @@DATEFIRST + DATEPART(dw, i.inception_date) = 8 /*Sunday*/
                    AND
                    DATEDIFF(d, i.inception_Date, @end_date) > 5 /*Five working days*/
                )
                OR
                (
                    @@DATEFIRST + DATEPART(dw, i.inception_date) IN (7, 14) /*Saturday*/
                    AND
                    DATEDIFF(d, i.inception_Date, @end_date) > 6 /*Five working days*/
                )
                OR
                (
                    @@DATEFIRST + DATEPART(dw, i.inception_date) NOT IN (7, 8, 14) /*Monday-Friday*/
                    AND
                    DATEDIFF(d, i.inception_Date, @end_date) > 7 /*Five working days*/
                )
            )
        )
    )


/*Incorrect Policy Documents Outstanding*/
INSERT INTO #overdue_contract_certainty
(
    insurance_file_cnt,
    line_no,
    days_overdue,
    group_by_code2,
    group_by_desc2
)
SELECT 
    i.insurance_file_cnt,
    1,
    DATEDIFF(d, DATEADD(d, 14, i.error_notification_date), @end_date),
    '2',
    'Incorrect Policy Documents Outstanding'
FROM insurance_file i
WHERE i.policy_version =
    (
        SELECT 
            MAX(i2.policy_version)
        FROM insurance_file i2
        JOIN insurance_file_type ift2
            ON ift2.insurance_file_type_id = i2.insurance_file_type_id
        WHERE i2.insurance_folder_cnt = i.insurance_folder_cnt
        AND i2.insurance_file_status_id IS NULL
        AND ift2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN')
    )
AND i.source_id = ISNULL(@branch_id, i.source_id)
AND i.inception_date BETWEEN @start_date AND @end_date
AND (
        @client_type = 'Both' 
        OR
        (
            @client_type = 'Retail' 
            AND 
            i.fsa_customer_category_id = 1
        )
        OR
        (
            @client_type='Commercial' 
            AND 
            i.fsa_customer_category_id = 0
        )
    )
AND i.policy_documents_correct = 0
AND (
        i.policy_documents_issued_date IS NULL
        OR
        i.policy_documents_issued_date < i.error_notification_date
    )
AND DATEDIFF(d, i.error_notification_date, @end_date) > 14
    
/*No Terms Agreed*/
INSERT INTO #overdue_contract_certainty
(
    insurance_file_cnt,
    line_no,
    days_overdue,
    group_by_code2,
    group_by_desc2
)
SELECT 
    i.insurance_file_cnt,
    1,
    -1,
    '3',
    'No Terms Agreed'
FROM insurance_file i
WHERE i.policy_version =
    (
        SELECT 
            MAX(i2.policy_version)
        FROM insurance_file i2
        JOIN insurance_file_type ift2
            ON ift2.insurance_file_type_id = i2.insurance_file_type_id
        WHERE i2.insurance_folder_cnt = i.insurance_folder_cnt
        AND i2.insurance_file_status_id IS NULL
        AND ift2.code IN ('POLICY', 'RENEWAL', 'MTA PERM', 'MTAPERMCAN')
    )
AND i.source_id = ISNULL(@branch_id, i.source_id)
AND i.inception_date BETWEEN @start_date AND @end_date
AND (
        @client_type = 'Both' 
        OR
        (
            @client_type = 'Retail' 
            AND 
            i.fsa_customer_category_id = 1
        )
        OR
        (
            @client_type='Commercial' 
            AND 
            i.fsa_customer_category_id = 0
        )
    )
AND ISNULL(i.terms_agreed, 0) = 0

/*Update details*/
UPDATE occ
SET policy_no = i.insurance_ref,
    terms_agreed = 
        CASE 
            WHEN i.terms_agreed = 1 THEN
                'Yes'
            WHEN i.terms_agreed = 0 THEN
                'No'
            ELSE
                ' -'
        END,
    terms_agreed_date = i.terms_agreed_date,
    inception_date = i.inception_date,
    policy_documents_correct = 
        CASE 
            WHEN i.policy_documents_correct = 1 THEN
                'Yes'
            WHEN i.policy_documents_correct = 0 THEN
                'No'
            ELSE
                ' -'
        END,
    error_notification_date = i.error_notification_date,
    client_type =
        CASE 
            WHEN i.fsa_customer_category_id = 1 THEN
                'Retail'
            ELSE
                'Commercial'
        END,
    has_multiple_clients = 
    (
        SELECT
            ISNULL(MAX(1), 0)
        FROM policy_shared_premiums
        WHERE insurance_file_cnt = occ.insurance_file_cnt
    ),
    has_multiple_insurers = 
    (
        SELECT
            ISNULL(MAX(1), 0)
        FROM policy_coinsurers
        WHERE insurance_file_cnt = occ.insurance_file_cnt
    ),
    sort_by1b = occ.days_overdue,
    sort_by2 = i.inception_date
FROM #overdue_contract_certainty occ
JOIN insurance_file i
    ON i.insurance_file_cnt = occ.insurance_file_cnt
        
/*Update risk details*/
UPDATE occ
SET risk_code = rc.code,
    risk_desc = rc.description
FROM #overdue_contract_certainty occ
JOIN insurance_file i
    ON i.insurance_file_cnt = occ.insurance_file_cnt
JOIN risk_code rc
    ON rc.risk_code_id = i.risk_code_id

/*Update client details*/
UPDATE occ
SET client_code = p.shortname,
    client_name = p.resolved_name,
    sort_by1a = p.shortname
FROM #overdue_contract_certainty occ
JOIN insurance_file i
    ON i.insurance_file_cnt = occ.insurance_file_cnt
JOIN party p
    ON p.party_cnt = i.insured_cnt

/*Update insurer details*/
UPDATE occ
SET insurer_code = p.shortname,
    insurer_name = p.resolved_name
FROM #overdue_contract_certainty occ
JOIN insurance_file i
    ON i.insurance_file_cnt = occ.insurance_file_cnt
JOIN party p
    ON p.party_cnt = i.lead_insurer_cnt
WHERE occ.has_multiple_insurers = 0

UPDATE occ
SET insurer_code = NULL,
    insurer_name = 'Multiple Insurers'
FROM #overdue_contract_certainty occ
JOIN insurance_file i
    ON i.insurance_file_cnt = occ.insurance_file_cnt
JOIN party p
    ON p.party_cnt = i.lead_insurer_cnt
WHERE occ.has_multiple_insurers = 1

/*Update the grouping to use the correct values*/
UPDATE occ
SET group_by_code1 = 
        CASE @group_by
            WHEN 'Client Type' THEN
                CASE i.fsa_customer_category_id
                    WHEN 1 THEN
                        '1'
                    ELSE
                        '2'
                END
            WHEN 'Risk' THEN
                risk_code
            WHEN 'Insurer' THEN
                insurer_code
        END,
    group_by_desc1 = 
        CASE @group_by
            WHEN 'Client Type' THEN
                CASE i.fsa_customer_category_id
                    WHEN 1 THEN
                        'Retail'
                    ELSE
                        'Commercial'
                END
            WHEN 'Risk' THEN
                risk_desc
            WHEN 'Insurer' THEN
                insurer_name
        END
FROM #overdue_contract_certainty occ
JOIN insurance_file i
    ON i.insurance_file_cnt = occ.insurance_file_cnt


DECLARE c_cursor CURSOR FORWARD_ONLY STATIC FOR
    SELECT
        insurance_file_cnt,
        group_by_code2
    FROM #overdue_contract_certainty
    WHERE has_multiple_clients = 1
    
OPEN c_cursor

FETCH NEXT FROM c_cursor INTO
    @insurance_file_cnt,
    @group_by_code2

WHILE @@FETCH_STATUS = 0
BEGIN

    INSERT INTO #overdue_contract_certainty
    (
        /*Policy Details*/
        insurance_file_cnt,
        has_multiple_clients,
        has_multiple_insurers,
    
        /*Client Details*/
        client_code,
        client_name,
        
        /*Group Details*/
        group_by_code1,
        group_by_desc1,
        group_by_code2,
        group_by_desc2,

        /*Sort Details*/
        sort_by1a,
        sort_by1b,
        sort_by2

    )
    SELECT
        /*Policy Details*/
        occ.insurance_file_cnt,
        occ.has_multiple_clients,
        occ.has_multiple_insurers,
    
        /*Client Details*/
        p.shortname,
        p.resolved_name,
        
        /*Group Details*/
        occ.group_by_code1,
        occ.group_by_desc1,
        occ.group_by_code2,
        occ.group_by_desc2,
        
        /*Sort Details*/
        occ.sort_by1a,
        occ.sort_by1b,
        occ.sort_by2
    FROM #overdue_contract_certainty occ
    JOIN policy_shared_premiums psp
        ON psp.insurance_file_cnt = occ.insurance_file_cnt
    JOIN party p
        ON p.party_cnt = psp.party_cnt
        AND p.shortname <> occ.client_code
    WHERE occ.insurance_file_cnt = @insurance_file_cnt
    AND occ.group_by_code2 = @group_by_code2
    
    FETCH NEXT FROM c_cursor INTO
        @insurance_file_cnt,
        @group_by_code2
END

CLOSE c_cursor
DEALLOCATE c_cursor

/*Mark all transactions with line numbers*/
SELECT @line_no = 2

IF NOT EXISTS
    (
        SELECT
            NULL
        FROM #overdue_contract_certainty
        WHERE line_no IS NULL
    )
BEGIN
    SELECT @line_no = -1
END

WHILE @line_no > 0
BEGIN

    UPDATE occ
    SET occ.line_no = @line_no
    FROM #overdue_contract_certainty occ
    WHERE occ.line_no IS NULL
    AND occ.client_code = 
        (
            SELECT 
                MIN(client_code)
            FROM #overdue_contract_certainty
            WHERE insurance_file_cnt = occ.insurance_file_cnt 
            AND group_by_code2 = occ.group_by_code2 
            AND line_no IS NULL
        )

    SELECT @line_no = @line_no + 1

    IF NOT EXISTS
        (
            SELECT
                NULL
            FROM #overdue_contract_certainty
            WHERE line_no IS NULL
        )
    BEGIN
        SELECT @line_no = -1
    END

END


DECLARE c_cursor CURSOR FORWARD_ONLY STATIC FOR
    SELECT
        insurance_file_cnt,
        group_by_code2
    FROM #overdue_contract_certainty
    WHERE has_multiple_insurers = 1
    AND line_no = 1
    
OPEN c_cursor

FETCH NEXT FROM c_cursor INTO
    @insurance_file_cnt,
    @group_by_code2

WHILE @@FETCH_STATUS = 0
BEGIN

    SELECT @line_no = 1

    DECLARE c_cursor2 CURSOR FORWARD_ONLY STATIC FOR
        SELECT
            p.shortname,
            p.resolved_name
        FROM #overdue_contract_certainty occ
        JOIN policy_coinsurers pc
            ON pc.insurance_file_cnt = occ.insurance_file_cnt
        JOIN party p
            ON p.party_cnt = pc.party_cnt
            AND p.shortname <> occ.client_code
        WHERE occ.insurance_file_cnt = @insurance_file_cnt
        AND occ.group_by_code2 = @group_by_code2
        AND occ.line_no = 1

    OPEN c_cursor2

    FETCH NEXT FROM c_cursor2 INTO
        @insurer_code,
        @insurer_name

    WHILE @@FETCH_STATUS = 0
    BEGIN

        IF EXISTS
            (
                SELECT
                    NULL
                FROM #overdue_contract_certainty
                WHERE insurance_file_cnt = @insurance_file_cnt
                AND group_by_code2 = @group_by_code2
                AND line_no = @line_no
            )
        BEGIN
        
            UPDATE #overdue_contract_certainty
            SET insurer_code = @insurer_code,
                insurer_name = @insurer_name
            WHERE insurance_file_cnt = @insurance_file_cnt
            AND group_by_code2 = @group_by_code2
            AND line_no = @line_no
        
        END
        ELSE
        BEGIN
        
            INSERT INTO #overdue_contract_certainty
            (
                /*Policy Details*/
                insurance_file_cnt,
                line_no,
                has_multiple_clients,
                has_multiple_insurers,

                /*Insurer Details*/
                insurer_code,
                insurer_name,

                /*Group Details*/
                group_by_code1,
                group_by_desc1,
                group_by_code2,
                group_by_desc2,
                
                /*Sort Details*/
                sort_by1a,
                sort_by1b,
                sort_by2
            )
            SELECT
                /*Policy Details*/
                occ.insurance_file_cnt,
                @line_no,
                occ.has_multiple_clients,
                occ.has_multiple_insurers,

                /*Insurer Details*/
                @insurer_code,
                @insurer_name,

                /*Group Details*/
                occ.group_by_code1,
                occ.group_by_desc1,
                occ.group_by_code2,
                occ.group_by_desc2,
                
                /*Sort Details*/
                occ.sort_by1a,
                occ.sort_by1b,
                occ.sort_by2
            FROM #overdue_contract_certainty occ
            WHERE occ.insurance_file_cnt = @insurance_file_cnt
            AND occ.group_by_code2 = @group_by_code2
            AND occ.line_no = 1
        END
        
        SELECT @line_no = @line_no + 1
        
        FETCH NEXT FROM c_cursor2 INTO
            @insurer_code,
            @insurer_name
    END
    
    CLOSE c_cursor2
    DEALLOCATE c_cursor2
    
    
    FETCH NEXT FROM c_cursor INTO
        @insurance_file_cnt,
        @group_by_code2
END

CLOSE c_cursor
DEALLOCATE c_cursor

/*Select all transactions in correct order*/
IF @sort_by = 'Client Code'
BEGIN

    SELECT 
        * 
    FROM #overdue_contract_certainty
    ORDER BY 
        group_by_code1,
        group_by_code2,
        sort_by1a,
        sort_by2,
        insurance_file_cnt,
        line_no
        
END
ELSE
BEGIN

    SELECT 
        * 
    FROM #overdue_contract_certainty
    ORDER BY 
        group_by_code1,
        group_by_code2,
        sort_by1b DESC,
        sort_by2,
        insurance_file_cnt,
        line_no
        
END

DROP TABLE #overdue_contract_certainty

GO
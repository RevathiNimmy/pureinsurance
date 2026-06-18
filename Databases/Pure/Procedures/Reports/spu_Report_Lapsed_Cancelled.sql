SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Lapsed_Cancelled'
GO

CREATE PROCEDURE spu_Report_Lapsed_Cancelled
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @status VARCHAR(20),
    @business_type CHAR(10),
    @group_by VARCHAR(20),
    @then_by VARCHAR(20),
    @unique_report_name VARCHAR(300)

AS

DECLARE 
    @insurance_file_cnt INT,
    @party_cnt INT,
    @order_id INT,
    @business_type_id INT,
    @line_no INT,
    @prev_insurance_file_cnt INT,
    @policy_ref VARCHAR(50)

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @business_type = NULL
BEGIN
    SELECT @business_type = 'ALL'
END

IF @business_type <> 'ALL'
BEGIN
    SELECT 
        @business_type_id = business_type_id 
    FROM business_type 
    WHERE code = @business_type
END

IF @status = ''
BEGIN
    SELECT @status = 'All'
END


CREATE TABLE #policies
(
    /*ID fields, unique to identify each line*/
    insurance_file_cnt INT,
    line_no INT,

    /*Extra infomation fields*/
    client_cnt INT,
    insurer_cnt INT,

    /*Columns on the report*/  
    policy_ref VARCHAR(30),
    client_code VARCHAR(20),
    client_name VARCHAR(255),
    executive_code VARCHAR(20),
    risk_desc VARCHAR(255),
    insurer_name VARCHAR(255),
    handler_code VARCHAR(20),
    inception_date DATETIME,
    lapse_date DATETIME,
    status_desc VARCHAR(255),    
    lapse_reason_desc VARCHAR(255),
    premium MONEY,
    commission MONEY,    
    
    /*Group fields*/
    group_by_code VARCHAR(50),
    group_by_desc VARCHAR(255),
    then_by_code VARCHAR(50),
    then_by_desc VARCHAR(255)
)
CREATE INDEX I_#policies_insurance_file_cnt_line_no ON #policies (insurance_file_cnt, line_no)

INSERT INTO #policies
(
    insurance_file_cnt,
    line_no,
    policy_ref,
    insurer_cnt,
    insurer_name,
    premium,
    commission,
    client_cnt,
    client_code,
    client_name,
    executive_code
)
SELECT 
    i.insurance_file_cnt,
    1,
    i.insurance_ref,
    ip.party_cnt,
    ip.resolved_name,
    (
        SELECT
            SUM(ROUND(this_premium, 2))
        FROM insurance_file
        WHERE insurance_folder_cnt = i.insurance_folder_cnt
        AND insurance_file_type_id IN (2, 5) /*Live Policy or Permanent MTA*/
        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
        AND policy_version <= i.policy_version
        AND policy_version >= 
        (
            SELECT
                ISNULL(MAX(policy_version), 1)
            FROM insurance_file 
            WHERE insurance_folder_cnt = i.insurance_folder_cnt
            AND insurance_file_type_id = 2 /*Live Policy*/
            AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
            AND policy_version <= i.policy_version
        )
    ),
    (
        SELECT
            SUM(ROUND(commission_amount, 2))
        FROM insurance_file
        WHERE insurance_folder_cnt = i.insurance_folder_cnt
        AND insurance_file_type_id IN (2, 5) /*Live Policy or Permanent MTA*/
        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
        AND policy_version <= i.policy_version
        AND policy_version >= 
        (
            SELECT
                ISNULL(MAX(policy_version), 1)
            FROM insurance_file 
            WHERE insurance_folder_cnt = i.insurance_folder_cnt
            AND insurance_file_type_id = 2 /*Live Policy*/
            AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
            AND policy_version <= i.policy_version
        )
    ),
    p.party_cnt,
    p.shortname,
    p.resolved_name,
   (    	
	SELECT TOP 1 ISNULL(AE.shortname, '') 
		FROM Party PP 
	LEFT OUTER JOIN Party AE
	ON PP.consultant_cnt = AE.party_cnt
	WHERE PP.party_cnt = p.party_cnt
    )
FROM insurance_file i 
JOIN party p
    ON p.party_cnt = i.insured_cnt
JOIN party ip
    ON ip.party_cnt = i.lead_insurer_cnt
WHERE i.source_id = ISNULL(@branch_id, i.source_id)
AND i.lapsed_date BETWEEN @start_date AND @end_date
AND i.policy_version = 
    (
        SELECT
            MAX(policy_version)
        FROM insurance_file
        WHERE insurance_folder_cnt = i.insurance_folder_cnt
        AND insurance_file_type_id IN (2, 5, 10)
    )

/*Remove policies that do not match the status that we are looking for*/
IF @status = 'All'
BEGIN
    DELETE #policies
    FROM #policies p 
    JOIN insurance_file i
        ON i.insurance_file_cnt = p.insurance_file_cnt
    WHERE i.insurance_file_status_id NOT IN (1, 2)        
END

IF @status = 'Lapsed Only'
BEGIN
    DELETE #policies
    FROM #policies p 
    JOIN insurance_file i
        ON i.insurance_file_cnt = p.insurance_file_cnt
    WHERE i.insurance_file_status_id <> 2     
END

IF @status = 'Cancelled Only'
BEGIN
    DELETE #policies
    FROM #policies p 
    JOIN insurance_file i
        ON i.insurance_file_cnt = p.insurance_file_cnt
    WHERE i.insurance_file_status_id <> 1     
END

IF @status = 'Lapsed & Cancelled'
BEGIN
    DELETE #policies
    FROM #policies p 
    JOIN insurance_file i
        ON i.insurance_file_cnt = p.insurance_file_cnt
    WHERE i.insurance_file_status_id NOT IN (1, 2)     
END

/*Remove policies that do not match the business_type that we are looking for*/
IF @business_type <> 'ALL'
BEGIN
    DELETE #policies
    FROM #policies p 
    JOIN insurance_file i
        ON i.insurance_file_cnt = p.insurance_file_cnt
    WHERE i.business_type_id <> @business_type_id
END


/*Update all other fields except client and insurer specific ones*/
UPDATE p
SET p.risk_desc = rc.description,
    p.handler_code = ISNULL(pah.shortname, ''),
    p.inception_date = f.inception_date,
    p.lapse_date = i.lapsed_date,
    p.status_desc = ISNULL(s.description, ''),    
    p.lapse_reason_desc = ISNULL(lr.description, '')
FROM #policies p
JOIN insurance_file i
    ON i.insurance_file_cnt = p.insurance_file_cnt
JOIN insurance_folder f
    ON f.insurance_folder_cnt = i.insurance_folder_cnt
JOIN risk_code rc
    ON rc.risk_code_id = i.risk_code_id
LEFT JOIN insurance_file_status s
    ON s.insurance_file_status_id = i.insurance_file_status_id 
LEFT JOIN party pah
    ON pah.party_cnt = i.account_handler_cnt
LEFT JOIN lapsed_reason lr
    ON lr.lapsed_reason_id = i.lapsed_reason_id


/*Add new lines for having extra share for policies*/
INSERT INTO #policies
(
    insurance_file_cnt,
    line_no,
    policy_ref, 
    client_cnt,
    client_code,
    client_name,
    executive_code
)
SELECT
    i.insurance_file_cnt,
    2,
    i.insurance_ref,
    psp.party_cnt,
    pt.shortname,
    pt.resolved_name,
   (    	
	SELECT 
            TOP 1 ISNULL(AE.shortname, '') 
	FROM Party PP 
	LEFT OUTER JOIN Party AE
	    ON PP.consultant_cnt = AE.party_cnt
	WHERE PP.party_cnt = pt.party_cnt
    )
FROM #policies p
JOIN insurance_file i
    ON i.insurance_file_cnt = p.insurance_file_cnt
JOIN policy_shared_premiums psp
    ON psp.insurance_file_cnt = i.insurance_file_cnt
    AND psp.party_cnt <> i.insured_cnt
JOIN party pt
    ON pt.party_cnt = psp.party_cnt


/*Now Update line no in order(1,2,3) for each client of each policy*/
DECLARE c_policies CURSOR FAST_FORWARD FOR
    SELECT 
        insurance_file_cnt,
        client_cnt
    FROM #policies
    WHERE line_no > 1
    ORDER BY insurance_file_cnt, client_cnt

OPEN c_policies

FETCH NEXT FROM c_policies INTO 
    @insurance_file_cnt,
    @party_cnt

SELECT @line_no = 2
SELECT @prev_insurance_file_cnt = @insurance_file_cnt

WHILE @@FETCH_STATUS = 0
BEGIN

    UPDATE p
        SET p.line_no = @line_no
    FROM #policies p
    WHERE p.insurance_file_cnt = @insurance_file_cnt
    AND p.client_cnt = @party_cnt
    
    SELECT @line_no = @line_no + 1

    FETCH NEXT FROM c_policies INTO 
        @insurance_file_cnt,
        @party_cnt

    IF @prev_insurance_file_cnt <> @insurance_file_cnt
    BEGIN
        SELECT @line_no = 2
        SELECT @prev_insurance_file_cnt = @insurance_file_cnt
    END

END

CLOSE c_policies
DEALLOCATE c_policies


/*Update multiple lines or add new lines for each coinsurer */

DECLARE c_policies CURSOR FAST_FORWARD FOR
    SELECT 
        insurance_file_cnt,
        policy_ref
    FROM #policies
    ORDER BY insurance_file_cnt

OPEN c_policies

FETCH NEXT FROM c_policies INTO 
    @insurance_file_cnt,
    @policy_ref

WHILE @@FETCH_STATUS = 0
BEGIN

    /*Add insurer code and amounts to the table and include an extra line for each coinsurer*/
    SELECT @line_no = 1
    
    IF EXISTS
        (
            SELECT 
                NULL
            FROM policy_coinsurers pc
            WHERE pc.insurance_file_cnt = @insurance_file_cnt
        )
    BEGIN
    
        UPDATE p
        SET p.insurer_cnt = NULL,
            p.insurer_name = '-',
            p.premium = 0, 
            p.commission = 0
        FROM #policies p
        WHERE p.insurance_file_cnt = @insurance_file_cnt
        AND p.line_no = @line_no
    
        SELECT @line_no = @line_no + 1

        DECLARE c_insurers CURSOR FAST_FORWARD FOR
	        SELECT 
	            pc.party_cnt,
	            pc.coinsurer_count
	        FROM insurance_file i
	        JOIN policy_coinsurers pc
	            ON pc.insurance_file_cnt = i.insurance_file_cnt
	        WHERE i.insurance_file_cnt = @insurance_file_cnt
	        ORDER BY coinsurer_count

        
	    OPEN c_insurers
	
	    FETCH NEXT FROM c_insurers INTO 
	        @party_cnt,
	        @order_id
	
	    WHILE @@FETCH_STATUS = 0
	    BEGIN
        
	        IF EXISTS
	            (
	                SELECT
	                    NULL
	                FROM #policies
	                WHERE insurance_file_cnt = @insurance_file_cnt
	                AND line_no = @line_no
	            )
	        BEGIN
	            /*Line already exists so update it*/
                UPDATE p
                SET p.insurer_cnt = @party_cnt,
                    p.insurer_name = 
                        (
                            SELECT
                                resolved_name
                            FROM party
                            WHERE party_cnt = @party_cnt
                        ),
                    p.premium =
                        (
                            SELECT
                                SUM(ROUND(pc.coinsurer_value, 2))
                            FROM insurance_file i
                            JOIN policy_coinsurers pc
                                ON pc.insurance_file_cnt = i.insurance_file_cnt
                                AND pc.party_cnt = @party_cnt
                            WHERE i.insurance_folder_cnt = ix.insurance_folder_cnt
                            AND i.insurance_file_type_id IN (2, 5) /*Live Policy or Permanent MTA*/
                            AND ISNULL(i.insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
                            AND i.policy_version <= ix.policy_version
                            AND i.policy_version >= 
                                (
                                    SELECT
                                        ISNULL(MAX(policy_version), 1)
                                    FROM insurance_file 
                                    WHERE insurance_folder_cnt = ix.insurance_folder_cnt
                                    AND insurance_file_type_id = 2 /*Live Policy*/
                                    AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
                                    AND policy_version <= ix.policy_version
                                )
                        ), 
                    p.commission =                     
                        (
                            SELECT
                                SUM(ROUND(pc.coinsurer_commission_amount, 2))
                            FROM insurance_file i
                            JOIN policy_coinsurers pc
                                ON pc.insurance_file_cnt = i.insurance_file_cnt
                                AND pc.party_cnt = @party_cnt
                            WHERE i.insurance_folder_cnt = ix.insurance_folder_cnt
                            AND i.insurance_file_type_id IN (2, 5) /*Live Policy or Permanent MTA*/
                            AND ISNULL(i.insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
                            AND i.policy_version <= ix.policy_version
                            AND i.policy_version >= 
                                (
                                    SELECT
                                        ISNULL(MAX(policy_version), 1)
                                    FROM insurance_file 
                                    WHERE insurance_folder_cnt = ix.insurance_folder_cnt
                                    AND insurance_file_type_id = 2 /*Live Policy*/
                                    AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
                                    AND policy_version <= ix.policy_version
                                )
                        )
                FROM #policies p
                JOIN insurance_file ix
                    ON ix.insurance_file_cnt = p.insurance_file_cnt
                WHERE p.insurance_file_cnt = @insurance_file_cnt
                AND p.line_no = @line_no
	        END
	        ELSE
	        BEGIN
	            /*Line does not exists so create it*/
	            INSERT INTO #policies
	            (
	                insurance_file_cnt,
	                line_no,
			policy_ref,
	                insurer_cnt,
	                insurer_name,
	                premium,
	                commission
	            )
	            SELECT
	                @insurance_file_cnt,
	                @line_no,
                    @policy_ref,
	                @party_cnt,
	                (
	                    SELECT
	                        resolved_name
	                    FROM party
	                    WHERE party_cnt = @party_cnt
	                ),
	                (
	                    SELECT
	                        SUM(ROUND(pc.coinsurer_value, 2))
	                    FROM insurance_file i
	                    JOIN policy_coinsurers pc
	                        ON pc.insurance_file_cnt = i.insurance_file_cnt
	                        AND pc.party_cnt = @party_cnt
	                    WHERE i.insurance_folder_cnt = ix.insurance_folder_cnt
	                    AND i.insurance_file_type_id IN (2, 5) /*Live Policy or Permanent MTA*/
	                    AND ISNULL(i.insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
	                    AND i.policy_version <= ix.policy_version
	                    AND i.policy_version >= 
	                        (
	                            SELECT
	                                ISNULL(MAX(policy_version), 1)
	                            FROM insurance_file 
	                            WHERE insurance_folder_cnt = ix.insurance_folder_cnt
	                            AND insurance_file_type_id = 2 /*Live Policy*/
	                            AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
	                            AND policy_version <= ix.policy_version
	                        )
	                ), 
	                (
	                    SELECT
	                        SUM(ROUND(pc.coinsurer_commission_amount, 2))
	                    FROM insurance_file i
	                    JOIN policy_coinsurers pc
	                        ON pc.insurance_file_cnt = i.insurance_file_cnt
	                        AND pc.party_cnt = @party_cnt
	                    WHERE i.insurance_folder_cnt = ix.insurance_folder_cnt
	                    AND i.insurance_file_type_id IN (2, 5) /*Live Policy or Permanent MTA*/
	                    AND ISNULL(i.insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
	                    AND i.policy_version <= ix.policy_version
	                    AND i.policy_version >= 
	                        (
	                            SELECT
	                                ISNULL(MAX(policy_version), 1)
	                            FROM insurance_file 
	                            WHERE insurance_folder_cnt = ix.insurance_folder_cnt
	                            AND insurance_file_type_id = 2 /*Live Policy*/
	                            AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2) /*Live, Cancelled or Lapsed*/
	                            AND policy_version <= ix.policy_version
	                        )
	                )
	            FROM insurance_file ix
	            WHERE ix.insurance_file_cnt = @insurance_file_cnt
			END

			SELECT @line_no = @line_no + 1
    
            FETCH NEXT FROM c_insurers INTO 
			    @party_cnt,
				@order_id
	    END

		CLOSE c_insurers
		DEALLOCATE c_insurers   
	    
    END

	FETCH NEXT FROM c_policies INTO 
	    @insurance_file_cnt,
	    @policy_ref

END

CLOSE c_policies
DEALLOCATE c_policies


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
    FROM #policies 
    WHERE insurance_file_cnt IN
        (
            SELECT
                p.insurance_file_cnt
            FROM #policies p
            JOIN party pc
                ON pc.party_cnt = p.client_cnt
            JOIN temp_report_exclude tre
                ON tre.id = pc.consultant_cnt
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
    DELETE FROM #policies
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pc
                ON pc.party_cnt = pp.client_cnt
            JOIN party pae
                ON pae.party_cnt = pc.consultant_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
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
    FROM #policies 
    WHERE insurance_file_cnt IN
        (
            SELECT
                p.insurance_file_cnt
            FROM #policies p
            JOIN insurance_file i
                ON i.insurance_file_cnt = p.insurance_file_cnt
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
    DELETE FROM #policies
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN insurance_file i
                ON i.insurance_file_cnt = pp.insurance_file_cnt
            JOIN party pc
                ON pc.party_cnt = i.account_handler_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            AND pp.line_no = 1
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
    FROM #policies 
    WHERE insurance_file_cnt IN
        (
            SELECT
                p.insurance_file_cnt
            FROM #policies p
            JOIN insurance_file i
                ON i.insurance_file_cnt = p.insurance_file_cnt
            JOIN temp_report_exclude tre
                ON tre.id = i.risk_code_id
                AND tre.type IN ('RC')
            WHERE tre.unique_report_name = @unique_report_name
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
    FROM #policies 
    WHERE insurance_file_cnt IN
        (
            SELECT
                p.insurance_file_cnt
            FROM #policies p
            JOIN temp_report_exclude tre
                ON tre.id = p.insurer_cnt
                AND tre.type IN ('IN')
            WHERE tre.unique_report_name = @unique_report_name
        )
END


/*Work out all of the groupings*/
IF @group_by = 'Account Executive'
BEGIN

    UPDATE p
    SET p.group_by_code = 'No Account Executive',
        p.group_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pc
                ON pc.party_cnt = pp.client_cnt
            JOIN party pae
                ON pae.party_cnt = pc.consultant_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
        )
        
    UPDATE p
    SET p.group_by_code = 'Multiple Account Executives',
        p.group_by_desc = NULL
    FROM #policies p
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pc
                ON pc.party_cnt = pp.client_cnt
            LEFT JOIN party pae
                ON pae.party_cnt = pc.consultant_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            GROUP BY pp.insurance_file_cnt
            HAVING MIN(ISNULL(pae.party_cnt, 0)) <> MAX(ISNULL(pae.party_cnt, 0))
        )
        
    UPDATE p
    SET p.group_by_code = pae.shortname,
        p.group_by_desc = pae.resolved_name
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN party pc
        ON pc.party_cnt = pp.client_cnt
    JOIN party pae
        ON pae.party_cnt = pc.consultant_cnt
    WHERE p.group_by_code IS NULL

END

IF @then_by = 'Account Executive'
BEGIN

    UPDATE p
    SET p.then_by_code = 'No Account Executive',
        p.then_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pc
                ON pc.party_cnt = pp.client_cnt
            JOIN party pae
                ON pae.party_cnt = pc.consultant_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
        )
        
    UPDATE p
    SET p.then_by_code = 'Multiple Account Executives',
        p.then_by_desc = NULL
    FROM #policies p
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pc
                ON pc.party_cnt = pp.client_cnt
            LEFT JOIN party pae
                ON pae.party_cnt = pc.consultant_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            GROUP BY pp.insurance_file_cnt
            HAVING MIN(ISNULL(pae.party_cnt, 0)) <> MAX(ISNULL(pae.party_cnt, 0))
        )
        
    UPDATE p
    SET p.then_by_code = pae.shortname,
        p.then_by_desc = pae.resolved_name
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN party pc
        ON pc.party_cnt = pp.client_cnt
    JOIN party pae
        ON pae.party_cnt = pc.consultant_cnt
    WHERE p.then_by_code IS NULL

END


IF @group_by = 'Account Handler'
BEGIN

    UPDATE p
    SET p.group_by_code = 'No Account Handler',
        p.group_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN insurance_file i
                ON i.insurance_file_cnt = pp.insurance_file_cnt
            JOIN party pah
                ON pah.party_cnt = i.account_handler_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            AND pp.line_no = 1
        )
        
    UPDATE p
    SET p.group_by_code = pah.shortname,
        p.group_by_desc = pah.resolved_name
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = pp.insurance_file_cnt
    JOIN party pah
        ON pah.party_cnt = i.account_handler_cnt
    WHERE p.group_by_code IS NULL

END

IF @then_by = 'Account Handler'
BEGIN

    UPDATE p
    SET p.then_by_code = 'No Account Handler',
        p.then_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN insurance_file i
                ON i.insurance_file_cnt = pp.insurance_file_cnt
            JOIN party pah
                ON pah.party_cnt = i.account_handler_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            AND pp.line_no = 1
        )
        
    UPDATE p
    SET p.then_by_code = pah.shortname,
        p.then_by_desc = pah.resolved_name
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = pp.insurance_file_cnt
    JOIN party pah
        ON pah.party_cnt = i.account_handler_cnt
    WHERE p.then_by_code IS NULL
        
END


IF @group_by = 'Branch'
BEGIN
        
    UPDATE p
    SET p.group_by_code = s.code,
        p.group_by_desc = s.description
    FROM #policies p
    JOIN insurance_file i
        ON i.insurance_file_cnt = p.insurance_file_cnt    
    JOIN source s
        ON s.source_id = i.source_id
    WHERE p.group_by_code IS NULL

END

IF @then_by = 'Branch'
BEGIN
        
    UPDATE p
    SET p.then_by_code = s.code,
        p.then_by_desc = s.description
    FROM #policies p
    JOIN insurance_file i
        ON i.insurance_file_cnt = p.insurance_file_cnt    
    JOIN source s
        ON s.source_id = i.source_id
    WHERE p.then_by_code IS NULL
        
END

IF @group_by = 'Business Type'
BEGIN
        
    UPDATE p
    SET p.group_by_code = 'No Business Type',
        p.group_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN insurance_file i
                ON i.insurance_file_cnt = pp.insurance_file_cnt
            JOIN business_type bt
                ON bt.business_type_id = i.business_type_id
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            AND pp.line_no = 1
        )
        
    UPDATE p
    SET p.group_by_code = bt.code,
        p.group_by_desc = bt.description
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = pp.insurance_file_cnt
    JOIN business_type bt
        ON bt.business_type_id = i.business_type_id
    WHERE p.group_by_code IS NULL

END

IF @then_by = 'Business Type'
BEGIN
        
    UPDATE p
    SET p.then_by_code = 'No Business Type',
        p.then_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN insurance_file i
                ON i.insurance_file_cnt = pp.insurance_file_cnt
            JOIN business_type bt
                ON bt.business_type_id = i.business_type_id
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            AND pp.line_no = 1
        )
        
    UPDATE p
    SET p.then_by_code = bt.code,
        p.then_by_desc = bt.description
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = pp.insurance_file_cnt
    JOIN business_type bt
        ON bt.business_type_id = i.business_type_id
    WHERE p.then_by_code IS NULL
        
END

IF @group_by = 'Insurer'
BEGIN

    UPDATE p
    SET p.group_by_code = 'No Insurer',
        p.group_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pi
                ON pi.party_cnt = pp.insurer_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
        )
        
    UPDATE p
    SET p.group_by_code = 'Multiple Insurers',
        p.group_by_desc = NULL
    FROM #policies p
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pi
                ON pi.party_cnt = pp.insurer_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            GROUP BY pp.insurance_file_cnt
            HAVING MIN(pi.party_cnt) <> MAX(pi.party_cnt)
        )
        
    UPDATE p
    SET p.group_by_code = pi.shortname,
        p.group_by_desc = pi.resolved_name
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN party pi
        ON pi.party_cnt = pp.insurer_cnt
    WHERE p.group_by_code IS NULL

END

IF @then_by = 'Insurer'
BEGIN

    UPDATE p
    SET p.then_by_code = 'No Insurer',
        p.then_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pi
                ON pi.party_cnt = pp.insurer_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
        )
        
    UPDATE p
    SET p.then_by_code = 'Multiple Insurers',
        p.then_by_desc = NULL
    FROM #policies p
    WHERE EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN party pi
                ON pi.party_cnt = pp.insurer_cnt
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            GROUP BY pp.insurance_file_cnt
            HAVING MIN(pi.party_cnt) <> MAX(pi.party_cnt)
        )
        
    UPDATE p
    SET p.then_by_code = pi.shortname,
        p.then_by_desc = pi.resolved_name
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN party pi
        ON pi.party_cnt = pp.insurer_cnt
    WHERE p.then_by_code IS NULL
        
END

IF @group_by = 'Lapsed Reason'
BEGIN
        
    UPDATE p
    SET p.group_by_code = 'No Lapsed Reason',
        p.group_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN insurance_file i
                ON i.insurance_file_cnt = pp.insurance_file_cnt
            JOIN lapsed_reason lr
                ON lr.lapsed_reason_id = i.lapsed_reason_id
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            AND pp.line_no = 1
        )
        
    UPDATE p
    SET p.group_by_code = lr.code,
        p.group_by_desc = lr.description
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = pp.insurance_file_cnt
    JOIN lapsed_reason lr
        ON lr.lapsed_reason_id = i.lapsed_reason_id
    WHERE p.group_by_code IS NULL

END

IF @then_by = 'Lapsed Reason'
BEGIN
        
    UPDATE p
    SET p.then_by_code = 'No Lapsed Reason',
        p.then_by_desc = NULL
    FROM #policies p
    WHERE NOT EXISTS
        (
            SELECT
                NULL
            FROM #policies pp
            JOIN insurance_file i
                ON i.insurance_file_cnt = pp.insurance_file_cnt
            JOIN lapsed_reason lr
                ON lr.lapsed_reason_id = i.lapsed_reason_id
            WHERE pp.insurance_file_cnt = p.insurance_file_cnt
            AND pp.line_no = 1
        )
        
    UPDATE p
    SET p.then_by_code = lr.code,
        p.then_by_desc = lr.description
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = pp.insurance_file_cnt
    JOIN lapsed_reason lr
        ON lr.lapsed_reason_id = i.lapsed_reason_id
    WHERE p.then_by_code IS NULL

END

IF @group_by = 'Status'
BEGIN
                
    UPDATE p
    SET p.group_by_code = ifs.code,
        p.group_by_desc = ifs.description
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = pp.insurance_file_cnt
    JOIN insurance_file_status ifs
        ON ifs.insurance_file_status_id = i.insurance_file_status_id
    WHERE p.group_by_code IS NULL

END

IF @then_by = 'Status'
BEGIN
                
    UPDATE p
    SET p.then_by_code = ifs.code,
        p.then_by_desc = ifs.description
    FROM #policies p
    JOIN #policies pp
        ON pp.insurance_file_cnt = p.insurance_file_cnt
        AND pp.line_no = 1
    JOIN insurance_file i
        ON i.insurance_file_cnt = pp.insurance_file_cnt
    JOIN insurance_file_status ifs
        ON ifs.insurance_file_status_id = i.insurance_file_status_id
    WHERE p.then_by_code IS NULL

END

SELECT
    *
FROM #policies
ORDER BY 
    group_by_code, 
    then_by_code, 
    --insurance_file_cnt, 
    policy_ref,
    line_no 

DROP TABLE #policies

GO
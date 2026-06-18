SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Renewal_List_With_Options'
GO

CREATE PROCEDURE spu_Report_Renewal_List_With_Options
    @branch_id INT,
    @start_date DATETIME,
    @end_date DATETIME,
    @Policy_Selection VARCHAR(10),
    @group_by VARCHAR(20),
    @then_by VARCHAR(20),
    @inc_exc_account_executives VARCHAR(1),
    @inc_exc_account_handlers VARCHAR(1),
    @inc_exc_risks_codes VARCHAR(1),
    @inc_exc_insurers VARCHAR(1),
    @unique_report_name VARCHAR(300),
    @fsa_customer_category VARCHAR(30),
    @inc_prospects VARCHAR(3)
AS
DECLARE 
    @Insurance_File_Cnt INT,
    @Client_Cnt INT,
    @party_cnt INT,
    @order_id INT,
    @insurer_order_id INT,
    @total_order_id INT,
    @premium INT,
    @commission INT,
    @commission_percent INT,
    @IsProspect BIT,
    @ExcAE INT,
    @ExcAH INT,
    @ExcRC INT,
    @ExcIN INT,
    @customer_category int,
    @fsa_disabled BIT

SET NOCOUNT ON
    
IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

CREATE TABLE #Ins_Status 
(
	Insurance_File_Status_Id INT
)

CREATE TABLE #Ins_Type 
(
	Insurance_File_Type_Id INT
)

IF LEFT(@Policy_Selection, 1) = 'C' OR LEFT(@Policy_Selection, 1) = 'Q'
BEGIN

    IF ISNULL(@inc_prospects,'No') = 'No'  
    BEGIN  
        SELECT @IsProspect = 0  
    END  

	INSERT INTO #Ins_Status
	SELECT Insurance_File_Status_Id
	FROM Insurance_File_Status
	where Insurance_File_Status_Id in (3) --Under Renewal

    IF LEFT(@Policy_Selection, 1) = 'C'
    BEGIN
	    INSERT INTO #Ins_Type
	    SELECT Insurance_File_Type_Id
	    FROM Insurance_File_Type
	    WHERE Insurance_File_Type_Id IN (2,5,6) -- Live, Permananet MTA, Temporary MTA
    END
    ELSE
    BEGIN
	    INSERT INTO #Ins_Type
	    SELECT Insurance_File_Type_Id
	    FROM Insurance_File_Type
	    WHERE Insurance_File_Type_Id IN (1) -- Live, Permananet MTA, Temporary MTA
    END
END
ELSE
BEGIN
	INSERT INTO #Ins_Status
	SELECT Insurance_File_Status_Id
	FROM Insurance_File_Status
	WHERE Insurance_File_Status_Id in (1,2,3,4) --Cancelled, Lapsed, Under Renewal, Replaced

    INSERT INTO #Ins_Type
	SELECT Insurance_File_Type_Id
	FROM Insurance_File_Type
	WHERE Insurance_File_Type_Id IN (1,2,5,6) -- Quote, Live, Permananet MTA, Temporary MTA
END

IF @then_by = 'None'
BEGIN
    SELECT @then_by = ''
END

IF @then_by = @group_by
BEGIN
    SELECT @then_by = ''
END

SELECT @ExcAE = 0
IF EXISTS ( SELECT * FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type='AE')
BEGIN
    SELECT @ExcAE = 1
END

SELECT @ExcAH = 0
IF EXISTS ( SELECT * FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type='AH')
BEGIN
    SELECT @ExcAH = 1
END

SELECT @ExcRC = 0
IF EXISTS ( SELECT * FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type='RC')
BEGIN
    SELECT @ExcRC = 1
END

SELECT @ExcIN = 0
IF EXISTS ( SELECT * FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type='IN')
BEGIN
    SELECT @ExcIN = 1
END

--Check if FSA enabled
IF NOT EXISTS  
    (  
        SELECT NULL  
        FROM hidden_options  
        WHERE option_number = 61  
        AND value = '1'  
    )  
BEGIN  
    SELECT @fsa_disabled = 1
END  

IF @fsa_customer_category='Commercial'
    SELECT @customer_category=0
ELSE
    IF @fsa_customer_category='Retail'
        SELECT @customer_category=1

CREATE TABLE #Renewal_List_With_options
(
    branch_id INT,
    branch VARCHAR(255),
    order_id INT,

    client_cnt INT,
    client_code VARCHAR(20),
    client_name VARCHAR(255),

    insurance_file_cnt INT,
    policy_ref VARCHAR(30),

    insurer_cnt INT,
    insurer_code VARCHAR(20),
    insurer_name VARCHAR(255),

    executive_cnt INT,
    executive_code VARCHAR(20),

    handler_cnt INT,
    handler_code VARCHAR(20),

    payment_method VARCHAR(60),
    cover_start_date DATETIME,
    renewal_date DATETIME,

    risk_cnt INT,
    risk_code VARCHAR(10),
    risk_desc VARCHAR(255),

    agent_code VARCHAR(20),

    premium MONEY,
    commission MONEY,
    commission_percent FLOAT,
    fees MONEY,
    extra_premium MONEY,
    extra_commission MONEY,
    discount MONEY,
    warning_message VARCHAR(255) 
)
    
    
INSERT INTO #Renewal_List_With_options
SELECT 
    S.source_id,
    S.description,
    1,

    PC.party_cnt,
    PC.Shortname,
    PC.resolved_name,

    IFI.Insurance_File_Cnt,
    IFI.insurance_ref,

    PIN.party_cnt,
    PIN.shortname,
    PIN.resolved_name,

    ISNULL(PAE.party_cnt,0),
    ISNULL(PAE.shortname, ''),

    ISNULL(PAH.party_cnt,0),
    ISNULL(PAH.shortname, ''),

    IFI.payment_method,
    (
        SELECT
            MIN(cover_start_date)
        FROM insurance_file
        WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
        AND insurance_file_type_id IN 
	    (
		    SELECT Insurance_File_Type_Id
		    FROM  #Ins_Type
	    )
        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
        AND policy_version <= IFI.policy_version
        AND policy_version >= 
            (
                SELECT
                    ISNULL(MAX(policy_version), 1)
                FROM insurance_file 
                WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                AND insurance_file_type_id = 2 /*Live Policy*/
                AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                AND policy_version <= IFI.policy_version
            )
    ),    
    IFI.renewal_date,

    ISNULL(RC.risk_code_id, ''),
    RC.code,
    RC.description,

    CASE  
        WHEN EXISTS  
                (  
                    /*Check if sub-agent present on policy*/  
                    SELECT  
                        NULL  
                    FROM policy_agents POA  
                    JOIN party P  
                        ON POA.agent_cnt = P.party_cnt
                    JOIN account ACC  
                        ON ACC.account_key = P.party_cnt 
                    JOIN ledger LED  
                        ON LED.ledger_id = ACC.ledger_id  
                        AND LED.ledger_short_name = 'UB'
                    WHERE POA.insurance_file_cnt = IFI.insurance_file_cnt  
                ) THEN 'YES' /*Sub-agent Present*/  
        ELSE '' /*Sub-agent Not Present*/  
    END,

    CASE 
        WHEN PIN.shortname LIKE 'MULTI%' THEN 
            NULL             
        ELSE  
            (
                SELECT
                    SUM(ROUND(this_premium, 2))
                FROM insurance_file
                WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                AND insurance_file_type_id IN 
	            (
		            SELECT Insurance_File_Type_Id
		            FROM  #Ins_Type
   	            )
                AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                AND policy_version <= IFI.policy_version
                AND policy_version >= 
                    (
                        SELECT
                            ISNULL(MAX(policy_version), 1)
                        FROM insurance_file 
                        WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                        AND insurance_file_type_id = 2 /*Live Policy*/
                        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                        AND policy_version <= IFI.policy_version
                    )
            )
    END,
    CASE 
        WHEN PIN.shortname LIKE 'MULTI%' THEN 
            NULL             
        ELSE  
            (
                SELECT
                    SUM(ROUND(commission_amount, 2))
                FROM insurance_file
                WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                AND insurance_file_type_id IN 
	            (
		            SELECT Insurance_File_Type_Id
		            FROM  #Ins_Type
   	            )
                AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                AND policy_version <= IFI.policy_version
                AND policy_version >= 
                    (
                        SELECT
                            ISNULL(MAX(policy_version), 1)
                        FROM insurance_file 
                        WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                        AND insurance_file_type_id = 2 /*Live Policy*/
                        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                        AND policy_version <= IFI.policy_version
                    )
            )
    END,
    CASE 
        WHEN PIN.shortname LIKE 'MULTI%' THEN 
            NULL
        ELSE 
            ISNULL((
                SELECT
                    ROUND(SUM(ROUND(commission_amount, 2)) / SUM(ROUND(net_premium, 2)) * 100, 2)
                FROM insurance_file
                WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                AND insurance_file_type_id IN 
	            (
		            SELECT Insurance_File_Type_Id
		            FROM  #Ins_Type
   	            )
                AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                AND policy_version <= IFI.policy_version
                AND policy_version >= 
                    (
                        SELECT
                            ISNULL(MAX(policy_version), 1)
                        FROM insurance_file 
                        WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                        AND insurance_file_type_id = 2 /*Live Policy*/
                        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                        AND policy_version <= IFI.policy_version
                    )
                HAVING SUM(ROUND(net_premium, 2)) <> 0
            ), 0)
    END,

    (
        SELECT
            SUM(ROUND(pf.fee_amount, 2))
        FROM insurance_file i
        JOIN policy_fee pf
            ON pf.insurance_file_cnt = i.insurance_file_cnt
        JOIN party p
            ON p.party_cnt = pf.party_cnt
        JOIN party_type pt
            ON pt.party_type_id = p.party_type_id
            AND pt.code = 'FE'
        WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
        AND insurance_file_type_id IN
        (
	        SELECT Insurance_File_Type_Id
	        FROM  #Ins_Type
   	    )
        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
        AND policy_version <= IFI.policy_version
        AND policy_version >= 
            (
                SELECT
                    ISNULL(MAX(policy_version), 1)
                FROM insurance_file 
                WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                AND insurance_file_type_id = 2 /*Live Policy*/
                AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                AND policy_version <= IFI.policy_version
            )
    ),
    (
        SELECT
            SUM(ROUND(pf.fee_amount, 2))
        FROM insurance_file i
        JOIN policy_fee pf
            ON pf.insurance_file_cnt = i.insurance_file_cnt
        JOIN party p
            ON p.party_cnt = pf.party_cnt
        JOIN party_type pt
            ON pt.party_type_id = p.party_type_id
            AND pt.code = 'EX'
        WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
        AND insurance_file_type_id IN
        (
	        SELECT Insurance_File_Type_Id
	        FROM  #Ins_Type
   	    )
        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
        AND policy_version <= IFI.policy_version
        AND policy_version >= 
            (
                SELECT
                    ISNULL(MAX(policy_version), 1)
                FROM insurance_file 
                WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                AND insurance_file_type_id = 2 /*Live Policy*/
                AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                AND policy_version <= IFI.policy_version
            )
    ),
    (
        SELECT
            SUM(ROUND(pf.commission_amount, 2))
        FROM insurance_file i
        JOIN policy_fee pf
            ON pf.insurance_file_cnt = i.insurance_file_cnt
        JOIN party p
            ON p.party_cnt = pf.party_cnt
        JOIN party_type pt
            ON pt.party_type_id = p.party_type_id
            AND pt.code = 'EX'
        WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
        AND insurance_file_type_id IN
        (
	        SELECT Insurance_File_Type_Id
	        FROM  #Ins_Type
   	    )
        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
        AND policy_version <= IFI.policy_version
        AND policy_version >= 
            (
                SELECT
                    ISNULL(MAX(policy_version), 1)
                FROM insurance_file 
                WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                AND insurance_file_type_id = 2 /*Live Policy*/
                AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                AND policy_version <= IFI.policy_version
            )
    ),
    (
        SELECT
            SUM(ROUND(pf.fee_amount, 2))
        FROM insurance_file i
        JOIN policy_fee pf
            ON pf.insurance_file_cnt = i.insurance_file_cnt
        JOIN party p
            ON p.party_cnt = pf.party_cnt
        JOIN party_type pt
            ON pt.party_type_id = p.party_type_id
            AND pt.code = 'DI'
        WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
        AND insurance_file_type_id IN 
        (
	        SELECT Insurance_File_Type_Id
	        FROM  #Ins_Type
   	    )
        AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
        AND policy_version <= IFI.policy_version
        AND policy_version >= 
            (
                SELECT
                    ISNULL(MAX(policy_version), 1)
                FROM insurance_file 
                WHERE insurance_folder_cnt = IFI.insurance_folder_cnt
                AND insurance_file_type_id = 2 /*Live Policy*/
                AND ISNULL(insurance_file_status_id, 0) IN (0, 1, 2, 3, 4) /*Cancelled, Lapsed, Under Renewal, Replaced*/
                AND policy_version <= IFI.policy_version
            )
    )  , 
RSC.description
FROM insurance_file IFI
JOIN insurance_folder IFO
    ON IFO.insurance_folder_cnt = IFI.insurance_folder_cnt
JOIN party PC
    ON PC.party_cnt = IFO.insurance_holder_cnt
JOIN party PIN
    ON PIN.party_cnt = IFI.lead_insurer_cnt
LEFT JOIN party PAH
    ON PAH.party_cnt = IFI.account_handler_cnt 
LEFT JOIN party PAE
    ON PC.consultant_cnt = PAE.party_cnt
LEFT JOIN Party PAG
    ON PAG.party_cnt = PC.agent_cnt
JOIN risk_code RC
    ON RC.risk_code_id = IFI.risk_code_id
JOIN source S
    ON S.source_id = IFI.source_id
JOIN currency C
    ON C.currency_id = IFI.currency_id 
LEFT JOIN renewal_stop_code RSC  
    ON RSC.renewal_stop_code_id = IFI.renewal_stop_code_id  
WHERE PC.party_type_id IN (1, 2, 4) /*Personal, Group and Corporate Clients*/
AND IFI.insurance_file_type_id IN 
    (
        SELECT Insurance_File_Type_Id
	    FROM  #Ins_Type
    )
AND ISNULL(IFI.insurance_file_status_id, 3) in 
	(
		SELECT Insurance_File_Status_Id
		FROM  #Ins_Status
	)
AND ISNULL(IFI.policy_ignore, 0) <> 1
AND IFI.policy_version = 
    (
        SELECT 
            MAX(policy_version) 
        FROM insurance_file
        WHERE insurance_folder_cnt = IFO.insurance_folder_cnt
        AND insurance_file_type_id IN
        (
	        SELECT Insurance_File_Type_Id
	        FROM  #Ins_Type
   	    )
    )
AND IFI.source_id = ISNULL(@branch_id, IFI.source_id)
AND PC.is_prospect = ISNULL(@IsProspect, PC.is_prospect)
AND IFI.renewal_date BETWEEN @start_date AND @end_date
AND (@ExcRC = 0 OR 
    (IFI.risk_code_id NOT IN (SELECT id FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type ='RC') AND @ExcRC = 1))
AND (@ExcAH = 0 OR 
    (IFI.account_handler_cnt NOT IN (SELECT id FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type ='AH') AND @ExcAH = 1))
AND (@fsa_disabled <> 1 OR
    (IFI.fsa_customer_category_id=ISNULL(@customer_category,IFI.fsa_customer_category_id)OR (IFI.fsa_customer_category_id IS NULL AND @customer_category IS NULL)))
AND (@fsa_disabled IS NULL OR 
    (@customer_category IS NULL AND IFI.fsa_customer_category_id IS NULL))


SELECT * INTO #rlwo_temp FROM #Renewal_List_With_options

DECLARE c_rlwo CURSOR FORWARD_ONLY STATIC FOR
    SELECT insurance_file_cnt, client_cnt 
    FROM #rlwo_temp

OPEN c_rlwo 

FETCH NEXT FROM c_rlwo into @Insurance_File_Cnt, @Client_Cnt
WHILE @@FETCH_STATUS = 0
BEGIN
    
    SELECT @order_id = 2
    DECLARE c_client CURSOR FAST_FORWARD FOR 
        SELECT party_cnt
        FROM policy_shared_premiums 
        WHERE insurance_file_cnt = @Insurance_File_Cnt 
        AND party_cnt <> @Client_Cnt

    OPEN c_client
    FETCH NEXT FROM c_client INTO @party_cnt

    WHILE @@FETCH_STATUS = 0
    BEGIN

        INSERT INTO #Renewal_List_With_options
        (
            branch_id,
            branch,
            order_id,   
            client_cnt,
            client_code,
            client_name,
            insurance_file_cnt,
            policy_ref,
            executive_cnt,
            executive_code,
            handler_cnt,
            handler_code,
            payment_method,
            cover_start_date,
            renewal_date,
            risk_cnt,
            risk_code,
            risk_desc,
            agent_code
        )
        SELECT 
            branch_id,
            branch,
            @order_id,
            @party_cnt,
            (   
                SELECT ISNULL(P.shortname,'') 
                FROM party p
                WHERE party_cnt = @party_cnt
            ),    
            (   
                SELECT ISNULL(P.resolved_name,'') 
                FROM party p
                WHERE party_cnt = @party_cnt
            ),
            @Insurance_File_Cnt,
            policy_ref,
            (   
                SELECT TOP 1 ISNULL(AE.party_cnt,0)
                FROM Party P 
                LEFT OUTER JOIN Party AE
                    ON P.consultant_cnt = AE.party_cnt
                WHERE P.party_cnt = @party_cnt
            ),
            (   
                SELECT TOP 1 ISNULL(AE.shortname, '') 
                FROM Party P 
                LEFT OUTER JOIN Party AE
                    ON P.consultant_cnt = AE.party_cnt
                WHERE P.party_cnt = @party_cnt
            ),
            handler_cnt,
            handler_code,
            payment_method,
            cover_start_date,
            renewal_date,
            risk_cnt,
            risk_code,
            risk_desc,
            agent_code

        FROM #rlwo_temp
        WHERE insurance_file_cnt = @Insurance_File_Cnt 

        SELECT @order_id = @order_id + 1
    
        FETCH NEXT FROM c_client INTO @party_cnt
    END

    CLOSE c_client
    DEALLOCATE c_client

    /*For Co-Insurers*/
    SELECT @total_order_id = @order_id - 1

    SELECT @insurer_order_id = 2
    DECLARE c_insurer CURSOR FAST_FORWARD FOR 
        SELECT 
            party_cnt, 
            (coinsurer_value + coinsurer_ipt_amount), 
            coinsurer_commission_amount,
            CASE 
                WHEN coinsurer_commission_amount = 0 THEN 
                    0
                ELSE
                    ((coinsurer_commission_amount * 100) / coinsurer_value)  
            END
        FROM policy_coinsurers  
        WHERE insurance_file_cnt = @Insurance_File_Cnt

    OPEN c_insurer
    FETCH NEXT FROM c_insurer INTO @party_cnt, @premium, @commission, @commission_percent

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @insurer_order_id <= @total_order_id
        BEGIN
            UPDATE #Renewal_List_With_options
            SET insurer_cnt = @party_cnt,
                insurer_code = 
                    (   
                        SELECT 
                            ISNULL(P.shortname,'') 
                        FROM party p
                        WHERE party_cnt = @party_cnt
                    ),    
                insurer_name = 
                    (
                        SELECT 
                            ISNULL(P.resolved_name,'') 
                        FROM party p
                        WHERE party_cnt = @party_cnt
                    ),    
                premium = @premium,
                commission = @Commission,
                commission_percent = @commission_percent
            WHERE order_id = @insurer_order_id 
            AND insurance_file_cnt = @Insurance_File_Cnt
        END
        ELSE
        BEGIN
            INSERT INTO #Renewal_List_With_options
            (
                branch_id,
                branch,
                order_id,   
                insurance_file_cnt,
                policy_ref,
                insurer_cnt,
                insurer_code,
                insurer_name,
                handler_cnt,
                handler_code,
                payment_method,
                cover_start_date,
                renewal_date,
                risk_cnt,
                risk_code,
                risk_desc,
                premium,
                commission,
                commission_percent
            )
            SELECT 
                branch_id,
                branch,
                @insurer_order_id,
                @Insurance_File_Cnt,
                policy_ref,
                @party_cnt,
                (   
                    SELECT 
                        ISNULL(P.shortname,'') 
                    FROM party p
                    WHERE party_cnt = @party_cnt
                ),    
                (   
                    SELECT 
                        ISNULL(P.resolved_name,'') 
                    FROM party p
                    WHERE party_cnt = @party_cnt
                ),    
                handler_cnt,
                handler_code,
                payment_method,
                cover_start_date,
                renewal_date,
                risk_cnt,
                risk_code,
                risk_desc,
                @premium,
                @commission,
                @commission_percent
            FROM #rlwo_temp
            WHERE insurance_file_cnt = @Insurance_File_Cnt 
        END

        SELECT @insurer_order_id = @insurer_order_id + 1
        FETCH NEXT FROM c_insurer INTO @party_cnt, @premium, @commission, @commission_percent

    END
    CLOSE c_insurer
    DEALLOCATE c_insurer
    /*End of Co-Insurer Part*/

    FETCH NEXT FROM c_rlwo into @Insurance_File_Cnt, @Client_Cnt
END

CLOSE c_rlwo
DEALLOCATE c_rlwo

UPDATE #Renewal_List_With_options
SET executive_cnt = 0, executive_code = ''
WHERE executive_cnt IS NULL

UPDATE #Renewal_List_With_options
SET handler_cnt = 0, handler_code = ''
WHERE handler_cnt IS NULL

UPDATE #Renewal_List_With_options
SET risk_cnt = 0, risk_desc = ''
WHERE risk_cnt IS NULL

UPDATE #Renewal_List_With_options
SET insurer_cnt = 0, insurer_code = '', insurer_name = ''
WHERE insurer_cnt IS NULL

UPDATE #Renewal_List_With_options
SET client_cnt = 0, client_code = '', client_name = ''
WHERE client_cnt IS NULL

IF @ExcRC = 1
BEGIN
    DELETE FROM #Renewal_List_With_options 
    WHERE risk_cnt = 0 
    AND EXISTS(SELECT NULL FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type ='RC' AND id=0)
END

IF @ExcAH = 1
BEGIN
    DELETE FROM #Renewal_List_With_options 
    WHERE handler_cnt = 0 
    AND EXISTS(SELECT NULL FROM Temp_Report_Exclude WHERE unique_report_name = @unique_report_name and type ='AH' AND id=0)
END

IF @ExcAE = 1
BEGIN
    DELETE #Renewal_List_With_options 
    FROM #Renewal_List_With_options 
    WHERE insurance_file_cnt in
    (
        SELECT 
            rlwo.insurance_file_cnt
        FROM #Renewal_List_With_options rlwo 
        JOIN 
            (
                SELECT rlwo2.insurance_file_cnt
                FROM #Renewal_List_With_options rlwo2  
                WHERE rlwo2.executive_cnt IN 
                    (
                        SELECT tp_ex.ID 
                        FROM Temp_Report_Exclude tp_ex 
                        WHERE  unique_report_name = @unique_report_name AND type='AE'
                    )
            ) as rlwo2
            ON rlwo.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE NOT((rlwo.executive_cnt = 0) OR LTRIM(RTRIM(rlwo.executive_cnt))='')
    GROUP BY rlwo.insurance_file_cnt
    HAVING COUNT(rlwo.insurance_file_cnt)=1)
END

IF @ExcIN = 1
BEGIN
    DELETE #Renewal_List_With_options 
    FROM #Renewal_List_With_options rlwo 
    WHERE insurance_file_cnt in
    (
    SELECT rlwo.insurance_file_cnt
    FROM #Renewal_List_With_options rlwo 
    JOIN 
        (
            SELECT rlwo2.insurance_file_cnt
            FROM #Renewal_List_With_options rlwo2  
            WHERE rlwo2.Insurer_Cnt  IN 
                (
                    SELECT tp_ex.ID 
                    FROM Temp_Report_Exclude tp_ex 
                    WHERE  unique_report_name = @unique_report_name AND type='IN'
                )
        ) as rlwo2
        ON rlwo.insurance_file_cnt = rlwo2.insurance_file_cnt
     WHERE NOT((rlwo.Insurer_Cnt = 0) OR LTRIM(RTRIM(rlwo.Insurer_Cnt))='')
    GROUP BY rlwo.insurance_file_cnt
    HAVING COUNT(rlwo.insurance_file_cnt)=1)
END

SELECT
    t.*,
    (
        CASE @group_by
            WHEN 'None' THEN
                ''
            WHEN '' THEN
                ''
            WHEN 'Account Executive' THEN
                t.executive_code
            WHEN 'Account Handler' THEN
                t.handler_code
            WHEN 'Branch' THEN
                (
                    SELECT code
                    FROM source
                    WHERE Source_id = t.branch_id
                )
            WHEN 'Client' THEN
                ISNULL((
                    SELECT top 1 'Multiple Clients'
                    FROM policy_shared_premiums PSP
                    WHERE PSP.insurance_file_cnt = t.insurance_file_cnt
                ), t.client_code)
            WHEN 'Insurer' THEN
                ISNULL((
                    SELECT top 1 'Multiple Insurers'
                    FROM policy_coinsurers PCO
                    WHERE PCO.insurance_file_cnt = t.insurance_file_cnt
                ), t.insurer_code)
            WHEN 'Risk' THEN
                t.risk_code
            WHEN 'Renewal Date' THEN
                ISNULL(CONVERT (varchar,renewal_date,103 ),'')
        END
    ) group_by_code,
    (
        CASE @group_by
            WHEN 'None' THEN
                ''
            WHEN '' THEN
                ''
            WHEN 'Account Executive' THEN
                ISNULL((
                    SELECT ISNULL(resolved_name, '')
                    FROM party 
                    WHERE party_cnt = t.executive_cnt
                ),'')
            WHEN 'Account Handler' THEN
                ISNULL((
                    SELECT ISNULL(resolved_name, '')
                    FROM party 
                    WHERE party_cnt = t.handler_cnt
                ),'')
            WHEN 'Branch' THEN
                t.branch
            WHEN 'Client' THEN
                ISNULL((
                    SELECT top 1 'Multiple Clients'
                    FROM policy_shared_premiums PSP
                    WHERE PSP.insurance_file_cnt = t.insurance_file_cnt
                ), t.Client_Name)
            WHEN 'Insurer' THEN
                ISNULL((
                    SELECT top 1 'Multiple Insurers'
                    FROM policy_coinsurers PCO
                    WHERE PCO.insurance_file_cnt = t.insurance_file_cnt
                ), t.Insurer_Name)
            WHEN 'Risk' THEN
                t.risk_desc
            WHEN 'Renewal Date' THEN
                ISNULL(CONVERT (varchar,renewal_date,103 ),'') 
        END
    ) group_by_desc,
    (
        CASE @then_by
            WHEN 'None' THEN
                ''
            WHEN '' THEN
                ''
            WHEN 'Account Executive' THEN
                t.executive_code
            WHEN 'Account Handler' THEN
                t.handler_code
            WHEN 'Branch' THEN
                (
                    SELECT code
                    FROM source
                    WHERE Source_id = t.branch_id
                )
            WHEN 'Client' THEN
                ISNULL((
                    SELECT top 1 'Multiple Clients'
                    FROM policy_shared_premiums PSP
                    WHERE PSP.insurance_file_cnt = t.insurance_file_cnt
                ), t.client_code)
            WHEN 'Insurer' THEN
                ISNULL((
                    SELECT top 1 'Multiple Insurers'
                    FROM policy_coinsurers PCO
                    WHERE PCO.insurance_file_cnt = t.insurance_file_cnt
                ), t.insurer_code)
            WHEN 'Risk' THEN
                t.risk_code
            WHEN 'Renewal Date' THEN
                ISNULL(CONVERT (varchar,renewal_date,103 ),'')
        END
    ) then_by_code,
    (
        CASE @then_by
            WHEN 'None' THEN
                ''
            WHEN '' THEN
                ''
            WHEN 'Account Executive' THEN
                ISNULL((
                    SELECT ISNULL(resolved_name, '')
                    FROM party 
                    WHERE party_cnt = t.executive_cnt
                ),'')
            WHEN 'Account Handler' THEN
                ISNULL((
                    SELECT ISNULL(resolved_name, '')
                    FROM party 
                    WHERE party_cnt = t.handler_cnt
                ),'')
            WHEN 'Branch' THEN
                t.branch
            WHEN 'Client' THEN
                ISNULL((
                    SELECT top 1 'Multiple Clients'
                    FROM policy_shared_premiums PSP
                    WHERE PSP.insurance_file_cnt = t.insurance_file_cnt
                ), t.Client_Name)
            WHEN 'Insurer' THEN
                ISNULL((
                    SELECT top 1 'Multiple Insurers'
                    FROM policy_coinsurers PCO
                    WHERE PCO.insurance_file_cnt = t.insurance_file_cnt
                ), t.Insurer_Name)
            WHEN 'Risk' THEN
                t.risk_desc
            WHEN 'Renewal Date' THEN
                ISNULL(CONVERT (varchar,renewal_date,103 ),'') 
        END
    ) then_by_desc 

INTO #rlwo_Report_Temp
FROM #Renewal_List_With_options t 


IF @group_by = 'Account Executive' 
BEGIN
    UPDATE #rlwo_Report_Temp
    SET group_by_code = 'No Executives',
        group_by_desc = 'No Executives' 
    FROM #rlwo_Report_Temp rlwo1
    WHERE group_by_code = '' 
    AND 1 = (
             SELECT 
             COUNT(insurance_file_cnt) 
             FROM #rlwo_Report_Temp rlwo2 
             WHERE rlwo2.insurance_file_cnt = rlwo1.insurance_file_cnt
            )

    UPDATE #rlwo_Report_Temp
    SET group_by_code = 'Multiple Executives', 
        group_by_desc = 'Multiple Executives' 
    FROM #rlwo_Report_Temp rlwo1
    WHERE group_by_code = '' 
    AND 0 = (
             SELECT 
             COUNT(executive_cnt) 
             FROM #rlwo_Report_Temp rlwo3 
             WHERE rlwo3.insurance_file_cnt = rlwo1.insurance_file_cnt AND executive_cnt <> 0
        )

    UPDATE #rlwo_Report_Temp
    SET group_by_code = rlwo2.executive_code 
    FROM #rlwo_Report_Temp
    Left JOIN #rlwo_Report_Temp rlwo2 
    ON #rlwo_Report_Temp.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE #rlwo_Report_Temp.group_by_code = '' 
    AND rlwo2.order_id = 1

    UPDATE #rlwo_Report_Temp
    SET group_by_code = rlwo2.executive_code
    FROM #rlwo_Report_Temp
    Left JOIN #rlwo_Report_Temp rlwo2 
    ON #rlwo_Report_Temp.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE #rlwo_Report_Temp.group_by_code = '' 
    AND rlwo2.executive_code <> ''

    UPDATE #rlwo_Report_Temp
    SET group_by_desc = ISNULL((
                                SELECT ISNULL(resolved_name, shortname)
                                FROM party 
                                WHERE shortname = group_by_code
                              ),'')
    WHERE group_by_desc = ''

END

IF @then_by = 'Account Executive'
BEGIN
    UPDATE #rlwo_Report_Temp
    SET then_by_code = 'No Executives',
        then_by_desc = 'No Executives' 
    FROM #rlwo_Report_Temp rlwo1
    WHERE then_by_code = '' 
    AND 1 = (
             SELECT 
             COUNT(insurance_file_cnt) 
             FROM #rlwo_Report_Temp rlwo2 
             WHERE rlwo2.insurance_file_cnt = rlwo1.insurance_file_cnt
            )

    UPDATE #rlwo_Report_Temp
    SET then_by_code = 'Multiple Executives', 
        then_by_desc = 'Multiple Executives' 
    FROM #rlwo_Report_Temp rlwo1
    WHERE then_by_code = '' 
    AND 0 = (
             SELECT 
             COUNT(executive_cnt) 
             FROM #rlwo_Report_Temp rlwo3 
             WHERE rlwo3.insurance_file_cnt = rlwo1.insurance_file_cnt AND executive_cnt <> 0
        )

    UPDATE #rlwo_Report_Temp
    SET then_by_code = rlwo2.executive_code
    FROM #rlwo_Report_Temp
    Left JOIN #rlwo_Report_Temp rlwo2 
    ON #rlwo_Report_Temp.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE #rlwo_Report_Temp.then_by_code = '' 
    AND rlwo2.order_id = 1

    UPDATE #rlwo_Report_Temp
    SET then_by_code = rlwo2.executive_code
    FROM #rlwo_Report_Temp
    Left JOIN #rlwo_Report_Temp rlwo2 
    ON #rlwo_Report_Temp.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE #rlwo_Report_Temp.then_by_code = '' 
    AND rlwo2.executive_code <> ''

    UPDATE #rlwo_Report_Temp
    SET then_by_desc = ISNULL((
                                SELECT ISNULL(resolved_name, shortname)
                                FROM party 
                                WHERE shortname = then_by_code
                              ),'')
    WHERE then_by_desc = ''

END

IF @group_by = 'Client'
BEGIN
    /*Updating groupby when multi insurer and one client on a policy*/
    UPDATE #rlwo_Report_Temp
    SET group_by_code = rlwo2.client_code,
        group_by_desc = rlwo2.client_name 
    FROM #rlwo_Report_Temp
    Left JOIN #rlwo_Report_Temp rlwo2 
    ON #rlwo_Report_Temp.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE #rlwo_Report_Temp.group_by_code = '' 
    AND rlwo2.order_id = 1  
END

IF @then_by = 'Client'
BEGIN
    /*Updating groupby when multi insurer and one client on a policy*/
    UPDATE #rlwo_Report_Temp
    SET then_by_code = rlwo2.client_code,
        then_by_desc = rlwo2.client_name 
    FROM #rlwo_Report_Temp
    Left JOIN #rlwo_Report_Temp rlwo2 
    ON #rlwo_Report_Temp.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE #rlwo_Report_Temp.then_by_code = '' 
    AND rlwo2.order_id = 1  
END

IF @group_by = 'Insurer'
BEGIN
    /*Updating groupby when one insurer and multi clients on a policy*/
    UPDATE #rlwo_Report_Temp
    SET group_by_code = rlwo2.insurer_code,
        group_by_desc = rlwo2.insurer_name 
    FROM #rlwo_Report_Temp
    Left JOIN #rlwo_Report_Temp rlwo2 
    ON #rlwo_Report_Temp.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE #rlwo_Report_Temp.group_by_code = '' 
    AND rlwo2.insurer_code <> ''
END

IF @then_by = 'Insurer'
BEGIN
    /*Updating groupby when one insurer and multi clients on a policy*/
    UPDATE #rlwo_Report_Temp
    SET then_by_code = rlwo2.insurer_code,
        then_by_desc = rlwo2.insurer_name 
    FROM #rlwo_Report_Temp
    Left JOIN #rlwo_Report_Temp rlwo2 
    ON #rlwo_Report_Temp.insurance_file_cnt = rlwo2.insurance_file_cnt
    WHERE #rlwo_Report_Temp.then_by_code = '' 
    AND rlwo2.insurer_code <> ''
END

IF @group_by = 'Account Handler' 
BEGIN
    UPDATE #rlwo_Report_Temp
    SET group_by_code = 'No Handler',
        group_by_desc = 'No Handler' 
    FROM #rlwo_Report_Temp
    WHERE group_by_code = ''
END

IF @then_by = 'Account Handler' 
BEGIN
    UPDATE #rlwo_Report_Temp
    SET then_by_code = 'No Handler',
        then_by_desc = 'No Handler' 
    FROM #rlwo_Report_Temp
    WHERE then_by_code = ''
END

SELECT * 
FROM #rlwo_Report_Temp 
ORDER BY group_by_code, then_by_code, renewal_date, insurance_file_cnt, order_id

DROP TABLE #Renewal_List_With_options
DROP TABLE #rlwo_temp
DROP TABLE #rlwo_Report_Temp
SET NOCOUNT OFF
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

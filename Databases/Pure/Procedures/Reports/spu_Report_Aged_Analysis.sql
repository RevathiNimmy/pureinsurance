SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Aged_Analysis'
GO

/*
EXEC spu_Report_Aged_Analysis 'Oct  8 2006 11:59:59:000PM',  'Client',0, 'base','Transaction Date', 'Including', '','', 'File Code'
*/
CREATE PROCEDURE spu_Report_Aged_Analysis
    @end_date DATETIME,
    @statement_type VARCHAR(30),
    @branch_id INT,
    @typeofcurrency VARCHAR(15),
    @date_type VARCHAR(20),
    @sincludeinstalments VARCHAR(20) = '',
    @account_exec VARCHAR(20),
    @unallocatedOnly VARCHAR(10),
    @group_by VARCHAR(255),
    @report_option VARCHAR(255)
AS

DECLARE
    @sStatementType CHAR(2),
    @IncludeInstalments INT,
    @ledger_min INT,
    @ledger_max INT,
    @multi_company_id INT

SET NOCOUNT ON

SELECT @IncludeInstalments = 0
IF @sIncludeInstalments = 'Including'
BEGIN
    SELECT @IncludeInstalments = 1
END

/*Check the input parameters.*/
IF @account_exec = 'ALL' OR @account_exec = '' OR @account_exec IS NULL 
BEGIN
    SELECT @account_exec = NULL
END

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF ISNULL(@TypeOfCurrency,'') = ''
BEGIN
    SELECT @TypeOfCurrency = 'Base'
END

SELECT @end_date = ISNULL(@end_date, GETDATE())

SELECT @statement_type = RTRIM(@statement_type)
SELECT @date_type = RTRIM(@date_type)

SELECT @sStatementType = 
    (
        CASE
            WHEN @statement_type = 'client' THEN
                'C '
            WHEN @statement_type = 'insurer' THEN
                'I '
            WHEN @statement_type = 'extras' THEN
                'E '
            WHEN @statement_type = 'agent' THEN
                'A '
            WHEN @statement_type = 'sub agent' THEN
                'SA'
            WHEN @statement_type = 'introducer' THEN
                'T '
            WHEN @statement_type = 'fees and discounts' THEN
                'F '
            WHEN @statement_type = 'discounts' THEN
                'DI'    
            WHEN @statement_type = 'purchase creditors' THEN
                'PU'
            WHEN @Statement_type = 'premium finance provider' THEN 
                'RF'
            ELSE
                'C '
        END
    )

SELECT @multi_company_id = 0

SELECT 
    @multi_company_id = value
FROM Hidden_options
WHERE option_number=16 
AND branch_id=1

IF @multi_company_id = 1
BEGIN
    IF @branch_id = 0 
    BEGIN
        SELECT @multi_company_id = 1
        SELECT @branch_id = 1
    END
    ELSE 
    BEGIN
        SELECT @multi_company_id = @branch_id
    END
END
ELSE
BEGIN
    SELECT @multi_company_id = 1
END

/*get the ledger ids first for performance*/
SELECT @ledger_min = 0

IF @sStatementType = 'F '
BEGIN
    SELECT
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'FE'
    AND company_id = @multi_company_id

    SELECT 
        @ledger_max = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'CO'
    AND company_id = @multi_company_id
END

IF @sStatementType = 'C '
BEGIN
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'SA'
    AND company_id = @multi_company_id

    SELECT @ledger_max = @ledger_min
END

IF @sStatementType = 'I ' OR @sStatementType = 'E '
BEGIN   
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'IN'
    AND company_id = @multi_company_id

    SELECT @ledger_max = @ledger_min
END 

IF @sStatementType = 'A '
BEGIN
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'AG'
    AND company_id = @multi_company_id

    SELECT @ledger_max = @ledger_min
END 

IF @sStatementType = 'SA'
BEGIN
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'UB'
    AND company_id = @multi_company_id


    SELECT @ledger_max = @ledger_min
END 

IF @sStatementType = 'T '
BEGIN
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'TR'
    AND company_id = @multi_company_id

    SELECT @ledger_max = @ledger_min
END 

IF @sStatementType = 'PU'
BEGIN
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'PU'
    AND company_id = @multi_company_id

    SELECT @ledger_max = @ledger_min
END 

IF @sStatementType = 'RF'
BEGIN
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'RF'
    AND company_id = @multi_company_id

    SELECT @ledger_max = @ledger_min
END

IF @sStatementType = 'DI'
BEGIN
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'DI'
    AND company_id = @multi_company_id

    SELECT @ledger_max = @ledger_min
END

IF @ledger_min = 0
BEGIN    
    SELECT 
        @ledger_min = ledger_id 
    FROM ledger 
    WHERE ledger_short_name = 'SA'
    AND company_id = @multi_company_id

    SELECT @ledger_max = @ledger_min
END 

/*Create the transaction table*/
CREATE TABLE #Report_Transaction
(
    /*Transaction Details*/
    document_id INT,
    document_date DATETIME,
    documenttype_id INT,   
    transdetail_id INT,
    effective_date DATETIME,
    currency_id INT,
    currency_code CHAR(4),
    document_ref VARCHAR(25),

    /*Transaction Amounts*/
    gross_premium MONEY,
    base_match_amount MONEY,
    unallocated_amount MONEY,
    account_total MONEY,
    match_total MONEY,
    Account_Total_CR MONEY,
    Account_Total_DB MONEY,
    Unpaid_On_Finance MONEY,
    Is_Debit BIT,
    unallocated_detail_amount MONEY,

    /*Account Details*/
    account_id INT,
    account_code CHAR(30),
    account_name VARCHAR(100),
    phone_area_code VARCHAR(10),
    phone_number VARCHAR(15),

    /*Other*/
    number_of_days INT,
    filecode VARCHAR (100),
    Group_By_File_Code VARCHAR (100),
    Group_By_Account_Exec VARCHAR (100),
    Group_By_Business_Type VARCHAR (100),
    IsCash INT,
    date_paid DATETIME,

    /* Aged Balance List Day Ranges*/
    start_day_1 INT,
    end_day_1 INT,
    start_day_2 INT,
    end_day_2 INT,
    start_day_3 INT,
    end_day_3 INT,
    start_day_4 INT,
    end_day_4 INT,
    start_day_5 INT,
    end_day_5 INT,
    start_day_6 INT,
    end_day_6 INT,
    start_day_7 INT,
    Group_By_Account_Handler VARCHAR (100)
)
CREATE INDEX I__#Report_Transaction__transdetail_id ON #Report_Transaction (transdetail_id)
CREATE INDEX I__#Report_Transaction__account_id__transdetail_id ON #Report_Transaction (account_id, transdetail_id)
CREATE INDEX I__#Report_Transaction__account_id__currency_id ON #Report_Transaction (account_id, currency_id)
CREATE INDEX I__#Report_Transaction__account_id__currency_id__document_id ON #Report_Transaction (account_id, currency_id, document_id)

/*Get the required transactions*/
INSERT INTO #Report_Transaction 
(
    transdetail_id,
    currency_id,
    currency_code,
    account_id,
    account_code,
    account_name,
    documenttype_id,
    document_date,
    effective_date,
    number_of_days,
    gross_premium,
    document_id,
    document_ref,
    phone_area_code,
    phone_number,
    IsCash
)
SELECT
    TD.transdetail_id,
    CASE @TypeOfCurrency
        WHEN 'Base' THEN CS.currency_id
        WHEN 'Transaction' THEN CTD.currency_id
    END,
    CASE @TypeOfCurrency
        WHEN 'Base' THEN CS.iso_code
        WHEN 'Transaction' THEN CTD.iso_code
    END,
    A.account_id,
    A.short_code,
    A.account_name,
    D.documenttype_id,
    D.document_date,
    TD.ref_date,
    /*Calculate the days delay taking into account adjustments.*/
    CASE 
        WHEN @date_type = 'effective date' 
        THEN DATEDIFF(DAY, 
                (
                    SELECT
                        ref_date
                    FROM transdetail
                    WHERE document_id = td.document_id
                    AND document_sequence = 1
                )
            , @end_date)
            
        WHEN 
            (
                @date_type = 'Transaction Date'
                AND
                ISNULL(TT.code, '') NOT IN ('COMMADJ', 'AGENTADJ', 'BROK ADJ')
            ) 
        THEN 
            DATEDIFF(DAY, D.document_date, @end_date)
            
        WHEN 
            (
                @date_type = 'Transaction Date'
                AND
                ISNULL(TT.code, '') IN ('COMMADJ', 'AGENTADJ', 'BROK ADJ')
            ) 
        THEN 
            DATEDIFF(DAY, TD.ref_date, @end_date)
            
    END,
    CASE @TypeOfCurrency
        WHEN 'Base' THEN ROUND(TD.Amount,2)
        WHEN 'Transaction' THEN ROUND(TD.currency_amount,2)
    END,
    D.document_id,
    D.document_ref,
    A.phone_area_code,
    A.phone_number,
    CASE 
        WHEN D.documenttype_id IN (22, 23, 28, 29) THEN 
            1
        ELSE 
            0
    END
FROM Account A
JOIN TransDetail TD
    ON TD.account_id = A.account_id
LEFT JOIN transdetail_type TT
    ON TT.transdetail_type_id = TD.transdetail_type_id
JOIN Document D
    ON D.document_id = TD.document_id
JOIN Source S
    ON S.source_id = D.company_id
JOIN Currency CTD
    ON CTD.currency_id = TD.currency_id
JOIN Currency CS
    ON CS.currency_id = S.base_currency_id
WHERE A.ledger_id BETWEEN @ledger_min AND @ledger_max
AND D.company_id = ISNULL(@branch_id, D.company_id)

/*Remove any transaction lines from the instalment suspence account*/ 
IF @sStatementType = 'RF'
BEGIN
    DELETE #Report_Transaction
    FROM #Report_Transaction RT
    JOIN Account A 
        ON A.account_id = RT.account_id
    WHERE a.short_code = 'ISUSP'
END

/*If reporting on Insurer accounts then remove Extras (they are linked to same ledger)*/
IF @sStatementType = 'I '
BEGIN
    DELETE #Report_Transaction
    FROM #Report_Transaction RT
    JOIN Account A 
        ON A.account_id = RT.account_id 
    JOIN Party P 
        ON P.party_cnt = A.account_key
    JOIN Party_Type PT 
        ON PT.party_type_id = P.party_type_id
    WHERE PT.code = 'EX'
END

/*If reporting on Extra accounts then remove Insurers (they are linked to same ledger)*/
IF @sStatementType = 'E '
BEGIN
    DELETE #Report_Transaction
    FROM #Report_Transaction RT
    JOIN Account A 
        ON A.account_id = RT.account_id
    JOIN Party P 
        ON P.party_cnt =  A.account_key
    JOIN Party_Type PT 
        ON PT.party_type_id = P.party_type_id
    WHERE PT.code = 'IN'
END

IF @IncludeInstalments = 0 AND @sStatementType NOT IN ('I ', 'E ', 'A ', 'SA', 'T ')
BEGIN
    DELETE #Report_Transaction
    FROM #Report_Transaction RT
    JOIN transdetail TD
        ON TD.transdetail_id = RT.transdetail_id
    JOIN document D
        ON D.document_id = td.document_id
    JOIN documenttype DT
        ON DT.documenttype_id = D.documenttype_id
    WHERE DT.Code IN ('IDR','ICR','INC','IND')
        
    /*Use temporary table for massive speed boost*/
    CREATE TABLE #Instalment_Allocations
    (
        allocation_id INT
    )
    CREATE INDEX I__#Instalment_Allocations__allocation_id ON #Instalment_Allocations (allocation_id)

    INSERT INTO #Instalment_Allocations
    SELECT
        AD.allocation_id
    FROM allocationdetail AD
    JOIN transmatch TM
        ON TM.allocationdetail_id = AD.allocationdetail_id
        AND TM.transdetail_id = AD.transdetail_id
        AND ISNULL(TM.is_reversed, 0) = 0
    JOIN transdetail TD
        ON TD.transdetail_id = AD.transdetail_id
    JOIN document D
        ON D.document_id = td.document_id
    JOIN documenttype DT
        ON DT.documenttype_id = D.documenttype_id
    WHERE DT.Code IN ('IDR','ICR','INC','IND')
        
    DELETE #Report_Transaction
    FROM #Report_Transaction RT
    JOIN allocationdetail AD
        ON AD.transdetail_id = RT.transdetail_id
    JOIN #Instalment_Allocations IA
        ON IA.allocation_id = AD.allocation_id
    
    DROP TABLE #Instalment_Allocations
        
END

/*Use temporary table for massive speed boost*/
CREATE TABLE #Future_Allocations
(
    allocation_id INT
)
CREATE INDEX I__#Future_Allocations__allocation_id ON #Future_Allocations (allocation_id)

INSERT INTO #Future_Allocations
SELECT
    ad.allocation_id
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
JOIN allocationdetail ad
    ON ad.transdetail_id = td.transdetail_id
WHERE d.document_date > @end_date

/*Get total allocated for each transaction*/
UPDATE rt
SET rt.base_match_amount = 
    (       
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN ISNULL(SUM(ROUND(tm.base_match_amount,2)),0)
                WHEN 'Transaction' THEN ISNULL(SUM(ROUND(tm.currency_match_amount,2)),0)
            END
        FROM transmatch tm
        JOIN matchgroup mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @end_date
        JOIN allocationdetail ad
            ON ad.allocationdetail_id = tm.allocationdetail_id
        WHERE tm.transdetail_id = rt.transdetail_id
        AND NOT EXISTS /*Don't include matches containing future transactions*/
            (
                SELECT 
                    NULL
                FROM #Future_Allocations
                WHERE allocation_id = ad.allocation_id
            )
    )
FROM #Report_Transaction rt

DROP TABLE #Future_Allocations

UPDATE RT  
SET RT.Group_By_Account_Handler=  
        (  
	SELECT top 1 ISNULL( AccH.resolved_name, 'No Account Handler')  
	        FROM  Document D  
	        LEFT JOIN  insurance_file IFI  
	        ON IFI.insurance_file_cnt = D.insurance_file_cnt  
	        LEFT JOIN Party AccH  
	        ON AccH.party_cnt = IFI.account_handler_cnt  
		WHERE D.document_id = RT.document_id
        )
FROM #Report_Transaction rt


/*Calculate Totals*/
IF @group_by='Account Handler'
BEGIN 
	UPDATE RT  
	SET RT.account_total =  
	        (  
		    SELECT  
	                ISNULL(SUM(ROUND(gross_premium,2) - ISNULL(base_match_amount,0)),0)  
	            FROM #Report_Transaction  
	            WHERE account_id = RT.account_id  
	            AND currency_id = RT.currency_id
		    And Group_By_Account_Handler= RT.Group_By_Account_Handler	  
	            AND number_of_days >= 0
		    Group By
			Group_By_Account_Handler 	
		)
	FROM #Report_Transaction RT  
END
ELSE
BEGIN
	UPDATE RT  
	SET RT.account_total =  
		(
		    SELECT  
	                ISNULL(SUM(ROUND(gross_premium,2) - ISNULL(base_match_amount,0)),0)  
	            FROM #Report_Transaction  
	            WHERE account_id = RT.account_id  
	            AND currency_id = RT.currency_id
		    AND number_of_days >= 0
	        ) /*account_total*/  
	FROM #Report_Transaction RT  
END
/*account_total*/


/*Calculate Totals*/
UPDATE RT
SET RT.unallocated_amount = 
        (
            SELECT 
                ISNULL(SUM(ROUND(gross_premium,2) - ISNULL(base_match_amount,0)),0)
            FROM #Report_Transaction
            WHERE account_id = RT.account_id
            AND currency_id = RT.currency_id
            AND documenttype_id IN (22, 23, 28, 29)
            AND number_of_days >= 0
        ), /*unallocated_cash*/
    RT.Unpaid_On_Finance = 
        (
            SELECT
                CASE @TypeOfCurrency
                    WHEN 'Base' THEN ISNULL(SUM(ROUND(td.amount,2)),0)
                    WHEN 'Transaction' THEN ISNULL(SUM(ROUND(td.currency_amount,2)),0)
                END
            FROM account a
            JOIN party p 
                ON p.party_cnt = a.account_key
            JOIN pfpremiumfinance pfpf 
                ON pfpf.clientid = p.party_cnt
            JOIN transdetail td 
                ON td.transdetail_id = pfpf.plantransaction_id
            JOIN document d 
                ON d.document_id = td.document_id
                AND d.documenttype_id = 1
                AND (
                        (
                            @date_type = 'Transaction Date'
                            AND 
                            d.document_date <= @end_date 
                        )
                        OR  
                        (
                            @date_type = 'Effective Date'
                            AND 
                            td.ref_date <= @end_date
                        )
                    )  
            WHERE a.account_id = RT.account_id
            AND td.currency_id = RT.currency_id
        )
        -
        (
            SELECT
                CASE @TypeOfCurrency
                    WHEN 'Base' THEN ISNULL(SUM(ROUND(tm.base_match_amount,2)),0)
                    WHEN 'Transaction' THEN ISNULL(SUM(ROUND(tm.currency_match_amount,2)),0)
                END
            FROM account a
            JOIN party p 
                ON p.party_cnt = a.account_key
            JOIN pfpremiumfinance pfpf 
                ON pfpf.clientid = p.party_cnt
            JOIN transdetail td 
                ON td.transdetail_id = pfpf.plantransaction_id
            JOIN document d 
                ON d.document_id = td.document_id
                AND d.documenttype_id = 1
                AND (
                        (
                            @date_type = 'Transaction Date'
                            AND 
                            d.document_date <= @end_date 
                        )
                        OR  
                        (
                            @date_type = 'Effective Date'
                            AND 
                            td.ref_date <= @end_date
                        )
                    )  
            JOIN transmatch tm
                ON tm.transdetail_id = td.transdetail_id
            WHERE a.account_id = RT.account_id     
            AND td.currency_id = RT.currency_id
        ), /*unpaid finance*/
    RT.unallocated_detail_amount=
        (
            SELECT 
                ISNULL(ROUND(gross_premium,2) - ISNULL(base_match_amount,0),0)
            FROM #Report_Transaction
            WHERE transdetail_id = RT.transdetail_id
            AND currency_id = RT.currency_id
            AND documenttype_id IN (22, 23, 28, 29)
        ) /*unallocated_cash*/

FROM #Report_Transaction RT

/*Default all transactions to credits*/
UPDATE RT
SET RT.Is_Debit = 0
FROM #Report_Transaction RT

/*Set to debit for all debits*/
UPDATE RT
SET RT.Is_Debit = 1
FROM #Report_Transaction RT
WHERE EXISTS
    (
        SELECT
            NULL
        FROM #Report_Transaction
        WHERE document_id = RT.document_id
        AND account_id = RT.account_id
        AND currency_id = RT.currency_id
        HAVING SUM(gross_premium - base_match_amount) > 0
    )

/*Update Totals for debits and credits*/
UPDATE RT
SET RT.Account_Total_CR = 
        (
            SELECT 
                ISNULL(SUM(RT2.gross_premium - base_match_amount),0)
            FROM #Report_Transaction RT2
            WHERE RT2.account_id = RT.account_id
            AND RT2.currency_id = RT.currency_id
            AND RT2.Is_Debit = 1
            AND number_of_days >= 0
        ), /*Account_Total_CR*/
    RT.Account_Total_DB = 
        (
            SELECT 
                ISNULL(SUM(RT2.gross_premium - base_match_amount),0)
            FROM #Report_Transaction RT2
            WHERE RT2.account_id = RT.account_id
            AND RT2.currency_id = RT.currency_id
            AND RT2.Is_Debit = 0
            AND number_of_days >= 0
        ) /*Account_Total_DB*/
FROM #Report_Transaction RT


IF @UnallocatedOnly = 'True'
BEGIN
    /*Remove transactions whose account has no unallocated cash*/
    DELETE
    FROM #Report_Transaction
    WHERE unallocated_amount = 0
END
ELSE
BEGIN
    /*Remove transactions whose account has a balance of zero*/
    IF @report_option = 'Unpaid Finance'
    BEGIN
        /*but only if there is no unpaid financed value as well PN23103 */
        DELETE #Report_Transaction
        FROM #Report_Transaction RT
        WHERE RT.account_total = 0
        AND RT.Unpaid_On_Finance = 0
		AND NOT EXISTS
            (
                SELECT 
                    NULL
                FROM #Report_Transaction
                WHERE account_id = RT.account_id
                AND currency_id = RT.currency_id
                AND number_of_days < 0
		    	HAVING ISNULL(SUM(ROUND(gross_premium,2) - ISNULL(base_match_amount,0)),0) <> 0
            )
    END
    ELSE
    BEGIN
        DELETE #Report_Transaction
        FROM #Report_Transaction RT
        WHERE RT.account_total = 0    
		AND NOT EXISTS
            (
                SELECT 
                    NULL
                FROM #Report_Transaction
                WHERE account_id = RT.account_id
                AND currency_id = RT.currency_id
                AND number_of_days < 0
		    	HAVING ISNULL(SUM(ROUND(gross_premium,2) - ISNULL(base_match_amount,0)),0) <> 0
            )
    END
END

/*Remove all future dated unallocated cash*/
DELETE
FROM #Report_Transaction
WHERE unallocated_detail_amount <> 0
AND number_of_days < 0

/*Update transactions with file code and base match amount*/
UPDATE rt
SET filecode = ISNULL(P.file_code,''),
    Group_By_File_Code = ISNULL(P.file_code, 'No File Code')
FROM #Report_Transaction RT
JOIN account A
    ON A.account_id = RT.account_id
LEFT JOIN party P
    ON P.party_cnt = A.account_key

/*Update working table with account exec from client*/
UPDATE RT
SET Group_By_Account_Exec = ISNULL(AEP.shortname, 'No Account Exec')
FROM #Report_Transaction RT
JOIN account A
    ON A.account_id = RT.account_id
LEFT JOIN Party P
        JOIN Party AEP
            ON AEP.Party_cnt = P.consultant_cnt 
     ON P.party_cnt = A.account_key

/*Delete transactions whose account has a different account exec than the one selected*/
IF @account_exec IS NOT NULL
BEGIN
    DELETE
    FROM #Report_Transaction 
    WHERE Group_By_Account_Exec <> @account_exec      
END     

/*Update working table with business type*/
UPDATE RT
SET Group_By_Business_Type = ISNULL(CONVERT(VARCHAR,BT.code), 'No Business Type')
FROM #Report_Transaction RT
JOIN transdetail TD
    ON TD.transdetail_id = RT.transdetail_id
JOIN document D
    ON D.document_id = TD.document_id
LEFT OUTER JOIN insurance_file I
        JOIN Business_Type BT
            ON BT.business_type_id = I.business_type_id
    ON I.insurance_file_cnt = D.insurance_file_cnt

/*A few miscellaneous updates*/
UPDATE RT
SET RT.date_paid = 
    (       
        SELECT 
            MAX(MG.match_date)
        FROM TransMatch TM
        JOIN MatchGroup MG
            ON MG.match_id = TM.match_id
            AND MG.match_date <= @end_date
        WHERE TM.transdetail_id = RT.transdetail_id
        AND TM.base_match_amount <> 0
        AND TM.allocationdetail_id IS NOT NULL
    )
FROM #Report_Transaction RT
JOIN Account A
    ON RT.account_id = A.account_id
JOIN DocumentType DT
    ON RT.documenttype_id = DT.documenttype_id

/*Inserting  Aged Balance List Day Ranges From System_options*/
UPDATE #Report_Transaction 
SET start_day_1 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3010 AND branch_id = ISNULL(@branch_id,1)),
    end_day_1 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3011 AND branch_id = ISNULL(@branch_id,1)),
    start_day_2 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3012 AND branch_id = ISNULL(@branch_id,1)),
    end_day_2 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3013 AND branch_id = ISNULL(@branch_id,1)),
    start_day_3 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3014 AND branch_id = ISNULL(@branch_id,1)),
    end_day_3 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3015 AND branch_id = ISNULL(@branch_id,1)),
    start_day_4 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3016 AND branch_id = ISNULL(@branch_id,1)),
    end_day_4 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3017 AND branch_id = ISNULL(@branch_id,1)),
    start_day_5 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3018 AND branch_id = ISNULL(@branch_id,1)),
    end_day_5 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3019 AND branch_id = ISNULL(@branch_id,1)),
    start_day_6 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3020 AND branch_id = ISNULL(@branch_id,1)),
    end_day_6 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3021 AND branch_id = ISNULL(@branch_id,1)),
    start_day_7 = (SELECT ISNULL(value,0) FROM system_options WHERE option_number = 3022 AND branch_id = ISNULL(@branch_id,1))

/*Extract the data*/
SET NOCOUNT OFF

SELECT 
    document_ref,
    transdetail_id,
    account_code,
    account_name,
    phone_area_code,
    phone_number,
    gross_premium,
    base_match_amount,
    unallocated_amount,
    unallocated_detail_amount,
    account_total,
    match_total,
    date_paid,
    number_of_days,
    filecode,
    Group_By_File_Code,    
    Group_By_Account_Exec,    
    Group_By_Business_Type,    
    Account_Total_CR,
    Account_Total_DB,
    IsCash,
    Unpaid_On_Finance,
    currency_id,
    currency_code,
    start_day_1,
    end_day_1,
    start_day_2,
    end_day_2,
    start_day_3,
    end_day_3,
    start_day_4,
    end_day_4,
    start_day_5,
    end_day_5,
    start_day_6,
    end_day_6,
    start_day_7,
    (
    CASE @group_by
         WHEN 'None' THEN
            ''
         WHEN 'Business Type' THEN
        Group_By_Business_Type
     WHEN 'File Code' THEN
        filecode
         WHEN 'Account Executive' THEN
        Group_By_Account_Exec
     WHEN 'Account Handler' THEN
       Group_By_Account_Handler
       /*(
       SELECT TOP 1 ISNULL( AccH.resolved_name, 'No Account Handler')
       FROM  Document D 
       LEFT JOIN  insurance_file IFI
       ON IFI.insurance_file_cnt = D.insurance_file_cnt
       LEFT JOIN Party AccH
       ON AccH.party_cnt = IFI.account_handler_cnt 
       WHERE D.document_id = #Report_Transaction.document_id
   	)*/
    END
) 'group_code'

FROM #Report_Transaction

SET NOCOUNT ON

DROP TABLE #Report_Transaction
    
GO

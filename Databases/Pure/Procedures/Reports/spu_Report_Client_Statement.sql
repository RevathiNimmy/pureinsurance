/*
This stored procedure is used by the following reports:

Client_Statement_of_Accounts.rpt
Client_Statement_With_Options.rpt
Client_Statement_By_PartyCnt.rpt
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_Statement'
GO

CREATE PROCEDURE spu_Report_Client_Statement
    @start_date DATETIME,
    @end_date DATETIME,
    @branch_id INT,
    @date_type VARCHAR(30)=NULL,
    --@statement_type VARCHAR(20),
    @party_code VARCHAR(20),
    @is_print_settled_Item VARCHAR(5),
    @reminder_type VARCHAR(100),
    @account_exec VARCHAR(20) = NULL,
    @party_cnt INT = NULL,
    @insurance_file_cnt INT = NULL,
    @TypeOfCurrency VARCHAR(15),
    @group_by  VARCHAR(30)
    
AS

DECLARE @ledger_short_name VARCHAR(2)
DECLARE @account_id INT
DECLARE @account_name VARCHAR(255)
DECLARE @system_option_3 VARCHAR(255)
DECLARE @account_total MONEY
DECLARE @multi_ledger BIT
DECLARE @currency_id INT
DECLARE @currency_code VARCHAR(20)
DECLARE @end_date_quick DATETIME
    

SET NOCOUNT ON

SET ANSI_WARNINGS OFF


IF ISNULL(@party_code,'') = '' AND @party_cnt IS NOT NULL 
BEGIN
    SELECT @party_code = (SELECT shortname FROM party WHERE party_cnt = @party_cnt)
END 

IF @party_code = 'ALL' OR @party_code = '' 
BEGIN
    SELECT @party_code = NULL
END

IF @account_exec = 'ALL' OR @account_exec = '' 
BEGIN
    SELECT @account_exec = NULL
END

IF @reminder_type = 'ALL' OR @reminder_type = '' 
BEGIN
    SELECT @reminder_type = NULL
END

/*If following date is passed in then trying to update @end_date takes a long time so use @end_date_quick to hold the end date*/
IF @end_date = '1899-12-30 23:59:59'
BEGIN
    SELECT @end_date = NULL
END
SELECT @end_date_quick = ISNULL(@end_date, GETDATE())

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF ISNULL(@TypeOfCurrency,'') = ''
BEGIN
    SELECT @TypeOfCurrency = 'Base'
END 

SELECT @ledger_short_name = 'SA'

SELECT @multi_ledger = 0

SELECT @multi_ledger = 1
WHERE EXISTS    
    (
        SELECT NULL
        FROM hidden_options
        WHERE branch_id = 1
        AND option_number = 16
        AND value = 1
    )
    
    
SELECT
    @system_option_3 = ISNULL(value,'0')
FROM system_options
WHERE option_number = 3
AND 
(
    (
        @multi_ledger = 1
        AND 
        branch_id = ISNULL(@branch_id,1)
    )
    OR
    (
        @multi_ledger = 0
        AND 
        branch_id = 1
    )
)

IF @insurance_file_cnt = 0
BEGIN
    SELECT @insurance_file_cnt = NULL
END

IF @date_type =''  OR @date_type=NULL
BEGIN
   SELECT @date_type='Transaction Date'
END

SELECT 
    @currency_id = c.currency_id,
    @currency_code = c.code
FROM source s
JOIN currency c
    ON c.currency_id = s.base_currency_id
WHERE source_id = 1

/*Generate Temporary Table*/
CREATE TABLE #Report_Transaction
(
    transdetail_id INT,
    document_id INT,
    account_id INT,
    documenttype_id INT,
    branch_id INT,
    ledger_id INT,
    account_code CHAR(30),
    account_name VARCHAR(255),
    document_ref VARCHAR(25),
    document_date DATETIME,
    effective_date DATETIME,
    account_address1 VARCHAR(60),
    account_address2 VARCHAR(60),
    account_address3 VARCHAR(60),
    account_address4 VARCHAR(60),
    account_postal_code VARCHAR(20),
    phone_area_code CHAR(10),
    phone_number VARCHAR(255),
    ledger_name VARCHAR(30),
    policy_number VARCHAR(30),
    transaction_type_code VARCHAR(10),
    transaction_type_description VARCHAR(255),
    policy_type_code CHAR(10),
    policy_type_description VARCHAR(255),
    gross_premium MONEY,
    base_match_amount MONEY,
    unallocated_amount MONEY,
    account_total MONEY,
    match_total MONEY,
    date_paid DATETIME,
    branch_name VARCHAR(255),
    branch_address1 VARCHAR(40),
    branch_address2 VARCHAR(40),
    branch_address3 VARCHAR(40),
    branch_address4 VARCHAR(40),
    branch_postal_code VARCHAR(20),
    matched INT,
    client_code CHAR(20),
    client_name VARCHAR(255),
    client_premium MONEY,
    client_match_amt MONEY,
    client_matched INT,
    comment VARCHAR(255),
    number_of_days INT,
    reminder_type VARCHAR(100),
    transaction_reason VARCHAR(255),
    account_handler VARCHAR(255),
    account_exec VARCHAR(255),
    regarding_line VARCHAR(255),
    insurance_file_cnt INT,
    reminder_type_desc VARCHAR(255),
    currency_id INT,
    currency_code VARCHAR(20)
)
CREATE INDEX RT__transdetail_id ON #Report_Transaction(transdetail_id)
CREATE INDEX RT__account_id ON #Report_Transaction(account_id)


/*Get all of the transactions up to the end date for the type of ledger we are using.*/
INSERT INTO #Report_Transaction
(
    transdetail_id,
    account_id, 
    account_code,
    document_id,
    document_ref,
    document_date,
    effective_date,
    ledger_name, 
    policy_number,
    documenttype_id,
    gross_premium,
    branch_id,
    matched,
    ledger_id,
    comment,
    Branch_Name,
    Branch_address1,
    Branch_address2,
    Branch_address3,
    Branch_address4,
    Branch_postal_code,
    transaction_type_code,
    transaction_type_description,
    policy_type_code,
    policy_type_description,
    transaction_reason,
    regarding_line,
    insurance_file_cnt,
    account_handler,
    account_exec,
    reminder_type_desc,
    currency_id,
    currency_code
)
SELECT 
    td.transdetail_id,
    a.account_id,
    a.short_code,
    d.document_id,
    d.document_ref,
    d.document_date,
    td.ref_date,
    l.ledger_name,
    td.insurance_ref,
    d.documenttype_id,
    CASE @TypeOfCurrency
        WHEN 'Base' THEN ROUND(td.amount,2)
        WHEN 'Transaction' THEN ROUND(td.currency_amount,2)
    END,
    a.company_id,
    0,
    a.ledger_id,
    td.comment,
    s.description,
    s.address1,
    s.address2,
    s.address3,
    s.address4,
    s.postal_code,
    dt.code,
    dt.description,
    rc.code, 
    rc.description,
    tef.reason, 
    ifs.last_trans_description,
    tef.insurance_file_cnt,
    pah.resolved_name,
    pae.shortname,
    rt.description,
    CASE @TypeOfCurrency
        WHEN 'Base' THEN @currency_id
        WHEN 'Transaction' THEN c.currency_id
    END,
    CASE @TypeOfCurrency
        WHEN 'Base' THEN @currency_code
        WHEN 'Transaction' THEN c.code
    END
FROM transdetail td
JOIN account a
    ON a.account_id = td.account_id
JOIN ledger l
    ON l.ledger_id = a.ledger_id
JOIN document d
    ON d.document_id = td.document_id
JOIN documenttype dt
    ON dt.documenttype_id = d.documenttype_id
JOIN source s
    ON s.source_id = a.company_id
JOIN currency c
    ON c.currency_id = td.currency_id
LEFT JOIN transaction_export_folder tef
        JOIN insurance_file i
            ON i.insurance_file_cnt = tef.insurance_file_cnt
        JOIN risk_code rc
            ON rc.risk_code_id = i.risk_code_id
        JOIN insurance_file_system ifs
            ON ifs.insurance_file_cnt = i.insurance_file_cnt
        LEFT JOIN party pah
            ON pah.party_cnt = tef.account_handler_cnt
    ON tef.document_ref = d.document_ref
    AND tef.source_id = d.company_id
    AND tef.accounts_export_status = 'c'
JOIN party p
    ON p.party_cnt = a.account_key  
LEFT JOIN party pae
    ON pae.party_cnt = p.consultant_cnt
LEFT JOIN reminder_type rt
    ON rt.reminder_type_id = p.reminder_type_id
WHERE l.ledger_short_name = @ledger_short_name
--AND d.document_date <= @end_date_quick
AND
    (
       (
	@date_type = 'Transaction Date'
	AND
	d.document_date BETWEEN @start_date AND @end_date_quick
      )
      OR
      (
	@date_type = 'Effective Date'
	AND
	td.ref_date BETWEEN @start_date AND @end_date_quick
      )
    )
AND a.short_code = ISNULL(@party_code, a.short_code)
AND a.company_id = ISNULL(@branch_id, a.company_id)
AND ISNULL(pae.shortname,'') = ISNULL(@account_exec, ISNULL(pae.shortname,''))
AND ISNULL(rt.description,'') = ISNULL(@reminder_type, ISNULL(rt.description,''))

AND ((@insurance_file_cnt IS NULL) OR (@insurance_file_cnt = d.insurance_file_cnt))

/*Get total allocated for each transaction*/
UPDATE rt
SET rt.match_total = 
    (
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN -ISNULL(SUM(ROUND(tm.base_match_amount,2)),0)
                WHEN 'Transaction' THEN -ISNULL(SUM(ROUND(tm.currency_match_amount,2)),0)
            END
        FROM transmatch tm
        JOIN matchgroup mg
            ON mg.match_id = tm.match_id
            AND mg.match_date <= @end_date_quick
        WHERE tm.transdetail_id = rt.transdetail_id
        AND tm.allocationdetail_id IS NOT NULL
        AND (tm.is_reversed <> 1 OR tm.is_reversed is null)
        AND NOT EXISTS /*Don't include matches containing future transactions*/
        (
            SELECT NULL
            FROM transmatch tm
            JOIN transdetail td
                ON td.transdetail_id = tm.transdetail_id
            JOIN document d
                ON d.document_id = td.document_id
            WHERE tm.match_id = mg.match_id
            --AND d.document_date > @end_date_quick
            AND
	    	(
	    		(
	    		D.document_date > @end_date_quick
	    		AND
	    		@date_type = 'Transaction Date'
	    		)
	    		OR
	    		(
	    		TD.ref_date > @end_date_quick 
	    		AND
	    		@date_type = 'Effective Date'
	    		)
		)	   
            
        )
    )
FROM #Report_Transaction rt

/*Identify matched amounts*/
UPDATE #Report_Transaction
SET matched = 1
WHERE match_total + gross_premium = 0

-- If @is_print_settled_Item= No then settled item not being displayed. 
--If it is set to YES then the settled item will need to be displayed
--if no value is paases in then the system option will decide this.
IF @is_print_settled_Item ='No'
	BEGIN
	    DELETE 
	    FROM #Report_Transaction
	    WHERE matched = 1
	END
	
else if rtrim(@is_print_settled_Item) =''
	BEGIN
		/*Remove matched transactions if system option not set to show them.*/
		IF @system_option_3 = '0'
		BEGIN
		    DELETE 
		    FROM #Report_Transaction
		    WHERE matched = 1
		END
	END


IF @ledger_short_name <> 'SA'
BEGIN
    -- Get client details
    UPDATE #Report_Transaction
    SET client_code = a.short_code,
        client_name = a.account_name,
        client_premium = 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN td.amount
                WHEN 'Transaction' THEN td.currency_amount
            END,
        client_match_amt =
            (
                SELECT 
                    CASE @TypeOfCurrency
                        WHEN 'Base' THEN SUM(tm.base_match_amount)
                        WHEN 'Transaction' THEN SUM(tm.currency_match_amount)
                    END
                FROM transmatch tm
                JOIN matchgroup mg
                    ON mg.match_id = tm.match_id
                    AND mg.match_date <= @end_date_quick
                WHERE tm.transdetail_id = td.transdetail_id
            ),
        client_matched = 0
    FROM #Report_Transaction rt
    JOIN transdetail td
        ON td.document_id = rt.document_id
        AND td.document_sequence = 1
    JOIN Account a
        ON a.account_id = td.account_id
END

-- Update Client Address
UPDATE #Report_Transaction
SET account_address1 = ad.address1,
    account_address2 = ad.address2,
    account_address3 = ad.address3,
    account_address4 = ad.address4,
    account_postal_code = ad.postal_code,
    phone_area_code = a.phone_area_code,
    phone_number = a.phone_number
FROM #Report_Transaction rt
JOIN account a
    ON a.account_id = rt.account_id
JOIN party p
    ON p.party_cnt = a.account_key 
JOIN party_address_usage pau
    ON pau.party_cnt = p.party_cnt
JOIN address ad
    ON ad.address_cnt = pau.address_cnt
JOIN address_usage_type aut
    ON aut.address_usage_type_id = pau.address_usage_type_id
    AND aut.code = '3131 XCO'

/*Get total for each account*/
DECLARE cAccount CURSOR FAST_FORWARD FOR
    SELECT a.account_id, p.resolved_name
    FROM account a
    JOIN party p
        ON p.party_cnt = a.account_key
    WHERE EXISTS
        (
            SELECT NULL 
            FROM #Report_Transaction 
            WHERE account_id = a.account_id
        )

OPEN cAccount
FETCH NEXT FROM cAccount INTO @account_id, @account_name
WHILE @@FETCH_STATUS = 0
BEGIN
    /*Get total for each account*/
    SELECT @account_total = SUM(RT.gross_premium)
    FROM #Report_Transaction RT
    WHERE RT.account_id = @account_id

    UPDATE #Report_Transaction
    SET account_total = @account_total,
        account_name = @account_name
    WHERE account_id = @account_id

    FETCH NEXT FROM cAccount INTO @account_id, @account_name
END

CLOSE cAccount
DEALLOCATE cAccount


IF EXISTS (SELECT NULL FROM hidden_options WHERE option_number = 1 AND value = 'U')
BEGIN
    UPDATE #Report_Transaction 
    SET policy_number = i.alternate_reference
    FROM #Report_Transaction rt
    JOIN insurance_file i
        ON i.insurance_file_cnt = rt.insurance_file_cnt
    JOIN source s
        ON s.source_id = i.source_id
    WHERE i.alternate_reference IS NOT NULL
    AND s.underwriting_branch_ind = 1
END

/*Delete all Reversed Transactions*/
DELETE FROM #Report_Transaction 
WHERE Transdetail_id in
    (
        SELECT Transdetail_id 
        FROM TransDetail 
        WHERE spare like 'revers%'
    )
    
/*Extract the data*/
SET NOCOUNT OFF

SELECT
    t.*,
    (
        CASE @group_by
            WHEN 'Branch' THEN
                (
		 t.Branch_Name
                )
            WHEN 'Account Handler' THEN
               (
		ISNULL( t.Account_Handler,'')
	)

            WHEN 'Account Executive' THEN
                ISNULL((
                    t.account_exec
                ),'')
           
        END
    ) group_code,
    (
        CASE @group_by
            WHEN 'Branch' THEN
                t.Branch_name
            WHEN 'Account Handler' THEN
               (
		ISNULL( t.Account_Handler,'')
	)
            WHEN 'Account Executive' THEN
               ISNULL((
                    SELECT ISNULL(P_AccExec.resolved_name, '')
                    FROM #Report_Transaction R3
                    JOIN account A
                        ON A.account_id = R3.account_id
                    JOIN party P_Client
                        ON P_Client.party_cnt = A.account_key
                    JOIN party P_AccExec
                        ON P_AccExec.party_cnt = P_Client.consultant_cnt
                    WHERE R3.transdetail_id=t.transdetail_id
                ),'')
        END
    ) group_desc

FROM #Report_Transaction t
ORDER BY 
    branch_name,
    account_code,
    currency_id,
    transdetail_id

SET NOCOUNT ON

DROP TABLE #Report_Transaction

GO



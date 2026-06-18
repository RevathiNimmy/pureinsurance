/*
This stored procedure is used by the following reports:

Third_Party_Statement_of_Accounts.rpt
Third_Party_Statement_With_Options.rpt
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Agent_Statement'
GO

CREATE PROCEDURE spu_Report_Agent_Statement
	@end_date DATETIME,
	@branch_id INT,
	@date_type VARCHAR(50),
	@statement_type VARCHAR(20),
	@party_code VARCHAR(20),
	@is_print_settled_Item VARCHAR(5),
	@start_date DATETIME
    
AS

DECLARE
    @dStartDate DATETIME,
    @dEndDate DATETIME,
    @ledgershortname VARCHAR(2)

SET NOCOUNT ON

SET ANSI_WARNINGS OFF

IF @party_code = 'ALL' OR @party_code = '' 
BEGIN
    SELECT @party_code = NULL
END

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

IF @date_type='' OR @date_type = null
BEGIN
   SELECT @date_type='Transaction Date'
END

SELECT @dEndDate = ISNULL(@end_date, GETDATE())
SELECT @dStartDate = @start_date

SELECT @statement_type = LOWER(RTRIM(@statement_type))

SELECT @ledgershortname =
    (
        CASE
            WHEN @statement_type = 'client' THEN 'SA'
        WHEN @statement_type = 'agent' THEN 'AG'
            WHEN @statement_type = 'sub agent' THEN 'UB'
        WHEN @statement_type = 'introducer' THEN 'TR'
            ELSE 'SA'
        END
    )

-- Generate Temporary Table
CREATE TABLE #Report_Transaction
(
    transdetail_id INT,
    document_id INT,
    account_id INT,
    documenttype_id INT,
    branch_id INT,
    ledger_id INT,
    account_code VARCHAR(30),
    account_name VARCHAR(255),
    document_ref VARCHAR(25),
    document_date DATETIME,
    effective_date DATETIME,
    account_address1 VARCHAR(60),
    account_address2 VARCHAR(60),
    account_address3 VARCHAR(60),
    account_address4 VARCHAR(60),
    account_postal_code VARCHAR(20),
    phone_area_code VARCHAR(10),
    phone_number VARCHAR(255),
    ledger_name VARCHAR(30),
    policy_number VARCHAR(30),
    transaction_type_code VARCHAR(10),
    transaction_type_description VARCHAR(255),
    policy_type_code VARCHAR(10),
    policy_type_description VARCHAR(255),
    gross_premium MONEY,
    base_match_amount MONEY,
    unallocated_amount MONEY,
    account_total MONEY,
    match_total MONEY,
    date_paid DATETIME,
    Branch_Name VARCHAR(255),
    Branch_address1 VARCHAR(40),
    Branch_address2 VARCHAR(40),
    Branch_address3 VARCHAR(40),
    Branch_address4 VARCHAR(40),
    Branch_postal_code VARCHAR(20),
    matched INT,
    client_code VARCHAR(20),
    client_name VARCHAR(255),
    client_premium MONEY,
    client_match_amt MONEY,
    client_matched INT,
    comment VARCHAR(255),
    number_of_days INT,
    reminder_type VARCHAR(100),
    transaction_reason VARCHAR(255),
    account_handler VARCHAR(255),
    renewal_date DATETIME,
    risk_code VARCHAR(10),
    report_indicator INT,
    order_code VARCHAR(30),
    latest_transaction_comment VARCHAR(500),
    has_comment BIT,
    spare VARCHAR(30),
    orig_document_ref VARCHAR(30),
    ledger_short_name VARCHAR(2)
)

-- Get the required transactions
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
    match_total,
    ledger_id,
    comment, 
    transaction_reason,
    account_handler,
    Branch_Name,
    Branch_address1,
    Branch_address2,
    Branch_address3,
    Branch_address4,
    Branch_postal_code,
    transaction_type_code,
    transaction_type_description,
    renewal_date,
    risk_code,
    report_indicator,
    latest_transaction_comment,
    spare,
    orig_document_ref,
    ledger_short_name
)
SELECT
    TD.transdetail_id,
    A.account_id,
    A.short_code,
    D.document_id,
    D.document_ref,
    D.document_date,
    TD.ref_date,
    L.ledger_name,
    TD.insurance_ref,
    D.documenttype_id,
    ROUND(TD.Amount,2),
    D.company_id,
    0,
    (
        SELECT ISNULL(SUM(ROUND(TM.base_match_amount,2)),0)
        FROM TransMatch TM
        JOIN MatchGroup MG
            ON MG.match_id = TM.match_id
            AND MG.match_date <= @dEndDate
        WHERE TM.transdetail_id = TD.transdetail_id
        AND TM.allocationdetail_id IS NOT NULL 
    ),
    A.ledger_id,
    TD.comment,
    TEF.reason,
    PAH.resolved_name,
    C.description,
    C.address1,
    C.address2,
    C.address3,
    C.address4,
    C.postal_code,
    DT.code,
    DT.description,
    I.renewal_date,
    RC.code,
    PA.report_indicator,
    (
        SELECT TOP 1 
            TC.description 
        FROM transaction_comment TC
        WHERE TC.transdetail_id = TD.transdetail_id 
        ORDER BY comment_date DESC
    ),
    TD.spare,
    '',
    L.ledger_short_name
FROM Account A
JOIN Ledger L
    ON A.ledger_id = L.ledger_id
JOIN TransDetail TD
    ON TD.account_id = A.account_id
JOIN Document D
    ON D.document_id = TD.document_id
JOIN Company C
    ON C.company_id = D.company_id
JOIN DocumentType DT
    ON DT.documenttype_id = D.documenttype_id
LEFT JOIN Transaction_Export_Folder TEF
    ON TEF.document_ref = D.document_ref
    AND TEF.source_id = D.company_id
    AND TEF.accounts_export_status = 'c'
LEFT JOIN Insurance_File I
    ON I.insurance_file_cnt = TEF.insurance_file_cnt
LEFT JOIN Risk_Code RC
    ON RC.risk_code_id = I.risk_code_id
LEFT JOIN Party PAH
    ON PAH.party_cnt = TEF.account_handler_cnt
LEFT JOIN Party P
    ON P.party_cnt = A.account_key 
LEFT JOIN Reminder_Type RT
    ON RT.reminder_type_id = P.reminder_type_id
LEFT JOIN Party_Agent PA
    ON PA.party_cnt = P.party_cnt
WHERE A.short_code = ISNULL(@party_code,A.short_code)
AND
    (
       (
	@date_type = 'Transaction Date'
	AND
	(
          (@dStartDate IS NULL AND d.document_date <= @dEndDate)
           OR
          (d.document_date >= @dStartDate AND d.document_date <= @dEndDate)
	 )
        )
        OR
       (
	@date_type = 'Effective Date'
	AND
        (
	  (@dStartDate IS NULL AND td.ref_date <= @dEndDate)
           OR
          (td.ref_date >= @dStartDate AND td.ref_date <= @dEndDate)
	 )
        )
    )
AND D.company_id = ISNULL(@branch_id,D.company_id)
AND L.ledger_short_name = @ledgershortname

IF @ledgershortname <> 'SA'
BEGIN
    /*Get client details*/
    /* DC270105 added check on end to ignore introducer journals, as client details are obtained differently for those */
    UPDATE RT
        SET client_code = A.short_code,
        client_name = A.account_name,
        client_premium = TD.amount,
        client_match_amt =
            (
                SELECT SUM(ROUND(TM.base_match_amount,2))
                FROM TransMatch TM
                JOIN MatchGroup MG
                    ON MG.match_id = TM.match_id
                WHERE TM.transdetail_id = TD.transdetail_id
                AND MG.match_date <= @dEndDate
            ),
        client_matched = 0
    FROM #Report_Transaction RT
    JOIN TransDetail TD
        ON TD.document_id = RT.document_id
        AND TD.document_sequence = 1
    JOIN Account A
        ON A.account_id = TD.account_id
        AND Left(RT.spare, 8) <> 'INT COMM' 
    AND Left(RT.spare, 8) <> 'INT ADJ '
    AND Left(RT.spare, 8) <> 'Revsd IC' 
    AND Left(RT.spare, 8) <> 'Revsl IC'
    AND Left(RT.spare, 8) <> 'Revd ICA' 
    AND Left(RT.spare, 8) <> 'Revl ICA'
        
    /* DC270105 added this to obtain the client details from the relating transaction of the introducer journal */
    UPDATE RT
        SET client_code = A.short_code,
        client_name = A.account_name,
        client_premium = TD.amount,
        client_match_amt =
            (
                SELECT SUM(ROUND(TM.base_match_amount,2))
                FROM TransMatch TM
                JOIN MatchGroup MG
                    ON MG.match_id = TM.match_id
                WHERE TM.transdetail_id = TD.transdetail_id
                AND MG.match_date <= @dEndDate
            ),
            client_matched = 0,
        orig_document_ref = D.document_ref
    FROM #Report_Transaction RT
    JOIN document D 
        ON rtrim(D.document_ref) = rtrim(substring(RT.spare, 10, 20))
        AND D.company_id = RT.branch_id
    JOIN TransDetail TD
        ON TD.document_id = D.document_id
        AND TD.document_sequence = 1
    JOIN Account A
        ON A.account_id = TD.account_id
        AND     (
        ((left(RT.spare, 8) = 'INT COMM' OR left(RT.spare, 8) = 'INT ADJ ') OR
            (left(RT.spare, 8) = 'Revsd IC' OR left(RT.spare, 8) = 'Revsl IC')) OR
        (left(RT.spare, 8) = 'Revd ICA' OR left(RT.spare, 8) = 'Revl ICA')
        )
    
END

/*Update client/agent/subagent address*/
UPDATE RT
SET account_address1 = AD.address1,
    account_address2 = AD.address2,
    account_address3 = AD.address3,
    account_address4 = AD.address4,
    account_postal_code = AD.postal_code,
    phone_area_code = A.phone_area_code,
    phone_number = A.phone_number
FROM #Report_Transaction RT
JOIN Account A
    ON A.account_id = RT.account_id
JOIN Party P
    ON P.party_cnt = A.account_key
JOIN Party_Address_Usage PAU
    ON PAU.party_cnt = P.party_cnt
JOIN Address AD
    ON AD.address_cnt = PAU.address_cnt
JOIN Address_Usage_Type AUT
    ON AUT.address_usage_type_id = PAU.address_usage_type_id 
    AND AUT.code = '3131 XCO'

/*Identify matched amounts*/
UPDATE #Report_Transaction
SET matched = 1
WHERE match_total = gross_premium

-- If @is_print_settled_Item= No then settled item not being displayed. 
--If it is set to YES then the settled item will need to be displayed

IF @is_print_settled_Item ='No'
	BEGIN
	    DELETE 
	    FROM #Report_Transaction
	    WHERE matched = 1
	END

/*Calculate the totals*/
UPDATE RT
SET account_total =
        (
            SELECT SUM(gross_premium)
            FROM #Report_Transaction
            WHERE account_id = RT.account_id
        ),
    account_name = P.resolved_name
FROM #Report_Transaction RT
JOIN Account A
    ON A.account_id = RT.account_id
JOIN Party P
    ON P.party_cnt = A.account_key

/*Set order_code for each account*/
UPDATE RT
SET order_code = 
        (
            SELECT 
                CASE RT.report_indicator
                    WHEN 0 THEN CONVERT(VARCHAR(30),RT.date_paid,20)
                    WHEN 1 THEN RT.policy_number
                    WHEN 2 THEN RT.client_code
                    WHEN 3 THEN CONVERT(VARCHAR(30),RT.renewal_date,20)
                    WHEN 4 THEN RT.risk_code
                    ELSE RT.document_ref
                END
        )
FROM #Report_Transaction RT

UPDATE RT
SET has_comment =
        CASE
            WHEN ISNULL(latest_transaction_comment,'') = '' THEN 0
            ELSE 1
        END
FROM #Report_Transaction RT

/*Extract the data*/
SET NOCOUNT OFF

SELECT 
    transdetail_id,
    document_id, 
    account_id, 
    documenttype_id, 
    branch_id, 
    ledger_id, 
    account_code, 
    account_name, 
    document_ref, 
    document_date, 
    effective_date, 
    account_address1, 
    account_address2, 
    account_address3, 
    account_address4, 
    account_postal_code, 
    phone_area_code, 
    phone_number, 
    ledger_name, 
    policy_number, 
    transaction_type_code, 
    transaction_type_description, 
    policy_type_code, 
    policy_type_description, 
    gross_premium, 
    base_match_amount, 
    unallocated_amount, 
    account_total, 
    match_total, 
    date_paid, 
    Branch_Name, 
    Branch_address1, 
    Branch_address2, 
    Branch_address3, 
    Branch_address4, 
    Branch_postal_code, 
    matched, 
    client_code, 
    client_name, 
    client_premium, 
    client_match_amt, 
    client_matched, 
    comment, 
    number_of_days, 
    reminder_type, 
    transaction_reason, 
    account_handler,
    order_code,
    latest_transaction_comment,
    has_comment,
    spare,
    orig_document_ref,
    ledger_short_name
FROM #Report_Transaction
ORDER BY 
    account_code,
    order_code

SET NOCOUNT ON

/*Remove the working table*/
DROP TABLE #Report_Transaction

GO

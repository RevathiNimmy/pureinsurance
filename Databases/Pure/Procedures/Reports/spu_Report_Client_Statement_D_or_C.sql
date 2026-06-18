SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Client_Statement_D_or_C'
GO
CREATE PROCEDURE spu_Report_Client_Statement_D_or_C
    @end_date DATETIME,
    @branch_id INT,
    @party_code VARCHAR(20),
    @acc_exec_code VARCHAR(20),
    @debit_or_credit VARCHAR(6),
    @TypeOfCurrency VARCHAR(15)
AS

DECLARE @dStartDate DATETIME
DECLARE @dEndDate DATETIME
DECLARE @iAccountID INT
DECLARE @nAccountTotal MONEY
DECLARE @nMatchTotal MONEY
DECLARE @nUnallocated MONEY
DECLARE @sAccountName VARCHAR(100)
DECLARE @currency_code VARCHAR(20)

SET NOCOUNT ON

SET ANSI_WARNINGS OFF

IF @party_code = 'ALL' OR @party_code = ''
BEGIN
    SELECT @party_code = NULL
END

IF @acc_exec_code = 'ALL' OR @acc_exec_code = ''
BEGIN
    SELECT @acc_exec_code = NULL
END

IF @branch_id = 0
BEGIN
    SELECT @branch_id = NULL
END

SELECT @dEndDate = ISNULL(@end_date, GETDATE())

IF @debit_or_credit = 'ALL' OR @debit_or_credit = '' 
BEGIN
    SELECT @debit_or_credit = NULL
END

IF ISNULL(@TypeOfCurrency,'') = ''
BEGIN
    SELECT @TypeOfCurrency = 'Base'
END 

SELECT 
    @currency_code = c.code
FROM source s
JOIN currency c
    ON c.currency_id = s.base_currency_id
WHERE source_id = 1

-- Generate Temporary Table
CREATE TABLE #Report_Transaction
(
    transdetail_id INT,
    document_id INT,
    account_id INT,
    documenttype_id INT,
    branch_id INT,
    ledger_id INT,
    account_code CHAR(30),
    account_name VARCHAR(100),
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
    Branch_Name VARCHAR(255),
    Branch_address1 VARCHAR(40),
    Branch_address2 VARCHAR(40),
    Branch_address3 VARCHAR(40),
    Branch_address4 VARCHAR(40),
    Branch_postal_code VARCHAR(20),
    matched INT,
    client_code CHAR(20),
    client_name VARCHAR(100),
    client_premium MONEY,
    client_match_amt MONEY,
    client_matched INT,
    comment VARCHAR(255),
    number_of_days INT,
    reminder_type VARCHAR(100),
    transaction_reason VARCHAR(255),
    account_handler VARCHAR(100),
    policy_description VARCHAR(255),
    risk_description VARCHAR(255),
    currency VARCHAR(6)
)

-- Get the required transactions
INSERT INTO #Report_Transaction
(
    transdetail_id, /* transdetail_id, TransDetail.transdetail_id */
    account_id, /* account_id, Account.account_id */
    account_code, /* account_code, Account.short_code */
    document_id,
    document_ref, /* document_ref, Document.document_ref */
    document_date, /* document_date, Document.document_date */
    effective_date, /* extra_datetime1, * TransDetail.ref_date */
    ledger_name, /* ledger_type, Ledger.ledger_name */
    policy_number, /* policy_number, TransDetail.insurance_ref */
    documenttype_id, /* documenttype_id, Document.documenttype_id */
    gross_premium, /* amount, TransDetail.amount */
    branch_id, /* branch_id, Account.company_id */
    matched, /* extra_int1, Match indicator */
    client_name,        /*1.6.9 031002 Policy Holder Name */        
    match_total, /* extra_numeric1, Total match amount for transaction */
    ledger_id, /* record_type, Account.ledger_id */
    comment, /* comment, */
    transaction_reason, /* extra_char3, Transaction_Export_Folder.reason */
    account_handler, /* extra_char4 Party.resolved_name - Account handler */
    Branch_Name,
    Branch_address1,
    Branch_address2,
    Branch_address3,
    Branch_address4,
    Branch_postal_code,
    transaction_type_code,
    transaction_type_description,
    policy_description,
    risk_description,
    currency

)
SELECT
    0, 
    A.account_id,
    A.short_code,
    D.document_id,
    D.document_ref,
    D.document_date,
    TD.ref_date, 
    L.ledger_name,
    TD.insurance_ref,
    D.documenttype_id,
    CASE @TypeOfCurrency
        WHEN 'Base' THEN ROUND(TD.amount,2)
        WHEN 'Transaction' THEN ROUND(TD.currency_amount,2)
    END,
    A.company_id,
    0,
    I.insured_name,  
    (
        SELECT 
            CASE @TypeOfCurrency
                WHEN 'Base' THEN -ISNULL(SUM(ROUND(TM.base_match_amount,2)),0)
                WHEN 'Transaction' THEN -ISNULL(SUM(ROUND(TM.currency_match_amount,2)),0)
            END
        FROM TransMatch TM
        JOIN MatchGroup MG
            ON MG.match_id = TM.match_id
        WHERE MG.match_date <= @dEndDate
        AND TM.transdetail_id = TD.transdetail_id
        AND TM.base_match_amount <> 0
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
    IFS.last_trans_description,
    RC.description,
    CASE @TypeOfCurrency
            WHEN 'Base' THEN @currency_code
            WHEN 'Transaction' THEN CY.code
    END
FROM Account A
JOIN Ledger L
    ON L.ledger_id = A.ledger_id    
JOIN TransDetail TD
    ON TD.account_id = A.account_id
JOIN Company C
    ON C.company_id = A.company_id
JOIN Document D
    ON D.document_id = TD.document_id
JOIN DocumentType DT
    ON DT.documenttype_id = D.documenttype_id
LEFT JOIN Transaction_Export_Folder TEF
    ON TEF.document_ref = D.document_ref
    AND TEF.source_id = D.company_id
    AND TEF.accounts_export_status = 'c'
LEFT JOIN Party P
    ON P.party_cnt = A.account_key 
LEFT JOIN Party PAE
    ON PAE.party_cnt = P.consultant_cnt    
LEFT JOIN Party PAH
    ON PAH.party_cnt = TEF.account_handler_cnt
LEFT JOIN Insurance_File I
    ON I.insurance_file_cnt = D.insurance_file_cnt
LEFT JOIN Insurance_File_System IFS
    ON IFS.insurance_file_cnt = I.insurance_file_cnt
LEFT JOIN Risk_Code RC
    ON RC.risk_code_id = I.risk_code_id
JOIN Currency CY
    ON CY.Currency_Id = TD.Currency_Id
WHERE L.ledger_short_name = 'SA'
AND TD.accounting_date <= @dEndDate
AND A.short_code = ISNULL(@party_code, A.short_code)
AND A.company_id = ISNULL(@branch_id, A.company_id)
AND P.shortname = ISNULL(@acc_exec_code, P.shortname)


/*Update client name field where there is no insured name on policy*/
UPDATE RT
SET RT.client_name = A.account_name
FROM #Report_Transaction RT
JOIN Account A
    ON A.account_id = RT.account_id
WHERE RT.client_name IS NULL

/*Update Client Address*/
UPDATE #Report_Transaction
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

/*Add the totals*/
DECLARE cAccount CURSOR FAST_FORWARD FOR
    SELECT 
        A.account_id, 
        P.resolved_name
    FROM Account A
    JOIN Party P
        ON P.party_cnt = A.account_key
    WHERE EXISTS
        (
            SELECT 
                NULL 
            FROM #Report_Transaction 
            WHERE account_id = A.account_id
        )

OPEN cAccount

FETCH NEXT FROM cAccount INTO 
    @iAccountID, 
    @sAccountName
    
WHILE @@FETCH_STATUS = 0
BEGIN

    SELECT 
        @nAccountTotal = SUM(RT.gross_premium) + SUM(RT.match_total)
    FROM #Report_Transaction RT
    WHERE RT.account_id = @iAccountID

    UPDATE #Report_Transaction
    SET account_total = @nAccountTotal,
        account_name = @sAccountName
    WHERE account_id = @iAccountID

    FETCH NEXT FROM cAccount INTO 
        @iAccountID, 
        @sAccountName
END

CLOSE cAccount
DEALLOCATE cAccount

-- Extract the data
SET NOCOUNT OFF

SELECT DISTINCT
    * 
FROM #Report_Transaction
WHERE gross_premium <> 0
AND account_total <> 0
AND (
        @debit_or_credit IS NULL
        OR
        (
            @debit_or_credit = 'Debit' 
            AND 
            account_total > 0
        )
        OR
        (
            @debit_or_credit = 'Credit' 
            AND 
            account_total < 0
        )
    )


SET NOCOUNT ON

DROP TABLE #Report_Transaction

SET NOCOUNT OFF

GO


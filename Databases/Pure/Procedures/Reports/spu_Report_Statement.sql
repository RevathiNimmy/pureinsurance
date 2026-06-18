SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Statement'
GO
CREATE PROCEDURE spu_Report_Statement(
    @end_date datetime,
    @branch_id integer,
    @statement_type varchar(20),
    @party_code varchar(20))
AS

DECLARE
    @dEndDate datetime,
    @iBranchID integer,
    @sStatementType char(2),
    @iLedgerID integer,
    @iAccountID integer,
    @nAccountTotal numeric(19, 4),
    @nMatchTotal numeric(19, 4),
    @nUnallocated numeric(19, 4),
    @sAccountName varchar(100),
    @sPartyCode varchar(20)

SET NOCOUNT ON

SET ANSI_WARNINGS OFF

-- TF0230701 - Include party_code parameter
IF @party_code = 'ALL' BEGIN
    SELECT @party_code = NULL
END
SELECT @sPartyCode = ISNULL(@party_code, '')

SELECT @dEndDate = ISNULL(@end_date, GETDATE())
SELECT @iBranchID = ISNULL(@branch_id, 0)
SELECT @statement_type = LOWER(RTRIM(@statement_type))

SELECT @sStatementType = (CASE
    WHEN @statement_type = 'client' THEN
                'C '
    WHEN @statement_type = 'agent' THEN
                    'A '
    WHEN @statement_type = 'sub agent' THEN
        'SA'
    ELSE
        'C '
        END)

SELECT @iLedgerID = (CASE
        WHEN @sStatementType = 'C ' THEN
                2
        WHEN @sStatementType = 'A ' THEN
                5
        WHEN @sStatementType = 'SA' THEN
                10
        ELSE
                2
        END)

-- Generate Temporary Table
CREATE TABLE #Report_Transaction
(
    transdetail_id int,
    document_id int,
    account_id int,
    documenttype_id int,
    branch_id int,
    ledger_id int,
    account_code char(30),
        account_name varchar(100),
        document_ref varchar(25),
        document_date datetime,
        effective_date datetime,
        account_address1 varchar(60),
        account_address2 varchar(60),
        account_address3 varchar(60),
        account_address4 varchar(60),
        account_postal_code varchar(20),
        phone_area_code char(10),
        phone_number varchar(255),
        ledger_name varchar(30),
        policy_number varchar(30),
        transaction_type_code varchar(10),
        transaction_type_description varchar(255),
        policy_type_code char(10),
        policy_type_description varchar(255),
        gross_premium numeric(19, 4),
        base_match_amount numeric(19, 4),
        unallocated_amount numeric(19, 4),
        account_total numeric(19, 4),
        match_total numeric(19, 4),
        date_paid datetime,
        Branch_Name varchar(255),
        Branch_address1 varchar(40),
        Branch_address2 varchar(40),
        Branch_address3 varchar(40),
        Branch_address4 varchar(40),
        Branch_postal_code varchar(20),
        matched int,
        client_code char(20),
        client_name varchar(100),
        client_premium numeric(19, 4),
        client_match_amt numeric(19, 4),
        client_matched int,
        comment varchar(255),
        number_of_days int,
        reminder_type varchar(100),
    transaction_reason varchar(255),
    account_handler varchar(100)
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
    --DC141101 not required
        --policy_type_code, /* extra_char1, Risk_Code.code */
        --policy_type_description, /* extra_char2, Risk_Code.description */
        gross_premium, /* amount, TransDetail.amount */
        branch_id, /* branch_id, Account.company_id */
        matched, /* extra_int1, Match indicator */
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
        transaction_type_description
)
SELECT
    TransDetail.transdetail_id,
        MAX(Account.account_id),
        MAX(Account.short_code),
    MAX(Document.document_id),
        MAX(Document.document_ref),
        MAX(Document.document_date),
        MAX(TransDetail.ref_date),
        MAX(Ledger.ledger_name),
        MAX(TransDetail.insurance_ref),
        MAX(Document.documenttype_id),
    --DC141101 not required
        --MAX(Risk_Code.code),
        --MAX(Risk_Code.description),
        MAX(TransDetail.Amount),
        MAX(Account.company_id),
        0,
        -SUM(ISNULL(AllocationDetail.alloc_base_amount, 0)),
        MAX(Account.ledger_id),
        MAX(TransDetail.comment),
        MAX(Transaction_Export_Folder.reason),
        MAX(PartyAH.resolved_name),
    MAX(Company.description),
        MAX(Company.address1),
        MAX(Company.address2),
        MAX(Company.address3),
        MAX(Company.address4),
        MAX(Company.postal_code),
        MAX(DocumentType.code),
        MAX(DocumentType.description)
FROM Account Account
        JOIN Ledger Ledger
            ON Account.ledger_id = Ledger.ledger_id
            AND (@sPartyCode = '' OR (@sPartyCode <> '' AND Account.short_code = @sPartyCode))
        JOIN TransDetail TransDetail
            ON Account.account_id = TransDetail.account_id
        JOIN Company Company
            ON Account.company_id = Company.company_id
        JOIN Document Document
            ON TransDetail.document_id = Document.document_id
            AND Document.document_date <= @dEndDate
        JOIN DocumentType DocumentType
            ON Document.documenttype_id = DocumentType.documenttype_id
        LEFT OUTER JOIN AllocationDetail AllocationDetail
            INNER JOIN Allocation Allocation
                    ON AllocationDetail.allocation_id = Allocation.allocation_id
                    AND Allocation.allocation_date <= @dEndDate
            ON TransDetail.transdetail_id = AllocationDetail.transdetail_id
            AND AllocationDetail.alloc_base_amount <> 0
    AND AllocationDetail.allocationdetail_id IS NOT NULL
        LEFT OUTER JOIN Transaction_Export_Folder
            ON Document.document_ref = Transaction_Export_Folder.document_ref
            AND Document.company_id = Transaction_Export_Folder.source_id
        --DC141101 not required
    --LEFT OUTER JOIN Insurance_File
            --ON Transaction_Export_Folder.insurance_file_cnt = Insurance_File.insurance_file_cnt
            AND Transaction_Export_Folder.accounts_export_status = 'c'
        --DC141101 not required
    --LEFT OUTER JOIN Risk_Code
            --ON Insurance_File.risk_code_id = Risk_Code.risk_code_id
        LEFT OUTER JOIN Party PartyAH
            ON PartyAH.party_cnt = Transaction_Export_Folder.account_handler_cnt
        LEFT OUTER JOIN Party
            ON Account.account_key = Party.party_cnt
        LEFT OUTER JOIN Reminder_Type
            ON Reminder_Type.reminder_type_id = Party.reminder_type_id
WHERE
    Account.ledger_id = @iLedgerID
AND
(
    @iBranchID = 0
    OR
    (@iBranchID > 0 AND Account.company_id = @iBranchID)
)
GROUP BY TransDetail.transdetail_id

IF @sStatementType <> 'C '
BEGIN
    -- Get client details
        UPDATE #Report_Transaction
    SET
            client_code = Account.short_code,
                client_name = Account.account_name,
                client_premium = Transdetail.amount,
                client_match_amt =
        (
            SELECT SUM(AllocationDetail.alloc_base_amount)
                    FROM AllocationDetail AllocationDetail,
                        Allocation Allocation
                    WHERE AllocationDetail.transdetail_id = Transdetail.transdetail_id
                    AND Allocation.allocation_id = AllocationDetail.allocation_id
                    AND Allocation.allocation_date <= @dEndDate
        ),
                client_matched = 0
        FROM #Report_Transaction RT
            JOIN TransDetail TransDetail
                    ON TransDetail.document_id = RT.document_id
            AND Transdetail.document_sequence = 1
            JOIN Account Account
                    ON TransDetail.account_id = Account.account_id
-- AND Account.ledger_id = 2
END
--ELSE
--BEGIN
    -- Update Client Address
    UPDATE #Report_Transaction
    SET account_address1 = Address.address1,
                account_address2 = Address.address2,
                account_address3 = Address.address3,
            account_address4 = Address.address4,
                account_postal_code = Address.postal_code,
                phone_area_code = Account.phone_area_code,
                phone_number = Account.phone_number
    FROM #Report_Transaction RT
        JOIN Account Account
                ON RT.account_id = Account.account_id
        JOIN Party
                ON Account.account_key = Party.party_cnt
        JOIN Party_Address_Usage
                ON Party_Address_Usage.party_cnt = Party.party_cnt
        JOIN Address
                ON Address.address_cnt = Party_Address_Usage.address_cnt
        JOIN Address_Usage_Type
                ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id
                AND Address_Usage_Type.code = '3131 XCO'
    --DC121101 not required -caused address not to be found
    --LEFT OUTER JOIN Party_Contact_Usage
    -- ON Party_Contact_Usage.party_cnt = Party.party_cnt
    --JOIN Contact
    -- ON Contact.contact_cnt = Party_Contact_Usage.contact_cnt
    --JOIN Contact_Type
    -- ON Contact_Type.contact_type_id = Contact.contact_type_id
    -- AND Contact_Type.code = 'TELEPHONE'
--END

-- Identify matched amounts
UPDATE #Report_Transaction
SET matched = 1
WHERE match_total = gross_premium

-- Add the totals
DECLARE cAccount CURSOR FAST_FORWARD FOR
        SELECT A.account_id, P.resolved_name
            FROM Account A, Party P
        WHERE P.party_cnt = A.account_key
        AND EXISTS
    (
        SELECT NULL FROM #Report_Transaction WHERE account_id = A.account_id
    )

OPEN cAccount
FETCH NEXT FROM cAccount INTO @iAccountID, @sAccountName
WHILE @@FETCH_STATUS = 0
BEGIN
        -- Get totals
        SELECT @nAccountTotal = SUM(RT.gross_premium)
        FROM #Report_Transaction RT
        WHERE RT.account_id = @iAccountID

        UPDATE #Report_Transaction
        SET account_total = @nAccountTotal,
                account_name = @sAccountName
        WHERE account_id = @iAccountID

        FETCH NEXT FROM cAccount INTO @iAccountID, @sAccountName
END

CLOSE cAccount
DEALLOCATE cAccount

-- Extract the data
SET NOCOUNT OFF
SELECT * FROM #Report_Transaction
SET NOCOUNT ON

DELETE FROM #Report_Transaction
DROP TABLE #Report_Transaction

SET NOCOUNT OFF

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_HC_Client_Statement_D_or_C'
GO
 
CREATE PROCEDURE spu_Report_HC_Client_Statement_D_or_C(
	@end_date datetime,
    	@branch_id integer,
    	@party_code varchar(20),
	@acc_exec_code varchar(20),
	@debit_or_credit varchar(6))
AS

DECLARE
	@dStartDate datetime,
	@dEndDate datetime,
	@iBranchID integer,
	@sStatementType char(2),
	@iLedgerID integer,
	@sDB sysname,
	@iAccountID integer,	
	@nAccountTotal numeric(19,4),
	@nMatchTotal numeric(19,4),
	@nUnallocated numeric(19,4),
	@sAccountName varchar(100),
	@sPartyCode varchar(20),
	@sAccExecCode varchar(20),
	@sDebitOrCredit varchar(6)

SET NOCOUNT ON

SET ANSI_WARNINGS OFF

IF @party_code = 'ALL' BEGIN
	SELECT @party_code = NULL
END
SELECT @sPartyCode = ISNULL(@party_code, '')

IF @acc_exec_code = 'ALL' BEGIN
	SELECT @acc_exec_code = NULL
END
SELECT @sAccExecCode = ISNULL(@acc_exec_code, '')

SELECT @dEndDate = ISNULL(@end_date, GETDATE())
SELECT @iBranchID = ISNULL(@branch_id, 0)

SELECT @sStatementType = 'C'

SELECT @iLedgerID = 2

IF @debit_or_credit = 'ALL' BEGIN
	SELECT @debit_or_credit = NULL
END
SELECT @sDebitOrCredit = ISNULL(@Debit_or_Credit, '')

-- Generate Temporary Table
CREATE TABLE    #Report_Transaction
(
	transdetail_id			int,
	document_id			int,
	account_id			int,
	documenttype_id			int,
	branch_id			int,
	ledger_id			int,
	account_code			char(30),
        account_name			varchar(100),
        document_ref			varchar(25),
        document_date			datetime,
        effective_date			datetime,
        account_address1		varchar(60),
        account_address2		varchar(60),
        account_address3		varchar(60),
        account_address4		varchar(60),
        account_postal_code		varchar(20),
        phone_area_code			char(10),
        phone_number			varchar(255),
        ledger_name			varchar(30),
        policy_number			varchar(30),
        transaction_type_code		varchar(10),
        transaction_type_description	varchar(255),
        policy_type_code		char(10),
        policy_type_description		varchar(255),
        gross_premium			numeric(19, 4),
        base_match_amount		numeric(19, 4),
        unallocated_amount		numeric(19, 4),
        account_total			numeric(19, 4),
        match_total			numeric(19, 4),
        date_paid			datetime,
        Branch_Name			varchar(255),
        Branch_address1			varchar(40),
        Branch_address2			varchar(40),
        Branch_address3			varchar(40),
        Branch_address4			varchar(40),
        Branch_postal_code		varchar(20),
        matched				int,
        client_code			char(20),
        client_name			varchar(100),
        client_premium			numeric(19, 4),
        client_match_amt		numeric(19, 4),
        client_matched			int,
        comment				varchar(255),
        number_of_days			int,
        reminder_type			varchar(100),
	transaction_reason		varchar(255),
	account_handler			varchar(100),
	policy_description		varchar(255),
	risk_description			varchar(255),
	currency			varchar(6)
)

-- Get the required transactions
INSERT INTO #Report_Transaction
(
	transdetail_id,		/* transdetail_id,  TransDetail.transdetail_id */
    	account_id,			/* account_id,  Account.account_id */
    	account_code,		/* account_code,  Account.short_code */
	document_id,
    	document_ref,		/* document_ref,    Document.document_ref */
    	document_date,		/* document_date,   Document.document_date */
    	effective_date,		/* extra_datetime1,    * TransDetail.ref_date */
    	ledger_name,		/* ledger_type,     Ledger.ledger_name */
    	policy_number,		/* policy_number,   TransDetail.insurance_ref */
    	documenttype_id,		/* documenttype_id,     Document.documenttype_id */
    	gross_premium,		/* amount,      TransDetail.amount */
    	branch_id,			/* branch_id,   Account.company_id */
    	matched,			/* extra_int1,  Match indicator */
    	client_code,
	client_name,
	client_premium,
	match_total,		/* extra_numeric1,  Total match amount for transaction */
    	ledger_id,			/* record_type,     Account.ledger_id */
    	comment,			/* comment, */
    	transaction_reason,		/* extra_char3,	 Transaction_Export_Folder.reason */
    	account_handler,		/* extra_char4    Party.resolved_name - Account handler */
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
	0, --TransDetail.transdetail_id,
    	Account.account_id,
    	Account.short_code,
	Document.document_id,
    	Document.document_ref,
    	Document.document_date,
    	TransDetail.accounting_date,
    	Ledger.ledger_name,
    	TransDetail.insurance_ref,
    	Document.documenttype_id,
    	TransDetail.Amount,
    	Account.company_id,
    	0,
	Account.short_Code,
	Account.account_name,
	Transdetail.amount,
    	ISNULL((SELECT -SUM(AllocationDetail.alloc_base_amount) FROM AllocationDetail AllocationDetail
        	INNER JOIN Allocation Allocation
            		ON AllocationDetail.allocation_id = Allocation.allocation_id
		WHERE Allocation.allocation_date <= @dEndDate
        	AND TransDetail.transdetail_id = AllocationDetail.transdetail_id
        	AND AllocationDetail.alloc_base_amount <> 0
	AND AllocationDetail.allocationdetail_id IS NOT NULL
	),0.0),    	
	Account.ledger_id,
    	TransDetail.comment,
    	Transaction_Export_Folder.reason,
    	PartyAH.resolved_name,	
	Company.description,
    	Company.address1,
    	Company.address2,
    	Company.address3,
    	Company.address4,
    	Company.postal_code,
    	DocumentType.code,
    	DocumentType.description,
	InsFS.last_trans_description,
	Risk_Code.description,
	Currency.code
FROM	Account Account
    	JOIN Ledger Ledger
        	ON Account.ledger_id = Ledger.ledger_id
        	AND (@sPartyCode = '' OR (@sPartyCode <> '' AND Account.short_code = @sPartyCode))
    	JOIN TransDetail TransDetail
        	ON Account.account_id = TransDetail.account_id
    	JOIN Company Company
        	ON Account.company_id = Company.company_id
    	JOIN Document Document
        	ON TransDetail.document_id = Document.document_id
	AND Transdetail.accounting_date <= @dEndDate
    	JOIN DocumentType DocumentType
        	ON Document.documenttype_id = DocumentType.documenttype_id

    	LEFT OUTER JOIN Transaction_Export_Folder
        	ON Document.document_ref = Transaction_Export_Folder.document_ref
        	AND Document.company_id = Transaction_Export_Folder.source_id
        	AND Transaction_Export_Folder.accounts_export_status = 'c'
    	LEFT OUTER JOIN Party Party
         	ON Account.account_key = Party.party_cnt
	LEFT OUTER JOIN Party PartyAE
        	ON PartyAE.party_cnt = Party.consultant_cnt
		AND (@sAccExecCode = '' OR (PartyAE.shortname = @sAccExecCode))
    	LEFT OUTER JOIN Party PartyAH
        	ON PartyAH.party_cnt = Transaction_Export_Folder.account_handler_cnt
	LEFT OUTER JOIN Insurance_File InsF
		ON InsF.Insurance_Ref = Transdetail.Insurance_Ref
		and InsF.policy_version = (SELECT MAX(policy_version) FROM insurance_file WHERE insurance_ref =InsF.insurance_ref)
	LEFT OUTER JOIN Insurance_File_System InsFS
		ON InsFS.Insurance_File_Cnt = InsF.Insurance_File_Cnt
	LEFT OUTER JOIN Risk_Code Risk_Code
	ON Risk_Code.Risk_Code_Id = InsF.Risk_Code_Id
	JOIN Currency Currency
	ON Currency.Currency_Id = Transdetail.Currency_Id

WHERE 
	Account.ledger_id = @iLedgerID
AND	
(
	@iBranchID = 0 
	OR 
	(@iBranchID > 0 AND Account.company_id = @iBranchID)
)

	
 

	-- Update Client Address
	UPDATE	#Report_Transaction 
	SET	account_address1 = Address.address1,
            	account_address2 = Address.address2,
            	account_address3 = Address.address3,
           	account_address4 = Address.address4,
            	account_postal_code = Address.postal_code,
            	phone_area_code = Account.phone_area_code,
            	phone_number = Account.phone_number
	FROM	#Report_Transaction	RT
        JOIN 	Account Account
            	ON  RT.account_id = Account.account_id
        JOIN 	Party
            	ON Account.account_key = P.party_cnt
        JOIN 	Party_Address_Usage
            	ON Party_Address_Usage.party_cnt = Party.party_cnt
        JOIN 	Address
            	ON Address.address_cnt = Party_Address_Usage.address_cnt
        JOIN 	Address_Usage_Type
            	ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id
            	AND Address_Usage_Type.code = '3131 XCO'

-- Identify matched amounts
UPDATE	#Report_Transaction
SET 	matched = 1 
WHERE 	match_total = gross_premium

-- Add the totals
DECLARE cAccount CURSOR FAST_FORWARD FOR
       	SELECT	A.account_id, P.resolved_name
        	FROM Account A, Party P
       	WHERE 	P.party_Cnt = A.account_key
       	AND EXISTS
	(
		SELECT NULL FROM #Report_Transaction WHERE account_id = A.account_id
	)

OPEN	cAccount
FETCH NEXT FROM cAccount INTO @iAccountID, @sAccountName
WHILE @@FETCH_STATUS = 0 
BEGIN
       	-- Get totals
       	SELECT 	@nAccountTotal = SUM(RT.gross_premium) + SUM(RT.match_total)
       	FROM 		#Report_Transaction RT
       	WHERE 	RT.account_id = @iAccountID

       	UPDATE	#Report_Transaction
       	SET 	account_total = @nAccountTotal,
	        	account_name = @sAccountName
        WHERE 	account_id = @iAccountID

        FETCH NEXT FROM cAccount INTO @iAccountID, @sAccountName
END

CLOSE 		cAccount
DEALLOCATE 	cAccount

UPDATE #Report_Transaction
SET client_premium = 0
WHERE client_premium IS NULL

-- Extract the data
SET NOCOUNT OFF
SELECT DISTINCT * FROM #Report_Transaction
WHERE (@sDebitOrCredit = '' OR
	((@sDebitOrCredit = 'Debit' AND account_total > 0) OR
	(@sDebitOrCredit = 'Credit' AND account_total < 0))) AND
	account_total <> 0
AND gross_premium <> 0
SET NOCOUNT ON

DELETE FROM	#Report_Transaction
DROP TABLE	#Report_Transaction

SET NOCOUNT OFF

GO
 

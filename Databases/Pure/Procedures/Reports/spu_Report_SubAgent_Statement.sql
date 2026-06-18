SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_SubAgent_Statement'
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

/***eck181102 Return policy holder name in place of account name ***/
/***eck011202 Put default account name when there is no policy holder ***/
/***eck 160603 PN4707 ***/

CREATE PROCEDURE spu_Report_SubAgent_Statement(
    @end_date datetime,
    @branch_id      integer,
    @party_code     varchar(20))
AS
-- spu_Report_SubAgent_Statement @end_date = '2006-10-01', @branch_id = 0 , @party_code = 'ALL'
DECLARE
	@dEndDate       datetime,
	@dPayByDate datetime,
	@iBranchID      integer,
	@sStatementType char(2),
	@iLedgerID      integer,
	@sDB            sysname,
	@iAccountID     integer,	
	@nAccountTotal  numeric(19,4),
	@nMatchTotal    numeric(19,4),
	@nUnallocated   numeric(19,4),
	@sAccountName   varchar(100),
	@sPartyCode     varchar(20),
	@iSALedgerId	integer, --'SA Ledger ID'
	@iFELedgerId 	integer --'Fee Ledger ID

SET NOCOUNT ON

SET ANSI_WARNINGS OFF

IF @party_code = 'ALL' BEGIN
	SELECT @party_code = NULL
END
SELECT @sPartyCode = ISNULL(@party_code, '')

SELECT @dEndDate = ISNULL(@end_date, GETDATE())
SELECT @iBranchID = ISNULL(@branch_id, 0)

SELECT @sStatementType = 'SA'

SELECT @iLedgerID = 10

SELECT @iSALedgerId =ledger_id FROM ledger WHERE ledger_short_name = 'SA'
SELECT @iFELedgerId =ledger_id FROM ledger WHERE ledger_short_name = 'FE'

-- Generate Temporary Table
CREATE TABLE    #Report_Transaction
(
    transdetail_id			int,
    document_id			    	int,
    account_id			    	int,
    documenttype_id			int,
    branch_id			    	int,
    ledger_id			    	int,
    account_code			char(30),
    account_name			varchar(100),
    document_ref			varchar(25),
    document_date			datetime,
    effective_date			datetime,
    account_address1			varchar(60),
    account_address2			varchar(60),
    account_address3			varchar(60),
    account_address4			varchar(60),
    account_postal_code			varchar(20),
    phone_area_code			char(10),
    phone_number			varchar(255),
    ledger_name			    	varchar(30),
    policy_number			varchar(30),
    transaction_type_code		varchar(10),
    transaction_type_description	varchar(255),
    policy_type_code			char(10),
    policy_type_description		varchar(255),
    gross_premium			numeric(19, 4),
    base_match_amount			numeric(19, 4),
    unallocated_amount			numeric(19, 4),
    account_total			numeric(19, 4),
    match_total				numeric(19, 4),
    date_paid				datetime,
    Branch_Name				varchar(255),
    Branch_address1			varchar(40),
    Branch_address2			varchar(40),
    Branch_address3			varchar(40),
    Branch_address4			varchar(40),
    Branch_postal_code			varchar(20),
    matched				int,
    client_code				char(20),
    client_name				varchar(100),
    client_premium			numeric(19, 4),
    client_match_amt			numeric(19, 4),
    client_matched			int,
    comment				varchar(255),
    number_of_days			int,
    reminder_type			varchar(100),
    transaction_reason			varchar(255),
    account_handler			varchar(100),
    policy_description			varchar(255),
    agency_account_number		varchar(255),
    ipt_amount				numeric(19, 4),
    client_doc_seq			int,
    fee_amount				numeric(19, 4),
    document_type			varchar(255)
)

CREATE TABLE #TempAmount
(
    account_id int,
    account_total money
)

CREATE INDEX I__#TempAmount__account_id ON #TempAmount (account_id)

-- Get the required transactions
INSERT INTO #Report_Transaction
(
	transdetail_id,		/* transdetail_id,  TransDetail.transdetail_id */
    	account_id,		/* account_id,  	Account.account_id */
    	account_code,		/* account_code,  	Account.short_code */
	document_id,
    	document_ref,		/* document_ref,    Document.document_ref */
    	document_date,		/* document_date,   Document.document_date */
    	effective_date,		/* extra_datetime1,    * TransDetail.ref_date */
    	ledger_name,		/* ledger_type,     Ledger.ledger_name */
    	policy_number,		/* policy_number,   TransDetail.insurance_ref */
    	documenttype_id,	/* documenttype_id,     Document.documenttype_id */
    	gross_premium,		/* amount,      TransDetail.amount */
    	branch_id,		/* branch_id,   Document.company_id */
    	matched,		/* extra_int1,  Match indicator */
    	match_total,		/* extra_numeric1,  Total match amount for transaction */
    	ledger_id,		/* record_type,     Account.ledger_id */
    	comment,		/* comment, */
    	transaction_reason,	/* extra_char3,	 Transaction_Export_Folder.reason */
    	account_handler,	/* extra_char4    Party.resolved_name - Account handler */
    	Branch_Name,
    	Branch_address1,
    	Branch_address2,
    	Branch_address3,
    	Branch_address4,
    	Branch_postal_code,
	transaction_type_code,
    	transaction_type_description,
	policy_description,
	agency_account_number,
	ipt_amount,
	client_doc_seq,
	fee_amount,
	document_type,
	account_name
)
SELECT 
	0, --TransDetail.transdetail_id,
    	Account.account_id,
    	Account.short_code,
	Document.document_id,
    	Document.document_ref,
    	Document.document_date,
    	TransDetail.ref_date,
    	Ledger.ledger_name,
    	TransDetail.insurance_ref,
    	Document.documenttype_id,
    	TransDetail.Amount,
    	Account.company_id,
    	0,
    	ISNULL((SELECT -SUM(AllocationDetail.alloc_base_amount) 
		FROM AllocationDetail AllocationDetail
        		INNER JOIN Allocation Allocation
            		ON AllocationDetail.allocation_id = Allocation.allocation_id
		WHERE Allocation.allocation_date <= @dEndDate
	        	AND TransDetail.transdetail_id = AllocationDetail.transdetail_id
        		AND AllocationDetail.alloc_base_amount <> 0
		AND AllocationDetail.allocationdetail_id IS NOT NULL
		),0),    	
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
	PartyAgent.agency_account_number, 
	ISNULL((
		SELECT TD1.ref_amount 
		FROM transdetail TD1 
		WHERE TD1.document_id = document.document_id
		AND TD1.Document_Sequence = (SELECT MIN(Document_Sequence) 
						FROM Transdetail TD2 
						JOIN account A1 
						ON TD2.account_id = A1.account_id
						WHERE TD1.document_id = TD2.document_id	
						AND A1.ledger_id = @iSALedgerId 
					    )
	     ),0),
	1,
	ISNULL((SELECT SUM(TD1.amount) 
		FROM transdetail TD1
		JOIN account A2
		ON A2.account_id = TD1.account_id
		WHERE TD1.document_id = document.document_id
		AND A2.ledger_id= @iFELedgerId 
		--GROUP BY TD1.account_id
	),0) * -1,
	DocumentType.Description,
	PT.resolved_name
	
FROM	Account Account
    	JOIN Ledger Ledger
        	ON Account.ledger_id = Ledger.ledger_id
        	AND (@sPartyCode = '' OR (@sPartyCode <> '' AND Account.short_code = @sPartyCode))
    	JOIN TransDetail TransDetail
        	ON Account.account_id = TransDetail.account_id
    	JOIN Document Document
        	ON TransDetail.document_id = Document.document_id
        	AND Document.document_date <= @dEndDate
    	JOIN Company Company
        	ON Document.company_id = Company.company_id
    	JOIN DocumentType DocumentType
        	ON Document.documenttype_id = DocumentType.documenttype_id
    	LEFT OUTER JOIN Transaction_Export_Folder
        	ON Document.document_ref = Transaction_Export_Folder.document_ref
        	AND Document.company_id = Transaction_Export_Folder.source_id
        	AND Transaction_Export_Folder.accounts_export_status = 'c'
    	LEFT OUTER JOIN Party PartyAH
        	ON PartyAH.party_cnt = Transaction_Export_Folder.account_handler_cnt
    	LEFT OUTER JOIN Party
        	ON Account.account_key = Party.party_cnt
	LEFT OUTER JOIN Insurance_File InsF
	     	ON InsF.Insurance_Ref = Transdetail.Insurance_Ref
		--DC150703 -ISS4923 -bring into line with 1.6.9
		AND InsF.Policy_Version = (select max(policy_version) from insurance_file 
					   where insurance_folder_cnt =InsF.insurance_folder_cnt)
		--DC150703
	LEFT OUTER JOIN Insurance_File_System InsFS
	    	ON InsFS.Insurance_File_Cnt = InsF.Insurance_File_Cnt
	JOIN Party_Agent PartyAgent
	    	ON PartyAgent.Party_Cnt = Party.Party_Cnt
	LEFT JOIN Party PT
            	ON PT.party_cnt=Account.account_key	
WHERE 
	Account.ledger_id = @iLedgerID
AND	
(
	@iBranchID = 0 
	OR 

	(@iBranchID > 0 AND Document.company_id = @iBranchID)
)
/*GROUP BY  Document.Company_id, Transdetail.Account_id, Transdetail.Document_id, Document.document_id*/

IF @sStatementType <> 'C ' 
BEGIN
	-- Get client details
    	UPDATE #Report_Transaction
	SET
        	client_code = Account.short_code,
--eck181102     client_name = Account.account_name,
		client_name = ISNULL(InsF.insured_name,Account.account_name),	--eck181102 --- Policy Holder ----
            	client_premium = Transdetail.amount,
            	client_match_amt =             

		(
			SELECT	SUM(AllocationDetail.alloc_base_amount)
                	FROM 	AllocationDetail AllocationDetail,
                		Allocation Allocation
                	WHERE 	AllocationDetail.transdetail_id = Transdetail.transdetail_id
                	AND 	Allocation.allocation_id = AllocationDetail.allocation_id
                	AND 	Allocation.allocation_date <= @dEndDate
		),
            	client_matched = 0
        	FROM 	#Report_Transaction RT
        	JOIN TransDetail TransDetail
            	ON TransDetail.document_id = RT.document_id
		AND Transdetail.Document_Sequence = (SELECT MIN(Document_Sequence) 
					FROM Transdetail TD2 
					JOIN account A1 
					ON TD2.account_id = A1.account_id
					WHERE TD2.document_id = RT.document_id	
					AND A1.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA' ))--PN4707)
        	JOIN Account Account
            	ON TransDetail.account_id = Account.account_id
--eck181102
		LEFT OUTER JOIN Insurance_File InsF
		ON InsF.Insurance_Ref = Transdetail.Insurance_Ref
		and InsF.policy_version = (SELECT MAX(policy_version) FROM insurance_file WHERE insurance_ref =InsF.insurance_ref)
--eck181102

		--DC150703 -ISS4923 -bring into line with 1,6.9
		LEFT OUTER JOIN Party P
		ON P.Party_Cnt = InsF.insured_cnt
		--DC150703

END

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
            	ON Account.account_key = Party.party_cnt
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
INSERT INTO #TempAmount
(
    account_id,
    account_total
)
SELECT 
    account_id,
    SUM(gross_premium)
FROM #Report_Transaction
GROUP BY account_id

UPDATE RT
SET RT.account_total = TA.account_total
FROM #Report_Transaction RT
JOIN #tempamount TA
ON RT.account_id=TA.account_id
 

--DC200303 do not include accounts that due nothing
DELETE FROM #Report_Transaction
WHERE account_total = 0

UPDATE #Report_Transaction
SET client_premium = 0
WHERE client_premium IS NULL

-- Extract the data
SET NOCOUNT OFF
SELECT DISTINCT * FROM	#Report_Transaction
ORDER BY branch_id, account_code, client_code, document_id
SET NOCOUNT ON

DROP TABLE	#TempAmount
DELETE FROM	#Report_Transaction
DROP TABLE	#Report_Transaction

SET NOCOUNT OFF

GO



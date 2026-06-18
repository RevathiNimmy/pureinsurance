SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Heritage_Balance_Statement'
GO

CREATE PROCEDURE spu_Report_Heritage_Balance_Statement
	@end_date		datetime,
	@branch_id		integer,
	@statement_type		varchar(20),
	@date_type		varchar(20),
	@party_code		varchar(20),
	@sIncludeInstalments	varchar(20)='',
	@start_risk		varchar(10),
	@end_risk		varchar(10)
AS

DECLARE
	@lBranchID		integer,
	@lLedgerID		integer,
	@lAccountID		integer,
	@lTransDetailID		integer,
	@dtEndDate		datetime,
	@sStatementType		char(2),
	@nAccountTotal		numeric(19, 4),
	@nMatchTotal		numeric(19, 4),
	@nUnallocated		numeric(19, 4),
	@sRiskCode		varchar(3),
	@sAccountName		varchar(100),
	@sPartyCode		varchar(20),
	@IncludeInstalments	int,
	@ledger_min		int,
	@ledger_max		int,
	@multi_company_id	int,
	@iStartRisk		integer,
	@iEndRisk		integer,
	@iRiskCode		integer,
	@iRiskRangeYN		integer

SET NOCOUNT ON

select @IncludeInstalments = 0
if @sIncludeInstalments = 'Include'
begin
	select @IncludeInstalments = 1
end

-- TF0230701 - Include party_code parameter
IF @party_code = 'ALL' BEGIN
	SELECT @party_code = NULL
END
SELECT @sPartyCode = ISNULL(@party_code, '')
SELECT @dtEndDate = ISNULL(@end_date, GETDATE())

SELECT @lBranchID = ISNULL(@branch_id, 0)

SELECT @statement_type = RTRIM(@statement_type)
SELECT @date_type = RTRIM(@date_type)
SELECT @sStatementType = (CASE
	WHEN @statement_type = 'client' THEN
		'C '
	WHEN @statement_type = 'insurer' THEN
		'I '
	WHEN @statement_type = 'agent' THEN
		'A '
	WHEN @statement_type = 'sub agent' THEN
		'SA'
	WHEN @statement_type = 'fees and discounts' THEN
		'F '
	WHEN @statement_type = 'purchase creditors' THEN
		'PU'
	ELSE
		'C '
	END)

SELECT @multi_company_id=0

SELECT @multi_company_id=value
FROM Hidden_options
WHERE option_number=16 AND branch_id=1

IF (@multi_company_id=1) BEGIN
	IF ISNULL(@branch_id,0)=0 BEGIN
 		SELECT @multi_company_id=1
--		SELECT @lBranchID=1
	END
	ELSE BEGIN
 		SELECT @multi_company_id=@lBranchID
	END
END
ELSE
 	SELECT @multi_company_id=1

-- DD 08/04/2004 get the ledger ids first for performance
SELECT @ledger_min=0

IF (@sStatementType = 'F ') BEGIN
	SELECT @ledger_min=ledger_id
	FROM ledger
	WHERE ledger_short_name = 'FE'
	AND company_id=@multi_company_id

	SELECT @ledger_max=ledger_id
	FROM ledger
	WHERE ledger_short_name = 'CO'
	AND company_id=@multi_company_id
END

IF (@sStatementType = 'C ') BEGIN
	SELECT @ledger_min=ledger_id
	FROM ledger
	WHERE ledger_short_name ='SA'
	AND company_id=@multi_company_id

	SELECT @ledger_max=@ledger_min
END

IF (@sStatementType = 'I ') BEGIN
	SELECT @ledger_min=ledger_id
	FROM ledger
	WHERE ledger_short_name ='IN'
	AND company_id=@multi_company_id

	SELECT @ledger_max=@ledger_min
END

IF (@sStatementType = 'A ') BEGIN
	SELECT @ledger_min=ledger_id
	FROM ledger
	WHERE ledger_short_name ='AG'
	AND company_id=@multi_company_id

	SELECT @ledger_max=@ledger_min
END

IF (@sStatementType = 'SA') BEGIN
	SELECT @ledger_min=ledger_id
	FROM ledger
	WHERE ledger_short_name ='UB'
	AND company_id=@multi_company_id

	SELECT @ledger_max=@ledger_min
END

IF (@sStatementType = 'PU') BEGIN
	SELECT @ledger_min=ledger_id
	FROM ledger
	WHERE ledger_short_name ='PU'
	AND company_id=@multi_company_id

	SELECT @ledger_max=@ledger_min
END

IF (@ledger_min=0) BEGIN
	SELECT @ledger_min=ledger_id
	FROM ledger
	WHERE ledger_short_name ='SA'
	AND company_id=@multi_company_id

	SELECT @ledger_max=@ledger_min
END

SELECT @iStartRisk = ISNULL(@start_risk, 0)
SELECT @iEndRisk = ISNULL(@end_risk, 0)
-- If Risk Range specified then will need different processing later.
IF @iStartRisk <> 0 AND @iEndRisk <> 0 BEGIN
	SELECT @iRiskRangeYN = 1
END
ELSE BEGIN
	SELECT @iRiskRangeYN = 0
END

	-- Empty the transaction table
DELETE Report_Transaction

	-- Get the required transactions
INSERT INTO Report_Transaction (
	transdetail_id,			/* TransDetail.transdetail_id */
	account_id,			/* Account.account_id */
	document_ref,			/* Document.document_ref */
	document_date,			/* Document.document_date */
	extra_datetime1,		/* TransDetail.ref_date */
	ledger_type,			/* Ledger.ledger_name */
	policy_number,			/* TransDetail.insurance_ref */
	documenttype_id,		/* Document.documenttype_id */
	extra_char1,			/* Risk_Code.code */
	extra_char2,			/* Risk_Code.description */
	amount,				/* TransDetail.amount */
	branch_id,			/* Account.company_id */
	extra_int1,			/* Match indicator */
	extra_numeric1,			/* Total match amount for transaction */
	record_type,			/* Account.ledger_id */
	comment,
	extra_char3,			/* Transaction_Export_Folder.reason */
	extra_char4			/* Party.resolved_name - Account handler */
)
SELECT
	TransDetail.transdetail_id,
	MAX(Account.account_id),
	MAX(Document.document_ref),
	MAX(Document.document_date),
	MAX(TransDetail.ref_date),
	MAX(Ledger.ledger_name),
	MAX(TransDetail.insurance_ref),
	MAX(Document.documenttype_id),
	MAX(Risk_Code.code),
	MAX(Risk_Code.description),
	MAX(ROUND(TransDetail.Amount,2)),
	MAX(Document.company_id),
	0,
	SUM(ROUND(AllocationDetail.alloc_base_amount,2)),
	MAX(Account.ledger_id),
	MAX(TransDetail.comment),
	LEFT(ISNULL(MAX(Document.reason), ''), 100),
	MAX(PartyAH.resolved_name)
FROM Account
	INNER JOIN Ledger
		ON Account.ledger_id = Ledger.ledger_id
	INNER JOIN TransDetail
		ON Account.account_id = TransDetail.account_id
	INNER JOIN Document
		ON TransDetail.document_id = Document.document_id
	INNER JOIN DocumentType
		ON Document.DocumentType_id=DocumentType.DocumentType_id
	LEFT OUTER JOIN (
		AllocationDetail
		INNER JOIN Allocation
			ON AllocationDetail.allocation_id = Allocation.allocation_id
			AND Allocation.allocation_date <= @dtEndDate
		)
		ON TransDetail.transdetail_id = AllocationDetail.transdetail_id
		AND AllocationDetail.allocationdetail_id IS NOT NULL
		AND AllocationDetail.alloc_base_amount <> 0
	LEFT OUTER JOIN Insurance_File
		ON Insurance_File.insurance_file_cnt = Document.insurance_file_cnt
	LEFT OUTER JOIN Risk_Code
		ON Insurance_File.risk_code_id = Risk_Code.risk_code_id
	LEFT OUTER JOIN Party PartyAH
		ON PartyAH.party_cnt = Insurance_File.account_handler_cnt
WHERE
	(
		@IncludeInstalments=1
		OR
		(
		@IncludeInstalments=0
		AND
		DocumentType.Code NOT IN ('IDR','ICR')
			AND NOT EXISTS
			(
				SELECT NULL
				FROM AllocationDetail origtm
				JOIN AllocationDetail tm
					ON tm.allocation_id = origtm.allocation_id
					AND tm.allocationdetail_id <> origtm.allocationdetail_id
				JOIN transdetail td
					ON td.transdetail_id = tm.transdetail_id
				JOIN document d
					ON d.document_id = td.document_id
				JOIN documenttype dt
					ON dt.documenttype_id = d.documenttype_id
				WHERE origtm.transdetail_id = TransDetail.transdetail_id
					AND dt.code IN ('IDR','ICR')
			)
		)
	)
	AND Ledger.LedgerType_id BETWEEN @ledger_min AND @ledger_max
--	AND Account.ledger_id BETWEEN @ledger_min AND @ledger_max
	AND (@lBranchID = 0 OR Document.company_id = @lBranchID)
	AND (@sPartyCode = '' OR Account.short_code = @sPartyCode)
	-- 1.6.9 start
	AND
	(
		(
			Document.document_date <= @dtEndDate
			AND NOT
			(
				TransDetail.spare IN ('COMM ADJ', 'AGENT ADJ')
				OR
				TransDetail.document_sequence IN (
						SELECT document_sequence + 1
						FROM TransDetail
						WHERE document_id = Document.document_id
							AND spare = 'COMM ADJ'
						)
			)
		)
		OR
		(
			TransDetail.ref_date <= @dtEndDate
			AND
			(
				TransDetail.spare IN ('COMM ADJ', 'AGENT ADJ')
				OR
				TransDetail.document_sequence IN (
						SELECT document_sequence + 1
						FROM TransDetail
						WHERE document_id = Document.document_id
							AND spare = 'COMM ADJ'
						)
			)
		)
        )
-- If risk range not specified then Payments and Receipts will cancel out any transactions they match
-- against. Otherwise, Payments and Receipts do not have a Risk Code (are unallocated) so are filtered
-- out here.
	AND ( @iRiskRangeYN = 0 OR
		( @iRiskRangeYN <> 0
			AND Document.DocumentType_id NOT IN
			(
				SELECT DocumentType_id
				FROM	DocumentType
				WHERE	Description like '%Receipt%'
					OR Description like '%Payment%'
			)
		)
	)

-- 1.6.9 end
GROUP BY TransDetail.transdetail_id

SELECT @ledger_min=ledger_id
FROM ledger
WHERE ledger_short_name = 'SA'
	AND company_id=@multi_company_id

IF @sStatementType <> 'C ' BEGIN
-- 1.6.9
	SELECT @lTransDetailID = Transdetail.transdetail_id
	FROM Report_Transaction RT
	INNER JOIN Document ON RT.document_ref = Document.document_ref
	INNER JOIN TransDetail ON Document.document_id = TransDetail.document_id
	INNER JOIN Account ON TransDetail.account_id = Account.account_id
	--DC130603 -ISS4707
	--WHERE Account.ledger_id = 2
	WHERE Account.ledger_id=@ledger_min

	IF EXISTS (
		SELECT NULL
		FROM Report_Transaction
		WHERE transdetail_id = @lTransDetailID
		)
	BEGIN
		-- 1.6.9
		-- Get client transactions
		INSERT INTO Report_Transaction (
			transdetail_id,
			account_id,
			account_code,
			account_name,
			document_ref,
			amount,
			extra_int1,
			extra_numeric1,
			record_type
			)
		SELECT
			TransDetail.transdetail_id,
			Account.account_id,
			Account.short_code,
			Account.account_name,
			Document.document_ref,
			ROUND(TransDetail.amount,2),
			0,
			(
				SELECT SUM(ROUND(AllocationDetail.alloc_base_amount,2))
				FROM AllocationDetail
				INNER JOIN Allocation ON AllocationDetail.allocation_id = Allocation.allocation_id
				WHERE AllocationDetail.transdetail_id = TransDetail.transdetail_id
				AND Allocation.allocation_date <= @dtEndDate
			),
			2
		FROM Report_Transaction RT
			INNER JOIN Document ON RT.document_ref = Document.document_ref
			INNER JOIN TransDetail ON Document.document_id = TransDetail.document_id
			INNER JOIN Account ON TransDetail.account_id = Account.account_id
			INNER JOIN AllocationDetail ON TransDetail.transdetail_id = AllocationDetail.transdetail_id
			INNER JOIN Allocation ON AllocationDetail.allocation_id = Allocation.allocation_id
			--DC130603 -ISS4707
			--WHERE Account.ledger_id = 2
		WHERE Account.ledger_id=@ledger_min
			AND Allocation.allocation_date <= @dtEndDate
			AND AllocationDetail.AllocationDetail_Id IS NOT NULL
		GROUP BY
			TransDetail.transdetail_id,
			Account.account_id,
			Account.short_code,
			Account.account_name,
			Document.document_ref,
			Transdetail.amount
	END
END

-- Identify matched amounts
UPDATE Report_Transaction
	SET extra_int1 = 1
	WHERE extra_numeric1 = amount

-- Convert the risk code to a numeric so Crystal sorts correctly later.
UPDATE Report_Transaction
	SET extra_int3 = extra_char1
	WHERE extra_char1 >= '0' AND extra_char1 <= '9999'

-- Delete Transactions that are matched (extra_int1 = 1) and/or are outside the range of risk codes required.
DELETE FROM Report_Transaction
	WHERE 	(@iRiskRangeYN = 1
			AND (extra_int3 < @iStartRisk OR extra_int3 > @iEndRisk OR extra_int1 = 1)
		)

-- Add the totals
DECLARE curAccounts CURSOR FAST_FORWARD FOR
	SELECT A.account_id, A.account_name
		FROM Account A
		WHERE A.account_id IN (SELECT account_id FROM Report_Transaction)

OPEN curAccounts
FETCH NEXT FROM curAccounts INTO @lAccountID, @sAccountName
WHILE @@FETCH_STATUS = 0 BEGIN

-- Get totals
	IF @iRiskRangeYN = 0
	BEGIN
		SELECT  @nAccountTotal = SUM(RT2.Amount),
			@sRiskCode = MAX(RT2.Extra_Char1)--, RT.*
		from Report_Transaction RT
		join Report_Transaction RT2
			ON RT.Extra_Char1 = RT2.Extra_Char1
			AND RT.Account_id = RT2.Account_id
		where RT.Account_id = @lAccountID
		and RT.Extra_Char1 = RT2.Extra_Char1
		GROUP BY RT.TransDetail_id

	END
	ELSE BEGIN

		SELECT @nAccountTotal =
			ISNULL((SELECT SUM(RT.Amount)
				FROM Report_Transaction RT
				WHERE RT.account_id = @lAccountID
					AND (@iStartRisk = 0 OR @iEndRisk = 0
						OR ( RT.Extra_Int3 >= @iStartRisk AND RT.Extra_Int3 <= @iEndRisk)
--						OR ( RT.Extra_Char1 >= @iStartRisk AND RT.Extra_Char1 <= @iEndRisk)
				)),0.0)
			-					-- Minus
			ISNULL((SELECT SUM(TM.alloc_base_amount)
				FROM Report_Transaction RT
				JOIN AllocationDetail TM
					ON RT.transdetail_id = TM.transdetail_id
						AND TM.alloc_base_amount <> 0
						AND TM.allocationdetail_id IS NOT NULL
				JOIN Allocation MG
					ON TM.allocation_id = MG.allocation_id
						AND MG.allocation_date <= @dtEndDate
			        WHERE RT.account_id = @lAccountID),0.0)
	END

	SELECT @nMatchTotal = SUM(ROUND(TM.alloc_base_amount,2))
	FROM Report_Transaction RT
		INNER JOIN AllocationDetail TM
			ON RT.transdetail_id = TM.transdetail_id
				AND TM.alloc_base_amount <> 0
				AND TM.allocationdetail_id IS NOT NULL -- 1.6.9
		INNER JOIN Allocation MG
			ON TM.allocation_id = MG.allocation_id
				AND MG.allocation_date <= @dtEndDate
	WHERE RT.account_id = @lAccountID

	IF @iRiskRangeYN = 0
	BEGIN
		SELECT @nUnallocated =
			ISNULL((SELECT SUM(RT.Amount)
				FROM Report_Transaction RT
				WHERE RT.account_id = @lAccountID
					AND (RT.documenttype_id BETWEEN 22 AND 23)), 0.0)
			+
			ISNULL((SELECT SUM(RT.Amount)
				FROM Report_Transaction RT
				WHERE RT.account_id = @lAccountID
					AND (RT.documenttype_id BETWEEN 28 AND 29)), 0.0)
			+
			ISNULL((SELECT SUM(TM.alloc_base_amount)
				FROM Report_Transaction RT
				JOIN AllocationDetail TM
					ON RT.transdetail_id = TM.transdetail_id
						AND TM.alloc_base_amount <> 0
				JOIN Allocation MG
					ON TM.allocation_id = MG.allocation_id
						AND MG.allocation_date <= @dtEndDate
		WHERE RT.account_id = @lAccountID
			AND TM.allocationdetail_id IS NOT NULL
			AND RT.documenttype_id NOT IN (22, 23, 28, 29)), 0.0)
	END
	ELSE BEGIN
		SELECT @nUnallocated = 0
	END

	UPDATE Report_Transaction
		SET 	extra_numeric2 = @nUnallocated,
			extra_numeric3 = @nAccountTotal,
			extra_numeric4 = @nMatchTotal,
			account_name = @sAccountName
		WHERE account_id = @lAccountID

FETCH NEXT FROM curAccounts INTO @lAccountID, @sAccountName
END
CLOSE curAccounts
DEALLOCATE curAccounts

-- Calculate the date differences
IF @date_type = 'effective date' BEGIN
	UPDATE Report_Transaction
		SET extra_int2 = DATEDIFF(day, extra_datetime1, @dtEndDate)
	END
ELSE 	IF @date_type = 'transaction date' BEGIN
		UPDATE Report_Transaction
			SET extra_int2 = DATEDIFF(day, document_date, @dtEndDate)
	END

-- Extract the data
SET NOCOUNT OFF

IF @sStatementType IN ('C ') BEGIN
	SELECT
		RT.transdetail_id		transdetail_id,
		Account.short_code		account_code,
		RT.account_name			account_name,
		RT.document_ref			document_ref,
		RT.document_date		document_date,
		RT.extra_datetime1		Effective_Date,
		Address.address1		account_address1,
		Address.address2		account_address2,
		Address.address3		account_address3,
		Address.address4		account_address4,
		Address.postal_code		account_postal_code,
		Account.phone_area_code		phone_area_code,
		Account.phone_number		phone_number,
		RT.ledger_type			ledger_name,
		RT.policy_number		policy_number,
		DocumentType.code		transaction_type_code,
		DocumentType.description	transaction_type_description,
		RT.extra_int3			policy_type_code,
		RT.extra_char2			policy_type_description,
		RT.amount			gross_premium,
		AllocationDetail.alloc_base_amount	alloc_base_amount,
		RT.extra_numeric2		unallocated_amount,
		RT.extra_numeric3		account_total,
		RT.extra_numeric4		match_total,
		Allocation.allocation_date		date_paid,
		Company.description		Branch_Name,
		Company.address1		Branch_address1,
		Company.address2		Branch_address2,
		Company.address3		Branch_address3,
		Company.address4		Branch_address4,
		Company.postal_code		Branch_postal_code,
		AllocationDetail.allocationdetail_id	allocationdetail_id,
		RT.extra_int1			matched,
		Account.short_code		client_code,
		Account.account_name		client_name,
		RT.amount			client_premium,
		RT.extra_numeric1		client_match_amt,
		RT.extra_int1			client_matched,
		RT.comment,
		RT.extra_int2			number_of_days,
		Reminder_Type.description	reminder_type,
		Party.file_code filecode, 	--1.6.9
		RT.extra_char3			transaction_reason,
		RT.extra_char4			account_handler
	FROM Report_Transaction RT
		INNER JOIN Account
			ON RT.account_id = Account.account_id
				AND (@iStartRisk = 0 OR @iEndRisk = 0
				OR (RT.Extra_Int3 >= @iStartRisk AND RT.Extra_Int3 <= @iEndRisk)
				)
		INNER JOIN Company
			ON Account.company_id = Company.company_id
		INNER JOIN DocumentType
			ON RT.documenttype_id = DocumentType.documenttype_id
		INNER JOIN Party
			ON Account.account_key = Party.party_cnt
		INNER JOIN Party_Address_Usage
			ON Party_Address_Usage.party_cnt = Party.party_cnt
		INNER JOIN Address
			ON Address.address_cnt = Party_Address_Usage.address_cnt
		INNER JOIN Address_Usage_Type
			ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id
				AND Address_Usage_Type.code = '3131 XCO'
		LEFT OUTER JOIN (
			AllocationDetail
				INNER JOIN Allocation
					ON AllocationDetail.allocation_id = Allocation.allocation_id
						AND Allocation.allocation_date <= @dtEndDate
				)
			ON RT.transdetail_id = AllocationDetail.transdetail_id
				AND AllocationDetail.alloc_base_amount <> 0
				AND AllocationDetail.allocationdetail_id IS NOT NULL
		LEFT OUTER JOIN Reminder_Type
			ON Reminder_Type.reminder_type_id = Party.reminder_type_id
 ORDER BY account_code, transdetail_id, allocationdetail_id  
END ELSE BEGIN
	SELECT 	RT.transdetail_id transdetail_id,
		Account.short_code account_code,
		RT.account_name account_name,
		RT.document_ref document_ref,
		RT.document_date document_date,
		RT.extra_datetime1 Effective_Date,
		Account.address1 account_address1,
		Account.address2 account_address2,
		Account.address3 account_address3,
		Account.address4 account_address4,
		Account.postal_code account_postal_code,
		Account.phone_area_code phone_area_code,
		Account.phone_number phone_number,
		RT.ledger_type ledger_name,
		RT.policy_number policy_number,
		DocumentType.code transaction_type_code,
		DocumentType.description transaction_type_description,
		RT.extra_int3 policy_type_code,
		RT.extra_char2 policy_type_description,
		RT.amount gross_premium,
		AllocationDetail.alloc_base_amount alloc_base_amount,
		RT.extra_numeric2 unallocated_amount,
		RT.extra_numeric3 account_total,
		RT.extra_numeric4 match_total,
		Allocation.allocation_date date_paid,
		Company.description Branch_Name,
		Company.address1 Branch_address1,
		Company.address2 Branch_address2,
		Company.address3 Branch_address3,
		Company.address4 Branch_address4,
		Company.postal_code Branch_postal_code,
		NULL, --ISNULL(TransMatch.transmatch_id, 0) transmatch_id,
		RT.extra_int1 matched,
		Client.account_code client_code,
		Client.account_name client_name,
		Client.amount client_premium,
		Client.extra_numeric1 client_match_amt,
		Client.extra_int1 client_matched,
		RT.comment,
		RT.extra_int2 number_of_days,
		NULL reminder_type,
		'' filecode,                        -- 1.6.9
		RT.extra_char3 transaction_reason,
		RT.extra_char4 account_handler
	FROM Report_Transaction RT
		INNER JOIN Account
			ON RT.account_id = Account.account_id
				AND (@iStartRisk = 0 OR @iEndRisk = 0
					OR (RT.Extra_Int3 >= @iStartRisk AND RT.Extra_Int3 <= @iEndRisk)
				)
		INNER JOIN Company
			ON Account.company_id = Company.company_id
		INNER JOIN DocumentType
			ON RT.documenttype_id = DocumentType.documenttype_id
		LEFT OUTER JOIN (
			AllocationDetail
				INNER JOIN Allocation
					ON AllocationDetail.allocation_id = Allocation.allocation_id
						AND Allocation.allocation_date <= @dtEndDate
				)
			ON RT.transdetail_id = AllocationDetail.transdetail_id
				AND AllocationDetail.alloc_base_amount <> 0
				AND AllocationDetail.allocationdetail_id IS NOT NULL
		LEFT OUTER JOIN Report_Transaction Client
			ON RT.document_ref = Client.document_ref
				AND Client.record_type = 2
 ORDER BY account_code, transdetail_id  
END
SET NOCOUNT ON

DELETE FROM Report_Transaction

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
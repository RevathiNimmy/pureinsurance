if exists (select * from sysobjects where id = object_id(N'[dbo].[sp_Report_Commission_Earnt_RH2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_Commission_Earnt_RH2]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO





CREATE PROCEDURE sp_Report_Commission_Earnt_RH2

	@branch_id	int,
	@start_date	datetime,
	@end_date	datetime
AS

DECLARE	
	@iBranchID		int,
	@iTransdetailID1	int,
	@iTransdetailID2	int,
	@sSpare			varchar(30),
	@iDocumentID		int,
	@sDocumentRef		varchar(30),
	@dtDocumentDate		datetime,
	@sShortCode		varchar(30),
	@sAccountName		varchar(30),
	@iCompanyID		int,
	@nAmount		numeric(19, 4),
	@iOrigDocumentID	int,
	@sOrigDocumentRef		varchar(30),
	@iOrigAccountID		varchar(30),
	@sOrigShortCode		varchar(30),
	@sOrigAccountName	varchar(30),	
	@nOrigAmount		numeric(19, 4),	
	@sOrigDocumentType	varchar(30),
	@sPolicyRef		varchar(30),
	@sClientName		varchar(30),
	@sClientCode	varchar(30),
	@dtEffectiveDate	datetime,
	@sAccountHandler	varchar(30)


SELECT 	@iBranchID = ISNULL(@branch_id, 0)


-- Set up empty Temporary Table
SELECT	@sDocumentRef		document_ref,
	@dtDocumentDate		document_date,
	@sShortCode		income_acc_code,
	@sAccountName		income_acc_name,
	@nAmount		income_acc_amount,
	@sOrigDocumentRef	original_document_ref,
	@sOrigDocumentType	original_document_type,
	@sOrigShortCode		original_acc_code,
	@sOrigAccountName	original_acc_name,
	@nOrigAmount		original_acc_amount,
	@sPolicyRef		policy_no,
	@iCompanyID		branch_id,
	@sClientName		account_name,
	@sClientCode	account_code,
	@dtEffectiveDate	effective_date,
	@sAccountHandler	account_handler
INTO 	#Report_Commission_Earnt 
WHERE	@sDocumentRef = 'X'

-- Get all transactions in Commission Earned accounts

DECLARE	c_cursor SCROLL CURSOR FOR 

SELECT 	T.transdetail_id,
	D.document_id,
	D.document_ref,
	D.document_date,
	A.short_code,
	A.account_name,
	A.company_id,
	T.amount
FROM	Orion_For_Broking.dbo.Transdetail	T,
	Orion_For_Broking.dbo.Document		D,
	Orion_For_Broking.dbo.StructureTree	S1,
	Orion_For_Broking.dbo.StructureTree	S2,
	Orion_For_Broking.dbo.Element		E,
	Orion_For_Broking.dbo.Account		A
WHERE	D.document_date <= @end_date
AND	T.document_id = D.document_id
AND	E.element_name = 'Comm Inc Earned'
AND	S2.element_id = E.element_id
AND	S1.parent_node_id = S2.node_id
AND	T.account_id = S1.account_id
AND	A.account_id = T.account_id
AND
(	
	(
		@iBranchID = 0
	)
	OR
	(
		@iBranchID <> 0
		AND	A.company_id = @iBranchID
	)
)
AND
(
	(
		(
		SELECT	ISNULL(spare, '')
		FROM		orion_for_broking..transdetail
		WHERE	document_id = T.document_id
		AND		document_sequence = T.document_sequence - 1
		) <> 'COMM ADJ'
		AND	T.spare <> 'AGENT ADJ'
	)
	OR
	(
		((
		SELECT	ISNULL(spare, '')
		FROM		orion_for_broking..transdetail
		WHERE	document_id = T.document_id
		AND		document_sequence = T.document_sequence - 1
		) = 'COMM ADJ'
		OR	T.spare = 'AGENT ADJ'
		)
		AND ref_date >= @start_date
		AND ref_date <= @end_date
	)
)

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO	@iTransdetailID1,
				@iDocumentID,
				@sDocumentRef,
				@dtDocumentDate,
				@sShortCode,
				@sAccountName,
				@iCompanyID,
				@nAmount

WHILE	@@FETCH_STATUS = 0
BEGIN
	-- Get corresponding Commission account transaction
	SELECT	@iTransdetailID2 = T2.transdetail_id,
		@sSpare = ISNULL(T2.spare, ''),
		@iOrigAccountID = A.account_id,
		@sOrigShortCode	= A.short_code,
		@sOrigAccountName = A.account_name,
		@nOrigAmount = ISNULL(T2.amount, 0)
	FROM	Orion_For_Broking.dbo.Transdetail	T1,
		Orion_For_Broking.dbo.Transdetail	T2,
		Orion_For_Broking.dbo.Account		A
	WHERE	
		T1.transdetail_id = @iTransdetailID1
	AND	T2.document_id = T1.document_id
	AND	T2.transdetail_id = T1.transdetail_id -1 
/*
	AND	T2.spare = T1.spare
		OR
		(
			T2.spare = 'COMM ADJ'
			AND
			T2.transdetail_id = T1.transdetail_id - 1
		)
		)
*/			
	AND	A.account_id = T2.account_id

	-- Get Data from original document
	SELECT	@sClientName = '',
		@sClientCode = '',
		@iOrigDocumentID = 0,
		@sOrigDocumentRef = '',
		@sOrigDocumentType = '',
		@sPolicyRef = '',
		@dtEffectiveDate = NULL,
		@sAccountHandler = ''

	IF RTRIM(@sSpare) LIKE  'COMM PAY%'
	BEGIN
		SELECT	@sClientName = P.resolved_name,
			@sClientCode = P.shortname,
			@iOrigDocumentID = D.document_id,
			@sOrigDocumentRef = EF.document_ref,
			@sOrigDocumentType = ISNULL(EF.transaction_type_code, ''),
			@dtEffectiveDate = EF.cover_start_date,
			@sPolicyRef = EF.insurance_ref,
			@sAccountHandler = EF.account_handler_shortname
		FROM	Orion_For_Broking.dbo.Document 				D,
			Orion_For_Broking.dbo.Transdetail 			T,
			Orion_For_Broking.dbo.Account				A,
			Party							P,
			Sirius_For_Broking.dbo.Transaction_Export_Folder	EF
		WHERE	D.document_ref = SUBSTRING(@sSpare, 10, 11)
		AND	T.document_id = D.document_id
		AND	A.account_id = T.account_id
		AND	A.ledger_id = 2
		AND	A.account_key = ((P.source_id - 1) * 268435456) + P.party_id
		AND	EF.document_ref = D.document_ref
		AND	EF.source_id = D.company_id
	END
	ELSE
	BEGIN
		SELECT	@sClientName = P.resolved_name,
			@sClientCode = P.shortname,
			@iOrigDocumentID = D.document_id,
			@sOrigDocumentRef = EF.document_ref,
			@sOrigDocumentType = ISNULL(EF.transaction_type_code, ''),
			@dtEffectiveDate = EF.cover_start_date,
			@sPolicyRef = EF.insurance_ref,
			@sAccountHandler = EF.account_handler_shortname
		FROM	Orion_For_Broking.dbo.Document 				D,
			Orion_For_Broking.dbo.Transdetail 			T,
			Orion_For_Broking.dbo.Account				A,
			Party							P,
			Sirius_For_Broking.dbo.Transaction_Export_Folder	EF
		WHERE	D.document_id = @iDocumentID
		AND	T.document_id = D.document_id
		AND	A.account_id = T.account_id		
		AND	A.ledger_id = 2
		AND	A.account_key = ((P.source_id - 1) * 268435456) + P.party_id
		AND	EF.document_ref = D.document_ref
		AND	EF.source_id = D.company_id
	END

	-- Effective Date for COMM ADJ
	IF	(
		RTRIM(@sSpare)  = 'COMM ADJ'
		OR
		RTRIM(@sSpare)  = 'AGENT ADJ'
		)
	BEGIN
		SELECT	@dtEffectiveDate = ref_date
		FROM		Orion_For_Broking.dbo.Transdetail
		WHERE	transdetail_id = @iTransdetailID1

		SELECT	@nAmount = amount
		FROM		Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Account		A
		WHERE	transdetail_id = @iTransdetailID1
		AND		A.account_id = T.account_id 
		AND		A.short_code = @sShortCode
	END

	SELECT	@nOrigAmount = SUM(amount)
	FROM	Orion_For_Broking.dbo.Transdetail			T,
		Orion_For_Broking.dbo.Document				D
	WHERE	T.document_id = @iOrigDocumentID
	AND	T.account_id = @iOrigAccountID
	AND	D.document_id = T.document_id

	-- Set unknown Effective date to Doc date
	IF @dtEffectiveDate IS NULL
		SELECT @dtEffectiveDate = @dtDocumentDate

	-- Insert values into Temporary Table

	IF
	(
		(
		@dtDocumentDate >= @start_date 
		AND
		@dtDocumentDate <= @end_date 
		AND
		@dtEffectiveDate <= @end_date
		)
		OR
		(
		@dtDocumentDate < @start_date 
		AND
		@dtEffectiveDate >= @start_date
		AND
		@dtEffectiveDate <= @end_date
		)
	)
--	AND	@nAmount <> 0


		INSERT INTO 	#Report_Commission_Earnt
		VALUES
		(
			@sDocumentRef,		
			@dtDocumentDate,		
			@sShortCode,		
			@sAccountName,		
			@nAmount,		
			@sOrigDocumentRef,
			@sOrigDocumentType,
			@sOrigShortCode,		
			@sOrigAccountName,	
			@nOrigAmount,		
			@sPolicyRef,
			@iCompanyID,		
			@sClientName,
			@sClientCode,
			@dtEffectiveDate,
			@sAccountHandler
		)


	FETCH NEXT FROM c_cursor INTO	@iTransdetailID1,
					@iDocumentID,
					@sDocumentRef,
					@dtDocumentDate,
					@sShortCode,
					@sAccountName,
					@iCompanyID,
					@nAmount
END

CLOSE 		c_cursor
DEALLOCATE	c_cursor

-- Return data from Temporary Table
SELECT	* 
FROM 	#Report_Commission_Earnt

DROP TABLE	#Report_Commission_Earnt
































GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


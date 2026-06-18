SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[sp_Report_Commission_Earnt]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_Commission_Earnt]
GO





/****** Object:  Stored Procedure dbo.sp_Report_Commission_Earnt    Script Date: 16/10/00 12:18:29 ******/


/****** Object:  Stored Procedure dbo.sp_Report_Commission_Earnt    Script Date: 25/05/00 16:42:33 ******/
CREATE PROCEDURE sp_Report_Commission_Earnt

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
	@iOrigAccountID		varchar(30),
	@sOrigShortCode		varchar(30),
	@sOrigAccountName	varchar(30),	
	@nOrigAmount		numeric(19, 4),	
	@sClientName		varchar(30),
	@iOrigDocTypeID		int,
	@iIsDirectToInsurer	int

SELECT 	@iBranchID = ISNULL(@branch_id, 0)


-- Set up empty Temporary Table
SELECT	@sDocumentRef		document_ref,
	@dtDocumentDate		document_date,
	@sShortCode		income_acc_code,
	@sAccountName		income_acc_name,
	@nAmount		income_acc_amount,
	@sOrigShortCode		original_acc_code,
	@sOrigAccountName	original_acc_name,
	@nOrigAmount		original_acc_amount,
	@iCompanyID		branch_id,
	@sClientName		account_name,
	@iIsDirectToInsurer	is_direct_to_ins
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
FROM	Orion_For_Broking.dbo.Transdetail		T,
	Orion_For_Broking.dbo.Document		D,
	Orion_For_Broking.dbo.StructureTree		S1,
	Orion_For_Broking.dbo.StructureTree		S2,
	Orion_For_Broking.dbo.Element		E,
	Orion_For_Broking.dbo.Account		A
WHERE	
	T.document_id = D.document_id
AND	E.element_name = 'Comm Inc Earned'
AND	S2.element_id = E.element_id
AND	S1.parent_node_id = S2.node_id
AND	T.account_id = S1.account_id
AND	A.account_id = T.account_id
AND
(
	T.ref_date >= @start_date
	AND	T.ref_date <= @end_date
)
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
	SELECT	@iTransdetailID2 = 0,
		@sSpare = '',
		@iOrigAccountID = 0,
		@sOrigShortCode	= '',
		@sOrigAccountName = '',
		@nOrigAmount = 0

	SELECT	@iTransdetailID2 = T2.transdetail_id,
		@sSpare = ISNULL(T1.spare, ''),
		@iOrigAccountID = A.account_id,
		@sOrigShortCode	= A.short_code,
		@sOrigAccountName = A.account_name,
		@nOrigAmount = T2.amount
	FROM	Orion_For_Broking.dbo.Transdetail	T1,
		Orion_For_Broking.dbo.Transdetail	T2,
		Orion_For_Broking.dbo.Account		A
	WHERE	T1.transdetail_id = @iTransdetailID1
	AND	T2.document_id = T1.document_id
	AND	T2.transdetail_id <> T1.transdetail_id
	AND	T2.spare = T1.spare
--	AND	T2.document_sequence = T1.document_sequence + 1
	AND	A.account_id = T2.account_id
	AND	T2.ref_date >= @start_date
	AND	T2.ref_date <= @end_date

	-- Get Data from original document
	SELECT	@sClientName = '',
		@iOrigDocumentID = 0,
		@iOrigDocTypeID = 0
		

	IF RTRIM(@sSpare) LIKE 'COMM PAY%'
	BEGIN
		SELECT	@sClientName = P.resolved_name,
			@iOrigDocumentID = D.document_id,
			@iOrigDocTypeID  = D.documenttype_id
		FROM	Orion_For_Broking.dbo.Document 		D,
			Orion_For_Broking.dbo.Transdetail 	T,
			Orion_For_Broking.dbo.Account		A,
			Party					P
		WHERE	D.document_ref = SUBSTRING(@sSpare, 10, 11)
		AND	T.document_id = D.document_id
		AND	A.account_id = T.account_id
		AND	A.ledger_id = 2
		AND	A.account_key = ((P.source_id - 1) * 268435456) + P.party_id
		END
	ELSE
	BEGIN
		SELECT	@sClientName = P.resolved_name,
			@iOrigDocumentID= @iDocumentID,
			@iOrigDocTypeID  = D.documenttype_id
		FROM	Orion_For_Broking.dbo.Document 		D,
			Orion_For_Broking.dbo.Transdetail 	T,
			Orion_For_Broking.dbo.Account		A,
			Party					P
		WHERE	D.document_id = @iDocumentID
		AND	T.document_id = D.document_id
		AND	A.account_id = T.account_id
		AND	A.ledger_id = 2
		AND	A.account_key = ((P.source_id - 1) * 268435456) + P.party_id
	END

	SELECT	@nOrigAmount = SUM(amount)
	FROM	Orion_For_Broking.dbo.Transdetail
	WHERE	document_id = @iOrigDocumentID
	AND	account_id = @iOrigAccountID
	AND	ref_date >= @start_date
	AND	ref_date <= @end_date


	-- Check if Direct To Insurer
	IF EXISTS 
	(	SELECT	* 
		FROM	Orion_For_Broking.dbo.AllocationDetail	A,
			Orion_For_Broking.dbo.Transdetail	T,
			Orion_For_Broking.dbo.Document	D,
			Orion_For_Broking.dbo.Account	C
		WHERE	T.transdetail_id = A.transdetail_id
		AND	D.document_id = T.document_id
		AND	D.document_id = @iOrigDocumentID
		AND	C.account_id = T.account_id
		AND	C.ledger_id = 2
		AND		
		(
			SELECT	D1.documenttype_id
			FROM	Orion_For_Broking.dbo.AllocationDetail	A1,
				Orion_For_Broking.dbo.Transdetail	T1,
				Orion_For_Broking.dbo.Document	D1
			WHERE	A1.allocation_id = A.allocation_id
			AND	A1.allocationdetail_id = A.allocationdetail_id  + 1
			AND	T1.transdetail_id = A1.transdetail_id
			AND	D1.document_id = T1.document_id
		) IN (33, 34)
	)
	BEGIN
		SELECT	@iIsDirectToInsurer = 1
		-- Check if Insurer Paid
		IF 
		(	
			SELECT	SUM(alloc_base_amount)
			FROM	Orion_For_Broking.dbo.AllocationDetail A,
				Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Document	D
			WHERE	A.transdetail_id = T.transdetail_id 
			AND	D.document_id = T.document_id
			AND	D.document_id = @iOrigDocumentID
			AND	T.transdetail_id = 
				(
				SELECT	MIN(T1.transdetail_id)
				FROM	Orion_For_Broking.dbo.Transdetail	T1,
					Orion_For_Broking.dbo.Account		A1
				WHERE	T1.document_id = @iOrigDocumentID
				AND	A1.account_id = T1.account_id 
				AND	A1.ledger_id =4
				)
	
		) = 
		(	SELECT	T.amount			
			FROM	Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Document	D
			WHERE	D.document_id = T.document_id
			AND	D.document_id = @iOrigDocumentID
			AND	T.transdetail_id = 
				(
				SELECT	MIN(T1.transdetail_id)
				FROM	Orion_For_Broking.dbo.Transdetail	T1,
					Orion_For_Broking.dbo.Account		A1
				WHERE	T1.document_id = @iOrigDocumentID
				AND	A1.account_id = T1.account_id 
				AND	A1.ledger_id =4
				)
		)
		SELECT	@iIsDirectToInsurer = 2
	END
	ELSE
		SELECT	@iIsDirectToInsurer = 0

	-- Insert values into Temporary Table
	INSERT INTO 	#Report_Commission_Earnt
	VALUES
	(
		@sDocumentRef,		
		@dtDocumentDate,		
		@sShortCode,		
		@sAccountName,		
		@nAmount,		
		@sOrigShortCode,		
		@sOrigAccountName,	
		@nOrigAmount,		
		@iCompanyID,		
		@sClientName,
		@iIsDirectToInsurer	
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


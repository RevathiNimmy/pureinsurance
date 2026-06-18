SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[sp_Report_Comm_Transacted_BC]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_Comm_Transacted_BC]
GO



CREATE PROCEDURE sp_Report_Comm_Transacted_BC
	(
	@branch_id	int,
	@start_date	datetime,
	@end_date	datetime
	)

AS


DECLARE	
	@iBranchID	int,
	@document_id	int,
	@account_id	int,
	@account_code	varchar(30),
	@account_name	varchar(60),
	@gross_comm	numeric(19,3),
	@opening_bal	numeric(19,3),
	@company_id	int,
	@effective_date	datetime

SELECT	@iBranchID = ISNULL(@branch_id, 0)

/* Clear the table */
DELETE	Orion_For_Broking.dbo.Report_Transaction 

/* Select all Documents containing non-zero Broking Transaction */ 
DECLARE	c_cursor SCROLL CURSOR FOR
	SELECT	DISTINCT(D.document_id),
		T.amount,
		T.account_id,
		A.company_id,
		A.short_code,
		A.account_name,
		T.accounting_date
	FROM	Orion_For_Broking.dbo.Transdetail	T,
		Orion_For_Broking.dbo.Account		A,
		Orion_For_Broking.dbo.Document		D
	WHERE	A.account_id = T.account_id
	AND	A.ledger_id = 9
	AND	T.amount <> 0
	AND	(
		T.spare <> 'COMM ADJ'
		AND	
		T.spare <> 'AGENT ADJ'
		AND
		T.spare NOT LIKE 'COMM PAY%'
		)
	AND	D.document_id = T.document_id 
	AND	D.document_date <= @end_date
	AND	(
			@iBranchID = 0
			OR
			(
				@iBranchID <> 0
				AND
				A.company_id = @iBranchID
			)
		)	

OPEN	c_cursor

FETCH FIRST FROM c_cursor INTO	@document_id,
				@gross_comm,
				@account_id,
				@company_id,
				@account_code,
				@account_name,
				@effective_date

WHILE @@FETCH_STATUS = 0
BEGIN

	/* Get Transacted documents */
	INSERT INTO Orion_For_Broking.dbo.Report_Transaction 
	(
	transdetail_id,
	document_ref,	
	amount, 	-- gross premium
	extra_numeric1,	-- IPT percent
	extra_numeric2,	-- gross commission
	extra_numeric3,	-- agent + sub-agent commission
	document_date,
	extra_numeric4,	-- opening balance
	branch_id,
	account_code,
	account_name,
	account_id,
	account_name2,	-- client_name
	extra_char1,	-- account executive,
	extra_char2,	-- document_type
	extra_datetime1	-- effective date
	)
	SELECT	0,
		D.document_ref,
		ISNULL(
			(
			SELECT	amount
			FROM	Orion_For_Broking.dbo.Transdetail
			WHERE	document_id = @document_id
			AND	document_sequence = 1
			)
		, 0),	
		I.rate,
		@gross_comm,
		ISNULL(
			(
			SELECT	SUM(amount)
			FROM	Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Account		A
			WHERE	T.document_id = @document_id
			AND	A.account_id = T.account_id 
			AND	A.ledger_id = 5			)
		, 0) -
			(	
				ISNULL(
					(
					SELECT	amount
					FROM	Orion_For_Broking.dbo.Transdetail
					WHERE	document_id = @document_id
					AND	document_sequence = 1
					AND EXISTS
					(
						SELECT	* 
						FROM	Orion_For_Broking.dbo.Transdetail	T,
							Orion_For_Broking.dbo.Account		A
						WHERE	T.document_id = @document_id
						AND	A.account_id = T.account_id 
						AND	A.ledger_id = 10					
					)
					)
				, 0) - 

				ISNULL(
					(
					SELECT	SUM(amount)
					FROM	Orion_For_Broking.dbo.Transdetail	T,
						Orion_For_Broking.dbo.Account		A
					WHERE	T.document_id = @document_id
					AND	A.account_id = T.account_id 
					AND	A.ledger_id = 10					)
				, 0)
			),
		D.document_date,
		0,
		@company_id,
		@account_code,
		@account_name,
		@account_id,
		ISNULL(
			(
			SELECT	P.resolved_name
			FROM	Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Account		A,
				Party 					P
			WHERE	document_id = @document_id
			AND	document_sequence = 1
			AND	A.account_id = T.account_id 
			AND	A.account_key = ((P.source_id - 1) * 268435456) + P.party_id
			)
		, '') Client,
		ISNULL(
			(
			SELECT	PE.resolved_name
			FROM	Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Account		A,
				Party 					PCli,
				Party 					PE
			WHERE	document_id = @document_id
			AND	document_sequence = 1
			AND	A.account_id = T.account_id 
			AND	A.account_key = ((PCli.source_id - 1) * 268435456) + PCli.party_id
			AND	PE.party_cnt = PCli.consultant_cnt	
			)
		, '') Account_Executive,
		DT.description,
		F.cover_start_date	
	FROM	Orion_For_Broking.dbo.Document		D,
		Transaction_Export_Folder		F,
		IPT					I,
		Insurance_File				N,
		Risk_Group				RG,
		Risk_Code				RC,
		Orion_For_Broking.dbo.DocumentType	DT
	WHERE	D.document_id = @document_id  	
	AND	F.document_ref = D.document_ref
	AND	F.source_id = @company_id
	AND	F.accounts_export_status = 'c'
	AND	N.insurance_file_cnt = F.insurance_file_cnt
	AND	RC.risk_code_id = N.risk_code_id	
	AND	RG.risk_group_id = RC.risk_group_id	
	AND	I.risk_code_id = RG.risk_group_id
	AND	I.effective_date = 
		(
		SELECT 	MAX(effective_date)
		FROM	IPT
		WHERE	effective_date <= D.document_date
		AND	risk_code_id = RG.risk_group_id	
		)
	AND	DT.documenttype_id = D.documenttype_id 

	/* Get Non-transacted documents */
	INSERT INTO Orion_For_Broking.dbo.Report_Transaction 
	(
	transdetail_id,
	document_ref,	
	amount, 	-- gross premium
	extra_numeric1,	-- IPT percent
	extra_numeric2,	-- gross commission
	extra_numeric3,	-- agent + sub-agent commission
	document_date,
	extra_numeric4,	-- opening balance
	branch_id,
	account_code,
	account_name,
	account_id,
	account_name2,	-- client_name
	extra_char1,	-- account executive
	extra_char2,	-- document_type
	extra_datetime1	-- effective date
	)
	SELECT	0,
		D.document_ref,
		ISNULL(
			(
			SELECT	amount
			FROM	Orion_For_Broking.dbo.Transdetail
			WHERE	document_id = @document_id
			AND	document_sequence = 1
			)
		, 0),	
		0,
		@gross_comm,
		ISNULL(
			(
			SELECT	SUM(amount)
			FROM	Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Account		A
			WHERE	T.document_id = @document_id
			AND	A.account_id = T.account_id 
			AND	A.ledger_id = 5			)
		, 0) +
			(	
				ISNULL(
					(
					SELECT	amount
					FROM	Orion_For_Broking.dbo.Transdetail
					WHERE	document_id = @document_id
					AND	document_sequence = 1
					AND EXISTS
					(
						SELECT	* 
						FROM	Orion_For_Broking.dbo.Transdetail	T,
							Orion_For_Broking.dbo.Account		A
						WHERE	T.document_id = @document_id
						AND	A.account_id = T.account_id 
						AND	A.ledger_id = 10					
					)
					)
				, 0) - 

				ISNULL(
					(
					SELECT	SUM(amount)
					FROM	Orion_For_Broking.dbo.Transdetail	T,
						Orion_For_Broking.dbo.Account		A
					WHERE	T.document_id = @document_id
					AND	A.account_id = T.account_id 
					AND	A.ledger_id = 10					)
				, 0)
			),
		D.document_date,
		0,
		@company_id,
		@account_code,
		@account_name,
		@account_id,
		ISNULL(
			(
			SELECT	P.resolved_name
			FROM	Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Account		A,
				Party 					P
			WHERE	document_id = @document_id
			AND	document_sequence = 1
			AND	A.account_id = T.account_id 
			AND	A.account_key = ((P.source_id - 1) * 268435456) + P.party_id
			)
		, '') Client,
		ISNULL(
			(
			SELECT	PE.resolved_name
			FROM	Orion_For_Broking.dbo.Transdetail	T,
				Orion_For_Broking.dbo.Account		A,
				Party 					PCli,
				Party 					PE
			WHERE	document_id = @document_id
			AND	document_sequence = 1
			AND	A.account_id = T.account_id 
			AND	A.account_key = ((PCli.source_id - 1) * 268435456) + PCli.party_id
			AND	PE.party_cnt = PCli.consultant_cnt	
			)
		, '') Account_Executive,
		DT.description,
		@effective_date		
	FROM	Orion_For_Broking.dbo.Document		D,
		Orion_For_Broking.dbo.DocumentType	DT
	WHERE	D.document_id = @document_id  	
	AND 	NOT EXISTS
		(
			SELECT	document_ref 
			FROM	Transaction_Export_Folder 
			WHERE	document_ref = D.document_ref
			AND	source_id = @company_id
		)
	AND	DT.documenttype_id = D.documenttype_id 

	FETCH NEXT FROM c_cursor INTO	@document_id,
					@gross_comm,
					@account_id,
					@company_id,
					@account_code,
					@account_name,
					@effective_date
END

CLOSE 		c_cursor
DEALLOCATE	c_cursor

/* Delete documents out of range */
DELETE FROM 	Orion_For_Broking.dbo.Report_Transaction 
WHERE
	(
		(
		extra_datetime1 > @end_date
		)
		OR
		(
			extra_datetime1 < @start_date
			AND
			document_date < @start_date
		)
	)

/* Set opening balances */
DECLARE	c_cursor SCROLL CURSOR FOR
	SELECT	DISTINCT account_id
	FROM	Orion_For_Broking.dbo.Report_Transaction 
	
OPEN	c_cursor

FETCH FIRST FROM c_cursor INTO	@account_id
WHILE @@FETCH_STATUS = 0
BEGIN

	UPDATE	Orion_For_Broking.dbo.Report_Transaction 
	SET	extra_numeric4 = 
		(
		SELECT	ISNULL(SUM(ISNULL(TD.amount,0)),0)

		FROM	Orion_For_Broking.dbo.Transdetail	TD,
			Orion_For_Broking.dbo.Document		D
		WHERE	D.document_id = TD.document_id
		AND	D.document_date < @start_date
		AND	TD.accounting_date < @start_date
		AND	TD.account_id = @account_id 
		)
	WHERE	account_id = @account_id 

	FETCH NEXT FROM c_cursor INTO	@account_id	
END

CLOSE 		c_cursor
DEALLOCATE	c_cursor


/* Return required data */
SELECT
	document_ref,	
	amount		gross_value,
	extra_numeric1	IPT_percent,
	extra_numeric2	gross_commission,
	extra_numeric3	agent_commissions,
	document_date,
	extra_numeric4	opening_balance,	
	branch_id,
	account_code,
	account_name,
	account_name2	client,
	extra_char1	account_executive,
	extra_char2	transaction_type,
	extra_datetime1	effective_date
FROM	Orion_For_Broking.dbo.Report_Transaction 




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


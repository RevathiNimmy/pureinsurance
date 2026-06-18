SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
--PN6169
EXECUTE DDLDropProcedure 'spu_Report_Commission_Adjustments'
GO
CREATE PROCEDURE spu_Report_Commission_Adjustments
	(
	@branch_id	int,
	@start_date	datetime,
	@end_date	datetime
	)
AS

BEGIN

DECLARE	@iBranchID	int,
	@document_ref   varchar(25),
	@document_id	int,
	@account_id	int,
	@account_code	varchar(30),
	@account_name	varchar(60),
	@gross_comm	numeric(19,3),
	@opening_bal	numeric(19,3),
	@company_id	int,
	@effective_date	datetime,
	@transaction_date datetime

SELECT	@iBranchID = ISNULL(@branch_id, 0)

/* Clear the table */
DELETE	Report_Transaction 
  
 
DECLARE	c_cursor CURSOR FAST_FORWARD FOR

	SELECT DISTINCT
		D.company_id,
		D.document_id,
		T.accounting_date,
		T.account_id,
		A.short_code,
 		A.account_name,
 		TEF.cover_start_date
	FROM document D	
	JOIN Transdetail T
	ON D.document_id = T.document_id
	JOIN Account A
	ON T.account_id = A.account_id 
	JOIN transaction_export_folder TEF
	ON TEF.document_ref = D.document_ref
	AND TEF.source_id = D.company_id
	AND TEF.accounts_export_status = 'c'
    WHERE T.spare = 'BROK ADJ'
	AND	T.ref_date >= @start_date
	AND	T.ref_date <= @end_date
	AND	
	(
		@iBranchID = 0
		OR
		(
			@iBranchID <> 0
			AND
			d.company_id = @iBranchID
		)
	)	 
	AND D.documenttype_id IN (4, 5, 15, 16, 17, 18, 31, 32, 35, 36)

OPEN c_cursor

FETCH NEXT FROM c_cursor INTO	@company_id,
				@document_id,
				@transaction_date,
				@account_id,
				@account_code,
				@account_name,
				@effective_date

WHILE @@FETCH_STATUS = 0
BEGIN

	/* Get Transacted documents */
 	INSERT INTO Report_Transaction 
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
		0,			-- Amount
		0,			-- IPT
        (
            SELECT 
                ISNULL(SUM(ROUND(T.amount,2)),0) * -1 
            FROM Transdetail T
            WHERE T.document_id = @document_id
            AND T.accounting_date = @transaction_date
            AND T.spare = 'COMM ADJ'
        ), /*Calculate Commission Adjustment*/
        
        (
            SELECT 
                ISNULL(SUM(ROUND(T.amount,2)),0) * -1 
            FROM Transdetail T
            WHERE T.document_id = @document_id
            AND T.accounting_date = @transaction_date
            AND T.spare = 'AGENT ADJ' 
        ), /*Calculate Agent Adjustment*/
		@transaction_date,
		0,
		@company_id,
		@account_code,
		@account_name,
		@account_id,
		ISNULL(
			(
			SELECT	P.resolved_name
			FROM	Transdetail	T,
				Account		A,
				Party 					P
			WHERE	document_id = @document_id
			AND	document_sequence = 1
			AND	A.account_id = T.account_id 
			AND	A.account_key =  P.party_cnt
			)
		, '') Client,
		ISNULL(
			(
			SELECT	PE.resolved_name
			FROM	Transdetail	T,
				Account		A,
				Party 					PCli,
				Party 					PE
			WHERE	document_id = @document_id
			AND	document_sequence = 1
			AND	A.account_id = T.account_id 
			AND	A.account_key =  PCli.party_cnt
			AND	PE.party_cnt = PCli.consultant_cnt	
			)
		, '') Account_Executive,
		DT.description,
		@effective_date	
	FROM	Document		D,
		DocumentType	DT
	WHERE	D.document_id = @document_id  	
 	AND	DT.documenttype_id = D.documenttype_id

	FETCH NEXT FROM c_cursor INTO	@company_id,
					@document_id,
					@transaction_date,
					@account_id,
					@account_code,
					@account_name,
					@effective_date 

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
FROM	Report_Transaction 

END

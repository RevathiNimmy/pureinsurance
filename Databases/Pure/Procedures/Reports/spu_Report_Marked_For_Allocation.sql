SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Marked_For_Allocation'
GO




CREATE PROCEDURE spu_Report_Marked_For_Allocation

	@account_code	varchar(30)

AS

SELECT	DISTINCT
	ISNULL(A.account_name, '')	Account,
	ISNULL(A.address1, '')		Address1,
	ISNULL(A.address2, '')		Address2,
	ISNULL(A.address3, '')		Address3,
	ISNULL(A.address4, '')		Address4,
	ISNULL(A.postal_code, '')	Postal_Code,
	D.document_ref			Document_Ref,
	D.document_id			Document_ID,
	D.document_date			Policy_Doc_Date,
	DT.description			Document_Type,
--	ISNULL(ACli.account_name, '')	Client,
--	ISNULL(ACli.short_code, '')	Client_Code,
	'Shite'				Client, 
	'Shite'				client,
	ISNULL
	(
		(
		SELECT	SUM(amount)
		FROM	Transdetail
		WHERE	document_id = D.document_id 
		AND	account_id = A.account_id
		)
 	, 0)				Value, 

	ISNULL
	(
		(
		SELECT	SUM(base_match_amount)
		FROM	TransMatch	A1,
			Transdetail	T1
		WHERE	T1.transdetail_id = A1.transdetail_id
		AND	T1.document_id = D.document_id
		AND	T1.account_id = A.account_id
		AND	A1.allocationdetail_id IS NULL 
		) 
	, 0)				This_Payment,

	ISNULL
	(
		(
		SELECT	SUM(base_match_amount)
		FROM	TransMatch	A1,
			OTransdetail	T1
		WHERE	T1.transdetail_id = A1.transdetail_id
		AND	T1.document_id = D.document_id
		AND	T1.account_id = A.account_id
		AND	A1.allocationdetail_id IS NOT NULL
		)
	, 0)				Total_Payment
FROM	Account		A,
	TransDetail	TD,
	Document		D,
	DocumentType	DT,
	TransMatch	TM,
	Document		DCli,
	DocumentType	DTCli,
	Account		ACli,
	TransDetail	TCli
 

WHERE	
	A.short_code = @account_code
AND	TD.account_id = A.account_id
AND	TM.transdetail_id = TD.transdetail_id
AND	TM.allocationdetail_id IS NULL
AND	D.document_id = TD.document_id
AND	DT.documenttype_id = D.documenttype_id
AND	DCli.document_id = TD.document_id
AND	DTCli.documenttype_id = DCli.documenttype_id
AND	TCli.document_id = TD.document_id
AND	ACli.account_id =
	(
		SELECT	 T2.account_id
		FROM	TransDetail	T2
		WHERE	transdetail_id = 
		(	SELECT	MIN(transdetail_id)
			FROM	Transdetail	T3,
				Account		A3
			WHERE	T3.document_id = DCli.document_id
			AND	A3.account_id = T3.account_id 
			AND	A3.ledger_id = 3	
 
		)
	)

 
 

ORDER BY 
	Account

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


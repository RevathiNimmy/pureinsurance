
--eck 160603 PN4707
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Marked_For_Settlement'
GO 
CREATE PROCEDURE spu_Report_Marked_For_Settlement

	@account_code	varchar(30)

AS

SELECT	 DISTINCT
	case 
		when D.documenttype_id in (22,23) then 'Cash Transactions'
		else 'Business Transactions'
	end				Type,
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
 	ISNULL(ACli.contact_name, '')	Client,
 	ISNULL(ACli.short_code, '')	Client_Code,
	ISNULL(TCli.insurance_ref, '')  Policy_Ref,
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
			Transdetail	T1
		WHERE	T1.transdetail_id = A1.transdetail_id
		AND	T1.document_id = D.document_id
		AND	T1.account_id = A.account_id
		AND	A1.allocationdetail_id IS NOT NULL
		)
	, 0)				Total_Payment
FROM	Account		A
	JOIN	TransDetail	TD 
	ON	TD.account_id = A.account_id
	JOIN	TransMatch	TM
	ON 	TM.transdetail_id = TD.transdetail_id
	JOIN 	Document		D 
	ON	D.document_id = TD.document_id
	JOIN	DocumentType	DT 
	ON	D.documenttype_id = DT.documenttype_id	
 	LEFT OUTER JOIN	Transdetail	TCli
		ON TCLi.document_id = D.document_id
		AND TCLi.document_sequence = 1
	LEFT JOIN Account	ACli
 		ON TCLi.account_id = ACli.account_id
		AND ACli.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA' ) --eckPN4707

	 
		 
 
WHERE	A.short_code = @account_code
AND	TM.allocationdetail_id IS NULL
order by type,policy_doc_date

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


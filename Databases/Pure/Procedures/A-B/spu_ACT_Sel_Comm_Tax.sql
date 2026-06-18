SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Sel_Comm_Tax'
GO

CREATE PROCEDURE spu_ACT_Sel_Comm_Tax 
    @transdetail_id int
AS

DECLARE @DocumentId INT

SELECT
	T2.account_id		'account_id',
	T2.amount		'amount',
	T2.insurance_ref		'insurance_ref',
	T2.transdetail_id	'transdetail_id',
	T2.company_id		'company_id',
	T2.currency_amount	'currency_amount'
FROM
	Transdetail		T
JOIN	Transdetail		T2
	ON T2.document_id = T.document_id
JOIN 	Transdetail_Type TT
	ON 	T2.transdetail_type_id = TT.transdetail_type_id
WHERE 	(TT.code = 'TAX' OR TT.code = 'FEETAX' OR TT.code = 'AGENTTAX')
AND	T.transdetail_id =  @transdetail_id

GO
 
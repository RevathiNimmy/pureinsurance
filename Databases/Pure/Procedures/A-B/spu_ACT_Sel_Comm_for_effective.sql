SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Sel_Comm_for_effective'
GO 

/****** Object:  Stored Procedure dbo.sp_ACT_Sel_Comm_for_effective    Script Date: 29/11/02 12:03:17 ******/
 
CREATE PROCEDURE spu_ACT_Sel_Comm_for_effective
    @document_ref varchar(11),
    @company_id int 
 
AS
 
SELECT
	T.account_id		'account_id',
	(
		SELECT T.amount - ISNULL(SUM(base_match_amount),0)
		FROM transmatch
		WHERE transdetail_id = T.transdetail_id
	) 'amount',
 	T.insurance_ref		'insurance_ref', 
	T.transdetail_id	'transdetail_id', 
	T.company_id		'company_id'
FROM
	Transdetail		T,
	Document 		D
WHERE T.spare in ('BROK','BROK ADJ')
AND	T.document_id = D.document_id
AND	D.document_ref = @Document_ref
AND	D.company_id = @company_id		 

GO
 
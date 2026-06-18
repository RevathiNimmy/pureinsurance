SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Paid_Brokerage_Trans'
GO

 
CREATE PROCEDURE spu_ACT_Sel_Paid_Brokerage_Trans
    @transdetail_id int 
 
AS

BEGIN
 
SELECT	T2.transdetail_id,  
	T2.currency_amount 
	
FROM	Document 		D
JOIN    Transdetail		T
	on T.document_id = D.document_id
JOIN    Transdetail		T2
	on T2.document_id = T.document_id
WHERE  T2.spare in ('BROK','BROK ADJ')
AND    T.transdetail_id = @transdetail_id 


END

GO
 
 
DDLDropprocedure 'spu_SAM_Get_Transaction_Details'
go

CREATE PROCEDURE spu_SAM_Get_Transaction_Details
	@transdetail_id int
AS
    SELECT 
	td.amount_updated as 'effective_date',
	td.spare 	
    FROM transdetail td WITH(NOLOCK)
	INNER JOIN document d WITH(NOLOCK)
		ON td.document_id = d.document_id
    WHERE td.transdetail_id = @transdetail_id
  
GO

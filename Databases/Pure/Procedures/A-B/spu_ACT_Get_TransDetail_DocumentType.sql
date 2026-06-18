SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_TransDetail_DocumentType'
GO

CREATE PROCEDURE spu_ACT_Get_TransDetail_DocumentType 

	@transdetail_id int
AS

BEGIN

	SELECT code from documenttype WITH(NOLOCK) WHERE documenttype_id in (
	SELECT documenttype_id FROM document d WITH(NOLOCK) 
	INNER JOIN transdetail td WITH(NOLOCK) ON d.document_id = td.document_id WHERE transdetail_id = @transdetail_id)


END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

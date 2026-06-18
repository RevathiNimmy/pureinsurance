SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_ClaimsDocument_ForReversal'
GO

CREATE PROCEDURE spu_Get_ClaimsDocument_ForReversal  
	@claim_id INT
AS 

SELECT d.document_id, d.document_ref FROM Stats_Folder sf
	INNER JOIN
	Document d ON d.document_ref = sf.document_ref 
	WHERE sf.loss_id = @claim_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
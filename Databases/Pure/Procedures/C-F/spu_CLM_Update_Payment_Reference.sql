SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO

EXECUTE DDLDropProcedure 'spu_CLM_Update_Payment_Reference'
GO

CREATE PROCEDURE spu_CLM_Update_Payment_Reference
	@document_id int
AS
BEGIN
    IF Exists(Select thirdpartyreference From claim_payment 
		Where document_id = @document_id And thirdpartyreference IS NOT NULL)
	UPDATE transdetail SET reference = 
	    (Select thirdpartyreference From claim_payment 
		Where document_id = @document_id And thirdpartyreference IS NOT NULL)
	WHERE document_id = @document_id 

END

SET QUOTED_IDENTIFIER OFF    
Go
SET ANSI_NULLS OFF  
GO


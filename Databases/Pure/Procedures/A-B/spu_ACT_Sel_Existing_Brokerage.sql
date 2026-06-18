SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Existing_Brokerage'
GO

CREATE PROCEDURE spu_ACT_Sel_Existing_Brokerage
    @transdetail_id int, 
    @existing char(1) OUTPUT    
AS
BEGIN

DECLARE @document_id int
 
	 
	SELECT @document_id  =  document_id
				FROM  transdetail  
 				WHERE transdetail_id = @transdetail_id
	 
	SELECT @existing = "N"

	SELECT @existing = "Y"
	FROM   Transdetail  
	WHERE	spare = "BROK"  
			AND  document_id = @document_id
END

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


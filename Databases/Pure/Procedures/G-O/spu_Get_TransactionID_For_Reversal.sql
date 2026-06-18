SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Get_TransactionID_For_Reversal'
GO

CREATE  PROCEDURE spu_Get_TransactionID_For_Reversal
    @insurance_file_cnt int

AS

BEGIN
    DECLARE @document_id int        
    
    SELECT @document_id = document_id 
    FROM document 
    WHERE insurance_file_cnt = @insurance_file_cnt

    SELECT transdetail_id 
    FROM allocationdetail 
    WHERE transdetail_id 
    IN 
    (SELECT transdetail_id 
    FROM transdetail 
    WHERE document_id =@document_id)
    
END
GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO


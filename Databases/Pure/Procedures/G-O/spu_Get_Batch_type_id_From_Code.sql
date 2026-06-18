SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Batch_type_id_From_Code'
GO

CREATE PROCEDURE spu_Get_Batch_type_id_From_Code      
@code VarChar(255)    
AS    
    
BEGIN      
SELECT top 1(batch_type_id) FROM Batch_Type WHERE code = @code      
END
GO
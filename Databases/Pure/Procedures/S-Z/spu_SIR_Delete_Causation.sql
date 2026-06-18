SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_Delete_Causation'
GO

CREATE PROCEDURE spu_SIR_Delete_Causation
    @product_id INT, 
    @primary_cause_id INT	
AS    
    
DELETE FROM    
    Product_Allowed_Causation
WHERE    
   product_id = @product_id    

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Delete_Product_Source'
GO

CREATE PROCEDURE spu_SIR_Delete_Product_Source --Delete    
    @product_id INT,    
    @source_id INT    
AS    
      
DELETE FROM    
    product_source    
WHERE    
   product_id = @product_id 
GO
  
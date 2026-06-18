
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Save_Product_Source'
GO
  
  
CREATE PROCEDURE spu_SIR_Save_Product_Source   
    @product_id INT,    
    @source_id INT    
    
AS    
INSERT INTO    
    Product_Source (product_id,source_id)    
VALUES    
    (@product_id,@source_id) 

GO
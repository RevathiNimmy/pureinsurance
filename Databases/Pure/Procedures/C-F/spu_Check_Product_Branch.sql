SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_Check_Product_Branch'
GO

 CREATE PROCEDURE spu_Check_Product_Branch    
    @ProductID INT,    
    @BranchID INT,  
    @IsValid INT OUTPUT  
AS    
BEGIN    
    
    If Exists(SELECT NULL FROM Product_Source WHERE product_id=@ProductID  
  AND product_id=@ProductID and source_id= @BranchID)  
 SELECT @IsValid = 1  
    Else  
 SELECT @IsValid = 0  
   
END 


Go
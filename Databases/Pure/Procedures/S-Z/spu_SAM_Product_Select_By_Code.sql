SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Product_Select_By_Code'
GO


CREATE PROCEDURE spu_SAM_Product_Select_By_Code
    @code varchar(10)
AS  
  
SELECT  
    product_id,  
    code,  
    description,  
    is_midnight_renewal  
  
FROM Product 
  
WHERE code = @code





GO

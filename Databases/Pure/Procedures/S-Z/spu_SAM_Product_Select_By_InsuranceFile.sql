SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Product_Select_By_InsuranceFile'
GO


CREATE PROCEDURE spu_SAM_Product_Select_By_InsuranceFile  
    @insurance_file_cnt varchar(10)  
AS    
    
SELECT    
    product.product_id,    
    product.code,    
    product.description,    
    product.is_midnight_renewal    
    
FROM Product   

	INNER JOIN insurance_file ON 
		product.product_id = insurance_file.product_id
    
WHERE insurance_file_cnt =@insurance_file_cnt
  




GO

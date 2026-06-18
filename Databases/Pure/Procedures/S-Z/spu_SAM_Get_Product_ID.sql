SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Product_ID'
GO
CREATE PROC spu_SAM_Get_Product_ID  
@insurance_file_cnt INT  
AS  
SELECT product_id   
FROM insurance_file  
WHERE insurance_file_cnt =@insurance_file_cnt   

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Get_out_of_sequence_mta_details'
GO

CREATE  PROCEDURE spu_Get_out_of_sequence_mta_details    
    @insurance_file_cnt int    
    
AS    
    
BEGIN    
      
 DECLARE @product_id INT    
     
 SELECT @product_id = product_id     
 FROM insurance_file     
 WHERE insurance_file_cnt = @insurance_file_cnt    
  
 SELECT out_of_sequence_mta_allocation,  
 out_of_sequence_mta_dates     
 FROM product     
 WHERE product_id = @product_id    
     
END    
GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO

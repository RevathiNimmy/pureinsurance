SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Update_Insurance_File_Discount'
GO

CREATE PROCEDURE spu_Update_Insurance_File_Discount
 @insurance_file_cnt INT,  
 @discounted_premium NUMERIC(19,4),  
 @discount_percentage NUMERIC(11,8),  
 @match_discount_premium TINYINT,   
 @discount_reason_id SMALLINT,
 @discount_recurring_type_id SMALLINT = NULL

AS
UPDATE  Insurance_File    
SET   	discounted_premium=@discounted_premium,    
      	discount_percentage=@discount_percentage,    
      	discount_reason_id=@discount_reason_id,  
       	match_discounted_premium_flag=@match_discount_premium,
        discount_recurring_type_id=@discount_recurring_type_id  
WHERE 	insurance_file_cnt = @insurance_file_cnt

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

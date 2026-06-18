SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_check_is_true_monthly_policy'
GO

CREATE PROCEDURE spu_check_is_true_monthly_policy  
 @insurance_file_cnt int,  
 @is_true_monthly_policy tinyint OUTPUT,  
 @anniversary_copy tinyint OUTPUT 

  AS  
  
SELECT   
 @is_true_monthly_policy=ISNULL(p.is_true_monthly_policy,0), 
 @anniversary_copy = ISNULL(ifi.anniversary_copy,0) 
  
    FROM Insurance_File ifi  
    JOIN Product p ON p.product_id = ifi.product_id  
WHERE  ifi.insurance_file_cnt=@insurance_file_cnt  


GO

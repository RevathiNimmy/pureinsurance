SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Get_Product_Option'
GO

CREATE PROCEDURE spu_SAM_Get_Product_option    
    @option_number int,      
    @branch_id int,    
    @option_value Varchar(10) OUTPUT, 
    @claim_id int =NULL        
AS      
BEGIN      
    
IF @claim_id IS NOT NULL  
BEGIN    
SELECT  @option_value= run_authorisation_scripts_claim_payments    
FROM   product   WITH(NOLOCK) 
WHERE product_id = (SELECT Product_id   
      FROM Insurance_file WITH(NOLOCK)   
      WHERE insurance_file_cnt = (SELECT Policy_id  
      FROM Claim  WITH(NOLOCK) 
      WHERE claim_id=@claim_id)  
		    )   
END  

ELSE
BEGIN  
--BRANCH IS ALWAYS HEAD OFFICE WHEN SETTING SYSTEM OPTIONS,
--SO FETCHING ALWAYS RECORD FOR HEADOFFICE
SET @branch_id=1  
SELECT   @option_value = Value    
FROM 	 hidden_options    
WHERE 	 option_number = @option_number    
AND 	 branch_id = @branch_id    
END
    
END    
    
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
  

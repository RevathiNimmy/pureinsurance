SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_CLM_media_type_mendatory'
GO

CREATE PROCEDURE spu_SAM_CLM_media_type_mendatory        
@Base_claim_id INT, 
@Version_id INT, 
@media_type_mandatory TINYINT = NULL OUTPUT
        
AS        
DECLARE @Policy_id INT      
    
SELECT @Policy_id = policy_id     
FROM Claim  WITH(NOLOCK)   
WHERE base_Claim_id = @base_Claim_id    
AND Version_id = @Version_id    
SELECT      
  @media_type_mandatory = p.media_type_mandatory    
FROM insurance_file i WITH(NOLOCK)      
  
INNER JOIN claim c  WITH(NOLOCK)       
   ON @Policy_id = i.insurance_file_cnt        
INNER JOIN Product P  WITH(NOLOCK)   
   ON i.Product_id = p.Product_id    
    
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


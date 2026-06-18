SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Validate_Claim_Payment_Data_Links'
GO

CREATE PROCEDURE spu_SAM_CLM_Validate_Claim_Payment_Data_Links    
    
@base_claim_id int,    
@base_claim_peril_id int,    
@base_reserve_id int    
    
AS    
    
SELECT TOP 1 claim.claim_id, claim_peril.claim_peril_id, reserve.reserve_id
    
FROM reserve
  
 INNER JOIN (SELECT claim_peril_id, base_claim_peril_id, claim_id    
      FROM Claim_Peril     
      WHERE base_claim_peril_id = @base_claim_peril_id) claim_peril ON     
  reserve.claim_peril_id = claim_peril.claim_peril_id    
    
  INNER JOIN (SELECT claim_id, base_claim_id    
       FROM claim     
       WHERE base_claim_id = @base_claim_id) claim ON    
   claim_peril.claim_id = claim.claim_id    
    
WHERE base_reserve_id = @base_reserve_id    
ORDER By reserve.version_id desc,claim.claim_id desc  
GO
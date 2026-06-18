SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Validate_Claim_Receipt_Data_Links'
GO

CREATE PROCEDURE spu_SAM_CLM_Validate_Claim_Receipt_Data_Links  
  
@base_claim_id int,  
@base_claim_peril_id int,  
@base_recovery_id int  
  
AS  
  
SELECT TOP 1 claim.claim_id, claim_peril.claim_peril_id, recovery.recovery_id   
  
FROM recovery  
  
 INNER JOIN (SELECT claim_peril_id, base_claim_peril_id, claim_id  
      FROM Claim_Peril   
      WHERE base_claim_peril_id = @base_claim_peril_id) claim_peril ON   
  recovery.claim_peril_id = claim_peril.claim_peril_id  
  
  INNER JOIN (SELECT claim_id, base_claim_id  
       FROM claim   
       WHERE base_claim_id = @base_claim_id) claim ON  
   claim_peril.claim_id = claim.claim_id  
  
WHERE base_recovery_id = @base_recovery_id  
ORDER By recovery.version_id desc
  
  



GO

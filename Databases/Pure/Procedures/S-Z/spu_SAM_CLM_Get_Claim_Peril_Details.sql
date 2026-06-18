SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Claim_Peril_Details'
GO

CREATE PROCEDURE spu_SAM_CLM_Get_Claim_Peril_Details  
  
@claim_peril_id integer  
  
AS  
  
SELECT claim_peril.gis_screen_id, 
	transaction_type.code transaction_type_code, 
	claim_peril.base_claim_peril_id
FROM claim_peril  
  
INNER JOIN claim on  
 claim.claim_id = claim_peril.claim_id  
  
INNER JOIN transaction_type ON  
 transaction_type.transaction_type_id = claim.transaction_type_id  
  
WHERE claim_peril_id = @claim_peril_id  
ORDER BY claim_peril.version_id DESC  



GO

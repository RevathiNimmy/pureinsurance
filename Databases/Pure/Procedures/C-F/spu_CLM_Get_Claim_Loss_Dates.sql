SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Loss_Dates'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Loss_Dates  
  
@claim_id int  
  
AS  
  
BEGIN  
 
 -- get loss date details for the current and previous version of the claim
 SELECT  claim.loss_from_date, 
	 claim.loss_to_date, 
	 previous_claim_version.loss_from_date, 
	 previous_claim_version.loss_to_date   

 FROM claim   

	LEFT OUTER JOIN (Select base_claim_id, version_id, loss_from_date, loss_to_date
			 FROM claim) previous_claim_version ON

		previous_claim_version.base_claim_id =claim.base_claim_id

		AND(
			((claim.version_id = 1) AND (previous_claim_version.version_id = 1)) 
			OR 
				(previous_claim_version.version_id = claim.version_id - 1))
			
 WHERE claim.claim_id = @claim_id  
  
  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

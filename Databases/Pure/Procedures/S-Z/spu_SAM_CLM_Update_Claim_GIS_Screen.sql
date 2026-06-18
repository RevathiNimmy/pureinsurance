SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Update_Claim_GIS_Screen'
GO

/*******************************************************************************************************/  
/* spu_SAM_CLM_Update_Claim_GIS_Screen                                                                 */  
/*                                                                                        	       */  
/* Get the Risk Screen Details and the Current Version of Claim Id 				       */  
/*******************************************************************************************************/  

CREATE  PROCEDURE spu_SAM_CLM_Update_Claim_GIS_Screen
@BaseClaim_id int
    
AS  

DECLARE @ClaimID int
SELECT TOP 1                   
@ClaimID = claim_id
FROM claim                   
WHERE base_claim_id = @BaseClaim_id
ORDER BY version_id DESC     

UPDATE claim SET claim.gis_screen_id = risk_type.claims_gis_screen_id
FROM claim
INNER JOIN risk ON
 claim.risk_type_id = risk.risk_cnt
INNER JOIN risk_type ON
 risk.risk_type_id = risk_type.risk_type_id 
WHERE claim.Claim_id = @ClaimID

UPDATE claim_peril SET claim_peril.gis_screen_id = peril_type.gis_screen_id
FROM Peril_type
INNER JOIN claim_peril ON             
 claim_peril.peril_type_id = peril_type.peril_type_id
INNER JOIN claim ON             
 claim.claim_id = claim_peril.Claim_id    
WHERE Claim.claim_id=@ClaimID

UPDATE claim_peril  
SET gis_screen_id = gs.Gis_Screen_id  
FROM claim_peril cp 
  INNER JOIN Claim c ON  
  cp.claim_id = c.claim_id  
  INNER JOIN  Gis_Screen gs ON  
   gs.Gis_Screen_id = c.Gis_Screen_id 
WHERE cp.claim_id = @claimid  
And cp.gis_screen_id IS NULL
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO




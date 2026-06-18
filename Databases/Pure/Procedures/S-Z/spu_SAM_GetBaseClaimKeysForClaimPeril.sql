SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_GetBaseClaimKeysForClaimPeril'
GO
CREATE PROCEDURE spu_SAM_GetBaseClaimKeysForClaimPeril    
@nClaim_Peril_ID int,  
@nClaim_ID int output   
 AS    
SELECT @nClaim_ID=Claim_id  FROM Claim_Peril    
		WHERE claim_peril_id = @nClaim_Peril_ID 
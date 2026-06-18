EXECUTE DDLDropProcedure 'spu_Get_Previous_ClaimKey'
GO

CREATE PROCEDURE spu_Get_Previous_ClaimKey 
      @nClaimKey INT,
      @nPreviousClaimKey INT OUTPUT
AS  
BEGIN
	SELECT TOP 1 @nPreviousClaimKey = OLDC.Claim_id FROM Claim OLDC
	JOIN Claim NEWC ON 
	OLDC.base_claim_id=NEWC.base_claim_id
	WHERE NewC.Claim_id=@nClaimKey
	AND OLDC.is_dirty=0
	AND OLDC.Claim_id<@nClaimKey
	ORDER BY OLDC.version_id DESC
		
END  
GO


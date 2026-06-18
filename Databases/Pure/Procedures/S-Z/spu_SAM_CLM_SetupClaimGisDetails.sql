SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_SetupClaimGisDetails'
GO

CREATE PROCEDURE spu_SAM_CLM_SetupClaimGisDetails

@claim_id int,
@base_claim_id int

AS

DECLARE @gis_screen_id int

SELECT @gis_screen_id = ISNULL(gis_screen_id,0) 
FROM Claim 
WHERE claim_id = @base_claim_id

IF @gis_screen_id <> 0 
BEGIN
	UPDATE claim set gis_screen_id = @gis_screen_id 
	WHERE claim_id = @claim_id 

	UPDATE claim_peril SET claim_peril.gis_screen_id = peril_type.gis_screen_id  
	FROM Peril_type  
		INNER JOIN claim_peril ON  
			 claim_peril.peril_type_id = peril_type.peril_type_id  
		INNER JOIN claim ON  
		 claim.claim_id = claim_peril.Claim_id  
	WHERE Claim.claim_id=@claim_id  
	AND claim_peril.gis_screen_id IS NULL
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

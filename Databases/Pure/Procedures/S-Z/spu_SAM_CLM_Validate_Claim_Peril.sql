SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Validate_Claim_Peril'
GO

CREATE PROCEDURE spu_SAM_CLM_Validate_Claim_Peril

@base_claim_id integer, 
@base_claim_peril_id integer

AS

SELECT TOP 1 claim_peril.claim_peril_id, claim.claim_id 

FROM claim_peril 

	INNER JOIN (SELECT claim_id 
		    FROM claim
		    WHERE base_claim_id = @base_claim_id) claim  ON 
		claim.claim_id = claim_peril.claim_id

WHERE base_claim_peril_id = @base_claim_peril_id
	




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

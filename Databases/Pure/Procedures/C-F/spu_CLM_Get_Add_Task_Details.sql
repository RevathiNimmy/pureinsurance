SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Add_Task_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Add_Task_Details

@claim_id int 

AS

BEGIN

	SELECT  
		claim.info_only,
		claim.claim_status_id,
		previous_claim_version.info_only
	
	FROM 	claim

		LEFT JOIN (Select Info_Only, 
				  version_id, 
			          base_claim_id
			   from claim) previous_claim_version ON
			previous_claim_version.base_claim_id = claim.base_claim_id
			AND(
				((claim.version_id = 1) AND (previous_claim_version.version_id = 1)) 
				OR 
					(previous_claim_version.version_id = claim.version_id -1))

	WHERE claim_id =@claim_id


END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

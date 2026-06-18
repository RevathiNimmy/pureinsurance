SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Risk_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Risk_Details

@claim_id int

AS

BEGIN

	SELECT  insurance_folder_cnt,
		insurance_file_cnt,
		product_id,
		risk.risk_cnt,
		claim.risk_type_id,
		risk.gis_screen_id
	
	FROM    Claim

		INNER JOIN Insurance_File ON
			claim.policy_id = insurance_file.insurance_file_cnt

		INNER JOIN Risk ON 
			claim.risk_type_id = risk.risk_cnt

		WHERE claim.claim_id=@claim_id
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

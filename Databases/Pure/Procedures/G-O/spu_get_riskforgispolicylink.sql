EXECUTE DDLDropProcedure 'spu_get_riskforgispolicylink'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_get_riskforgispolicylink  
	@gis_policy_link_id int  
As  
SELECT risk_type_id FROM claim clm  
INNER JOIN gis_policy_link gpl ON gpl.claim_id=clm.claim_id   
WHERE gpl.gis_policy_link_id= @gis_policy_link_id  

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

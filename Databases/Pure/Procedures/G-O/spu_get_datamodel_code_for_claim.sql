SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_DataModel_Code_For_Claim'
GO

CREATE PROCEDURE spu_Get_DataModel_Code_For_Claim  

	@Claim_Id int,  
	@Datamodel_Code char(10) OUTPUT  

AS  
BEGIN

	SELECT @Datamodel_Code = gdm.code  
	FROM gis_data_model gdm WITH (NOLOCK),  
		gis_screen gs WITH (NOLOCK),  
		risk_type rt WITH (NOLOCK),  
		risk  r WITH (NOLOCK),  
		claim c WITH (NOLOCK) 
	WHERE c.risk_type_id = r.risk_cnt  
	AND r.risk_type_id = rt.risk_type_id  
	AND rt.Claims_Gis_Screen_Id = gs.gis_screen_id  
	AND gs.gis_data_model_id = gdm.gis_data_model_id  
	AND c.claim_id = @Claim_Id  
 
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

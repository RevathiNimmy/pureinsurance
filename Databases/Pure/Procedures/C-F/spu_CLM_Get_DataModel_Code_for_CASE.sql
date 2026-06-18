SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_CLM_Get_DataModel_Code_for_CASE'
GO

CREATE PROCEDURE spu_CLM_Get_DataModel_Code_for_CASE  

	@case_id int,  
	@Datamodel_Code char(10) OUTPUT  

AS  
BEGIN

	SELECT @Datamodel_Code=gdm.code
	FROM gis_data_model gdm 
		JOIN gis_policy_link gpl
			ON  gdm.gis_data_model_id= gpl.gis_data_model_id
		JOIN [case] cs
			ON cs.case_id= gpl.case_id  
	WHERE  cs.case_id = @case_id 

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO



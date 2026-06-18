SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Gis_Data'
GO

CREATE PROCEDURE spu_CLM_Get_Gis_Data  

@Claim_Id int  

AS  

BEGIN  
	SELECT gis_policy_link_id, gdm.code  
	FROM gis_policy_link gpl  
		LEFT JOIN gis_data_model gdm ON  
			gdm.gis_data_model_id = gpl.gis_data_model_id  
	WHERE gpl.claim_id = @claim_id  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

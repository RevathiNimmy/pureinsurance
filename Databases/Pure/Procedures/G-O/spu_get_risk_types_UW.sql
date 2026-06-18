SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_risk_types_UW'
GO

CREATE PROCEDURE spu_get_risk_types_UW  
AS  
 SELECT Risk_type_id, Risk_type.Code, Risk_type.Description, claims_gis_screen_id,gis_screen.Description  
 FROM Risk_type  
 	Left join gis_screen on Risk_type.claims_gis_screen_id = gis_screen.gis_screen_id  
 WHERE risk_type.is_deleted = 0


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

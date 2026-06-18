SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_gis_qem_usage_get    Script Date: 13/02/2002 15:49:22 ******/
EXECUTE DDLDropProcedure 'spu_gis_qem_usage_get'
GO

CREATE PROCEDURE spu_gis_qem_usage_get 

@schemeId integer

 AS

SELECT u.gis_QEM_ID,u.gis_scheme_id,u.gis_business_type_id,u.gis_data_model_id FROM GIS_QEM q INNER JOIN gis_qem_usage u ON q.gis_qem_id=u.gis_qem_id WHERE u.gis_scheme_id=@schemeid
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


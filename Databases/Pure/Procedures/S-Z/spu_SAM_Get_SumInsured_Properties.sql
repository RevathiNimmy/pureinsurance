SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_SumInsured_Properties'
GO

CREATE PROCEDURE spu_SAM_Get_SumInsured_Properties
    @GisDataModelCode varchar(10)
AS

SELECT 
	UPPER(go.object_name) gis_object_name, 
	UPPER(gp.property_name) gis_property_name, 
	gp.specials_type_reference

FROM gis_property gp 

INNER JOIN gis_object go ON 
	go.gis_object_id = gp.gis_object_id 

INNER JOIN gis_data_model gdm ON 
	gdm.gis_data_model_id = go.gis_data_model_id

WHERE gp.specials_type = 4 
AND gdm.code = @gisdatamodelcode
ORDER BY gp.specials_type_reference

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Dataset_Address_Properties'
GO

CREATE PROCEDURE spu_SAM_Get_Dataset_Address_Properties
    @GisDataModelCode varchar(10)
AS

SELECT 
	'//' + upper(go.object_name) + '/' + upper(gp.property_name) 'XPathElements',
	'//' + upper(go.object_name) + '/@' + upper(gp.property_name) 'XPathAttributes'
FROM
	gis_property gp 
INNER JOIN gis_object go ON go.gis_object_id = gp.gis_object_id  
INNER JOIN gis_data_model gdm ON gdm.gis_data_model_id = go.gis_data_model_id
WHERE 
	gp.property_name LIKE '%ADDRESS_CNT%'
    AND gdm.code = @GisDataModelCode
ORDER BY 
	go.gis_object_id,
	gp.gis_property_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

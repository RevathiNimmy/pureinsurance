SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_polaris_mapping_getcode'
GO

CREATE PROCEDURE spu_gis_polaris_mapping_getcode
    @sGisObject varchar(70),
    @sGisProperty varchar(70)
AS

SELECT GIS_Object.gis_object_id,
    GIS_Property.gis_property_id
    FROM GIS_Object
    INNER JOIN GIS_Property ON GIS_Property.gis_object_id = GIS_Object.gis_object_id
    WHERE GIS_Object.object_name = @sGisObject
    AND GIS_Property.property_name = @sGisProperty

GO


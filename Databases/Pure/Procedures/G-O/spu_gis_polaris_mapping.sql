SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_polaris_mapping'
GO

CREATE PROCEDURE spu_gis_polaris_mapping
    @sPolObject varchar(70)
AS

SELECT GIS_Object.gis_object_id,
    GIS_Property.gis_property_id,
    GIS_Polaris_Mapping.pol_property_name,
    GIS_Polaris_Mapping.gis_object_name
    FROM GIS_Polaris_Mapping
    INNER JOIN GIS_Object
        ON GIS_Polaris_Mapping.gis_object_name = GIS_Object.object_name
    INNER JOIN GIS_Property
        ON GIS_Property.gis_object_id = GIS_Object.gis_object_id
        AND GIS_Property.property_name = GIS_Polaris_Mapping.gis_property_name
    WHERE pol_object_name = @sPolObject
    AND in_out = 'I'
    AND NBQ_Default IS NULL

GO


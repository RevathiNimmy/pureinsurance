SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Property_sel'
GO


CREATE PROCEDURE spu_GIS_Property_sel
    @property_id int
AS


SELECT gis_property_id,
    gis_object_id
FROM GIS_Property
WHERE polaris_property_id = @property_id
GO



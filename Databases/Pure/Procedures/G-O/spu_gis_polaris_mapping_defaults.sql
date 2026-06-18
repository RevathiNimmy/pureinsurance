SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_polaris_mapping_defaults'
GO

CREATE PROCEDURE spu_gis_polaris_mapping_defaults
    @sPolObject varchar(70)
AS

SELECT pol_property_name,
    nbq_default
    FROM GIS_Polaris_Mapping
    WHERE pol_object_name = @sPolObject
    AND nbq_default IS NOT NULL
    AND in_out = 'I'

GO


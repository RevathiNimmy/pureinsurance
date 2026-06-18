SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_Scheme_del'
GO

CREATE PROCEDURE spe_GIS_Scheme_del
    @gis_scheme_id int
AS

DELETE FROM GIS_Scheme
WHERE gis_scheme_id = @gis_scheme_id

GO


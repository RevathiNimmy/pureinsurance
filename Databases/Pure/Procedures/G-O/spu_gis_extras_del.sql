SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_gis_extras_del'
GO


CREATE PROCEDURE spu_gis_extras_del
    @gis_scheme_extra_id int
AS


BEGIN
    delete from GIS_Scheme_extras
    WHERE gis_scheme_extra_id = @gis_scheme_extra_id
END
GO



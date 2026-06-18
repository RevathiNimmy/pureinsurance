SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_del'
GO


CREATE PROCEDURE spu_GIS_Scheme_del
    @gis_insurer_id int,
    @scheme_no int,
    @scheme_ver int
AS


DELETE FROM GIS_Scheme
WHERE gis_insurer_id = @gis_insurer_id
AND scheme_no = @scheme_no
AND scheme_ver = @scheme_ver
GO



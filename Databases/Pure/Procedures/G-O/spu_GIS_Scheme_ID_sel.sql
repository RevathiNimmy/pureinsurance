SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_ID_sel'
GO


CREATE PROCEDURE spu_GIS_Scheme_ID_sel
    @scheme_no int,
    @scheme_ver int,
    @Polaris_insurer_no int
AS


SELECT Scheme.gis_scheme_id
FROM GIS_Scheme Scheme,
    GIS_Insurer Insurer
WHERE Scheme.scheme_no = @scheme_no
AND Scheme.scheme_ver = @scheme_ver
AND Insurer.Polaris_insurer_no = @Polaris_insurer_no
AND Insurer.gis_insurer_id = scheme.gis_insurer_id
GO



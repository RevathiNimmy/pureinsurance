SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Gis_SchemeGrpMemByCode_sel'
GO


CREATE PROCEDURE spu_Gis_SchemeGrpMemByCode_sel
    @code char(10)
AS


SELECT m.gis_scheme_id
FROM gis_scheme_group g,
     gis_scheme_group_member m
WHERE g.gis_scheme_group_id = m.gis_scheme_group_id
AND g.code = @code
GO



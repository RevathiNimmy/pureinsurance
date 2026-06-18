SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_SCL_Del_By_Scheme'
GO


CREATE PROCEDURE spu_GIS_SCL_Del_By_Scheme
    @qm_insurer_ref varchar(10),
    @scheme_no int
AS


BEGIN
DELETE FROM gis_scheme_cobol_linkage
    FROM gis_scheme s
     WHERE gis_scheme_cobol_linkage.gis_scheme_id = s.gis_scheme_id
  AND   s.qm_insurer_ref = @qm_insurer_ref
  AND   s.scheme_no = @scheme_no
END
GO



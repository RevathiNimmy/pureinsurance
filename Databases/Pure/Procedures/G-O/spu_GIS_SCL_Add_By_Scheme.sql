SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_SCL_Add_By_Scheme'
GO


CREATE PROCEDURE spu_GIS_SCL_Add_By_Scheme
    @qm_insurer_ref varchar(10),
    @scheme_no int
AS


BEGIN
INSERT INTO gis_scheme_cobol_linkage (linkage_map_id,item_sequence,gis_scheme_id)
    SELECT l.linkage_map_id,
           l.item_sequence,
           s.gis_scheme_id
    FROM   gis_cobol_linkage l,
           gis_scheme s,
           wrk_cobol_linkage w
    WHERE  w.qmcoy = s.qm_insurer_ref
    AND    w.schm = s.scheme_no
    AND    w.fld_name = l.item_name
    AND    s.scheme_status > 0
    AND    s.qm_insurer_ref = @qm_insurer_ref
 AND    s.scheme_no = @scheme_no

END
GO



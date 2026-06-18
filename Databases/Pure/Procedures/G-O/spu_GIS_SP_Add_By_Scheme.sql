SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_SP_Add_By_Scheme'
GO


CREATE PROCEDURE spu_GIS_SP_Add_By_Scheme
    @qm_insurer_ref varchar(10),
    @scheme_no int
AS


BEGIN

INSERT INTO gis_scheme_property (gis_scheme_id,object_name,property_name,required_pre,required_post)

    SELECT   l.gis_property_id,
             l.gis_object_id,
             s.gis_scheme_id,
             MAX(w.pre),
             MAX(w.post)
    FROM     gis_cobol_linkage l,
             gis_scheme s,
             wrk_scheme_properties w
    WHERE    w.qmcoy = s.qm_insurer_ref
    AND      w.schm = s.scheme_no
    AND      w.fld_name = l.item_name    AND      s.scheme_status > 0
    AND      s.qm_insurer_ref = @qm_insurer_ref
    AND      s.scheme_no = @scheme_no
    AND      l.gis_property_id is not null
    GROUP BY l.gis_property_id,l.gis_object_id, s.gis_scheme_id
END
GO



EXECUTE DDLDropProcedure 'spu_GIS_SP_Add_By_Scheme_V2'
GO

CREATE PROCEDURE spu_GIS_SP_Add_By_Scheme_V2
    @qm_insurer_ref varchar(10),
    @scheme_no int,
    @linkage_map_min int,
    @linkage_map_max int,
    @class_of_business varchar(10)
AS

BEGIN
 
    INSERT INTO gis_scheme_property (gis_scheme_id,object_name,property_name,required_pre,required_post)
    SELECT s.gis_scheme_id,
        l.object_name,
        l.property_name,
        MAX(w.pre),
        MAX(w.post)
    FROM gis_cobol_linkage l,
        gis_scheme s,
        wrk_scheme_properties w,
        gis_insurer i
    WHERE w.qmcoy = s.qm_insurer_ref
    AND w.schm = s.scheme_no
    AND i.gis_insurer_id = s.gis_insurer_id
    AND s.class_of_business = @class_of_business
    AND w.fld_name = l.item_name
    AND (l.linkage_map_id > @linkage_map_min AND l.linkage_map_id < @linkage_map_max)
    AND s.scheme_status > 0
    AND s.qm_insurer_ref = @qm_insurer_ref
    AND s.scheme_no = @scheme_no
    AND l.property_name IS NOT NULL
    AND l.object_name IS NOT NULL
    AND (l.insurer_code = i.abi_81_insurer OR l.insurer_code is NULL)
    GROUP BY s.gis_scheme_id, l.object_name, l.property_name
END


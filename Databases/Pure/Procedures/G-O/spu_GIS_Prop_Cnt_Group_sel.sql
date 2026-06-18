SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Prop_Cnt_Group_sel'
GO


CREATE PROCEDURE spu_GIS_Prop_Cnt_Group_sel
    @gis_business_type_id int,
    @gis_scheme_group_id int
AS
/*
    SELECT DISTINCT gis_property_id
      FROM GIS_Scheme_Property sp,
           GIS_Scheme_Group_Member sgm,
           GIS_Scheme s
     WHERE sp.GIS_Scheme_id = sgm.GIS_scheme_id AND
           sgm.GIS_scheme_id = s.GIS_scheme_id AND
           gis_business_type_id = @gis_business_type_id and
           gis_scheme_group_id = @gis_scheme_group_id and
           required_pre > 0
*/

    SELECT DISTINCT p.gis_property_id
      FROM GIS_Scheme_Property sp,
           GIS_Scheme_Group_Member sgm,
           GIS_Scheme s,
           GIS_object o,
           GIS_property p
     WHERE sp.GIS_Scheme_id = sgm.GIS_scheme_id AND
           sgm.GIS_scheme_id = s.GIS_scheme_id AND
           gis_business_type_id = @gis_business_type_id and
           gis_scheme_group_id = @gis_scheme_group_id and
           required_pre > 0
    AND    sp.object_name = o.object_name
    AND    sp.property_name = p.property_name
    AND    o.gis_object_id = p.gis_object_id
GO



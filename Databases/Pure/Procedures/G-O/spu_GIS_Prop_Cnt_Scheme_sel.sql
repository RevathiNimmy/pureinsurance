SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Prop_Cnt_Scheme_sel'
GO


CREATE PROCEDURE spu_GIS_Prop_Cnt_Scheme_sel
    @gis_scheme_id int
AS
/*
    SELECT DISTINCT gis_property_id
      FROM GIS_Scheme_Property
     WHERE gis_scheme_id = @gis_scheme_id and
           required_pre > 0
*/
    SELECT DISTINCT p.gis_property_id
      FROM GIS_Scheme_Property sp,
           GIS_object o,
           GIS_property p
     WHERE sp.gis_scheme_id = @gis_scheme_id and
           sp.required_pre > 0
    AND    sp.object_name = o.object_name
    AND    sp.property_name = p.property_name
    AND    o.gis_object_id = p.gis_object_id
GO



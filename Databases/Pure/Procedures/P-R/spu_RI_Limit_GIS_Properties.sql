SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ri_limit_gis_properties'
GO


CREATE PROCEDURE spu_ri_limit_gis_properties
    @risk_type_id int
AS


select  distinct
    p.gis_property_id,
    o.object_name,
    p.property_name,
    p.gis_object_id
from    risk_type r,
    gis_screen s,
    gis_object o,
    gis_property p,
    gis_user_def_header_inds i
where   r.risk_type_id = @risk_type_id
and r.gis_screen_id = s.gis_screen_id
and s.gis_data_model_id = o.gis_data_model_id
and o.gis_object_id = p.gis_object_id
and (convert(int, p.Specials_Type_Reference) = i.gis_user_def_header_id
and p.Specials_Type = 6)
order   by object_name, property_name
GO


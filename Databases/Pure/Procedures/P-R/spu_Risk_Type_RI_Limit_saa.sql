SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_risk_type_ri_limit_saa'
GO


CREATE PROCEDURE spu_risk_type_ri_limit_saa  
    @risk_type_id int,
    @risk_type_ri_limit_version_id int  
AS  
  
select  rrp.risk_type_id,  
    rrp.risk_type_ri_properties_seq_id,  
    rrp.gis_property_id,  
    o.object_name,  
    p.property_name  
from    risk_type_ri_properties rrp,  
    gis_property p,  
    gis_object o  
where   rrp.risk_type_id = @risk_type_id  
and rrp.gis_property_id = p.gis_property_id  
and p.gis_object_id = o.gis_object_id
and rrp.risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id

GO



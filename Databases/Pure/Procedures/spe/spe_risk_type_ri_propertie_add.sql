SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_type_ri_propertie_add'
GO

CREATE PROCEDURE spe_risk_type_ri_propertie_add  
    @risk_type_id int,  
    @risk_type_ri_properties_seq_id int,  
    @gis_property_id int,
    @risk_type_ri_limit_version_id int  
  
AS  
  
BEGIN  
INSERT INTO risk_type_ri_properties (  
    risk_type_id ,  
    risk_type_ri_properties_seq_id ,  
    gis_property_id,
    risk_type_ri_limit_version_id )  
VALUES (  
    @risk_type_id,  
    @risk_type_ri_properties_seq_id,  
    @gis_property_id,
    @risk_type_ri_limit_version_id)  
END  

GO


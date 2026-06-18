SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_type_ri_propertie_sel'
GO

CREATE PROCEDURE spe_risk_type_ri_propertie_sel  
    @risk_type_id int,  
    @risk_type_ri_properties_seq_id int,
    @risk_type_ri_limit_version_id int  

AS  

BEGIN  
SELECT  risk_type_id,  
    risk_type_ri_properties_seq_id,  
    gis_property_id  
FROM    risk_type_ri_properties  
WHERE   risk_type_id = @risk_type_id  
AND risk_type_ri_properties_seq_id = @risk_type_ri_properties_seq_id
AND risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id  
END  

GO


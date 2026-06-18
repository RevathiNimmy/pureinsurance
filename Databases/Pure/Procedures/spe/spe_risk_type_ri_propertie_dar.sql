SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_type_ri_propertie_dar'
GO

CREATE PROCEDURE spe_risk_type_ri_propertie_dar  
 @risk_type_id int,
 @risk_type_ri_limit_version_id int,
 @UserId int = NULL,
 @UniqueId varchar(50) = NULL,
 @ScreenHierarchy varchar(500) = NULL
AS  
  
UPDATE rtrp
SET UserId = @UserId,
    UniqueId = @UniqueId,
    ScreenHierarchy = @ScreenHierarchy + '/(' + gso.object_name + ')'
FROM risk_type_ri_properties rtrp
INNER JOIN GIS_Property gp ON gp.gis_property_id = rtrp.gis_property_id
INNER JOIN GIS_Object gso ON gso.gis_object_id = gp.gis_object_id
WHERE rtrp.risk_type_id = @risk_type_id
  AND rtrp.risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id


DELETE  
FROM risk_type_ri_properties  
WHERE risk_type_id = @risk_type_id 
AND risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id 

GO


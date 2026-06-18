SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_type_ri_values_dar'
GO

CREATE PROCEDURE spe_risk_type_ri_values_dar  
 @risk_type_id int,
 @risk_type_ri_limit_version_id int  
AS  
  
DELETE  
FROM risk_type_ri_values  
WHERE risk_type_id = @risk_type_id  
AND risk_type_ri_limit_version_id=@risk_type_ri_limit_version_id

GO


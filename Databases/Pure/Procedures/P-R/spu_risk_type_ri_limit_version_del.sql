EXECUTE DDLDropProcedure 'spu_risk_type_ri_limit_version_del'
GO
CREATE PROCEDURE spu_risk_type_ri_limit_version_del
 @risk_type_id INT,  
 @risk_type_ri_limit_version_id INT,
	@UserId int = NULL,
	@UniqueId varchar(50) = NUll,
	@ScreenHierarchy varchar(500) = NUll   
     
AS    
    
BEGIN    
  
DELETE FROM Risk_Type_RI_Values   
WHERE risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id  and risk_type_id = @risk_type_id
  
DELETE FROM risk_type_ri_properties   
WHERE  risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id  and risk_type_id = @risk_type_id

IF @UniqueId IS NOT NULL
BEGIN
	UPDATE rtrlv
	SET UserId = @UserId,
		UniqueId = @UniqueId,
		ScreenHierarchy = @ScreenHierarchy + '/Limit(' + rtrlv.description + ')'
	FROM Risk_Type_RI_Limit_Version rtrlv
	WHERE risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id 
	  AND risk_type_id = @risk_type_id
END


DELETE FROM Risk_Type_RI_Limit_Version  
WHERE risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id and risk_type_id = @risk_type_id 
  
END

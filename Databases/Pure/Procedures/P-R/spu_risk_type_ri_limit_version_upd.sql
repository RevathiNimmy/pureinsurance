EXECUTE DDLDropProcedure 'spu_risk_type_ri_limit_version_upd'
GO

CREATE PROCEDURE spu_risk_type_ri_limit_version_upd
	@risk_type_ri_limit_version_id INT, 
    @risk_type_id INT,  
    @ri_limit_desc VARCHAR(255),  
    @limit_effective_date DATETIME,
    @limit_expiry_date DATETIME,
	@UserId int = NULL,
	@UniqueId varchar(50) = NUll,
	@ScreenHierarchy varchar(500) = NUll  
  
AS  
  
BEGIN  
UPDATE Risk_Type_RI_Limit_Version
SET risk_type_id = @risk_type_id,  
    description =@ri_limit_desc  ,  
    ri_limit_start_date = @limit_effective_date,
    ri_limit_end_date=@limit_expiry_date,
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
WHERE risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id

END

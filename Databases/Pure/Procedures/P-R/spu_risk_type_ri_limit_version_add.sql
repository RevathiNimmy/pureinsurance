EXECUTE DDLDropProcedure 'spu_risk_type_ri_limit_version_add'
GO

CREATE PROCEDURE spu_risk_type_ri_limit_version_add  
    @risk_type_id INT,  
    @ri_limit_desc VARCHAR(255),  
    @limit_effective_date DATE,
    @limit_expiry_date DATE,  
	@risk_type_ri_limit_version_id INT OUTPUT,
	@UserId int = NULL,
	@UniqueId varchar(50) = NUll,
	@ScreenHierarchy varchar(500) = NUll  
AS  
  
BEGIN  
INSERT INTO [Risk_Type_RI_Limit_Version] (  
    risk_type_id ,  
    description  ,  
    ri_limit_start_date,
    ri_limit_end_date,
	UserId,
	UniqueId,
	ScreenHierarchy)  
VALUES (  
    @risk_type_id,  
    @ri_limit_desc,  
    @limit_effective_date,
    @limit_expiry_date,
	@UserId,
	@UniqueId,
	@ScreenHierarchy)  

SELECT @risk_type_ri_limit_version_id = SCOPE_IDENTITY() 
    
END  


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Account_Executive_Handler_Transfer'
GO

CREATE PROCEDURE spu_Account_Executive_Handler_Transfer
	@Code VARCHAR(20) = NULL,
	@OldReference VARCHAR(100) = NULL,
	@NewReference VARCHAR(100) = NULL,
	@OldReferenceCnt INT = NULL,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL  
  
AS  

DECLARE @UpdateDate VARCHAR(21);
DECLARE @configuration_audit_master_id int
DECLARE @FieldDisplayName VARCHAR(50);

SET @UpdateDate = CONVERT(NVARCHAR(30), GETDATE(), 126);


 
IF EXISTS(SELECT 1 FROM configuration_audit_master WHERE UniqueId = @UniqueId)
BEGIN
	SELECT @configuration_audit_master_id = @configuration_audit_master_id FROM configuration_audit_master WHERE UniqueId = @UniqueId
END
ELSE
BEGIN
	IF @Code = 'AH'
	BEGIN
		INSERT INTO configuration_audit_master (UniqueId, Module_Id, ModuleName, UpdateDate, UserID)
		VALUES (@UniqueID, 15, 'Account Handler', @UpdateDate, @UserId)

		SELECT @configuration_audit_master_id = SCOPE_IDENTITY()
	END
	ELSE IF @Code = 'CO'
	BEGIN
		INSERT INTO configuration_audit_master (UniqueId, Module_Id, ModuleName, UpdateDate, UserID)
		VALUES (@UniqueID, 14, 'Account Executive', @UpdateDate, @UserId)

		SELECT @configuration_audit_master_id = SCOPE_IDENTITY()
	END
END

IF @Code = 'AH'
	BEGIN
		SELECT @FieldDisplayName = 'Transfer Policies to Account Handler'
	END
ELSE IF @Code = 'CO'
	BEGIN
		SELECT @FieldDisplayName = 'Transfer Clients to Account Executive'
	END


BEGIN  
INSERT INTO configuration_audit_details(  
    configuration_audit_master_id ,  
    Type ,  
    TableName ,  
    key_field_name,
	key_field_value,
	key_field_desc,
	FieldName,
	FieldDisplayName,
	OldValue,
	NewValue)  
VALUES (  
    @configuration_audit_master_id,  
    'I',  
    'Party',  
    'party_cnt',  
    @OldReferenceCnt,
	@ScreenHierarchy,
	@FieldDisplayName,
	@FieldDisplayName,
	 @OldReference,
	 @NewReference)  
END  



GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_UDL_Version_upd'
GO

CREATE PROCEDURE spu_UDL_Version_upd  
    @table varchar(30),
    @version Integer = 0,
	@Oldversion Integer = 0,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL
AS  

DECLARE @SQL nvarchar(4000)  
DECLARE @ModuleId INT = 13
DECLARE @ModuleName VARCHAR(100) = 'Lookup Maintenance'
DECLARE @configuration_audit_master_id INT
  
IF @version <> 0  
BEGIN  
	-- Create audit trail if UserId provided
	IF @UserId IS NOT NULL
	BEGIN
		-- Use provided UniqueId or generate one if not provided
		IF @UniqueId IS NULL
  			SET @UniqueId = @table + '_' + CONVERT(VARCHAR(10), @UserId) + '_' + CONVERT(CHAR(8), GETDATE(), 112)
		
		-- Check if UniqueId already exists
		SELECT @configuration_audit_master_id = configuration_audit_master_id 
		FROM configuration_audit_master 
		WHERE UniqueId = @UniqueId
		
		-- Create audit master record only if it doesn't exist
		IF @configuration_audit_master_id IS NULL
		BEGIN
			INSERT INTO configuration_audit_master (UniqueId, Module_Id, ModuleName, UpdateDate, UserID) 
			VALUES (@UniqueId, @ModuleId, @ModuleName, GETDATE(), @UserId)
			SELECT @configuration_audit_master_id = SCOPE_IDENTITY()
		END
		
		-- Insert audit details for version update
		IF @Oldversion<>@version
		INSERT INTO configuration_audit_details (
			configuration_audit_master_id,
			Type, 
			TableName, 
			key_field_name,
			key_field_value, 
			key_field_desc,
			FieldName, 
			FieldDisplayName,
			OldValue, 
			NewValue
		)
		VALUES (
			@configuration_audit_master_id,
			'U',
			@table,
			'udl_version',
			'ALL',
			RTRIM(@table) +  '/Version(' + ISNULL(CONVERT(VARCHAR, @version), '0') + ')',
			'udl_version',
			'UDL Version',
			CONVERT(VARCHAR, @Oldversion),
			CONVERT(VARCHAR, @version)
		)
	END

 SET @SQL = ' UPDATE ' + QUOTENAME(@table) + ' Set udl_version = ' + CONVERT(VARCHAR, @version)  
 SET @SQL = @SQL + ' Where isnull(udl_version,0) =' + CONVERT(VARCHAR, @Oldversion)
END  
ELSE  
BEGIN  
 SET @SQL = 'SELECT @version= MAX(udl_version) FROM ' + @table  
 EXEC SP_EXECUTESQL @SQL,N'@Version INT OUTPUT',@version OUTPUT  
  
 SET @SQL = ' UPDATE ' + @table + ' Set udl_version = ' + Convert(varchar,@version)  
 SET @SQL = @SQL + ' Where isnull(udl_version,0) = 0 '  
END  
EXEC(@SQL)  
 
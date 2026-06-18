SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_UDL_Data_upd'
GO

CREATE PROCEDURE spu_UDL_Data_upd  
    @table varchar(30),  
    @code char(10),  
    @caption_id int,  
    @description varchar(255),
	@version int = 0,
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL
AS  
BEGIN  
  
    DECLARE @sql varchar(255)  
  	DECLARE @V_SQL nvarchar(350)
	DECLARE @Last_Version integer
	DECLARE @id integer
	
	SET @V_SQL = 'SELECT @last_Version= MAX(udl_version) FROM ' + @table + ' Where code='+''''+@code +''''
	EXEC SP_EXECUTESQL @V_SQL,N'@last_Version INT OUTPUT',@Last_Version OUTPUT

	SET @V_SQL = 'SELECT @ID = '+ @table+ '_id FROM ' + @table + ' Where code='+ ''''+ @code + '''' + ' AND udl_version =' + Convert(varchar,@last_version)  
	EXEC SP_EXECUTESQL @V_SQL,N'@ID INT OUTPUT',@ID OUTPUT

IF EXISTS(SELECT NULL FROM sysobjects WHERE [name] = @table)  
BEGIN  

	-- Capture old values for audit
	DECLARE @OldCaptionId int
	DECLARE @OldDescription varchar(255)
	DECLARE @GetOldSQL nvarchar(500)
 SET @GetOldSQL = 'SELECT @OldCaptionId = caption_id, @OldDescription = description FROM ' + QUOTENAME(@table) + ' WHERE code = @code'
 IF @version > 0
     SET @GetOldSQL = @GetOldSQL + ' AND udl_version = @version'

 EXEC SP_EXECUTESQL
     @GetOldSQL,
     N'@OldCaptionId int OUTPUT, @OldDescription varchar(255) OUTPUT, @code char(10), @version int',
     @OldCaptionId OUTPUT,
     @OldDescription OUTPUT,
     @code,
     @version

	-- Create audit trail if UserId provided and values changed
	IF @UserId IS NOT NULL AND (@OldCaptionId != @caption_id OR @OldDescription != @description)
	BEGIN
		DECLARE @ModuleId INT = 13
		DECLARE @ModuleName VARCHAR(100) = 'Lookup Maintenance'
		DECLARE @configuration_audit_master_id INT
		
		-- Use provided UniqueId or generate one if not provided
		IF @UniqueId IS NULL
			SET @UniqueId = @table + '_' + CONVERT(VARCHAR, @UserId) + '_' + CONVERT(VARCHAR, CAST(GETDATE() AS DATE), 121)
		
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
			
		-- Insert audit details for description change
		IF @OldDescription != @description
		BEGIN
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
				'code',
				@code,
				RTRIM(@table) + '(' + RTRIM(@code) + ')/Version(' + ISNULL(CONVERT(VARCHAR, @Last_Version), '0') + ')',
				'description',
				'Description',
				@OldDescription,
				@description
			)
		END
	END

	-- Perform the update
	Set @SQL = 'UPDATE ' + @table + ' SET caption_id = ' + cast(@caption_id as varchar) + ', description = ''' + @description + ''' WHERE code='+ ''''+  @code + ''''    	
	IF @version <>0 
   		Set @SQL = @SQL +' AND ' + @table+ '_id = ' + CONVERT(varchar,@ID)
		EXECUTE (@SQL)  

 END  
  
END  


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

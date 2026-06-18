SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_gis_listentry_update'
GO

CREATE  PROCEDURE spu_gis_listentry_update  
@table varchar(30),  
@field varchar(1500),  
@code varchar(10),  
@data varchar(1500),  
@effdate varchar(20),  
@language_id smallint,  
@caption varchar(255),
@UserId INT = NULL,
@UniqueId VARCHAR(50) = NULL
  
AS  
  
DECLARE @updateStmt NVARCHAR(1000)  
DECLARE @selectStmt NVARCHAR(1000)  
DECLARE @id integer  
DECLARE @SQL nvarchar(4000)  
DECLARE @captionId int  
DECLARE @V_SQL NVARCHAR(400)
DECLARE @VERSION INT
DECLARE @OldValue NVARCHAR(1500)
DECLARE @OldEffDate NVARCHAR(20)
DECLARE @GetOldSQL  nvarchar(4000)  

Set @SQL = 'DECLARE id_cursor CURSOR GLOBAL for  SELECT ' + @table + '_id from ' + @table  + ' where code='+ ''''+  @code + ''''  
 
EXEC(@SQL)  
OPEN id_cursor  
FETCH NEXT FROM id_cursor INTO @id  
CLOSE id_cursor  
  
DEALLOCATE id_cursor  
  
if @id=0 return  

SET @V_SQL = 'SELECT @Version= MAX(udl_version) FROM ' + @table + ' where code='+ ''''+  @code + '''' 
EXEC SP_EXECUTESQL @V_SQL,N'@Version INT OUTPUT',@VERSION OUTPUT

-- Capture old values for audit
SET @GetOldSQL = 'SELECT @OldValue = ' + @field + ', @OldEffDate = effective_date FROM ' + @table + ' WHERE code = ''' + @code + ''''
IF @version > 0
    SET @GetOldSQL = @GetOldSQL + ' AND udl_version = ' + CONVERT(varchar, @version)

EXEC SP_EXECUTESQL @GetOldSQL, N'@OldValue NVARCHAR(1500) OUTPUT, @OldEffDate NVARCHAR(20) OUTPUT', @OldValue OUTPUT, @OldEffDate OUTPUT

-- Create audit trail if UserId provided and values changed
IF @UserId IS NOT NULL AND (
       (@OldValue IS NULL AND @data IS NOT NULL)
    OR (@OldValue IS NOT NULL AND @data IS NULL)
    OR (@OldValue IS NOT NULL AND @data IS NOT NULL AND @OldValue <> @data)
    OR (@OldEffDate IS NULL AND @effdate IS NOT NULL)
    OR (@OldEffDate IS NOT NULL AND @effdate IS NULL)
    OR (@OldEffDate IS NOT NULL AND @effdate IS NOT NULL AND @OldEffDate <> @effdate)
)
BEGIN
    DECLARE @ModuleId INT = 13
    DECLARE @ModuleName VARCHAR(100) = 'Lookup Maintenance'
    DECLARE @configuration_audit_master_id INT
    
    -- Generate UniqueId based on table, UserId, and date
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
    
    -- Insert audit details for field change
    IF (@OldValue IS NULL AND @data IS NOT NULL)
       OR (@OldValue IS NOT NULL AND @data IS NULL)
       OR (@OldValue IS NOT NULL AND @data IS NOT NULL AND @OldValue <> @data)
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
            RTRIM(@table) + '(' + RTRIM(@code) + ')/Version(' + ISNULL(CONVERT(VARCHAR, @VERSION), '0') + ')',
            @field,
            @field,
            @OldValue,
            @data
        )
    END
    
    
END

-- Perform the update
SELECT @updateStmt='UPDATE ' + @table +' set ' +  @field +'=' + ''''+ @data + ''''  
SELECT @updateStmt=@updateStmt + ',effective_date=' +''''+@effdate +''''+ ' where code='+ ''''+  @code  +''''  

IF @version > 0
SELECT @updateStmt=@updateStmt + ' AND udl_version =' + convert(varchar,@version)

EXECUTE sp_executeSQL @updateStmt  

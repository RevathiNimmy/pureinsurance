SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_gis_listentry_add'
GO

CREATE PROCEDURE spu_gis_listentry_add  
@table varchar(30),  
@fields varchar(max),  
@data varchar(max),  
@effdate varchar(20),  
@language_id smallint,  
@caption varchar(255),  
@version integer,
@UserId INT = NULL,
@UniqueId VARCHAR(50) = NULL
  
AS  
DECLARE @selectstmt NVARCHAR(max)  
DECLARE @initialStmt VARCHAR(max)  
DECLARE @initialData VARCHAR(max)  
DECLARE @id integer  
DECLARE @SQL nvarchar(max)  
DECLARE @captionId int
DECLARE @code varchar(10)
  
Set @SQL = 'DECLARE id_cursor CURSOR GLOBAL FOR select MAX(' + @table + '_id) from ' + @table  
EXEC(@SQL)  
OPEN id_cursor  
FETCH NEXT FROM id_cursor INTO @id  
CLOSE id_cursor  
  
DEALLOCATE id_cursor  
exec spu_pm_caption_id_return @language_id , @caption ,@captionId output  

SELECT @fields = @fields + ',[UDL_version]'
SELECT @data = @data + ','+ convert(varchar,@version)+ ''
  
select @id = isnull(@id,0)+1  
SELECT @initialStmt = 'INSERT ' + @table + '(' + @table + '_id,caption_ID,is_deleted,effective_date,code,description'  
SELECT @initialData = convert(varchar(10),@captionId) + ',0,' + '''' + @EffDate + ''''  
SELECT @selectstmt = @initialstmt + @fields + ') values ('+ CONVERT(varchar(10),@id) + ',' + @initialData + '' + @data + ')'  

-- Extract code from @data for audit trail (first value after initial comma and quotes)
DECLARE @startPos INT = CHARINDEX('''', @data) + 1
DECLARE @endPos INT

IF @startPos > 1
BEGIN
    SET @endPos = CHARINDEX('''', @data, @startPos)
    IF @endPos > @startPos
        SET @code = SUBSTRING(@data, @startPos, @endPos - @startPos)
END

-- Create audit trail if UserId provided
IF @UserId IS NOT NULL
BEGIN
    DECLARE @ModuleId INT = 13
    DECLARE @ModuleName VARCHAR(100) = 'Lookup Maintenance'
    DECLARE @configuration_audit_master_id INT
    DECLARE @AuditMessage VARCHAR(50) = 'New record added'
    DECLARE @VersionExists INT = 0
    DECLARE @AuditExists INT = 0
    DECLARE @CheckSQL NVARCHAR(500)
	DECLARE @KeyFieldDescription VARCHAR(100)
    
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
    
    -- Check if version already exists in table
    SET @CheckSQL = 'SELECT @VersionExists = COUNT(*) FROM ' + @table + ' WHERE UDL_version = ' + CONVERT(VARCHAR, @version)
    EXEC sp_executesql @CheckSQL, N'@VersionExists INT OUTPUT', @VersionExists OUTPUT
    
    -- Check if New UDL Version added audit already exists
    SELECT @AuditExists = COUNT(*) FROM configuration_audit_details 
    WHERE TableName = @table AND FieldDisplayName = 'New UDL Version added' 
    AND configuration_audit_master_id = @configuration_audit_master_id
    
    -- Set message if first version entry and no existing audit
    IF @VersionExists = 0 AND @AuditExists = 0
	BEGIN
        SET @AuditMessage = 'New UDL Version added'
		SET @KeyFieldDescription= RTRIM(@table) +  '/Version(' + ISNULL(CONVERT(VARCHAR, @version), '0') + ')'
    END
	ELSE
		SET @KeyFieldDescription= RTRIM(@table) + '(' + RTRIM(@code) + ')/Version(' + ISNULL(CONVERT(VARCHAR, @version), '0') + ')'

        -- Insert audit detail for new record
    IF @AuditExists = 0
    INSERT INTO configuration_audit_details (
        configuration_audit_master_id,
        Type, 
        TableName, 
        key_field_name,
        key_field_value, 
        key_field_desc,
        FieldName, 
        FieldDisplayName
    )
    VALUES (
        @configuration_audit_master_id,
        'I',
        @table,
        'code',
        @code,
        @KeyFieldDescription,
        @AuditMessage,
        @AuditMessage
    )
END

EXECUTE sp_executeSQL @selectstmt  
 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_list_pm_caption'
GO

CREATE PROCEDURE spu_gis_list_pm_caption  
    @table varchar(30),  
    @fields varchar(max)='',
    @UserId INT = NULL,
    @UniqueId VARCHAR(50) = NULL
AS  
BEGIN  
    DECLARE @SQL nvarchar(max)  
    DECLARE @primarykey varchar(128)
    DECLARE @TableCreated BIT = 0

    -- Basic validation to prevent unsafe characters in dynamic field list
    IF @fields IS NOT NULL AND PATINDEX('%[^0-9A-Za-z_,\[\] ]%', @fields) > 0
    BEGIN
        RAISERROR('Invalid characters in @fields parameter.', 16, 1);
        RETURN;
    END
    SELECT @table = rtrim(@table)  
    SELECT @primarykey = @table + '_id'  
  
    IF NOT EXISTS (SELECT NULL FROM sysobjects WHERE name = @table AND type = 'U') BEGIN  
        SELECT @SQL = 'CREATE TABLE [' + @table + '] (' +  
            '[' + @primarykey + '] integer, ' +  
            '[caption_id] integer NOT NULL, ' +  
            '[code] char(10) NOT NULL, ' +  
            '[description] varchar(255), ' +  
            '[is_deleted] tinyint NOT NULL, ' +  
            '[effective_date] datetime NOT NULL, ' +  
            '[UDL_version] integer, ' + 
            @fields + ' ' +  
            'CONSTRAINT [PK__' + @table + '] PRIMARY KEY CLUSTERED ([' + @primarykey + '])' +  
            ')'  
  
        EXECUTE sp_executesql @SQL
        SET @TableCreated = 1
    END  
  
    EXECUTE DDLAddIndex @table, 'caption_id'  
  
    EXECUTE DDLAddIndex @table, 'code'  
    
    EXECUTE DDLAddIndex @table, 'udl_version'	

    -- Create audit trail if UserId provided and table was created
    IF @UserId IS NOT NULL AND @TableCreated = 1
    BEGIN
        DECLARE @ModuleId INT = 13
        DECLARE @ModuleName VARCHAR(100) = 'Lookup Maintenance'
        DECLARE @configuration_audit_master_id INT
        
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
        
        -- Insert audit detail for table creation
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
            'C',
            @table,
            'table_name',
            @table,
            'UDL Creation: ' + @table,
            'New UDL created',
            'New UDL created',
            NULL,
            NULL
        )
    END
  
    INSERT INTO PMProduct_Lookup (  
        pmproduct_id,  
        lookup_table_name,  
        edit_privilege_level,  
        linked_caption_id,  
        linked_class_name,  
        linked_object_name,  
        is_generic_maintenance  
    ) VALUES (  
        2,  
        @table,  
        3,  
        null,  
        null,  
        null,  
        1  
    )  
END  
GO


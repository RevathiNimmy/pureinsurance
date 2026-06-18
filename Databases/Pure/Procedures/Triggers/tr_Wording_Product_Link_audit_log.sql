SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropTrigger 'tr_Wording_Product_Link_audit_log'
GO
CREATE TRIGGER tr_Wording_Product_Link_audit_log ON dbo.Wording_Product_Link 
    FOR INSERT, UPDATE, DELETE 
AS
SET NOCOUNT ON;

/*declare all the variables*/
DECLARE @bit INT;
DECLARE @field INT;
DECLARE @maxfield INT;
DECLARE @char INT;
DECLARE @fieldname VARCHAR(128);
DECLARE @TableName VARCHAR(128);
DECLARE @TableSchema VARCHAR(128);
DECLARE @PKCols VARCHAR(1000);
DECLARE @sql VARCHAR(2000);
DECLARE @UpdateDate VARCHAR(21);
DECLARE @UserId INT;
DECLARE @Type CHAR(1);
DECLARE @PKSelect VARCHAR(1000);
DECLARE @isForeignKey BIT;
DECLARE @parentTable VARCHAR(128);
DECLARE @parentKeyColumn VARCHAR(128);
DECLARE @parentValueColumn VARCHAR(128);
DECLARE @UniqueID VARCHAR(50)
DECLARE @PKColName VARCHAR(100);
DECLARE @ModuleName VARCHAR(100);
DECLARE @ValueColName VARCHAR(100);
DECLARE @FieldDesc varchar(200)
DECLARE @ModuleId INT
DECLARE @branch_id INT
DECLARE @docTemplateId INT
DECLARE @docTemplateCode VARCHAR(100)
DECLARE @default INT

SELECT @ModuleId=28, @ModuleName='Product Risk Maintenance', @PKColName = 'branch_id',@ValueColName='ScreenHierarchy';

--SET @UserName = SYSTEM_USER;
SET @UpdateDate = CONVERT(NVARCHAR(30), GETDATE(), 126);

/*now set some of these variables*/
SELECT 
    @TableName = OBJECT_NAME(parent_object_id),
    @TableSchema = OBJECT_SCHEMA_NAME(parent_object_id)
FROM sys.objects
WHERE 
    sys.objects.name = OBJECT_NAME(@@PROCID);

/*Action*/
IF EXISTS (SELECT * FROM INSERTED)
    IF EXISTS (SELECT * FROM DELETED)
         SET @Type = 'U'
    ELSE SET @Type = 'I'
    ELSE SET @Type = 'D'
;
/*get list of columns*/
SELECT * INTO #ins FROM INSERTED;

SELECT * INTO #del FROM DELETED;

IF @Type='D'
	SELECT @UserId=UserId,@UniqueID=UniqueID,@branch_id=branch_id FROM #del
ELSE
	SELECT @UserId=UserId,@UniqueID=UniqueID,@branch_id=branch_id,@docTemplateId=document_template_id,@default = [default] FROM #ins

SELECT @docTemplateCode=code  FROM Document_Template where @docTemplateId=document_template_id

/*set @PKCols and @PKSelect via SELECT statement.*/
SELECT @PKCols = /*Get primary key columns for full outer join*/
        COALESCE(@PKCols + ' and', ' on') 
        + ' i.[' + c.COLUMN_NAME + '] = d.[' + c.COLUMN_NAME + ']'
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS pk
        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS c ON (
            c.TABLE_NAME = pk.TABLE_NAME
            AND c.TABLE_SCHEMA = pk.TABLE_SCHEMA
            AND c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME
        )
    WHERE pk.TABLE_NAME = @TableName
        AND pk.TABLE_SCHEMA = @TableSchema
        AND CONSTRAINT_TYPE = 'PRIMARY KEY'
;
SELECT @PKSelect = /*Get primary key select for insert*/
        COALESCE(@PKSelect + '+', '') 
        + '''<[' + COLUMN_NAME + ']=''+CONVERT(VARCHAR(100),
        COALESCE(I.[' + COLUMN_NAME + '],d.[' + COLUMN_NAME + ']))+''>'''
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk,
        INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
    WHERE  pk.TABLE_NAME = @TableName
        AND pk.TABLE_SCHEMA = @TableSchema
        AND CONSTRAINT_TYPE = 'PRIMARY KEY'
        AND c.TABLE_NAME = pk.TABLE_NAME
            AND c.TABLE_SCHEMA = pk.TABLE_SCHEMA
        AND c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME
;
IF @PKCols IS NULL
BEGIN
    RAISERROR('no PK on table %s', 16, -1, @TableName);
    RETURN;
END

SET @field = 0;
SET @maxfield = (
    SELECT 
        MAX(
            COLUMNPROPERTY(
                OBJECT_ID(TABLE_SCHEMA + '.' + @TableName),
                COLUMN_NAME,
                'ColumnID'
            )
        )
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE  
        TABLE_NAME = @TableName
        AND TABLE_SCHEMA = @TableSchema
);


WHILE @field < @maxfield
BEGIN
	SELECT @fieldname=null,@isForeignKey=0,@parentTable=null,@parentKeyColumn=null
    SET @field = (
        SELECT 
            MIN(
                COLUMNPROPERTY(
                    OBJECT_ID(TABLE_SCHEMA + '.' + @TableName),
                    COLUMN_NAME,
                    'ColumnID'
                )
            )
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE  
            TABLE_NAME = @TableName
            AND TABLE_SCHEMA = @TableSchema
            AND COLUMNPROPERTY(
                    OBJECT_ID(TABLE_SCHEMA + '.' + @TableName),
                    COLUMN_NAME,
                    'ColumnID'
                ) > @field
    );
    SET @bit = (@field - 1)% 8 + 1;
    SET @bit = POWER(2, @bit - 1);
    SET @char = ((@field - 1) / 8) + 1;

    IF (
        SUBSTRING(COLUMNS_UPDATED(), @char, 1) & @bit > 0
        OR @Type IN ('I', 'D')
    )
    BEGIN
        SET @fieldname = (
            SELECT 
                COLUMN_NAME
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE  
                TABLE_NAME = @TableName
                AND TABLE_SCHEMA = @TableSchema
                AND COLUMNPROPERTY(
                        OBJECT_ID(TABLE_SCHEMA + '.' + @TableName),
                        COLUMN_NAME,
                        'ColumnID'
                    ) = @field
        );

IF @fieldname = 'branch_id'
BEGIN
	SELECT @FieldDesc='Branch Chosen'
END
ELSE IF @fieldname = 'default'
BEGIN
	SELECT @FieldDesc='Default'
	UPDATE i
	SET
		i.UserId          = @UserId,
		i.UniqueID        = @UniqueID,
		i.ScreenHierarchy = CONCAT(
			COALESCE(i.ScreenHierarchy, ''),
			'/(Branch',
			LTRIM(RTRIM(COALESCE(s.description, ''))),
			')'
		)
	FROM #ins AS i
	JOIN Source AS s
	  ON s.source_id = i.branch_id

	UPDATE d
	SET
		d.UserId          = @UserId,
		d.UniqueID        = @UniqueID,
		d.ScreenHierarchy = CONCAT(
			COALESCE(d.ScreenHierarchy, ''),
			'/(Branch',
			LTRIM(RTRIM(COALESCE(s.description, ''))),
			')'
		)
	FROM #del AS d
	JOIN Source AS s
	  ON s.source_id = d.branch_id
END

IF @fieldname in ('branch_id','default') AND @UniqueID IS NOT NULL
BEGIN	

DECLARE @configuration_audit_master_id int
IF @Type='D'
	SELECT @configuration_audit_master_id=m.configuration_audit_master_id FROM configuration_audit_master m INNER JOIN #del d ON m.UniqueId=d.UniqueId
ELSE
	SELECT @configuration_audit_master_id=m.configuration_audit_master_id FROM configuration_audit_master m INNER JOIN #ins i ON m.UniqueId=i.UniqueId
IF @configuration_audit_master_id IS NULL
BEGIN
	INSERT into configuration_audit_master (UniqueId,Module_Id,ModuleName,UpdateDate,UserID) Values (@UniqueID,@ModuleId, @ModuleName,@UpdateDate,@UserId)
	SELECT @configuration_audit_master_id=SCOPE_IDENTITY()
END

IF @Type='I' AND EXISTS(SELECT 1 FROM configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
			AND key_field_value=@branch_id AND FieldName='branch_id'  AND key_field_desc like ('%' + LTRIM(RTRIM(@docTemplateCode)) + '%') AND OldValue = (SELECT description From Source WHERE source_id = @branch_id) AND Type = 'D')
BEGIN
	DELETE configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
   AND key_field_value=@branch_id
   AND FieldName = @fieldname
   AND key_field_desc LIKE ('%' + LTRIM(RTRIM(@docTemplateCode)) + '%')
   AND OldValue = (SELECT description FROM Source WHERE source_id = @branch_id)
   AND Type = 'D'

END
ELSE IF @Type='I' AND EXISTS(SELECT 1 FROM configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
			AND key_field_value=@branch_id AND FieldName='default'  AND key_field_desc like ('%' + LTRIM(RTRIM(@docTemplateCode)) + '%') AND Type = 'D' AND OldValue <> @default)
BEGIN
	UPDATE configuration_audit_details SET Type = 'U',NewValue = (CASE WHEN OldValue = 0 Then 1 ELSE 0 END)  WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
   AND key_field_value=@branch_id
   AND FieldName = 'default'
   AND key_field_desc LIKE ('%' + LTRIM(RTRIM(@docTemplateCode)) + '%')
   AND OldValue <> @default
   AND Type = 'D'
END
ELSE IF @Type='I' AND EXISTS(SELECT 1 FROM configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
			AND key_field_value=@branch_id AND FieldName='default'  AND key_field_desc like ('%' + LTRIM(RTRIM(@docTemplateCode)) + '%') AND Type = 'D' AND OldValue = @default)
BEGIN
	DELETE FROM configuration_audit_details  WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
   AND key_field_value=@branch_id
   AND FieldName = 'default'
   AND key_field_desc LIKE ('%' + LTRIM(RTRIM(@docTemplateCode)) + '%')
   AND OldValue = @default
   AND Type = 'D'
END
ELSE
BEGIN
SELECT 
            @isForeignKey = COUNT(*),
            @parentTable = fk.foreign_key_table_name,
            @parentKeyColumn = fk.foreign_key_column_name,
			@parentValueColumn=fk.foreign_key_value_column_name
        FROM foreign_key_table fk
        WHERE fk.TABLE_NAME = @TableName
            AND fk.COLUMN_NAME = @fieldname
		GROUP BY fk.foreign_key_table_name, fk.foreign_key_column_name,foreign_key_value_column_name;
        
		--select @TableName, @fieldname, @isForeignKey,@parentTable, @parentKeyColumn 
        IF ISNULL(@isForeignKey,0) = 0
        BEGIN
            SET @sql = ('
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
                SELECT ' + CONVERT(VARCHAR(30),@configuration_audit_master_id) + ',''' 
                    + @Type + ''',''' 
                    + @TableName + ''',''' + @PKColName + ''',' 
                    + 'ISNULL(i.' + @PKColName + ',' + 'd.' + @PKColName  + '),'
					+ 'ISNULL(i.' + @ValueColName + ',' + 'd.' + @ValueColName  + '),''' 
					+ @fieldname  + ''','''
                    + @FieldDesc + ''''
                    + ',CONVERT(VARCHAR(1000),d.' + QUOTENAME(@fieldname)  + ')'
                    + ',CONVERT(VARCHAR(1000),i.' + QUOTENAME(@fieldname)  + ')' + 
                ' FROM #ins AS i FULL OUTER JOIN #del AS d'
                        + @PKCols + 
                ' WHERE i.' + QUOTENAME(@fieldname)  + ' <> d.' + QUOTENAME(@fieldname)  
                        + ' or (i.' + QUOTENAME(@fieldname)  + ' IS NULL AND  D.'
                        + QUOTENAME(@fieldname) 
                        + ' IS NOT NULL)' 
                        + ' OR (I.' + QUOTENAME(@fieldname)  + ' IS NOT NULL AND  D.' 
                        + QUOTENAME(@fieldname) 
                        + ' IS NULL)' 
            );
        END
        ELSE
        BEGIN
            SET @sql = ('
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
                SELECT ' + convert(varchar(30),@configuration_audit_master_id) + ',''' 
                    + @Type + ''',''' 
                    + @TableName + ''',''' + @PKColName + ''',' 
                    + 'ISNULL(i.' + @PKColName + ',' + 'd.' + @PKColName  + '),'
					+ 'ISNULL(i.' + @ValueColName + ',' + 'd.' + @ValueColName  + '),''' 
					+ @fieldname + ''','''
                    + @FieldDesc + ''''
                    + ',CONVERT(VARCHAR(1000),pd.' + @parentValueColumn + ')'
                    + ',CONVERT(VARCHAR(1000),pi.' + @parentValueColumn + ')' + 
                ' FROM #ins AS i FULL OUTER JOIN #del AS d'
                        + @PKCols + 
                ' LEFT JOIN ' + @parentTable + ' AS pi ON i.' + @fieldname + ' = pi.' + @parentKeyColumn +
				' LEFT JOIN ' + @parentTable + ' AS pd ON d.' + @fieldname + ' = pd.' + @parentKeyColumn
                           + ' WHERE i.' + QUOTENAME(@fieldname) + ' <> d.' + QUOTENAME(@fieldname)  
                        + ' OR (I.' + QUOTENAME(@fieldname) + ' is null and  d.'
                        + QUOTENAME(@fieldname)
                        + ' IS NOT NULL)' 
                        + ' OR (I.' + QUOTENAME(@fieldname) + ' IS NOT NULL AND  D.' 
                        + QUOTENAME(@fieldname)
                        + ' IS NULL)' 
            );
        END
        EXEC (@sql);
		END
	
    END
	END
END
SET NOCOUNT OFF;
GO

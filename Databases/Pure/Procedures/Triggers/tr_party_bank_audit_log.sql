SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropTrigger 'tr_party_bank_audit_log'
GO
CREATE TRIGGER tr_party_bank_audit_log ON dbo.party_bank
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
DECLARE @ModuleId INT
DECLARE @ValueColName VARCHAR(100);
DECLARE @FieldDesc VARCHAR(500)
DECLARE @ScreenHierarchy VARCHAR(500)

SELECT @PKColName = 'party_bank_id',@ValueColName='ScreenHierarchy'

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
	SELECT @UserId=UserId,@UniqueID=UniqueID,@ScreenHierarchy=ScreenHierarchy FROM #del
ELSE
	SELECT @UserId=UserId,@UniqueID=UniqueID,@ScreenHierarchy=ScreenHierarchy FROM #ins

IF UPPER(LEFT(@ScreenHierarchy,3))='FEE' 
	SELECT @ModuleId=4,@ModuleName='Fee Maintenance' 
ELSE IF UPPER(LEFT(@ScreenHierarchy,5))='AGENT' 
	SELECT @ModuleId=6,@ModuleName='Agent Maintenance' 
ELSE IF UPPER(LEFT(@ScreenHierarchy,9))='REINSURER' 
	SELECT @ModuleId=7,@ModuleName='Reinsurer Maintenance' 
Else IF UPPER(LEFT(@ScreenHierarchy,5)) = 'OTHER'
	SELECT @ModuleId=21,@ModuleName='Other Party Maintenance'


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
    RAISERROR('No PK on table %s', 16, -1, @TableName);
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
	SELECT @fieldname=NULL,@isForeignKey=0,@parentTable=NULL,@parentKeyColumn=NULL
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


SELECT @FieldDesc=(SELECT cast(value as varchar(500)) FROM sys.extended_properties  WHERE 
        name = N'MS_Description' 
        AND major_id = OBJECT_ID(@TableSchema + '.' + @TableName) 
        AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(@TableSchema + '.' + @TableName) AND name = @fieldname))

SELECT @FieldDesc= ISNULL(@FieldDesc,@fieldname)

IF @fieldname NOT IN ('UniqueId','UserId','ScreenHierarchy', 'is_default') AND @UniqueID IS NOT NULL
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

IF EXISTS(SELECT NULL FROM configuration_audit_master cam 
		INNER JOIN configuration_audit_details cad ON cam.configuration_audit_master_id=cad.configuration_audit_master_id 
		WHERE cam.configuration_audit_master_id=@configuration_audit_master_id AND FieldName IN ('New Agent Added', 'New Re-Insurer Added', 'New Other Party Added'))
RETURN

DECLARE @FieldValue VARCHAR(50)
IF @Type='I'
    SELECT @FieldValue='New Party Bank Added'
ELSE IF @Type='D'
	SELECT @FieldValue='Party Bank Deleted'

If @Type IN ('I','D')
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
					FieldDisplayName

                )
                SELECT ' + CONVERT(VARCHAR(30),@configuration_audit_master_id) + ',''' 
                    + @Type + ''',''' 
                    + @TableName + ''',''' + @PKColName + ''',' 
                    + 'ISNULL(i.' + @PKColName + ',' + 'd.' + @PKColName  + '),'
					+ 'ISNULL(i.' + @ValueColName + ',' + 'd.' + @ValueColName  + '),''' 	
					+ @FieldValue + ''','''
                    + @FieldValue + ''''   + 
                ' FROM #ins AS i FULL OUTER JOIN #del AS d'
                        + @PKCols + 
                ' WHERE i.' + @fieldname + ' <> d.' + @fieldname 
                        + ' or (i.' + @fieldname + ' IS NULL AND  D.'
                        + @fieldname
                        + ' IS NOT NULL)' 
                        + ' OR (I.' + @fieldname + ' IS NOT NULL AND  D.' 
                        + @fieldname
                        + ' IS NULL)'        );

EXEC (@sql);
RETURN
END

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
					+ @fieldname + ''','''
                    + @FieldDesc + ''''
                    + ',CONVERT(VARCHAR(1000),d.' + @fieldname + ')'
                    + ',CONVERT(VARCHAR(1000),i.' + @fieldname + ')' + 
                ' FROM #ins AS i FULL OUTER JOIN #del AS d'
                        + @PKCols + 
                ' WHERE i.' + @fieldname + ' <> d.' + @fieldname 
                        + ' or (i.' + @fieldname + ' IS NULL AND  D.'
                        + @fieldname
                        + ' IS NOT NULL)' 
                        + ' OR (I.' + @fieldname + ' IS NOT NULL AND  D.' 
                        + @fieldname
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
                        + ' WHERE i.' + @fieldname + ' <> d.' + @fieldname 
                        + ' OR (I.' + @fieldname + ' is null and  d.'
                        + @fieldname
                        + ' IS NOT NULL)' 
                        + ' OR (I.' + @fieldname + ' IS NOT NULL AND  D.' 
                        + @fieldname
                        + ' IS NULL)' 
            );
        END
   EXEC (@sql);

   UPDATE i SET OldValue=a.FieldDescription  FROM configuration_audit_details i 
			INNER JOIN Audit_trail_custom_fields a ON a.TableName= i.TableName AND a.FieldName=i.FieldName AND a.fieldValue=i.OldValue WHERE i.FieldName=@fieldname AND i.configuration_audit_master_id=@configuration_audit_master_id
   UPDATE i SET NewValue=a.FieldDescription  FROM configuration_audit_details i 
			INNER JOIN Audit_trail_custom_fields a ON a.TableName= i.TableName AND a.FieldName=i.FieldName AND a.fieldValue=i.NewValue WHERE i.FieldName=@fieldname AND i.configuration_audit_master_id=@configuration_audit_master_id
		
    END
	END
END
SET NOCOUNT OFF;
GO

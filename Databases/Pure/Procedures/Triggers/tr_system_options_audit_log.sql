SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropTrigger 'tr_system_options_audit_log'
GO
CREATE TRIGGER tr_system_options_audit_log ON system_options 
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
DECLARE @FieldDesc varchar(200);
DECLARE @ModuleId INT;
DECLARE @OptionNumber INT;
SELECT  @ModuleId=10,@ModuleName='Maintain System Options', @PKColName = 'option_number',@ValueColName='description';

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
	SELECT @UserId=UserId,@UniqueID=UniqueID,@OptionNumber=option_number FROM #del
ELSE
	SELECT @UserId=UserId,@UniqueID=UniqueID,@OptionNumber=option_number FROM #del
	SELECT @UserId=UserId,@UniqueID=UniqueID FROM #ins
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

SELECT @FieldDesc='option_number'

IF @fieldname ='value' AND @UniqueID IS NOT NULL
BEGIN	

DECLARE @configuration_audit_master_id int
IF @Type='D'
	SELECT @configuration_audit_master_id=m.configuration_audit_master_id FROM configuration_audit_master m INNER JOIN #del d ON m.UniqueId=d.UniqueId
ELSE
	SELECT @configuration_audit_master_id=m.configuration_audit_master_id FROM configuration_audit_master m INNER JOIN #ins i ON m.UniqueId=i.UniqueId
IF @configuration_audit_master_id IS NULL
BEGIN
	INSERT into configuration_audit_master (UniqueId,Module_Id,ModuleName,UpdateDate,UserID) Values (@UniqueID,@ModuleId, @ModuleName,@UpdateDate,@UserId)
	SELECT @configuration_audit_master_id=@@IDENTITY
END

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
                SELECT DISTINCT ' + CONVERT(VARCHAR(30),@configuration_audit_master_id) + ',''' 
                    + @Type + ''',''' 
                    + @TableName + ''',''' + @PKColName + ''',' 
                    + 'ISNULL(i.' + @PKColName + ',' + 'd.' + @PKColName  + '),'''
					+ 'System Options' + ''','''
					+ @FieldDesc + ''','
                     + 'ISNULL(i.' + @ValueColName + ',' + 'i.' + @PKColName  + ')'
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

EXEC (@sql);
 
IF @OptionNumber IN (1020,1022,5048,5049,5050,5051,5052,5053,5054,5055,5041,5042,79,5202)
BEGIN
	UPDATE i SET OldValue=ug.Description  FROM configuration_audit_details i 
			INNER JOIN pmuser_group ug ON ug.pmuser_group_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=ug.Description  FROM configuration_audit_details i 
			INNER JOIN pmuser_group ug ON ug.pmuser_group_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
END
ELSE IF @OptionNumber IN (1019,1021,5068)
BEGIN
	UPDATE i SET OldValue=tg.Description  FROM configuration_audit_details i 
			INNER JOIN PMWrk_Task_Group tg ON tg.pmwrk_task_group_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=tg.Description  FROM configuration_audit_details i 
			INNER JOIN PMWrk_Task_Group tg ON tg.pmwrk_task_group_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
END
ELSE IF @OptionNumber IN (1003)
BEGIN
	UPDATE i SET OldValue=c.Description  FROM configuration_audit_details i 
			INNER JOIN Country c ON c.country_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=c.Description  FROM configuration_audit_details i 
			INNER JOIN Country c ON c.country_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
END
ELSE IF @OptionNumber IN (5240)
BEGIN
	UPDATE i SET OldValue=tg.Description  FROM configuration_audit_details i 
			INNER JOIN Tax_Group tg ON tg.tax_group_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=tg.Description  FROM configuration_audit_details i 
			INNER JOIN Tax_Group tg ON tg.tax_group_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE configuration_audit_details SET OldValue= CASE OldValue WHEN '0' THEN '' ELSE OldValue END,NewValue=CASE NewValue WHEN '0' THEN '' ELSE NewValue END WHERE key_field_value=@OptionNumber AND configuration_audit_master_id=@configuration_audit_master_id

END
ELSE IF @OptionNumber IN (61,63, 5032, 5033,5034,5003)
BEGIN
	UPDATE i SET OldValue=dt.code  FROM configuration_audit_details i 
			INNER JOIN Document_Template dt ON dt.document_template_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=dt.code  FROM configuration_audit_details i 
			INNER JOIN Document_Template dt ON dt.document_template_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE configuration_audit_details SET OldValue= CASE OldValue WHEN '0' THEN '' ELSE OldValue END,NewValue=CASE NewValue WHEN '0' THEN '' ELSE NewValue END WHERE key_field_value=@OptionNumber AND configuration_audit_master_id=@configuration_audit_master_id
END
ELSE IF @OptionNumber IN (5031)
BEGIN
	UPDATE i SET OldValue=ns.description  FROM configuration_audit_details i 
			INNER JOIN Numbering_Scheme ns ON ns.numbering_scheme_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=ns.description  FROM configuration_audit_details i 
			INNER JOIN Numbering_Scheme ns ON ns.numbering_scheme_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE configuration_audit_details SET OldValue= CASE OldValue WHEN '0' THEN '' ELSE OldValue END,NewValue=CASE NewValue WHEN '0' THEN '' ELSE NewValue END WHERE key_field_value=@OptionNumber AND configuration_audit_master_id=@configuration_audit_master_id
END
ELSE IF @OptionNumber IN (5035)
BEGIN
	UPDATE i SET OldValue=gs.description  FROM configuration_audit_details i 
			INNER JOIN GIS_Screen gs ON gs.gis_screen_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=gs.description  FROM configuration_audit_details i 
			INNER JOIN GIS_Screen gs ON gs.gis_screen_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE configuration_audit_details SET OldValue= CASE OldValue WHEN '0' THEN '' ELSE OldValue END,NewValue=CASE NewValue WHEN '0' THEN '' ELSE NewValue END WHERE key_field_value=@OptionNumber AND configuration_audit_master_id=@configuration_audit_master_id

END
ELSE IF @OptionNumber IN (5159, 5160, 5161)
BEGIN
	UPDATE i SET OldValue=rt.description  FROM configuration_audit_details i 
			INNER JOIN risk_type_rule_set_type rt ON rt.risk_type_rule_set_type_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=rt.description  FROM configuration_audit_details i 
			INNER JOIN risk_type_rule_set_type rt ON rt.risk_type_rule_set_type_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
END
ELSE IF @OptionNumber IN (5243)
BEGIN
	UPDATE i SET OldValue=c.code  FROM configuration_audit_details i 
			INNER JOIN currency c ON c.currency_id=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
    UPDATE i SET NewValue=c.code  FROM configuration_audit_details i 
			INNER JOIN currency c ON c.currency_id=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
END
ELSE
BEGIN
	UPDATE i SET OldValue=a.FieldDescription  FROM configuration_audit_details i 
			INNER JOIN Audit_trail_custom_fields a ON a.TableName= i.TableName  AND a.FieldName=i.key_field_value AND a.fieldValue=i.OldValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id
	UPDATE i SET NewValue=a.FieldDescription  FROM configuration_audit_details i 
			INNER JOIN Audit_trail_custom_fields a ON a.TableName= i.TableName AND a.FieldName=i.key_field_value AND a.fieldValue=i.NewValue WHERE i.key_field_value=@OptionNumber AND i.configuration_audit_master_id=@configuration_audit_master_id

END		
END  	
END
END
SET NOCOUNT OFF;
GO

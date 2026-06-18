SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropTrigger 'tr_Party_Address_Usage_audit_log'
GO
CREATE TRIGGER tr_Party_Address_Usage_audit_log ON dbo.Party_Address_Usage 
    FOR INSERT,DELETE 
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
DECLARE @FieldDesc VARCHAR(500);
DECLARE @party_cnt INT;
DECLARE @ScreenHierarchy VARCHAR(500);
DECLARE @Configuration_Audit_Master_Id INT
DECLARE @Address_Cnt INT
DECLARE @Party_Type_Id INT
DECLARE @Is_Deleted INT
DECLARE @address_usage_type_id INT

SELECT @PKColName = 'address_cnt',@ValueColName='ScreenHierarchy';

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


IF @Type = 'D'
	SELECT @Party_Cnt=party_cnt, @Address_Cnt = address_cnt, @Is_Deleted = is_deleted, @address_usage_type_id = address_usage_type_id FROM #del
ELSE IF @Type = 'I'
	SELECT @Party_Cnt=party_cnt, @Address_Cnt = address_cnt FROM #ins

SELECT @Party_Type_Id = party_type_id from Party  WHERE party_cnt = @Party_Cnt


IF @Party_Type_Id = 3
BEGIN
	Select @UniqueID=pa.UniqueId, @ScreenHierarchy = a.ScreenHierarchy, 
	@configuration_audit_master_id = configuration_audit_master_id from Party_Agent pa 
	INNER JOIN configuration_audit_master cam ON pa.UniqueId = cam.UniqueId
	INNER JOIN Address a ON a.address_cnt = @Address_Cnt
	WHERE pa.party_cnt = @Party_Cnt
END
ElSE IF @Party_Type_Id IN (SELECT Party_Type_Id FROM Party_Type)
BEGIN
	Select @UniqueID=p.UniqueId, @ScreenHierarchy = a.ScreenHierarchy, 
	@configuration_audit_master_id = configuration_audit_master_id from Party p 
	INNER JOIN configuration_audit_master cam ON p.UniqueId = cam.UniqueId
	INNER JOIN Address a ON a.address_cnt = @Address_Cnt
	WHERE p.party_cnt = @Party_Cnt
END

IF UPPER(LEFT(@ScreenHierarchy,3))='FEE' 
	SELECT @ModuleId=4,@ModuleName='Fee Maintenance' 
Else IF UPPER(LEFT(@ScreenHierarchy,11)) = 'AGENT GROUP'
	SELECT @ModuleId=22,@ModuleName='Agent Group Maintenance'
ELSE IF UPPER(LEFT(@ScreenHierarchy,5))='AGENT' 
	SELECT @ModuleId=6,@ModuleName='Agent Maintenance' 
ELSE IF UPPER(LEFT(@ScreenHierarchy,9))='REINSURER' 
	SELECT @ModuleId=7,@ModuleName='Reinsurer Maintenance'
Else IF UPPER(LEFT(@ScreenHierarchy,17)) = 'ACCOUNT EXECUTIVE'
	SELECT @ModuleId=14,@ModuleName='Account Executive'
Else IF UPPER(LEFT(@ScreenHierarchy,15)) = 'ACCOUNT HANDLER'
	SELECT @ModuleId=15,@ModuleName='Account Handler'
Else IF UPPER(LEFT(@ScreenHierarchy,8)) = 'DISCOUNT'
	SELECT @ModuleId=16,@ModuleName='Discount Maintenance'
Else IF UPPER(LEFT(@ScreenHierarchy,5)) = 'EXTRA'
	SELECT @ModuleId=17,@ModuleName='Extra Maintenance'
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

SELECT @FieldDesc=(SELECT CAST(VALUE AS VARCHAR(500)) FROM sys.extended_properties  WHERE 
        name = N'MS_Description' 
        AND major_id = OBJECT_ID(@TableSchema + '.' + @TableName) 
        AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(@TableSchema + '.' + @TableName) AND name = @fieldname))

SELECT @FieldDesc= ISNULL(@FieldDesc,@fieldname)

IF ISNULL(@Is_Deleted,0) <> 0
BEGIN
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
	Values (
				@Configuration_Audit_Master_Id,
				'D',
				'Party_Address_Usage',
				'address_cnt',
				@Address_Cnt,
				@ScreenHierarchy,
				'Address Deleted',
				'Address Deleted')
	Return
END

IF EXISTS(SELECT NULL FROM configuration_audit_master cam 
		INNER JOIN configuration_audit_details cad ON cam.configuration_audit_master_id=cad.configuration_audit_master_id 
		WHERE cam.configuration_audit_master_id=@configuration_audit_master_id AND (FieldName IN ('New Agent Added', 'New Fee Added', 'New Re-Insurer Added', 'New Account Executive Added', 'New Account Handler Added', 'New Discount Account Added', 'New Extra Account Added', 'New Other Party Added', 'New Agent Group Added')))
RETURN

IF @Type='I'
BEGIN
DECLARE @newSql nvarchar(2000)
DECLARE @NewValue VARCHAR(100)
SET @newSql ='SELECT @NewValue= CAST(' + QUOTENAME(@fieldname) + ' AS VARCHAR) FROM #ins'
EXEC sp_executesql @newSql, N'@NewValue VARCHAR(100) OUTPUT', @NewValue OUTPUT
END

IF @Type='I' AND EXISTS(SELECT 1 FROM configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
			AND key_field_value=@Address_Cnt AND FieldName=@fieldname AND (OldValue=@NewValue OR FieldName='address_cnt'))     
BEGIN
	DELETE configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
			AND key_field_value=@Address_Cnt AND FieldName=@fieldname AND (OldValue=@NewValue OR FieldName='address_cnt')
END
ELSE IF @Type='I' AND EXISTS(SELECT 1 FROM configuration_audit_details WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
			AND key_field_value=@Address_Cnt AND FieldName=@fieldname AND OldValue<>@NewValue )  
BEGIN
	UPDATE configuration_audit_details SET Type='U', NewValue=@NewValue WHERE configuration_audit_master_id=@configuration_audit_master_id AND TableName=@TableName 
			AND key_field_value=@Address_Cnt AND FieldName=@fieldname 
END
ELSE IF @Type = 'I' AND @NewValue IS NOT NULL
BEGIN
DECLARE @FieldValue VARCHAR(50)
SELECT @FieldValue='New Address Added'

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
                    + 'ISNULL(i.' + @PKColName + ',' + 'd.' + @PKColName  + '),'''
					+ @ScreenHierarchy + ''',''' 	
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
                SELECT ' + CONVERT(VARCHAR(30),@configuration_audit_master_id) + ',''' 
                    + @Type + ''',''' 
                    + @TableName + ''',''' + @PKColName + ''',' 
                    + 'ISNULL(i.' + @PKColName + ',' + 'd.' + @PKColName  + '),'''
					+ @ScreenHierarchy + ''',''' 	
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
  
   EXEC (@sql);

END

UPDATE i SET OldValue=aut.Description  FROM configuration_audit_details i 
			INNER JOIN Address_Usage_Type aut ON CAST(aut.address_usage_type_id AS VARCHAR)=i.OldValue WHERE configuration_audit_master_id=@configuration_audit_master_id AND type='U' and FieldName='address_usage_type_id'
UPDATE i SET NewValue=aut.Description  FROM configuration_audit_details i 
			INNER JOIN Address_Usage_Type aut ON CAST(aut.address_usage_type_id AS VARCHAR)=i.NewValue WHERE configuration_audit_master_id=@configuration_audit_master_id AND type='U' and FieldName='address_usage_type_id'
END

END

SET NOCOUNT OFF;




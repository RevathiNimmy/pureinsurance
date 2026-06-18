DDLDROPPROCEDURE spu_create_audit_trail_for_lookups
GO

CREATE PROCEDURE spu_create_audit_trail_for_lookups
 @TableName VARCHAR(50),
 @TableData lookup_data READONLY, --Table-Valued Parameter 
 @KeyFieldDescription VARCHAR(100),
 @UniqueId VARCHAR(50),
 @UserId  INT,
 @bAddlookupData TINYINT
AS

BEGIN
DECLARE @pkColumnName VARCHAR(100),
@pkColumnValue INT

SELECT * INTO #NewData FROM @TableData

UPDATE #NewData
SET Column_Value = CASE Column_Value
                      WHEN 'Checked' THEN '1'
                      WHEN 'Unchecked' THEN '0'
                      ELSE Column_Value
                      END;
  

SELECT TOP 1 @pkColumnName=Column_Name,@pkColumnValue=Column_Value FROM @TableData

CREATE TABLE #UnmatchedData (
	column_name NVARCHAR(128),
	New_value NVARCHAR(MAX),
	Old_value NVARCHAR(MAX)
);

IF @bAddlookupData<>1 
BEGIN
	DECLARE @SQLInsert NVARCHAR(MAX)
	DECLARE @ParamDef NVARCHAR(MAX)

	SET @SQLInsert = '
		SELECT * INTO ##OldData
		FROM ' + QUOTENAME(@TableName) + '
		WHERE ' + QUOTENAME(@pkColumnName) + ' = @Value;'

	SET @ParamDef = N'@Value NVARCHAR(100)' 

	EXEC sp_executesql @SQLInsert, @ParamDef, @Value = @pkColumnValue

	DECLARE @sql NVARCHAR(MAX) = '';
	DECLARE @columns TABLE (colname SYSNAME);

	INSERT INTO @columns
	SELECT c.name AS COLUMN_NAME
	FROM tempdb.sys.columns c
	JOIN tempdb.sys.objects o ON c.object_id = o.object_id
	WHERE o.name LIKE '##OldData%' 
	AND o.type = 'U' 

	SELECT @sql = @sql + 
		'SELECT ''' + colname + ''' AS column_name, ' +
		'CAST(t1.[' + colname + '] AS NVARCHAR(MAX)) AS column_value ' +
		'FROM ##OldData t1 ' +
		'UNION ALL '
	FROM @columns;

	SET @sql = LEFT(@sql, LEN(@sql) - 10);

	SET @sql = '
	WITH temp AS (
		' + @sql + '
	)
	INSERT INTO #UnmatchedData (column_name, New_value, Old_value)
	SELECT t.column_name, 
		   t.column_value ,
		   t1.column_value 
	FROM #NewData t
	LEFT JOIN temp t1 ON t.column_name = t1.column_name
	WHERE NOT (  
		ISNULL(t.column_value,'''') = ISNULL(t1.column_value,''''))';

	EXEC sp_executesql @sql;

DROP TABLE ##OldData
END  

IF (SELECT COUNT(*) FROM #UnmatchedData) > 0 OR @bAddlookupData=1 
BEGIN
	DECLARE @ModuleName VARCHAR(100);
	DECLARE @ModuleId INT

	SELECT @ModuleId=13,@ModuleName='Lookup Maintenance' 

	DECLARE @configuration_audit_master_id INT
	SELECT @configuration_audit_master_id=configuration_audit_master_id FROM configuration_audit_master WHERE UniqueId=@UniqueId
	IF @configuration_audit_master_id IS NULL
	BEGIN
		INSERT into configuration_audit_master (UniqueId,Module_Id,ModuleName,UpdateDate,UserID) Values (@UniqueID,@ModuleId,@ModuleName,GETDATE(),@UserId)
		SELECT @configuration_audit_master_id=SCOPE_IDENTITY()
	END

DECLARE @Column_name VARCHAR(100),@Column_DisplayName VARCHAR(100),@Old_value VARCHAR(255),@New_value VARCHAR(255),@parentTable VARCHAR(100)

IF @bAddlookupData=1 
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
			VALUES
			( @configuration_audit_master_id,
			'U',
			@TableName,
			@pkColumnName,
			@pkColumnValue,
			@KeyFieldDescription,
			'New Record Added',
	        'New Record Added'
			)
END
ELSE
BEGIN

DECLARE lookup_Cursor Cursor for
SELECT u.column_name,n.Column_DisplayName,Old_value,New_value FROM #UnmatchedData u INNER JOIN #NewData n ON u.column_name=n.Column_Name

OPEN lookup_Cursor
FETCH NEXT FROM lookup_Cursor INTO @Column_name,@Column_DisplayName,@Old_value,@New_value
WHILE @@FETCH_STATUS = 0 
BEGIN
	SELECT    @parentTable =  o.name 
	FROM sysobjects o
	INNER JOIN sysreferences r ON o.id = r.rkeyid
	INNER JOIN syscolumns c ON c.id = r.rkeyid AND c.colid = r.rkey1  -- Referenced column
	INNER JOIN sysobjects fo ON fo.id = r.fkeyid                      -- Foreign key table
	INNER JOIN syscolumns fc ON fc.id = r.fkeyid AND fc.colid = r.fkey1 -- Foreign key column
	WHERE r.fkeyid =OBJECT_ID(@TableName) AND fc.name= @Column_name

IF @parentTable IS NOT NULL
BEGIN

DECLARE @OldDescription VARCHAR(MAX);
DECLARE @NewDescription VARCHAR(MAX);

DECLARE @paramDefinition NVARCHAR(MAX);

-- For OldDescription
SET @sql = N'SELECT @OldDescription = description FROM ' + QUOTENAME(@parentTable) +
           ' WHERE ' + QUOTENAME(@parentTable + '_id') + ' = @Old_value';

SET @paramDefinition = N'@Old_value INT, @OldDescription VARCHAR(MAX) OUTPUT';

EXEC sp_executesql 
    @sql, 
    @paramDefinition,
    @Old_value = @Old_value,
    @OldDescription = @OldDescription OUTPUT;

-- For NewDescription
SET @sql = N'SELECT @NewDescription = description FROM ' + QUOTENAME(@parentTable) +
           ' WHERE ' + QUOTENAME(@parentTable + '_id') + ' = @New_value';

SET @paramDefinition = N'@New_value INT, @NewDescription VARCHAR(MAX) OUTPUT';

EXEC sp_executesql 
    @sql, 
    @paramDefinition,
    @New_value = @New_value,
    @NewDescription = @NewDescription OUTPUT;
END
DECLARE @bUnmatchedData AS TINYINT=1
IF @Column_name='effective_date' 
BEGIN
IF CAST(@Old_value as DATE) = CAST(@New_value AS DATE)
	SELECT @bUnmatchedData = 0
END
IF @Column_name <> 'Caption_Id'  AND @bUnmatchedData = 1
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
			VALUES
			( @configuration_audit_master_id,
			'U',
			@TableName,
			@pkColumnName,
			@pkColumnValue,
			@KeyFieldDescription,
			@Column_name,
	        @Column_DisplayName,
	        ISNULL(@OldDescription, CASE WHEN TRY_CAST(@Old_value AS DECIMAL(18,10)) IS NOT NULL      AND TRY_CAST(@Old_value AS DECIMAL(18,0)) <> TRY_CAST(@Old_value AS DECIMAL(18,10))
            THEN CAST(ROUND(CAST(@Old_value AS DECIMAL(18,10)), 2) AS VARCHAR(50)) ELSE @Old_value  END),
            ISNULL(@NewDescription,@New_value)
			)
 END            
  SELECT @parentTable=NULL,@OldDescription=null, @NewDescription=null

  FETCH NEXT FROM lookup_Cursor INTO @Column_name,@Column_DisplayName,@Old_value,@New_value
END
Close lookup_Cursor
Deallocate lookup_Cursor

END
END

DELETE FROM configuration_audit_details
WHERE 
    TRY_CAST(OldValue AS FLOAT) = TRY_CAST(NewValue AS FLOAT)
    AND TRY_CAST(OldValue AS FLOAT) IS NOT NULL
    AND configuration_audit_master_id=@configuration_audit_master_id;

DROP TABLE #NewData
DROP TABLE #UnmatchedData

END

GO
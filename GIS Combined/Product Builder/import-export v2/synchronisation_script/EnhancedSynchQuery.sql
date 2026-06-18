-- It now generates update statements for code value instead of id column, makes it easier for populated target
-- GIS_Lookup_Header
-- The table above does not have guid's updated! Need to do it manually
-- UPDATE GIS_Lookup_Header SET pie_guid = NEWID()

-- Create table to hold details of any table with pie guid
IF NOT EXISTS(SELECT NULL FROM sysobjects WHERE name = 'TablesWithGUID' AND type = 'u')
BEGIN
	CREATE TABLE TablesWithGUIDTemp(TableName varchar(255))
	CREATE TABLE TablesWithGUID(TableName varchar(255),PKColumn varchar(255), DataType varchar(255),SQLStatement varchar(8000) NULL)
	CREATE TABLE PKList(PKColumn varchar(255), DataType varchar(255))
	CREATE TABLE UpdateStatement(SQLTEXT varchar(8000))
END

-- Get a list of all tables with pie_guid column and populate tableswithguid
INSERT INTO TablesWithGUIDTemp
SELECT OBJECT_NAME(id)
FROM syscolumns 
WHERE NAME = 'pie_guid'

-- INSERT Code columns into table
INSERT INTO TablesWithGUID
SELECT INFORMATION_SCHEMA.COLUMNS.TABLE_NAME, 
	   'CODE', 
	   'CHAR',
	   NULL
FROM TablesWithGUIDTemp 
INNER JOIN INFORMATION_SCHEMA.COLUMNS  ON TablesWithGUIDTemp.TABLENAME = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME
WHERE UPPER(INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME) = 'CODE' 
ORDER BY TablesWithGUIDTemp.tablename

-- INSERT PK columns into table
INSERT INTO TablesWithGUID
SELECT INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME, 
	   INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME, 
	   INFORMATION_SCHEMA.COLUMNS.DATA_TYPE,
	   NULL
FROM TablesWithGUIDTemp 
	INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE  ON TablesWithGUIDTemp.TABLENAME = INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME
	INNER JOIN INFORMATION_SCHEMA.COLUMNS ON (INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME 
											  AND INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME = INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME)
WHERE INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME LIKE 'PK%'
AND TablesWithGUIDTemp.TableName NOT IN 
      (
	   SELECT TableName 
       FROM TablesWithGUID
       ) 
ORDER BY TablesWithGUIDTemp.tablename

-- For any that don't have PK's all we can do is match on everything apart from pie_guid and pie_last_updated
-- So get all column info and insert for these tables
-- We do not want to include any NULLable columns as they may not match
INSERT INTO TablesWithGUID
SELECT INFORMATION_SCHEMA.COLUMNS.TABLE_NAME, 
	   INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME, 
	   INFORMATION_SCHEMA.COLUMNS.DATA_TYPE,
	   NULL	
FROM TablesWithGUIDTemp 
	INNER JOIN INFORMATION_SCHEMA.COLUMNS ON TablesWithGUIDTemp.TableName = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME
WHERE INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME <> 'pie_guid' AND
	  INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME <> 'pie_last_updated' AND
	  INFORMATION_SCHEMA.COLUMNS.IS_NULLABLE = 'NO' AND
	  INFORMATION_SCHEMA.COLUMNS.DATA_TYPE NOT IN('datetime') AND
      TablesWithGUIDTemp.TableName NOT IN 
      (
	   SELECT TableName 
       FROM TablesWithGUID
       )
ORDER BY TablesWithGUIDTemp.tablename

-- =============================================
-- Declare and using a READ_ONLY cursor
-- =============================================
DECLARE UpdateSQLStatement CURSOR READ_ONLY
FOR SELECT TableName, PKColumn, datatype 
    FROM TablesWithGUID

DECLARE @TableName varchar(255)
DECLARE @PKColumn  varchar(255)
DECLARE @DataType  varchar(255)

OPEN UpdateSQLStatement

FETCH NEXT FROM UpdateSQLStatement INTO @TableName, @PKColumn, @DataType
WHILE (@@fetch_status <> -1)
BEGIN
	IF (@@fetch_status <> -2)
	BEGIN
		DECLARE @SQLStatement varchar(8000)
		DECLARE @SQLWHERE	  varchar(8000)

		SET @SQLStatement = 'SELECT ''UPDATE ' + @TableName + ' SET pie_guid = '''''' + pie_guid + '''''' WHERE '
		SET @sqlWHERE = ''
		--Clear PKList
		TRUNCATE TABLE PKList

 		--Get a list of all pk's required
		INSERT INTO PKLIST
		SELECT PKColumn,DataType 
		FROM TablesWithGUID 
		WHERE TableName = @TableName

		DECLARE @PKCOL		varchar(255)
		DECLARE @DATTYPE	varchar(255)

		WHILE EXISTS(SELECT NULL FROM PKLIST)
		BEGIN

			SELECT @PKCOL = (SELECT TOP 1 PKColumn FROM PKLIST	ORDER BY PKColumn)
			SELECT @DATTYPE = (SELECT TOP 1 DataType  FROM PKLIST ORDER BY PKColumn)

			IF UPPER(@DATTYPE) IN ('VARCHAR', 'CHAR') 
				BEGIN
					SELECT @SQLWHERE = @SQLWHERE + @PKCOL + ' = ' + '''' + '+''''''''' + ' + CAST(' + @PKCOL + ' AS VARCHAR(255))' + '+''''''''' 
				END
			-- Everything else (int etc) 
			ELSE 
				BEGIN
					SELECT @SQLWHERE = @SQLWHERE + @PKCOL + ' = ' + '''' + ' + CAST(' + @PKCOL + ' AS VARCHAR(255))'
				END

			-- Add if we need an AND
			IF (SELECT COUNT(*) FROM PKLIST) > 1
			BEGIN
				SELECT @SQLWHERE = @SQLWHERE + ' + ' + '''' + ' AND '
			END

			DELETE PKLIST 
			WHERE PKColumn 
				IN (SELECT TOP 1 PKColumn
					FROM PKLIST ORDER BY PKColumn)
		END

		SET @SQLStatement = @SQLStatement + @sqlwhere + ' FROM ' + @TableName

		UPDATE TablesWithGUID SET SQLStatement = @SQLStatement 
		WHERE TableName = @TableName
	END
	FETCH NEXT FROM UpdateSQLStatement INTO @TableName, @PKColumn, @DataType
END

CLOSE UpdateSQLStatement
DEALLOCATE UpdateSQLStatement

-- =============================================
-- Declare and using a READ_ONLY cursor
-- =============================================
DECLARE EXEC_SQL CURSOR
READ_ONLY
FOR SELECT DISTINCT SQLStatement FROM TABLESWITHGUID

DECLARE @SQL nvarchar(4000)
OPEN EXEC_SQL

FETCH NEXT FROM EXEC_SQL INTO @SQL
WHILE (@@fetch_status <> -1)
BEGIN
	IF (@@fetch_status <> -2)
	BEGIN
		DECLARE @MSG nvarchar(4000)
		set @msg = 'INSERT INTO UpdateStatement ' + @SQL + ''
		EXEC SP_EXECUTESQL @msg

	END
	FETCH NEXT FROM EXEC_SQL INTO @SQL
END

CLOSE EXEC_SQL
DEALLOCATE EXEC_SQL

DELETE UpdateStatement WHERE SQLTEXT = 'NULL'

SELECT * FROM UpdateStatement

DROP TABLE TablesWithGUID
DROP TABLE TablesWithGUIDTemp
DROP TABLE UpdateStatement
DROP TABLE PKLIST





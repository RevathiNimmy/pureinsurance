SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_IsInUse'
GO
CREATE PROCEDURE spu_IsInUse
    @IDfield varchar(30),
    @id integer
AS

DECLARE @inuse bit
DECLARE @table varchar(30)
DECLARE @vID as varchar(30)

set @vID = convert(varchar(30),@id)
DECLARE mycursor CURSOR FAST_FORWARD FOR
    SELECT t.table_name
    FROM INFORMATION_SCHEMA.TABLES AS t
    INNER JOIN INFORMATION_SCHEMA.COLUMNS AS c ON t.table_name = c.table_name
    WHERE c.column_name = @IDfield

OPEN mycursor
FETCH NEXT FROM mycursor INTO @table

WHILE @@FETCH_STATUS = 0 BEGIN
    EXECUTE ('IF ' +
        'EXISTS (SELECT NULL FROM ' + @table + ' WHERE ' + @IDfield + ' = ' + @vID + ') ' +
        'BEGIN SELECT ''' + @table + ''' END')
    FETCH NEXT FROM mycursor INTO @table
END

CLOSE mycursor
DEALLOCATE mycursor

GO



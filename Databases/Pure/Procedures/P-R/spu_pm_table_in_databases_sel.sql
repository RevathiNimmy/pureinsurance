SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pm_table_in_databases_sel'
GO


CREATE PROCEDURE spu_pm_table_in_databases_sel
    @table_name char(30)
AS
/*******************************************************************************************/
/* Returns a list of All databases which have the supplied table.                          */
/*******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 16/12/1998 RFC */
/*******************************************************************************************/
BEGIN
    SET NOCOUNT ON

    CREATE TABLE #table_in_databases(
        id integer,
        database_name varchar(30) NOT NULL)

    DECLARE c_database CURSOR FAST_FORWARD FOR
        SELECT dbid, name FROM master..sysdatabases

    DECLARE @database_name varchar(255),
        @SQL varchar(255),
        @db_id smallint

    OPEN c_database

    FETCH NEXT FROM c_database INTO @db_id, @database_name

    WHILE @@FETCH_STATUS = 0 BEGIN
        SELECT @SQL = "INSERT INTO #table_in_databases SELECT " + CONVERT(varchar(16), @db_id) + ", '" + @database_name + "' FROM "
        SELECT @SQL = @SQL + @database_name
        SELECT @SQL = @SQL + "..sysobjects WHERE type = 'U' AND name = "
        SELECT @SQL = @SQL + "'" + @table_name + "' "
        EXECUTE (@SQL)

        FETCH NEXT FROM c_database INTO @db_id, @database_name
    END

    CLOSE c_database
    DEALLOCATE c_database

    SET NOCOUNT OFF

    SELECT * FROM #table_in_databases

    DROP Table #table_in_databases
END
GO



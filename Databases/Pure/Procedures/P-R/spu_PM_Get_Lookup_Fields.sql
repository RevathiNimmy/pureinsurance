SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PM_Get_Lookup_Fields'
GO

CREATE PROCEDURE spu_PM_Get_Lookup_Fields
    @table_name varchar(255),
    @product_code varchar(255)
AS
/**************************************************************************************/
/* History : 13/05/99 - Created */
/* DAK011299 - Use temporary table to store results */
/**************************************************************************************/
BEGIN
    DECLARE @sDatabase varchar(255)
    DECLARE @lObjectID integer
    DECLARE @sSQL varchar(255)

    SET NOCOUNT ON

    SELECT @sDatabase = database_name
        FROM PMProduct
        WHERE code = @product_code
		
    SELECT @lObjectID = object_id(@sDatabase + ".." + @table_name)

    CREATE TABLE #PMLookup_Field(
        foriegn_key_column_id int null,
        referenced_table_name varchar(100) null
    )

    SELECT @sSQL = "INSERT INTO #PMLookup_Field " +
        "SELECT r.fkey1, o.name " +
        "FROM " + QUOTENAME(@sDatabase) + "..sysobjects AS o " +
        "INNER JOIN " + QUOTENAME(@sDatabase) + "..sysreferences AS r ON o.id = r.rkeyid " +
        "WHERE r.fkeyid = " + CONVERT(varchar(10), @lObjectID)

    EXECUTE (@sSQL)

    SET NOCOUNT OFF

    SELECT * FROM #PMLookup_Field ORDER BY referenced_table_name

    SET NOCOUNT ON

    DROP TABLE #PMLookup_Field
END
GO


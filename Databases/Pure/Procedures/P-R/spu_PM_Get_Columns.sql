SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PM_Get_Columns'
GO

CREATE PROCEDURE spu_PM_Get_Columns
    @table_name varchar(255),
    @product_code varchar(255)
AS
/**************************************************************************************/
/* History : 13/05/99 - Created */
/* 18/05/99 - CF - Now lives in the architecture database, and performs */
/* the selects on other databases. Added extra product */
/* code parameter. */
/* 30/11/99 - CF - Extract data into PMLookup_Columns before passing */
/* details back */
/* 30/11/99 - DAK Further changes */
/* 01/12/99 - DAK Replace c.offset with 1 or -1 depending on value of */
/* c.isnullable */
/* 01/03/00 - DAK isnullable is not available in SQL Server 6.5 */
/**************************************************************************************/
BEGIN
    DECLARE @sDatabase varchar(1000)
    DECLARE @lObjectID integer
    DECLARE @sSQL varchar(8000)

    SET NOCOUNT ON

    SELECT @sDatabase = database_name
        FROM PMProduct
        WHERE code = @product_code

    SELECT @lObjectID = object_id('['+@sDatabase +']' + ".." + @table_name)

    CREATE TABLE #PMLookup_Column(
        column_id int null,
        column_name sysname null,
        column_type_name sysname null,
        column_length int null,
        column_offset int null,
        column_prec int null,
        column_scale int null
    )

    /* Perform the select */
    SELECT @sSQL = "INSERT INTO #PMLookup_Column " +
        "SELECT c.colid, c.name, t.name, c.length, 1, c.prec, c.scale " +
        "FROM [" + @sDatabase + "]..syscolumns AS c " +
        "INNER JOIN [" + @sDatabase + "]..systypes AS t ON c.usertype = t.usertype " +
        "WHERE c.id = " + CONVERT(varchar(10), @lObjectID) + " " +
        "AND c.isnullable = 0"
    EXECUTE (@sSQL)

    SELECT @sSQL = "INSERT INTO #PMLookup_Column " +
        "SELECT c.colid, c.name, t.name, c.length, -1, c.prec, c.scale " +
        "FROM [" + @sDatabase + "]..syscolumns AS c " +
        "INNER JOIN [" + @sDatabase + "]..systypes AS t ON c.usertype = t.usertype " +
        "WHERE c.id = " + CONVERT(varchar(10), @lObjectID) + " " +
        "AND c.isnullable = 1"
    EXECUTE (@sSQL)

    SET NOCOUNT OFF

    SELECT * FROM #PMLookup_Column WHERE column_name<>'product_option' ORDER BY column_id

    SET NOCOUNT ON

    DROP TABLE #PMLookup_Column
END
GO


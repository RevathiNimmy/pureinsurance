SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_get_code_from_id'
GO
CREATE PROCEDURE spu_pm_get_code_from_id
    @tablename varchar(255),
    @id int,
    @code char(10) OUTPUT
AS
BEGIN
    DECLARE @SQL varchar(255)

    IF RTRIM(@code) = '' BEGIN
        SELECT @code = NULL
        RETURN -100
    END

    IF RTRIM(@tablename) = '' BEGIN
        SELECT @code = NULL
        RETURN -100
    END

    CREATE TABLE #temp_code (
        code char(10)
    )

    SELECT @SQL = 'INSERT INTO #temp_code' +
        ' SELECT code' +
        ' FROM ' + @tablename +
        ' WHERE ' + @tablename + '_id = ' + CONVERT(varchar(10), @id)

    EXECUTE (@SQL)

    SELECT @code = code FROM #temp_code

    DROP TABLE #temp_code

    IF RTRIM(@code) = '' BEGIN
        SELECT @code = NULL
        RETURN -100
    END

    IF @code IS NULL BEGIN
        SELECT @code = NULL
        RETURN -100
    END
END
GO


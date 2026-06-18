SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pm_table_in_database'
GO


CREATE PROCEDURE spu_pm_table_in_database
    @table_name varchar(30),
    @database_name varchar(30)
AS

/*******************************************************************************************/
/* sp_pm_table_in_database returns whether the supplied table is in the supplied database. */
/* RETURN: 0 Table found in database. */
/* -100 Table NOT found in database */
/*******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 16/12/1998 RFC */
/*******************************************************************************************/
BEGIN

SET NOCOUNT ON

DECLARE @SQL varchar(255),
    @num_rows integer

SELECT @SQL = "SELECT name INTO #table_in_db FROM " + @database_name + "..sysobjects WHERE type = 'U' AND name = '" + @table_name + "'"

EXECUTE (@SQL)

SELECT @num_rows = @@rowcount

--SELECT @num_rows = COUNT(name) FROM #table_in_db

IF @num_rows > 0
    RETURN
ELSE
    RETURN -100

END
GO



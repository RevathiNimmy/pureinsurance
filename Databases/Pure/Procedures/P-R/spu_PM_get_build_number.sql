EXEC DDLDropProcedure 'spu_PM_get_build_number'
GO

/* Return build number of the installed database */

CREATE PROCEDURE spu_PM_get_build_number
	@build_number INT OUTPUT
AS

DECLARE @current_version varchar(30)

/*Get version of previous build as this table is updated after the*/
/*stored procedures are installed but before the data scripts are executed*/
SELECT @current_version = version
FROM pmlogicaldatabasehistory 
WHERE pmlogicaldatabase_id = 1
AND pmlogicaldatabasehistory_id = 
	(
		SELECT MAX(pmlogicaldatabasehistory_id)
		FROM pmlogicaldatabasehistory 
		WHERE pmlogicaldatabase_id = 1
	)

/*If sr22 then return build number, else return 0*/
IF(CHARINDEX('.', RIGHT(@current_version,5)) = 0)
    SELECT @build_number = CAST(RIGHT(@current_version,5) AS INT)
ELSE
    SELECT @build_number = 0

GO



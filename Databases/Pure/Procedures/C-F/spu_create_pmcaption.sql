SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_create_pmcaption'
GO
CREATE PROCEDURE spu_create_pmcaption
AS
/****************************************************************************************/
/* sp_create_pmcaption builds the PMCaption table using the following steps:-           */
/* 1. Get list of reference tables                                                      */
/* 2. Take first table and get list of values in its description column                 */
/* 3. Get first description value and compare with PMCaption's caption column           */
/* 4. If value exists in PMCaption then assign the relative caption_id from             */
/*    PMCaption to the reference table's description.                                   */
/* 5. If value does not exist in PMCaption then assign a new caption_id and             */
/*    the reference table's description value to PMCaption. Also assign the             */
/*    new caption_id to the reference table's description.                              */
/* INPUTS:  @database_name  The database to check captions in.                          */
/* OUTPUTS: n/a                                                                         */
/****************************************************************************************/
/* Revision Description of Modification                     Date        Who             */
/* -------- ---------------------------                     ----        ---             */
/* 1.0      Original                                        25/07/1997  PH              */
/* 1.1      Replace single quotes with 2 single quotes      26/11/1998  SP              */
/*          in description                                                              */
/* 1.2      Only caption_id & description required for      19/01/1999  RFC             */
/*          table to be updated.                                                        */
/*          Amended to use sp_pmcaption to get/add caption_id.                          */
/*          Amended to update tables in a supplied db.                                  */
/*          Amended to only update the caption_id if it is  25/01/1999  RFC             */
/*          wrong.                                                                      */
/****************************************************************************************/
BEGIN
    DECLARE @tablename varchar(100) -- Name of table being used to create PMCaption.
    DECLARE @description varchar(255) -- Value from description column of tablename.
    DECLARE @SQL varchar(255) -- SQL script.
    DECLARE @SQL2 varchar(255) -- SQL script.
    DECLARE @caption_id int -- caption_id of current description.
    
    SET NOCOUNT ON
    
    CREATE TABLE #TempID
	(
		description varchar(255)
	)

    /* Create the List of Tables Cursor */
    DECLARE tablename_cursor CURSOR FAST_FORWARD FOR
    	SELECT so.name
    	FROM sysobjects so
    	WHERE so.type = 'U'
    	AND so.id IN
    		(SELECT id FROM syscolumns WHERE name = 'caption_id' AND id IN
    			(SELECT id FROM syscolumns WHERE name = 'description'))

    OPEN tablename_cursor

    /* Get next tablename */
    FETCH NEXT FROM tablename_cursor INTO @tablename

    WHILE @@FETCH_STATUS = 0
    BEGIN
    	SELECT @SQL = "INSERT INTO #TempID (description) "
    	SELECT @SQL = @SQL + "SELECT DISTINCT x.description FROM " + @tablename + " x WHERE not exists (SELECT description FROM #tempID WHERE description = x.description)"
    	EXECUTE (@SQL)
    	
    	FETCH NEXT FROM tablename_cursor INTO @tablename
    END
    
	CLOSE tablename_cursor


	/*Add all of the descriptions to the PMCaption table*/
	DECLARE description_cursor CURSOR FAST_FORWARD FOR 
		SELECT LTRIM(description) FROM #TempID

	OPEN description_cursor

	FETCH NEXT FROM description_cursor INTO @description

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF LTRIM(@description) IS NOT NULL
		BEGIN
			EXECUTE spu_pm_caption_id_return 1, @description, @caption_id OUTPUT
		END

		FETCH NEXT FROM description_cursor INTO @description
	END

	CLOSE description_cursor
	DEALLOCATE description_cursor


    /* Update tablename's caption_id with that of PMCaption's */
    OPEN tablename_cursor
    
    FETCH NEXT FROM tablename_cursor INTO @tablename

    WHILE @@FETCH_STATUS = 0
    BEGIN
		SELECT @SQL = "UPDATE x"
		SELECT @SQL = @SQL + " SET x.caption_id = c.caption_id" 
		SELECT @SQL = @SQL + " FROM " + @Tablename + " x"
		SELECT @SQL = @SQL + " JOIN PMCaption c"
		SELECT @SQL = @SQL + " ON c.caption = x.description"
		SELECT @SQL = @SQL + " WHERE x.caption_id <> c.caption_id"
		EXECUTE (@SQL)
    	
    	FETCH NEXT FROM tablename_cursor INTO @tablename
    END

    CLOSE tablename_cursor
    DEALLOCATE tablename_cursor

    SET NOCOUNT OFF
END
GO



SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_Screen_del'
GO

CREATE PROCEDURE spe_GIS_Screen_del
    @GIS_screen_id integer,
    @mode char(20)
AS

DECLARE @risk_group_id integer

IF lower(@mode) = 'extended' BEGIN
    -- Create temporary table
    CREATE TABLE #TempID (risk_group_id integer)

    -- Build SQL statement that gets all values we are interested in
    INSERT INTO #TempID
        SELECT risk_group_id
        FROM risk_group
        WHERE gis_screen_id = @GIS_screen_id

    UPDATE risk_group
        SET risk_group.gis_screen_id = null
        WHERE risk_group.gis_screen_id = @GIS_screen_id
END

-- Attempt to null the Peril_Type.gis_screen_id BEFORE the delete as it causes a referential integrity issue --
-- NOTE : User will have to re-select the appropriate GIS screen in the peril type maintenance... --
UPDATE Peril_Type SET gis_screen_id = null WHERE gis_screen_id = @GIS_screen_id

DELETE FROM GIS_Screen
    WHERE GIS_screen_id = @GIS_screen_id

/*
DECLARE cCursor CURSOR FAST_FORWARD FOR
    SELECT risk_group_id FROM #TempID

OPEN cCursor
FETCH NEXT FROM cCursor INTO @risk_group_id

WHILE @@FETCH_STATUS = 0 BEGIN
    -- Do stuff with @risk_group_id here
    UPDATE risk_group
        SET risk_group.gis_screen_id = @GIS_screen_id
        WHERE risk_group_id = @risk_group_id

    -- Read the next value
    FETCH NEXT FROM cCursor INTO @risk_group_id
END

CLOSE cCursor
DEALLOCATE cCursor
*/

IF lower(@mode) = 'extended' BEGIN
    -- Drop temporary table
    DROP TABLE #TempID
END

GO


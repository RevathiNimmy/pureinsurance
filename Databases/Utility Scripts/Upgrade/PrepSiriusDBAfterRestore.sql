-------------------------------------------------------------------------------
--  Author:  AMB
--  Name:    PrepSiriusDBAfterRestore.sql
--  Date:    1-Oct-2003
--
--  Desc:    This script should be run AFTER restoring a Sirius database from 
--           another machine.  If the restored database name is different
--           from the original database name, it also makes necessary changes
--           to the restored Sirius database tables.
--
--           The script does the following:
--               · Changes the database ownership to the 'Sirius' login
--               · Creates a record in PMSystem for the local machine
--               · Update the PMProduct table with the restored database name
--               · Resets any leftover logons in the PMUser table
--
--  Usage:   @sOriginalDBName: the original name of the database
--
--           e.g. EXEC PrepSiriusDBAfterRestore 'Sirius_Zimnat'
-- 
--           **CAUTION** this script should ONLY be run on a newly restored
--           database.  For Sirius internal use only.
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'PrepSiriusDBAfterRestore'
GO

CREATE PROCEDURE PrepSiriusDBAfterRestore
    @sOriginalDBName sysname
AS 
BEGIN

    DECLARE @sMachineName       sysname
    DECLARE @iPMSystemIDCount   int
    DECLARE @iMaxPMSystemID     int
    DECLARE @iSlashPos          int
    DECLARE @iErrorNum          int

    SET NOCOUNT ON

    SET @iErrorNum = 0
    SET @sMachineName = @@SERVERNAME

    -- strip off instance name, if any
    SELECT @iSlashPos = CHARINDEX('\', @sMachineName)
    IF @iSlashPos > 0
        SELECT @sMachineName = LEFT(@sMachineName, @iSlashPos - 1)


    -- change database ownership
    PRINT 'Changing database ownership to ''SIRIUS''...'
    PRINT '------------------------------------------'
    EXECUTE sp_changedbowner @loginame = 'SIRIUS'
    PRINT ''


    -- make an entry in PMSystem table by copying the last entry in the table
    -- and setting the 'system_name' to the name of the local machine.
    PRINT 'Checking PMSystem table...'
    PRINT '--------------------------'

    IF EXISTS (SELECT system_id FROM PMSystem WHERE system_name = @sMachineName) 
    BEGIN
        PRINT 'Server already exists in PMSystem table.'
        PRINT '*** The PMSystem table has not been updated. ***'
        PRINT ''
    END ELSE 
    BEGIN

        SELECT 
            @iPMSystemIDCount = COUNT(system_id), 
            @iMaxPMSystemID = MAX(system_id) 
        FROM 
            PMSystem

        IF @iPMSystemIDCount <= 1 
        BEGIN
            PRINT 'At least one system must exist in PMSystem table other than ''(Template)''.'
            PRINT '*** The PMSystem table has not been updated. ***'
            PRINT ''
            RETURN
        END ELSE 
        BEGIN
        
            PRINT 'Creating PMSystem table entries...'

            INSERT INTO 
                PMSystem
	            SELECT
                    @iMaxPMSystemID + 1,    -- system_id
                    product_id,
                    @sMachineName,          -- system_name
                    default_source_id,
                    home_country_id,
                    currency_id,
                    language_id,
                    licence_limit,
                    licence_key,
                    log_level,
                    pool_size,
                    NULL                    -- timestamp
	            FROM
	                PMSystem
	            WHERE
	                system_id = @iMaxPMSystemID

            SET @iErrorNum = @@ERROR
            IF @iErrorNum <> 0 
            BEGIN
                PRINT '*** Error occurred updating PMSystem table: ' + CONVERT(nchar(10), @iErrorNum) + ' ***'
                PRINT ''
                RETURN
            END ELSE 
            BEGIN
                PRINT 'PMSystem table update complete.'
                PRINT ''
            END

        END

    END


    -- update the PMProduct table, the 'database_name' field
    -- must be set the the restored database name.
    PRINT 'Checking PMProduct table...'
    PRINT '---------------------------'

    IF EXISTS (SELECT pmproduct_id FROM PMProduct WHERE database_name = DB_NAME()) 
    BEGIN
        PRINT 'Restored database name already exists in PMProduct table.'
        PRINT '*** The PMProduct table has not been updated. ***'
        PRINT ''
    END ELSE 
    BEGIN

        PRINT 'Updating PMProduct table entries...'

        UPDATE PMProduct SET database_name = DB_NAME() WHERE database_name = @sOriginalDBName

        SET @iErrorNum = @@ERROR
        IF @iErrorNum <> 0 
        BEGIN
            PRINT '*** Error occurred updating PMProduct table: ' + CONVERT(nchar(10), @iErrorNum) + ' ***'
            PRINT ''
            RETURN
        END ELSE 
        BEGIN
            PRINT 'PMProduct table update complete.'
            PRINT ''
        END

    END


    -- clear out any leftover users that may look like they're logged in
    PRINT 'Checking PMUser table for leftover logged-on users...'
    PRINT '-----------------------------------------------------'

    IF EXISTS (SELECT [user_id] FROM PMUser WHERE logged_on_at_client IS NOT NULL) 
    BEGIN

        PRINT 'Resetting leftover logged on users...'

        UPDATE PMUser SET logged_on_at_client = NULL WHERE logged_on_at_client IS NOT NULL

        SET @iErrorNum = @@ERROR
        IF @iErrorNum <> 0 
        BEGIN
            PRINT '*** Error occurred updating PMUser table: ' + CONVERT(nchar(10), @iErrorNum) + ' ***'
            PRINT ''
            RETURN
        END ELSE 
        BEGIN
            PRINT 'PMUser table update complete.'
            PRINT ''
        END

    END ELSE 
    BEGIN
        PRINT 'No logged-on users found.'
        PRINT ''
        RETURN
    END

    PRINT 'Database updates are complete.  The restored database is now ready to use.'

END
GO

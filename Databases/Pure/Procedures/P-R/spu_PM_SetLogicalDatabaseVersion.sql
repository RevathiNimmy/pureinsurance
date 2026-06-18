SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PM_SetLogicalDatabaseVersion'
GO
-- Called by the database install/upgrade program.
-- Returns zero for success and error code for failure.
CREATE PROCEDURE spu_PM_SetLogicalDatabaseVersion
    @sName varchar(30),
    @sVersion varchar(30)
AS
BEGIN
    DECLARE @lID integer
    DECLARE @sPreviousVersion varchar(30)
    DECLARE @sPreviousDate datetime

    SET NOCOUNT ON
    BEGIN TRANSACTION

    -- Read current details.
    SELECT @lID = pmlogicaldatabase_id,
        @sPreviousVersion = version,
        @sPreviousDate = install_date
        FROM PMLogicalDatabase
        WHERE name = @sName

    IF @@ROWCOUNT = 0 BEGIN
        -- Row does not yet exist, so we must create it.
        SELECT @lID = ISNULL((SELECT MAX(pmlogicaldatabase_id) FROM PMLogicalDatabase), 0) + 1
        IF @@ERROR <> 0 BEGIN
            ROLLBACK TRANSACTION
            SET NOCOUNT OFF
            RETURN @@ERROR
        END

        INSERT INTO PMLogicalDatabase(
            pmlogicaldatabase_id,
            name,
            version,
            install_date)
        VALUES(
            @lID,
            @sName,
            @sVersion,
            getdate())
        IF @@ERROR <> 0 BEGIN
            ROLLBACK TRANSACTION
            SET NOCOUNT OFF
            RETURN @@ERROR
        END
    END ELSE BEGIN
        -- Row does exist, so we move it to the history then
        -- update to the new version.
        INSERT INTO PMLogicalDatabaseHistory(
            pmlogicaldatabase_id,
            version,
            install_date)
        VALUES(
            @lID,
            @sPreviousVersion,
            @sPreviousDate)
        IF @@ERROR <> 0 BEGIN
            ROLLBACK TRANSACTION
            SET NOCOUNT OFF
            RETURN @@ERROR
        END

        UPDATE PMLogicalDatabase
            SET version = @sVersion,
            install_date = getdate()
            WHERE pmlogicaldatabase_id = @lID
        IF @@ERROR <> 0 BEGIN
            ROLLBACK TRANSACTION
            SET NOCOUNT OFF
            RETURN @@ERROR
        END
    END

    COMMIT TRANSACTION
    SET NOCOUNT OFF
    RETURN 0
END
GO


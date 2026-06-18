SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PM_GetLanguageID'
GO
-- Returns a Language ID given the Language Code. If the language doesn't yet exist
-- it is created. This procedure is guaranteed to work even if the Language and
-- PMCaption tables are empty, and is useful for bootstrapping reference data.
-- Returns zero for success and nonzero for failure.
CREATE PROCEDURE spu_PM_GetLanguageID
    @sCode varchar(10),
    @sDescription varchar(255),
    @r_lLanguageID smallint OUTPUT
AS
BEGIN
    DECLARE @lLanguageID smallint
    DECLARE @lCaptionID integer

    SET NOCOUNT ON
    BEGIN TRANSACTION

    SELECT @r_lLanguageID = NULL

    -- Find the requested language.
    SELECT @lLanguageID = language_id FROM Language WHERE code = @sCode

    -- If the language doesn't exist, create it and a matching caption.
    IF @@ROWCOUNT = 0 BEGIN
        SELECT @lLanguageID = ISNULL((SELECT MAX(language_id) FROM Language), 0) + 1
        IF @@ERROR <> 0 BEGIN
            ROLLBACK TRANSACTION
            SET NOCOUNT OFF
            RETURN 1
        END

        INSERT INTO Language(
            language_id,
            code,
            description,
            caption_id,
            is_deleted,
            effective_date)
        VALUES(
            @lLanguageID,
            @sCode,
            @sDescription,
            0,
            0,
            getdate())
        IF @@ERROR <> 0 BEGIN
            ROLLBACK TRANSACTION
            SET NOCOUNT OFF
            RETURN 1
        END

        EXECUTE spu_pm_caption_id_return @lLanguageID, @sDescription, @lCaptionID OUTPUT
        IF @@ERROR <> 0 BEGIN
            ROLLBACK TRANSACTION
            SET NOCOUNT OFF
            RETURN 1
        END

        UPDATE Language SET caption_id = @lCaptionID WHERE language_id = @lLanguageID
        IF @@ERROR <> 0 BEGIN
            ROLLBACK TRANSACTION
            SET NOCOUNT OFF
            RETURN 1
        END
    END

    SELECT @r_lLanguageID = @lLanguageID

    COMMIT TRANSACTION
    SET NOCOUNT OFF
    RETURN 0
END
GO


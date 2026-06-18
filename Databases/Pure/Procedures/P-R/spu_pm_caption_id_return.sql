SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_caption_id_return'
GO
-- Returns a valid Caption ID for the specified text in the specified language.
-- Returns 0 for success, error number for failure.
CREATE PROCEDURE spu_pm_caption_id_return
    @language_id smallint,
    @caption varchar(255),
    @caption_id integer OUTPUT
AS
BEGIN
    DECLARE @nError integer

    -- Default values.
    SELECT @nError = 0
    SELECT @caption_id = NULL

    -- Find the ID of the specified text.
    SELECT @caption_id = caption_id
        FROM PMCaption
        WHERE language_id = @language_id
        AND convert(varbinary(255), caption) = convert(varbinary(255), @caption) 

    -- If it doesn't exist, create it.
    IF @caption_id IS NULL BEGIN
        -- SELECT @caption_id = ISNULL((SELECT MAX(caption_id) FROM PMCaption), 0) + 1
        -- EXECUTE spu_Get_Unique_Number 'PMCaption', @caption_id OUTPUT
        INSERT INTO PMCaptionIDGen DEFAULT VALUES
        SELECT @caption_id = @@IDENTITY

        INSERT INTO PMCaption(caption_id, language_id, caption)
            VALUES(@caption_id, @language_id, @caption)

        SELECT @nError = @@ERROR
        IF @nError <> 0 BEGIN
            SELECT @caption_id = NULL
            RETURN @nError
        END
    END

    -- Return success.
    RETURN 0
END
GO


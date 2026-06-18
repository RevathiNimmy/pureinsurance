SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_caption_desc'
GO

-- Returns the caption text given the language and caption IDs.
CREATE PROCEDURE spu_pm_caption_desc
    @language_id smallint,
    @caption_id integer
AS
BEGIN
    SELECT caption
        FROM PMCaption
        WHERE caption_id = @caption_id
        AND language_id = @language_id
END
GO


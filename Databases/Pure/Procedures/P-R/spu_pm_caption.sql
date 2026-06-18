SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_caption'
GO

-- Original function to generate a caption ID from the text and language.
-- This is now implemented as a wrapper round the preferred procedure.
CREATE PROCEDURE spu_pm_caption
    @language_id smallint,
    @caption varchar(255)
AS
BEGIN
    DECLARE @caption_id integer

    -- Generate the caption ID.
    EXECUTE spu_pm_caption_id_return @language_id, @caption, @caption_id OUTPUT

    -- Return it in a resultset.
    SELECT @caption_id AS caption_id
END
GO


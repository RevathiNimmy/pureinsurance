SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_caption_id_reseed'
GO
-- Re-seeds the Caption ID generator. Should be run after manually changing
-- the contents of the PMCaption table.
CREATE PROCEDURE spu_pm_caption_id_reseed
AS
BEGIN
    DECLARE @lLastCaptionID integer

    SELECT @lLastCaptionID = ISNULL((SELECT MAX(caption_id) FROM PMCaption), 0)

    SET IDENTITY_INSERT PMCaptionIDGen ON

    DELETE FROM PMCaptionIDGen
    INSERT INTO PMCaptionIDGen(caption_id) VALUES(@lLastCaptionID)

    SET IDENTITY_INSERT PMCaptionIDGen OFF

    DBCC CHECKIDENT('PMCaptionIDGen', RESEED, 1)
    DBCC CHECKIDENT('PMCaptionIDGen', RESEED)
END
GO


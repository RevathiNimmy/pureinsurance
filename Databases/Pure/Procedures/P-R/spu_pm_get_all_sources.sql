IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spu_pm_get_all_sources') AND sysstat & 0xf = 4)
    DROP PROCEDURE spu_pm_get_all_sources
GO

CREATE PROCEDURE [dbo].[spu_pm_get_all_sources]
    @UserID INT,
    @IncludeClosed TINYINT = 0
AS
BEGIN
    SET NOCOUNT ON

    SELECT source.source_id, [description], country_id, source.code
    FROM source
    WHERE (source.is_deleted <> 1 OR @IncludeClosed = 1)
END
GO
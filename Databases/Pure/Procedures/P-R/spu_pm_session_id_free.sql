SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_session_id_free'
GO
-- Releases a previously-created Session ID for re-cycling.
CREATE PROCEDURE spu_pm_session_id_free
    @lSessionID integer
AS
BEGIN
    DELETE FROM PMSessionIDGen
        WHERE session_id = @lSessionID
END
GO


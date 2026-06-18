SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_session_id_alloc'
GO
-- Creates and returns a unique Session ID.
CREATE PROCEDURE spu_pm_session_id_alloc
    @r_lSessionID integer OUTPUT
AS
BEGIN
    SELECT @r_lSessionID = NULL

    INSERT INTO PMSessionIDGen DEFAULT VALUES
    SELECT @r_lSessionID = @@IDENTITY
END
GO


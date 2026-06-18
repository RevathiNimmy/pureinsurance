SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_TempIDList_clear'
GO
CREATE PROCEDURE spu_TempIDList_clear
    @lSessionID integer
AS BEGIN
    DELETE FROM TempIDList
        WHERE session_id = @lSessionID
END
GO

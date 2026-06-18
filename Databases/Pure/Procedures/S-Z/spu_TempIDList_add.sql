SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_TempIDList_add'
GO
CREATE PROCEDURE spu_TempIDList_add
    @lSessionID integer,
    @lLinkID integer
AS BEGIN
    INSERT INTO TempIDList (
        session_id,
        link_id
    ) VALUES (
        @lSessionID,
        @lLinkID
    )
END
GO

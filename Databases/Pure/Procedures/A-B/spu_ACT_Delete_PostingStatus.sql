SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_PostingStatus'
GO


CREATE PROCEDURE spu_ACT_Delete_PostingStatus
    @postingstatus_id smallint
AS


DELETE FROM PostingStatus
WHERE postingstatus_id = @postingstatus_id
GO



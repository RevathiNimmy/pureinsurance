SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_PostingStatus'
GO


CREATE PROCEDURE spu_ACT_Select_PostingStatus
    @postingstatus_id smallint
AS


SELECT
    postingstatus_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM PostingStatus
WHERE postingstatus_id = @postingstatus_id
GO



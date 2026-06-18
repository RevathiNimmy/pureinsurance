SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_BatchStatus'
GO


CREATE PROCEDURE spu_ACT_Select_BatchStatus
    @batchstatus_id smallint
AS


SELECT
    batchstatus_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM BatchStatus
WHERE batchstatus_id = @batchstatus_id
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_BatchStatus'
GO


CREATE PROCEDURE spu_ACT_SelAll_BatchStatus
AS


SELECT
    batchstatus_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM BatchStatus
GO



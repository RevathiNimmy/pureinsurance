SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_PurgeFrequency'
GO


CREATE PROCEDURE spu_ACT_SelAll_PurgeFrequency
AS


SELECT
    purgefrequency_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM PurgeFrequency
ORDER BY code
GO



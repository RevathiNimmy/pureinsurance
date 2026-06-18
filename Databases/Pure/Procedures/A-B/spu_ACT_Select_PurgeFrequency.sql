SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_PurgeFrequency'
GO


CREATE PROCEDURE spu_ACT_Select_PurgeFrequency
    @purgefrequency_id smallint
AS


SELECT
    purgefrequency_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM PurgeFrequency
WHERE purgefrequency_id = @purgefrequency_id
GO



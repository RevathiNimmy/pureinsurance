SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_PurgeFrequency'
GO


CREATE PROCEDURE spu_ACT_Delete_PurgeFrequency
    @purgefrequency_id smallint
AS


DELETE FROM PurgeFrequency
WHERE purgefrequency_id = @purgefrequency_id
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMNav_Batch_Set_del'
GO

CREATE PROCEDURE spe_PMNav_Batch_Set_del
    @pmnav_batch_set_id int
AS
DELETE FROM PMNav_Batch_Set
WHERE pmnav_batch_set_id = @pmnav_batch_set_id

GO


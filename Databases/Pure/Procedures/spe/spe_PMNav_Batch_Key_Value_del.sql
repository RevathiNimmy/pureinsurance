SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMNav_Batch_Key_Value_del'
GO

CREATE PROCEDURE spe_PMNav_Batch_Key_Value_del
    @pmnav_batch_set_id int,
    @pmnav_batch_record_id int,
    @pmnav_batch_key_value_id int
AS
DELETE FROM PMNav_Batch_Key_Value
WHERE pmnav_batch_set_id = @pmnav_batch_set_id AND pmnav_batch_record_id = @pmnav_batch_record_id AND pmnav_batch_key_value_id = @pmnav_batch_key_value_id

GO


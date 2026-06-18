SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMNav_Batch_Key_Value_add'
GO

CREATE PROCEDURE spe_PMNav_Batch_Key_Value_add
    @pmnav_batch_set_id int,
    @pmnav_batch_record_id int,
    @pmnav_batch_key_value_id int,
    @pmnav_batch_id int,
    @pmnav_key_id int,
    @key_value varchar(60)
AS
BEGIN
INSERT INTO PMNav_Batch_Key_Value (
    pmnav_batch_set_id ,
    pmnav_batch_record_id ,
    pmnav_batch_key_value_id ,
    pmnav_batch_id ,
    pmnav_key_id ,
    key_value )
VALUES (
    @pmnav_batch_set_id,
    @pmnav_batch_record_id,
    @pmnav_batch_key_value_id,
    @pmnav_batch_id,
    @pmnav_key_id,
    @key_value)
END

GO


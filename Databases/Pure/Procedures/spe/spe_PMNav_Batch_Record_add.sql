SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMNav_Batch_Record_add'
GO

CREATE PROCEDURE spe_PMNav_Batch_Record_add
    @pmnav_batch_set_id int,
    @pmnav_batch_record_id int
AS
BEGIN
INSERT INTO PMNav_Batch_Record (
    pmnav_batch_set_id ,
    pmnav_batch_record_id )
VALUES (
    @pmnav_batch_set_id,
    @pmnav_batch_record_id)
END

GO


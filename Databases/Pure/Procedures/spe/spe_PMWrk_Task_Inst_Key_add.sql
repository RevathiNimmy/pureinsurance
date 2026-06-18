SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Inst_Key_add'
GO
CREATE PROCEDURE spe_PMWrk_Task_Inst_Key_add
    @pmwrk_task_instance_cnt int,
    @pmnav_key_id int,
    @key_value varchar(60)
AS
BEGIN
INSERT INTO PMWrk_Task_Inst_Key (
    pmwrk_task_instance_cnt,
    pmnav_key_id,
    key_value )
VALUES (
    @pmwrk_task_instance_cnt,
    @pmnav_key_id,
    @key_value)
END
GO


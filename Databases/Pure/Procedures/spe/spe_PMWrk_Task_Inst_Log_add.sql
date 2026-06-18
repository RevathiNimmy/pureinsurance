SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Inst_Log_add'
GO
CREATE PROCEDURE spe_PMWrk_Task_Inst_Log_add
    @pmwrk_task_instance_cnt int,
    @date_created datetime,
    @text varchar(255),
    @created_by_id smallint
AS
BEGIN
INSERT INTO PMWrk_Task_Inst_Log (
    pmwrk_task_instance_cnt,
    date_created,
    text,
    created_by_id )
VALUES (
    @pmwrk_task_instance_cnt,
    @date_created,
    @text,
    @created_by_id)
END
GO


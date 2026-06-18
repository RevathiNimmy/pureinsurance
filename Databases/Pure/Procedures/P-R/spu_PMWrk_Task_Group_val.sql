SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_PMWrk_Task_Group_val'
GO


CREATE PROCEDURE spu_PMWrk_Task_Group_val
    @pmwrk_task_group_id INT
AS


select distinct wtg.pmwrk_task_group_id id
    from PMWrk_Task_Group wtg,
    PMWrk_Task_Group_Task tgt,
    PMUser_Group_Activity uga
where tgt.pmwrk_task_group_id = @pmwrk_task_group_id
and wtg.pmwrk_task_group_id = tgt.pmwrk_task_group_id
and uga.pmwrk_task_group_id = tgt.pmwrk_task_group_id
GO



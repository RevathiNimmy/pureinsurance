SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Instance_Temp_upd'
GO

CREATE PROCEDURE spe_PMWrk_Task_Instance_Temp_upd
    @pmwrk_task_instance_temp_cnt int,
    @customer varchar(255) ,
    @pmwrk_task_group_id int ,
    @pmwrk_task_id int ,
    @description varchar(255) ,
    @task_due_date datetime ,
    @pmuser_group_id int ,
    @user_id smallint ,
    @task_status tinyint ,
    @is_urgent tinyint ,
    @date_created datetime ,
    @created_by_id smallint ,
    @last_modified datetime ,
    @modified_by_id smallint,
    @is_visible tinyint
AS
BEGIN
UPDATE PMWrk_Task_Instance_Temp
SET customer=@customer,
    pmwrk_task_group_id=@pmwrk_task_group_id,
    pmwrk_task_id=@pmwrk_task_id,
    description=@description,    
    task_due_date=@task_due_date,
    pmuser_group_id=@pmuser_group_id,
    user_id=@user_id,
    task_status=@task_status,
    is_urgent=@is_urgent,
    date_created=@date_created,
    created_by_id=@created_by_id,
    last_modified=@last_modified,
    modified_by_id=@modified_by_id,
    is_visible=@is_visible        
WHERE pmwrk_task_instance_temp_cnt = @pmwrk_task_instance_temp_cnt
END

GO













SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_SAM_PMWrk_Task_Instance_upd
GO

--Start (Sankar)-(Tech Spec - UIIC WR33 - Work Manager - Update Task.doc)-(6.0)

CREATE  PROCEDURE spu_SAM_PMWrk_Task_Instance_upd
 @pmwrk_task_instance_cnt int,
    @customer varchar(255),
    @task_due_date datetime,
    @pmuser_group_id int,
    @user_id smallint,
    @description varchar(255),
    @task_status tinyint,
    @is_urgent tinyint,
    @last_modified datetime,
    @modified_by_id smallint,
    @is_visible tinyint,
    @workflow_information varchar(255)
AS
BEGIN
Declare @Party_cnt INT

SELECT @party_cnt=ISNULL(p.party_cnt,0) from PMUSER u JOIN party p
on u.party_cnt=p.party_cnt
WHERE user_id=@user_id and party_type_id=3

UPDATE PMWrk_Task_Instance
    SET
    customer=@customer,
    task_due_date=@task_due_date,
    pmuser_group_id=@pmuser_group_id,
    user_id=@user_id,
    description=@description,
    task_status=@task_status,
    is_urgent=@is_urgent,
    last_modified=@last_modified,
    modified_by_id=@modified_by_id,
    is_visible=@is_visible,
    workflow_information=@workflow_information,
    Party_cnt=@Party_cnt
WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
END

--End (Sankar)-(Tech Spec - UIIC WR33 - Work Manager - Update Task.doc)-(6.0)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


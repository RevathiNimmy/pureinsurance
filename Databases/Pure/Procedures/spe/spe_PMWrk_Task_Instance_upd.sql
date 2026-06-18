SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Instance_upd'
GO
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.1 Add is_visible column 14/12/1999 DAK */
/********************************************************************************************************/
CREATE PROCEDURE spe_PMWrk_Task_Instance_upd
    @pmwrk_task_instance_cnt int,
    @pmwrk_task_group_id int,
    @pmwrk_task_id int,
    @customer varchar(255),
    @task_due_date datetime,
    @pmuser_group_id int,
    @user_id smallint,
    @description varchar(255),
    @task_status tinyint,
    @is_urgent tinyint,
    @date_created datetime,
    @created_by_id smallint,
    @last_modified datetime,
    @modified_by_id smallint,
    @is_visible tinyint,
    @workflow_information varchar(255),
    @is_task_review tinyint,
    @external_workflow_id uniqueidentifier = NULL,
    @pmwrk_task_parent_instance_cnt INT = NULL,
    @is_external_workItem TINYINT = 0
    
AS
BEGIN
Declare @Party_cnt INT

SELECT @party_cnt=ISNULL(p.party_cnt,0) from PMUSER u JOIN party p
on u.party_cnt=p.party_cnt
WHERE user_id=@user_id and party_type_id=3  

If @external_workflow_id IS NOT NULL 
       BEGIN

UPDATE PMWrk_Task_Instance
    SET
    pmwrk_task_group_id=@pmwrk_task_group_id,
    pmwrk_task_id=@pmwrk_task_id,
    customer=@customer,
    task_due_date=@task_due_date,
    pmuser_group_id=@pmuser_group_id,
    user_id=@user_id,
    description=@description,
    task_status=@task_status,
    is_urgent=@is_urgent,
    date_created=@date_created,
    created_by_id=@created_by_id,
    last_modified=@last_modified,
    modified_by_id=@modified_by_id,
    is_visible=@is_visible,
    workflow_information=@workflow_information,
    is_task_review=@is_task_review,
              Party_cnt=@party_cnt,
              external_Workflow_id=@external_workflow_id,
              PMWrk_task_parent_instance_cnt=@pmwrk_task_parent_instance_cnt,
              Is_External_WorkItem = @is_external_workItem
       WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
       END
ELSE
       BEGIN
		UPDATE PMWrk_Task_Instance
			SET
			pmwrk_task_group_id=@pmwrk_task_group_id,
			pmwrk_task_id=@pmwrk_task_id,
			customer=@customer,
			task_due_date=@task_due_date,
			pmuser_group_id=@pmuser_group_id,
			user_id=@user_id,
			description=@description,
			task_status=@task_status,
			is_urgent=@is_urgent,
			date_created=@date_created,
			created_by_id=@created_by_id,
			last_modified=@last_modified,
			modified_by_id=@modified_by_id,
			is_visible=@is_visible,
			workflow_information=@workflow_information,
			is_task_review=@is_task_review,
    Party_cnt=@party_cnt
WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
END

if (@task_status = 3) BEGIN
	IF EXISTS (SELECT 1 FROM claim_link WHERE link_id = @pmwrk_task_instance_cnt) BEGIN
		Update claim_link set processed=1 where link_id = @pmwrk_task_instance_cnt and link_type_id = 2
	END
END
END

GO


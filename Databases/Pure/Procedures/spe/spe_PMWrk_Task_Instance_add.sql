SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Instance_add'
GO
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.1 Add is_visible column 14/12/1999 DAK */
/* 1.2 Added workflow_information column 17/1/2003 AMB */
/* 1.3 Added source_id for sp 246 23/09/2003 RSB */
/********************************************************************************************************/
CREATE PROCEDURE spe_PMWrk_Task_Instance_add
    @pmwrk_task_instance_cnt int OUTPUT,
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
    @source_id int=NULL,
    @is_task_review tinyint=NULL,
    @external_workflow_id uniqueidentifier = NULL,
    @pmwrk_task_parent_instance_cnt INT = NULL,
    @is_external_workItem TINYINT = 0
   

AS
BEGIN
Declare @Party_cnt INT

IF ISNULL(@user_id,0)=0

  BEGIN
       SELECT @party_cnt=ISNULL(p.party_cnt,0) from PMUSER u JOIN party p
          on u.party_cnt=p.party_cnt
       WHERE user_id=@created_by_id and party_type_id=3
  END
ELSE
  BEGIN
        SELECT @party_cnt=ISNULL(p.party_cnt,0) from PMUSER u JOIN party p
          on u.party_cnt=p.party_cnt
       WHERE user_id=@user_id and party_type_id=3
  END
INSERT INTO PMWrk_Task_Instance (
    pmwrk_task_group_id,
    pmwrk_task_id,
    customer,
    task_due_date,
    pmuser_group_id,
    user_id,
    description,
    task_status,
    is_urgent,
    date_created,
    created_by_id,
    last_modified,
    modified_by_id,
    is_visible,
    workflow_information,
    source_id,
    is_task_review,
    original_pmuser_group_id,
    Party_cnt,
    external_workflow_id,
    PMWrk_task_parent_instance_cnt,
    Is_External_WorkItem
    )
VALUES (
    @pmwrk_task_group_id,
    @pmwrk_task_id,
    @customer,
    @task_due_date,
    @pmuser_group_id,
    @user_id,
    @description,
    @task_status,
    @is_urgent,
    @date_created,
    @created_by_id,
    @last_modified,
    @modified_by_id,
    @is_visible,
    @workflow_information,
    @source_id,
    @is_task_review,
    @pmuser_group_id,
    @party_cnt,
    @external_workflow_id,
    @pmwrk_task_parent_instance_cnt,
    @is_external_workItem
    )
END
BEGIN
SELECT @pmwrk_task_instance_cnt = @@IDENTITY
END

GO


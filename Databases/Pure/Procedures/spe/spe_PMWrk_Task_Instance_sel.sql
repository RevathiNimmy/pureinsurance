SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Instance_sel'
GO
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.1 Add is_visible column 14/12/1999 DAK */
/********************************************************************************************************/

CREATE PROCEDURE spe_PMWrk_Task_Instance_sel
    @pmwrk_task_instance_cnt int,
	@key_name			varchar(30)  = '',
	@key_value			varchar(8000) = ''
AS

If @pmwrk_task_instance_cnt <> -1
	BEGIN
SELECT
    pmwrk_task_instance_cnt,
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
    is_task_review,  
		original_pmuser_group_id,
		Is_External_WorkItem,
		external_workflow_id,
		PMWrk_task_parent_instance_cnt
    FROM PMWrk_Task_Instance WITH (NOLOCK)
    WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
	END
ELSE
	BEGIN
		SELECT
		pmwrk_task_instance_cnt,
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
		is_task_review,
		original_pmuser_group_id,
		Is_External_WorkItem,
		external_workflow_id,
		PMWrk_task_parent_instance_cnt
		FROM PMWrk_Task_Instance WITH (NOLOCK)
		WHERE pmwrk_task_instance_cnt in ( 
										   select pmwrk_task_instance_cnt from PMWrk_Task_Inst_Key 
										   where UPPER(RTRIM(LTRIM(key_value))) = UPPER(RTRIM(LTRIM( @key_value))) 
										   AND pmnav_key_id = ( 
																select pmnav_key_id from PMNav_Key where UPPER(RTRIM(LTRIM(name))) = UPPER(RTRIM(LTRIM(@key_name)))
															  )
										  )
		ORDER By pmwrk_task_instance_cnt
	END
GO


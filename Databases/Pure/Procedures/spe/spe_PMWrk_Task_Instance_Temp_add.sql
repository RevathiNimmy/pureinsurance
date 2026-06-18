EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Instance_Temp_add'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


/****** Object:  Stored Procedure dbo.spe_PMWrk_Task_Instance_Temp_add    Script Date: 26/06/2001 11:56:15 ******/
CREATE PROCEDURE spe_PMWrk_Task_Instance_Temp_add
    @pmwrk_task_instance_temp_cnt int OUTPUT ,
    @pmwrk_task_group_id int ,
    @pmwrk_task_id int ,
    @customer varchar(255) ,
    @task_due_date datetime ,
    @pmuser_group_id int ,
    @user_id smallint ,
    @description varchar(255) ,
    @task_status tinyint ,
    @is_urgent tinyint ,
    @date_created datetime ,
    @created_by_id smallint ,
    @last_modified datetime ,
    @modified_by_id smallint,
    @is_visible tinyint,
    @workflow_information varchar(255)
AS
/********************************************************************************************************/
/* Revision            	Description of Modification                                    	Date	    Who */
/* --------            	---------------------------                                    	----        --- */
/* 1.0                  Lifted from spe_PMWrk_Task_Instance_add                   26/06/2001  MSS */
/********************************************************************************************************/

BEGIN
INSERT INTO PMWrk_Task_Instance_Temp (
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
    workflow_information)

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
    @workflow_information)
END
BEGIN
SELECT @pmwrk_task_instance_temp_cnt = @@IDENTITY
END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

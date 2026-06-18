EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Instance_Temp_sel'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spe_PMWrk_Task_Instance_Temp_sel
    @pmwrk_task_instance_temp_cnt int
AS
/********************************************************************************************************/
/* Revision            	Description of Modification                                    	Date	    Who */
/* --------            	---------------------------                                    	----        --- */
/* 1.0                  Lifted from spe_PMWrk_Task_Instance_sel		27/06/2000  MSS */
/********************************************************************************************************/
SELECT
    pmwrk_task_instance_temp_cnt,
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
    is_visible
 FROM PMWrk_Task_Instance_Temp
WHERE pmwrk_task_instance_temp_cnt = @pmwrk_task_instance_temp_cnt 

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO





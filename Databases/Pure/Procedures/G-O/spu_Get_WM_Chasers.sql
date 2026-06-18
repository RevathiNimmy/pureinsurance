/* AK - 100402
    Stored procedure to return all the wm tasks marked for chaser
*/

EXECUTE DDLDropProcedure 'spu_get_wm_chasers'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_get_wm_chasers
@Description varchar(255)
 AS
     	
     	SELECT t.PMWrk_Task_Instance_cnt, d.document_template_id, d.document_type_id, kp.key_value PartyCnt, ki.key_value InsuranceFileCnt 
     		FROM PMWrk_Task_Inst_Key kp, 
     			PMWrk_Task_Inst_Key ki,
     			PMWrk_Task_Instance t,
     			document_template d
     			
     	WHERE t.description = @Description
     	AND   kp.PMWrk_Task_Instance_cnt = t.PMWrk_Task_Instance_cnt
     	AND   kp.PMNav_Key_Id = (Select PMNav_Key_Id from PMNav_Key where name = 'party_cnt')
     	AND   ki.PMWrk_Task_Instance_cnt = t.PMWrk_Task_Instance_cnt
     	AND   ki.PMNav_Key_Id = (Select PMNav_Key_Id from PMNav_Key where name = 'insurance_file_cnt')
     	AND   t.task_status <> 3 /* no lookup for this, Constants are being used at VB side */
     	AND   t.task_due_date <= getdate()
     	AND   d.document_template_id = (Select min(document_template_id) 
     					from document_template 
     					where chaser_description = @Description) /* in case there are more than one */
     	


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO


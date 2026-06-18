SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Import_Get_Default_Task_Details'
GO

CREATE PROCEDURE spu_ACT_Import_Get_Default_Task_Details    
    
@pmwrk_task_id int OUTPUT,     
@pmwrk_task_group_id int OUTPUT,     
@pmuser_group_id int OUTPUT,     
@pmuser_id int OUTPUT    
    
AS    
    
select @pmwrk_task_id = pmwrk_task_id from pmwrk_task where code = 'memo'    
select @pmwrk_task_group_id = pmwrk_task_group_id from pmwrk_task_group where code ='common'    
select @pmuser_group_id = pmuser_group_id from pmuser_group where code = 'sysadmin'    
select @pmuser_id = user_id from pmuser where username = 'sirius'    


GO

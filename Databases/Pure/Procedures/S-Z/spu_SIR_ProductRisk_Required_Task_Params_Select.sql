SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_ProductRisk_Required_Task_Params_Select'
GO

CREATE PROCEDURE spu_SIR_ProductRisk_Required_Task_Params_Select  
  
AS  
  
BEGIN  
  
DECLARE @TaskGroupId int  
DECLARE @TaskId int  
DECLARE @UserGroupId int  
  
SELECT @TaskGroupId = pmwrk_task_group_id from pmwrk_task_group  WHERE code ='COMMON'  
SELECT @TaskId = pmwrk_task_id from pmwrk_task WHERE code ='MEMO'  
SELECT @UserGroupId = pmuser_group_id from pmuser_group  WHERE  code ='SYSADMIN'  
  
SELECT @TaskGroupId as task_group_id, @TaskId as task_id, @UserGroupId as user_group_id  
  
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

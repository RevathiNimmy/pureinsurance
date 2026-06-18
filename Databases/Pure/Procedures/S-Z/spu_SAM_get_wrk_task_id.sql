SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_get_wrk_task_id'
GO

CREATE PROCEDURE spu_SAM_get_wrk_task_id  
  
@Task_code varchar(50),  
@Task_id int OUTPUT  
  
AS  
  
SELECT @task_id  = pmwrk_task_id FROM pmwrk_task WHERE code = @task_code  
  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

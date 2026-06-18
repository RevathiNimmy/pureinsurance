/* AK - 100603
    Stored procedure to remove all the work manager tasks created for Claims Payment Authorisation
*/

EXECUTE DDLDropProcedure 'spu_clm_remove_authorisation_tasks'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_clm_remove_authorisation_tasks
	@description varchar(255)
AS
	DECLARE @TaskId int
	DECLARE @TaskInstance int
	SELECT @TaskId = PmWrk_Task_Id from PMWrk_Task 
	       WHERE code = 'AUTHPMNT'
     	
	DECLARE cur_Task CURSOR FAST_Forward  FOR	
		SELECT PMWrk_Task_Instance_cnt
		FROM PMWrk_Task_Instance
		WHERE PMWrk_Task_Id = @TaskId
			     			

	OPEN cur_Task
	FETCH NEXT FROM cur_Task INTO @TaskInstance 

	WHILE @@FETCH_STATUS = 0	
	BEGIN
		DELETE PMWrk_Task_Inst_Log WHERE PMWrk_Task_Instance_Cnt = @TaskInstance
		DELETE PMWrk_Task_Inst_Key WHERE PMWrk_Task_Instance_Cnt = @TaskInstance
		DELETE PMWrk_Task_Instance WHERE PMWrk_Task_Instance_Cnt = @TaskInstance

		FETCH NEXT FROM cur_Task INTO @TaskInstance 
	END
	Close cur_Task 
	Deallocate cur_Task


     	


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
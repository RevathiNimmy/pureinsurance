
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_Get_TaskGroup_For_WrkTaskID
GO

CREATE PROCEDURE spu_Get_TaskGroup_For_WrkTaskID
	@pmwrk_task_id int,
	@Group_code varchar(10) = NULL
AS

	Select TG.pmwrk_task_group_id, TG.Code FROM PMWrk_Task_Group_Task TGT
	JOIN pmwrk_task_group TG ON TG.pmwrk_task_group_id = TGT.pmwrk_task_group_id
	Where pmwrk_task_id = @pmwrk_task_id  and ( @Group_code IS NULL OR RTRIM(TG.CODE)=  @Group_code)


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_PMwrk_Task_Group_Tasks_Select'
GO

CREATE PROCEDURE spu_ACT_PMwrk_Task_Group_Tasks_Select

AS

BEGIN
	select pmwrk_task_group.pmwrk_task_group_id, pmwrk_task_group.description, 
		pmwrk_task.pmwrk_task_id, pmwrk_task.description TaskDescription

	FROM (Select * from pmwrk_task_group where is_deleted = 0 and effective_date <GetDate()) pmwrk_task_group

		left join pmwrk_task_group_task on
			pmwrk_task_group_task.pmwrk_task_group_id = pmwrk_task_group.pmwrk_task_group_id
	
			inner join (select * from pmwrk_task where is_deleted = 0 and effective_date < GetDate()) pmwrk_task on
				pmwrk_task_group_task.pmwrk_task_id = pmwrk_task.pmwrk_task_id

	order by pmwrk_task_group.pmwrk_task_group_id, 
		 TaskDescription
END 


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

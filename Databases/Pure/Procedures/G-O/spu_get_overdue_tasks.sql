SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_Overdue_Tasks'
GO

CREATE PROCEDURE spu_Get_Overdue_Tasks

AS

select 	pti.pmwrk_task_instance_cnt
from 	pmwrk_task_instance pti
join	pmwrk_task pt
on	pti.pmwrk_task_id = pt.pmwrk_task_id
join 	pmwrk_task_check ptc
on	pti.pmwrk_task_id = ptc.pmwrk_task_id
where 	pti.task_due_date < getdate()
and 	pti.pmwrk_task_id in 
		(select pmwrk_task_id from pmwrk_task_check where effective_date <= GetDate() and is_deleted = 0 and pmwrk_task_id <> 0) 
and 	pti.task_status in (0,2)
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_PMwrk_Task_Group_PMUserGroup_Select'
GO

CREATE PROCEDURE spu_ACT_PMwrk_Task_Group_PMUserGroup_Select

AS

BEGIN
	SELECT pmwrk_task_group.pmwrk_task_group_id, pmwrk_task_group.description, 
		pmuser_group.pmuser_group_id, pmuser_group.description UserGroupDescription
	
	FROM (Select * from pmwrk_task_group where is_deleted = 0 and effective_date <GetDate()) pmwrk_task_group

		left join pmuser_group_activity on
			pmwrk_task_group.pmwrk_task_group_id = pmuser_group_activity.pmwrk_task_group_id
		
			inner join (select * from pmuser_group where is_deleted = 0 and effective_date < GetDate()) pmuser_group on 
				pmuser_group_activity.pmuser_group_id = pmuser_group.pmuser_group_id

	order by pmwrk_task_group.pmwrk_task_group_id, 
		 UserGroupDescription	
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

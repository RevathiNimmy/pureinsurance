SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMWrk_Product_User_Sel'
GO

CREATE PROCEDURE spu_PMWrk_Product_User_Sel
    @category_code char(10)
AS

SELECT u.username, i.description 
	FROM PMWrk_Task_Category c WITH (NOLOCK)
		INNER JOIN PMWrk_Task t WITH (NOLOCK) ON t.pmwrk_task_category_id = c.pmwrk_task_category_id 
		INNER JOIN PMWrk_Task_Instance i WITH (NOLOCK) ON i.pmwrk_task_id = t.pmwrk_task_id 
		INNER JOIN PMUser u WITH (NOLOCK) ON u.user_id = i.user_id 
	WHERE c.code = @category_code AND i.task_status = 1 
	ORDER BY username ASC
GO
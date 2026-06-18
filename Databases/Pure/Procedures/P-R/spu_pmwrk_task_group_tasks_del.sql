SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_pmwrk_task_group_tasks_del'
GO
--Start (girija) - (Tech Spec - UIIC WR01 - User Access - Update Task Group Tasks.doc)-(6.1)
CREATE PROCEDURE spu_pmwrk_task_group_tasks_del  
    @pmwrk_task_group_id  int 
    
AS  
  
BEGIN  
DELETE FROM pmwrk_task_group_task Where pmwrk_task_group_id = @pmwrk_task_group_id
END  
--End (girija) - (Tech Spec - UIIC WR01 - User Access - Update Task Group Tasks.doc)-(6.1)

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


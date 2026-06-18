SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_pmwrk_task_group_task_add'
GO
--Start (girija) - Tech Spec - UIIC WR01 - User Access - Update Task Group Tasks.doc-(6.2)
CREATE PROCEDURE spu_pmwrk_task_group_task_add  
    @pmwrk_task_group_id int ,
    @pmwrk_task_id int,
    @display_sequence_num int=0
    
AS  
  
BEGIN  
INSERT INTO pmwrk_task_group_task (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num) 
Values (@pmwrk_task_group_id, @pmwrk_task_id, @display_sequence_num)

END  
--End (girija) - Tech Spec - UIIC WR01 - User Access - Update Task Group Tasks.doc-(6.2)

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


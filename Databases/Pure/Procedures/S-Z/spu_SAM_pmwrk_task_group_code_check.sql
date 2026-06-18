--Start (Vijayakumar Ramasamy) - (Tech Spec - UIIC WR01 - User Access - Add Task Group.doc) - (This is to find Group Code is Exist or not ,it is not mentioned in tech spec) 
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_pmwrk_task_group_code_check'
GO
CREATE PROCEDURE spu_SAM_pmwrk_task_group_code_check
@code varchar(10)
AS
BEGIN
SELECT pmwrk_task_group_id FROM pmwrk_task_group WHERE code=@code
END
--End (Vijayakumar Ramasamy) - (Tech Spec - UIIC WR01 - User Access - Add Task Group.doc) - (This is to find Group Code is Exist or not ,it is not mentioned in tech spec)
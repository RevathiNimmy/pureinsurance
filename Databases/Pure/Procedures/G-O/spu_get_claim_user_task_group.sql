SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_claim_user_task_group'
GO

CREATE PROCEDURE spu_get_claim_user_task_group
AS

declare @pmwrk_task_group_id int
declare @pmuser_group_id int

-- Get claim task group ID
SELECT	@pmwrk_task_group_id = pmwrk_task_group_id
FROM	PMWrk_Task_Group
WHERE	code = 'CLAIMS'

-- Get claim user group ID
SELECT	@pmuser_group_id = pmuser_group_id
FROM	PMuser_group
WHERE	code = 'Claims'

-- Return
SELECT @pmwrk_task_group_id, @pmuser_group_id

GO
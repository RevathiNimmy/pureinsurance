SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerTasksByUser') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerTasksByUser
GO

CREATE PROCEDURE spu_SAM_WorkManagerTasksByUser
    @user_group_ids VARCHAR(MAX) = NULL,
    @branch_ids VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON

    SELECT u.username AS UserName, COUNT(*) AS PendingTaskCount, MIN(t.task_due_date) AS OldestDueDate
    FROM PMWrk_Task_Instance t WITH (NOLOCK)
    INNER JOIN PMUser u WITH (NOLOCK) ON u.user_id = t.user_id
    WHERE t.task_status IN (0, 1, 2, 5) AND t.is_visible = 1
      AND (@user_group_ids IS NULL OR t.pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR t.source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
    GROUP BY u.username
    ORDER BY COUNT(*) DESC
END
GO

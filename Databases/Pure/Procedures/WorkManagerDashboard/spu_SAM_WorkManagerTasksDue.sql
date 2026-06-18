SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerTasksDue') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerTasksDue
GO

CREATE PROCEDURE spu_SAM_WorkManagerTasksDue
    @user_group_ids VARCHAR(MAX) = NULL,
    @branch_ids VARCHAR(MAX) = NULL,
    @for_user_id INT = NULL,
    @date_range INT = 1
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @today DATE = CAST(GETDATE() AS DATE)
    DECLARE @date_to DATE

    SET @date_to = CASE @date_range
        WHEN 0 THEN CAST('2099-12-31' AS DATE)
        WHEN 1 THEN @today
        WHEN 2 THEN DATEADD(DAY, 1, @today)
        WHEN 3 THEN DATEADD(DAY, 2, @today)
        WHEN 4 THEN DATEADD(DAY, 3, @today)
        WHEN 5 THEN DATEADD(DAY, 4, @today)
        WHEN 6 THEN DATEADD(DAY, 5, @today)
        WHEN 7 THEN DATEADD(DAY, 6, @today)
        WHEN 8 THEN DATEADD(DAY, 7, @today)
        WHEN 9 THEN DATEADD(DAY, 14, @today)
        WHEN 10 THEN DATEADD(DAY, 28, @today)
        ELSE @today
    END

    -- Sargable: use < day_after instead of CAST(col AS DATE) <= @date_to
    DECLARE @date_to_exclusive DATETIME = DATEADD(DAY, 1, @date_to)

    SELECT t.pmwrk_task_instance_cnt AS TaskInstanceKey, t.description AS Description, t.task_due_date AS DueDate,
        g.description AS UserGroupDescription, t.pmuser_group_id AS UserGroupId, u.username AS UserCode,
        t.is_urgent AS IsUrgent, t.task_status AS TaskStatusKey, tg.description AS TaskType, t.customer AS Customer
    FROM PMWrk_Task_Instance t WITH (NOLOCK)
    LEFT JOIN PMUser_Group g WITH (NOLOCK) ON g.pmuser_group_id = t.pmuser_group_id
    LEFT JOIN PMUser u WITH (NOLOCK) ON u.user_id = t.user_id
    LEFT JOIN PMWrk_Task_Group tg WITH (NOLOCK) ON tg.pmwrk_task_group_id = t.pmwrk_task_group_id
    WHERE t.task_status IN (0, 1, 2, 5) AND t.task_due_date < @date_to_exclusive AND t.is_visible = 1
      AND (@for_user_id IS NULL OR t.user_id = @for_user_id)
      AND (@user_group_ids IS NULL OR t.pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR t.source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
    ORDER BY t.is_urgent DESC, t.task_due_date ASC
END
GO

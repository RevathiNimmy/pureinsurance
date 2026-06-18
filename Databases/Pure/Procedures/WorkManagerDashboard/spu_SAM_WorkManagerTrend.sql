SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerTrend') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerTrend
GO

CREATE PROCEDURE spu_SAM_WorkManagerTrend
    @user_group_ids VARCHAR(MAX) = NULL,
    @branch_ids VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @today DATE = CAST(GETDATE() AS DATE)
    -- DATEFIRST-independent Monday calculation
    DECLARE @current_week_start DATE = DATEADD(DAY, -((DATEPART(dw, @today) + @@DATEFIRST - 2) % 7), @today)
    DECLARE @previous_week_start DATE = DATEADD(DAY, -7, @current_week_start)
    DECLARE @next_week_start DATE = DATEADD(DAY, 7, @current_week_start)

    ;WITH Days AS (SELECT 0 AS DayOffset UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6)
    SELECT d.DayOffset, DATENAME(WEEKDAY, DATEADD(DAY, d.DayOffset, @current_week_start)) AS DayOfWeek,
        ISNULL(cw.TaskCount, 0) AS CurrentWeekCount, ISNULL(pw.TaskCount, 0) AS PreviousWeekCount
    FROM Days d
    LEFT JOIN (
        SELECT DATEDIFF(DAY, @current_week_start, CAST(task_due_date AS DATE)) AS DayOffset, COUNT(*) AS TaskCount
        FROM PMWrk_Task_Instance WITH (NOLOCK)
        WHERE task_status IN (0,1,2,5) AND task_due_date >= @current_week_start AND task_due_date < @next_week_start AND is_visible = 1
          AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
          AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
        GROUP BY DATEDIFF(DAY, @current_week_start, CAST(task_due_date AS DATE))
    ) cw ON cw.DayOffset = d.DayOffset
    LEFT JOIN (
        SELECT DATEDIFF(DAY, @previous_week_start, CAST(task_due_date AS DATE)) AS DayOffset, COUNT(*) AS TaskCount
        FROM PMWrk_Task_Instance WITH (NOLOCK)
        WHERE task_status IN (0,1,2,5) AND task_due_date >= @previous_week_start AND task_due_date < @current_week_start AND is_visible = 1
          AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
          AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
        GROUP BY DATEDIFF(DAY, @previous_week_start, CAST(task_due_date AS DATE))
    ) pw ON pw.DayOffset = d.DayOffset
    ORDER BY d.DayOffset

    SELECT COUNT(*) AS TodayDueCount FROM PMWrk_Task_Instance WITH (NOLOCK)
    WHERE task_status IN (0,1,2,5) AND task_due_date < DATEADD(DAY, 1, @today) AND is_visible = 1
      AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
END
GO

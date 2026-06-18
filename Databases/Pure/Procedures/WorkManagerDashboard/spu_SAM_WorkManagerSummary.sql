SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerSummary') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerSummary
GO

CREATE PROCEDURE spu_SAM_WorkManagerSummary
    @user_group_ids VARCHAR(MAX) = NULL,
    @branch_ids VARCHAR(MAX) = NULL,
    @date_range INT = 1
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @today DATE = CAST(GETDATE() AS DATE)
    DECLARE @date_from DATE, @date_to DATE, @is_all_dates BIT = 0

    -- Use same enumerated codes as TasksDue for consistency
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

    IF @date_range = 0 BEGIN SET @date_from = '2000-01-01'; SET @is_all_dates = 1 END
    ELSE SET @date_from = @today

    -- Use range boundary for sargability: task_due_date < DATEADD(DAY, 1, @date_to)
    DECLARE @date_to_exclusive DATETIME = DATEADD(DAY, 1, @date_to)
    DECLARE @today_exclusive DATETIME = DATEADD(DAY, 1, @today)
    DECLARE @date_from_dt DATETIME = @date_from

    DECLARE @due_count INT = 0, @upcoming_count INT = 0, @completed_count INT = 0
    DECLARE @today_assigned INT = 0, @today_completed INT = 0, @in_progress_count INT = 0

    SELECT @due_count = COUNT(*)
    FROM PMWrk_Task_Instance WITH (NOLOCK)
    WHERE task_status IN (0,1,2,5) AND task_due_date >= @date_from_dt AND task_due_date < @date_to_exclusive AND is_visible = 1
      AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))

    IF @is_all_dates = 1
        SELECT @upcoming_count = COUNT(*) FROM PMWrk_Task_Instance WITH (NOLOCK)
        WHERE task_status IN (0,1,2,5) AND task_due_date >= @today_exclusive AND is_visible = 1
          AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
          AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
    ELSE
        SELECT @upcoming_count = COUNT(*) FROM PMWrk_Task_Instance WITH (NOLOCK)
        WHERE task_status IN (0,1,2,5) AND task_due_date >= @today_exclusive AND task_due_date < @date_to_exclusive AND is_visible = 1
          AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
          AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))

    SELECT @completed_count = COUNT(*) FROM PMWrk_Task_Instance WITH (NOLOCK)
    WHERE task_status = 3 AND last_modified >= @date_from_dt AND last_modified < @date_to_exclusive AND is_visible = 1
      AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))

    SELECT @today_assigned = COUNT(*) FROM PMWrk_Task_Instance WITH (NOLOCK)
    WHERE task_due_date >= @today AND task_due_date < @today_exclusive AND is_visible = 1
      AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))

    SELECT @today_completed = COUNT(*) FROM PMWrk_Task_Instance WITH (NOLOCK)
    WHERE task_status = 3 AND last_modified >= @today AND last_modified < @today_exclusive AND is_visible = 1
      AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))

    SELECT @in_progress_count = COUNT(*) FROM PMWrk_Task_Instance WITH (NOLOCK)
    WHERE task_status = 1 AND task_due_date >= @date_from_dt AND task_due_date < @date_to_exclusive AND is_visible = 1
      AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))

    SELECT @due_count AS DueTaskCount, CAST(0 AS DECIMAL(5,1)) AS DueTaskTrendPercent,
        @upcoming_count AS UpcomingTaskCount, CAST(0 AS DECIMAL(5,1)) AS UpcomingTaskTrendPercent,
        @completed_count AS CompletedTaskCount, CAST(0 AS DECIMAL(5,1)) AS CompletedTaskTrendPercent,
        @today_assigned AS TodayAssignedCount, @today_completed AS TodayCompletedCount,
        @in_progress_count AS InProgressCount
END
GO

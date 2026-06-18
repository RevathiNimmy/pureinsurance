SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'spu_SAM_WorkManagerCompletedVsTotal') AND type = N'P')
    DROP PROCEDURE spu_SAM_WorkManagerCompletedVsTotal
GO

CREATE PROCEDURE spu_SAM_WorkManagerCompletedVsTotal
    @user_group_ids VARCHAR(MAX) = NULL,
    @branch_ids VARCHAR(MAX) = NULL,
    @months INT = 6
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @start_date DATE = DATEADD(MONTH, -@months, DATEADD(DAY, 1 - DAY(GETDATE()), CAST(GETDATE() AS DATE)))

    SELECT LEFT(DATENAME(MONTH, DATEFROMPARTS(YEAR(task_due_date), MONTH(task_due_date), 1)), 3) + ' ' + CAST(YEAR(task_due_date) AS VARCHAR(4)) AS MonthYear,
        YEAR(task_due_date) AS SortYear, MONTH(task_due_date) AS SortMonth,
        COUNT(*) AS TotalTasks, SUM(CASE WHEN task_status = 3 THEN 1 ELSE 0 END) AS CompletedTasks
    FROM PMWrk_Task_Instance WITH (NOLOCK)
    WHERE task_due_date >= @start_date AND is_visible = 1
      AND (@user_group_ids IS NULL OR pmuser_group_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@user_group_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
      AND (@branch_ids IS NULL OR source_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@branch_ids, ',') WHERE TRY_CAST(value AS INT) IS NOT NULL))
    GROUP BY YEAR(task_due_date), MONTH(task_due_date)
    ORDER BY YEAR(task_due_date), MONTH(task_due_date)
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_PMWrk_Task_Group_Task_add'
GO

-- This procedure is intended for adding data in upgrade scripts, not for calling from normal code.
CREATE PROCEDURE spu_SAM_PMWrk_Task_Group_Task_add
    @pmwrk_task_group_code char(10),
    @pmwrk_task_code char(10)
AS BEGIN
    SET NOCOUNT ON

    -- Generate the task group ID.
    DECLARE @pmwrk_task_group_id integer
    SELECT @pmwrk_task_group_id = pmwrk_task_group_id FROM PMWrk_Task_Group WHERE code = @pmwrk_task_group_code

    -- Generate the task ID.
    DECLARE @pmwrk_task_id integer
    SELECT @pmwrk_task_id = pmwrk_task_id FROM PMWrk_Task WHERE code = @pmwrk_task_code

    -- Re-runnable check.
    IF EXISTS (SELECT NULL
        FROM PMWrk_Task_Group_Task
        WHERE pmwrk_task_group_id = @pmwrk_task_group_id
        AND pmwrk_task_id = @pmwrk_task_id) BEGIN
        RETURN
    END

    -- Generate the display sequence number.
    DECLARE @display_sequence_num integer
    SELECT @display_sequence_num = ISNULL(MAX(display_sequence_num), 0) + 1 FROM PMWrk_Task_Group_Task
        WHERE pmwrk_task_group_id = @pmwrk_task_group_id
        AND pmwrk_task_id = @pmwrk_task_id

    -- Insert the row.
    INSERT INTO PMWrk_Task_Group_Task (
        pmwrk_task_group_id,
        pmwrk_task_id,
        display_sequence_num
    ) VALUES (
        @pmwrk_task_group_id,
        @pmwrk_task_id,
        @display_sequence_num
    )
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

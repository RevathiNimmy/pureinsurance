SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_PMUser_Group_Activity_add'
GO

-- This procedure is intended for adding data in upgrade scripts, not for calling from normal code.
CREATE PROCEDURE spu_SAM_PMUser_Group_Activity_add
    @pmuser_group_code char(10),
    @pmwrk_task_group_code char(10)
AS BEGIN
    SET NOCOUNT ON

    -- Generate the user group ID.
    DECLARE @pmuser_group_id integer
    SELECT @pmuser_group_id = pmuser_group_id FROM PMUser_Group WHERE code = @pmuser_group_code

    -- Generate the task group ID.
    DECLARE @pmwrk_task_group_id integer
    SELECT @pmwrk_task_group_id = pmwrk_task_group_id FROM PMWrk_Task_Group WHERE code = @pmwrk_task_group_code

    -- Re-runnable check.
    IF EXISTS (SELECT NULL
        FROM PMUser_Group_Activity
        WHERE pmuser_group_id = @pmuser_group_id
        AND pmwrk_task_group_id = @pmwrk_task_group_id) BEGIN
        RETURN
    END

    -- Generate the display sequence number.
    DECLARE @display_sequence_num integer
    SELECT @display_sequence_num = ISNULL(MAX(display_sequence_num), 0) + 1 FROM PMUser_Group_Activity
        WHERE pmuser_group_id = @pmuser_group_id
        AND pmwrk_task_group_id = @pmwrk_task_group_id

    -- Insert the row.
    INSERT INTO PMUser_Group_Activity (
        pmuser_group_id,
        pmwrk_task_group_id,
        display_sequence_num
    ) VALUES (
        @pmuser_group_id,
        @pmwrk_task_group_id,
        @display_sequence_num
    )
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

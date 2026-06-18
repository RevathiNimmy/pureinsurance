SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_PMWrk_Task_add'
GO

-- This procedure is intended for adding data in upgrade scripts, not for calling from normal code.
CREATE PROCEDURE spu_SAM_PMWrk_Task_add
    @language_id smallint,
    @code char(10),
    @description varchar(255),
    @display_icon integer = 1,
    @type_of_task tinyint = 1,
    @is_available_task tinyint = 0,
    @is_system_task tinyint = 0,
    @is_view_only_task tinyint = 0,
    @pmwrk_task_category_code char(10) = 'SBO'
AS BEGIN
    SET NOCOUNT ON

    -- Re-runnable check.
    IF EXISTS (SELECT NULL FROM PMWrk_Task WHERE code = @code) BEGIN
        RETURN
    END

    -- Generate the caption ID.
    DECLARE @caption_id integer
    EXECUTE spu_pm_caption_id_return @language_id, @description, @caption_id OUTPUT

    -- Generate the task category ID.
    DECLARE @pmwrk_task_category_id integer
    SELECT @pmwrk_task_category_id = pmwrk_task_category_id FROM PMWrk_Task_Category WHERE code = @pmwrk_task_category_code

    -- Insert the row.
    INSERT INTO PMWrk_Task (
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        display_icon,
        type_of_task,
        is_available_task,
        is_system_task,
        is_view_only_task,
        pmwrk_task_category_id
    ) VALUES (
        @caption_id,
        @code,
        @description,
        0,
        GETDATE(),
        @display_icon,
        @type_of_task,
        @is_available_task,
        @is_system_task,
        @is_view_only_task,
        @pmwrk_task_category_id
    )
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

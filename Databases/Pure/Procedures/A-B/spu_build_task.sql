SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_build_task'
GO


CREATE PROCEDURE spu_build_task
    @pmwrk_task_code char(4),
    @pmwrk_task_description varchar(255),
    @pmnav_code char(10),
    @pmwrk_task_group_code char(10),
    @pmwrk_task_group_desc varchar(255),
    @pmuser_group_code char(10)
AS


/* Insert new task, task_group, task_group_group and task_group_activity */

DECLARE @caption_id int
DECLARE @column_id int
DECLARE @pmwrk_task_category_id int
DECLARE @pmnav_process_id int
DECLARE @pmwrk_task_group_id int
DECLARE @pmwrk_task_id int
DECLARE @pmuser_group_id int

/* Update Task */

SELECT @pmnav_process_id = MIN(pmnav_process_id) FROM pmnav_process WHERE code = @pmnav_code

IF NOT EXISTS (SELECT pmwrk_task_id FROM pmwrk_task WHERE code = @pmwrk_task_code)
BEGIN
    EXECUTE spu_pm_caption_id_return 1, @pmwrk_task_description, @caption_id OUTPUT

    SELECT @pmwrk_task_category_id =
        (SELECT pmwrk_task_category_id FROM PMWrk_Task_Category WHERE code = 'NONLICENCE')

    INSERT INTO pmwrk_task
    (caption_id, code, description, is_deleted, effective_date, is_system_task, type_of_task, pmnav_process_id, component_object_name, component_class_name, auto_delete_after_num_days, display_icon, is_view_only_task, linked_object_name, linked_class_name, linked_caption_id, is_available_task, pmwrk_task_category_id )
    VALUES
    (@caption_id, @pmwrk_task_code, @pmwrk_task_description, 0, getdate(), 0, 2, @pmnav_process_id, null, null, 0, 1, 0, null, null, null, 1, @pmwrk_task_category_id )
END

/* Update task_group */

IF NOT EXISTS (SELECT pmwrk_task_group_id FROM PMWrk_Task_Group WHERE code = @pmwrk_task_group_code)
BEGIN
    EXECUTE spu_pm_caption_id_return 1, @pmwrk_task_group_desc, @caption_id OUTPUT

    INSERT INTO pmwrk_task_group (caption_id,code,description,is_deleted,effective_date,display_icon)
        VALUES (@caption_id, @pmwrk_task_group_code, @pmwrk_task_group_desc, 0, getdate(), 5)
END

/* Update task_group_task */

IF EXISTS (SELECT pmwrk_task_group_id FROM PMWrk_Task_Group WHERE code = @pmwrk_task_group_code)
BEGIN
    SELECT @pmwrk_task_id =
        (SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = @pmwrk_task_code)
    SELECT @pmwrk_task_group_id =
        (SELECT pmwrk_task_group_id FROM PMWrk_Task_Group WHERE code = @pmwrk_task_group_code)

    IF NOT EXISTS (SELECT pmwrk_task_id FROM pmwrk_task_group_task WHERE pmwrk_task_id = @pmwrk_task_id)
    BEGIN
        INSERT INTO pmwrk_task_group_task
        ( pmwrk_task_group_id, pmwrk_task_id, display_sequence_num )
        VALUES
        ( @pmwrk_task_group_id, @pmwrk_task_id, 0 )
    END
END

/* Update user_group_activity */
IF EXISTS (SELECT pmuser_group_id FROM Pmuser_Group WHERE code = @pmuser_group_code)
BEGIN
    SELECT @pmuser_group_id =
        (SELECT pmuser_group_id FROM PMUser_group WHERE code = @pmuser_group_code)
    SELECT @pmwrk_task_group_id =
        (SELECT pmwrk_task_group_id FROM PMWrk_Task_Group WHERE code = @pmwrk_task_group_code)

    IF NOT EXISTS (SELECT pmuser_group_id FROM pmuser_group_activity WHERE pmwrk_task_group_id = @pmwrk_task_group_id)
    BEGIN
        INSERT INTO pmuser_group_activity
        ( pmuser_group_id, pmwrk_task_group_id, display_sequence_num )
        VALUES
        ( @pmuser_group_id, @pmwrk_task_group_id, 0 )
    END

END
GO



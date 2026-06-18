SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_upd'
GO


CREATE PROCEDURE spu_pmwrk_task_upd
    @pmwrk_task_id INT,
    @caption_id INT,
    @code VARCHAR(10),
    @description VARCHAR(255),
    @is_deleted SMALLINT,
    @effective_date DATETIME,
    @is_system_task SMALLINT,
    @type_of_task SMALLINT,
    @pmnav_process_id INT,
    @component_object_name VARCHAR(30),
    @component_class_name VARCHAR(30),
    @auto_delete_after_num_days INT,
    @display_icon INT,
    @is_view_only_task SMALLINT,
    @linked_object_name VARCHAR(30),
    @linked_class_name VARCHAR(30),
    @linked_caption_id INT,
    @is_available_task SMALLINT,
    @task_category_id INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.1 New column, display_icon 20/09/1999 DAK */
/* 1.2 More new columns 06/10/1999 DAK */
/* 1.3 Add task category 21/12/1999 DAK */
/* 1.4 Ensure parameters same as for add procedure 22/12/1999 DAK */
/********************************************************************************************************/
UPDATE pmwrk_task SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date,
    is_system_task = @is_system_task,
    type_of_task = @type_of_task,
    pmnav_process_id = @pmnav_process_id,
    component_object_name = @component_object_name,
    component_class_name = @component_class_name,
    auto_delete_after_num_days = @auto_delete_after_num_days,
    display_icon = @display_icon,
    is_view_only_task = @is_view_only_task,
    linked_object_name = @linked_object_name,
    linked_class_name = @linked_class_name,
    linked_caption_id = @linked_caption_id,
    is_available_task = @is_available_task,
    pmwrk_task_category_id = @task_category_id
WHERE pmwrk_task_id = @pmwrk_task_id
GO



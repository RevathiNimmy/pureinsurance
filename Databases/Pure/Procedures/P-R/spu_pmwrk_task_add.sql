SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_add'
GO


CREATE PROCEDURE spu_pmwrk_task_add
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
    @linked_caption_id SMALLINT,
    @is_available_task SMALLINT,
    @task_category_id INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 01/12/1998 TOT */
/* 1.1 caption_id changed to INT from SMALLINT 21/12/1998 RFC */
/* 1.2 New column, display_icon 20/09/1999 DAK */
/* 1.3 More new columns 06/10/1999 DAK */
/* 1.4 Add category id 20/12/1999 DAK */
/********************************************************************************************************/
DECLARE @@pmwrk_task_id int

    SELECT @@pmwrk_task_id = max(pmwrk_task_id) FROM PMWrk_Task

    if @@pmwrk_task_id is null

        select @@pmwrk_task_id = 1
    else
        select @@pmwrk_task_id = @@pmwrk_task_id + 1

    insert into PMWrk_Task(pmwrk_task_id,
                caption_id,
                code,
                description,
                is_deleted,
                effective_date,
                is_system_task,
                type_of_task,
                pmnav_process_id,
                component_object_name,
                component_class_name,
                auto_delete_after_num_days,
                display_icon,
                is_view_only_task,
                linked_object_name,
                linked_class_name,
                linked_caption_id,
                is_available_task,
                pmwrk_task_category_id)
    values (@@pmwrk_task_id,
        @caption_id,
        @code,
        @description,
        @is_deleted,
        @effective_date,
        @is_system_task,
        @type_of_task,
        @pmnav_process_id,
        @component_object_name,
        @component_class_name,
        @auto_delete_after_num_days,
        @display_icon,
        @is_view_only_task,
        @linked_object_name,
        @linked_class_name,
        @linked_caption_id,
        @is_available_task,
        @task_category_id)

    SELECT @@pmwrk_task_id
GO



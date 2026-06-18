SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_diary_task_sel'
GO

CREATE PROCEDURE spe_diary_task_sel
    @diary_task_id int
AS
SELECT
    diary_task_id,
    code,
    caption_id,
    description,
    is_deleted,
    effective_date,
    pmuser_group_code,
    pmwrk_task_group_code,
    pmwrk_task_code
 FROM diary_task
WHERE diary_task_id = @diary_task_id

GO


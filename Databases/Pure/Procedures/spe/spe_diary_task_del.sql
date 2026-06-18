SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_diary_task_del'
GO

CREATE PROCEDURE spe_diary_task_del
    @diary_task_id int
AS
DELETE FROM diary_task
WHERE diary_task_id = @diary_task_id

GO


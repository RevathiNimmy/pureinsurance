SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_diary_task_upd'
GO

CREATE PROCEDURE spe_diary_task_upd
    @diary_task_id int,
    @code char(10),
    @caption_id int,
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @pmuser_group_code char(10),
    @pmwrk_task_group_code char(10),
    @pmwrk_task_code char(10)
AS
BEGIN
UPDATE diary_task
    SET
    code=@code,
    caption_id=@caption_id,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    pmuser_group_code=@pmuser_group_code,
    pmwrk_task_group_code=@pmwrk_task_group_code,
    pmwrk_task_code=@pmwrk_task_code
WHERE diary_task_id = @diary_task_id
END

GO


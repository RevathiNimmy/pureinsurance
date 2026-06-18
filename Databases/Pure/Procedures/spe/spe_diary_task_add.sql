SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_diary_task_add'
GO

CREATE PROCEDURE spe_diary_task_add
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
INSERT INTO diary_task (
    diary_task_id,
    code,
    caption_id,
    description,
    is_deleted,
    effective_date,
    pmuser_group_code,
    pmwrk_task_group_code,
    pmwrk_task_code)
VALUES (
    @diary_task_id,
    @code,
    @caption_id,
    @description,
    @is_deleted,
    @effective_date,
    @pmuser_group_code,
    @pmwrk_task_group_code,
    @pmwrk_task_code)
END

GO


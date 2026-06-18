SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_tasks'
GO


CREATE PROCEDURE spu_pmuser_group_tasks
    @pmuser_group_id INT,
    @effective_date DATETIME
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.1 Add task group category foriegn key 08/10/1999 DAK */
/* 1.2 Remove task group category foriegn key 21/12/1999 DAK */
/********************************************************************************************************/
select tg.pmwrk_task_group_id id,
    tg.caption_id caption_id,
    tg.code code,
    tg.description description,
    tg.is_deleted is_deleted,
    tg.effective_date effective_date,
    1 included,
    uga.display_sequence_num display_sequence  
    from PMWrk_Task_Group tg,
    PMUser_Group_Activity uga
where tg.pmwrk_task_group_id = uga.pmwrk_task_group_id
and uga.pmuser_group_id = @pmuser_group_id
and tg.is_deleted = 0
and tg.effective_date <= @effective_date
union
select tg.pmwrk_task_group_id id,
    tg.caption_id caption_id,
    tg.code code,
    tg.description description,
    tg.is_deleted is_deleted,
    tg.effective_date effective_date,
    0 included,
    0 display_sequence 
    from PMWrk_Task_group tg 
where tg.pmwrk_task_group_id not in (select uga.pmwrk_task_group_id
            from PMUser_Group_Activity uga
            where uga.pmuser_group_id = @pmuser_group_id)
and tg.is_deleted = 0
and tg.effective_date <= @effective_date
order by code
GO



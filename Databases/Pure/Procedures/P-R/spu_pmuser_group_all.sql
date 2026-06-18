SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_all'
GO


CREATE PROCEDURE spu_pmuser_group_all
    @pmuser_group_id INT,
    @effective_date DATETIME
AS

/********************************************************************************************************//* Revision Description of Modification Date Who */
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 20/08/1997 JW */
/* 1.1 Added Is Supervisor and Is Sys Admin Group flags 20/11/1997 Tom */
/********************************************************************************************************/
select "user" user_or_group,
    u.user_id id,
    0 caption_id,
    u.username code,
    u.username description,
    u.is_deleted is_deleted,
    u.effective_date effective_date,
    0 included,
    ugu.is_supervisor is_supervisor,
    0 is_sys_admin_group
    from PMUser u,
    PMUser_Group_User ugu
where u.user_id = ugu.user_id
and ugu.pmuser_group_id = @pmuser_group_id
and u.is_deleted = 0
and u.effective_date <= @effective_date
union
select "group" user_or_group,
    ug.pmuser_group_id id,
    ug.caption_id caption_id,
    ug.code code,
    ug.description description,
    ug.is_deleted is_deleted,
    ug.effective_date effective_date,
    0 included,
    0 is_supervisor,
    ug.is_sys_admin_group is_sys_admin_group
    from PMUser_Group ug,
    PMUser_Group_Group ugg
where ug.pmuser_group_id = ugg.pmuser_member_group_id
and ugg.pmuser_group_id = @pmuser_group_id
and ug.is_deleted = 0
and ug.effective_date <= @effective_date
order by code
GO



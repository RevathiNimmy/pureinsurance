SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_sysadmins'
GO


CREATE PROCEDURE spu_pmuser_group_sysadmins
    @effective_date DATETIME
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 23/11/1997 Tom */
/********************************************************************************************************/
select count ('x') how_many
    from PMUser_Group ug
where ug.is_deleted = 0
and ug.effective_date <= @effective_date
and ug.is_sys_admin_group = 1
GO



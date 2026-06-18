SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_add'
GO


CREATE PROCEDURE spu_pmuser_group_add
    @caption_id INTEGER,
    @code VARCHAR(10),
    @description VARCHAR(255),
    @is_deleted INTEGER,
    @effective_date DATETIME,
    @is_sys_admin_group INT
AS

/********************************************************************************************************//* Revision Description of Modification Date Who */
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 20/08/1997 JW */
/* 1.1 Added Is Sys Admin Group flag 20/11/1997 Tom */
/********************************************************************************************************/
DECLARE @pmuser_group_id int

    insert into PMUser_Group(
                caption_id,
                code,
                description,
                is_deleted,
                effective_date,
                is_sys_admin_group)
    values (
        @caption_id,
        @code,
        @description,
        @is_deleted,
        @effective_date,
        @is_sys_admin_group)

    SELECT @@IDENTITY
GO



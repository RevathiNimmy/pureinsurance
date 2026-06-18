SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_upd'
GO


CREATE PROCEDURE spu_pmuser_group_upd
    @pmuser_group_id INT,
    @caption_id INT,
    @code VARCHAR(10),
    @description VARCHAR(255),
    @is_deleted SMALLINT,
    @effective_date DATETIME,
    @is_sys_admin_group INT
AS

/********************************************************************************************************//* Revision Description of Modification Date Who */
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 20/08/1997 JW */
/* 1.1 Added Is Sys Admin Group flag 20/11/1997 TO */
/********************************************************************************************************/
UPDATE pmuser_group SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date,
    is_sys_admin_group = @is_sys_admin_group
    WHERE pmuser_group_id = @pmuser_group_id
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_users_quick_start_saa'
GO


CREATE PROCEDURE spu_pmwrk_users_quick_start_saa
    @user_id integer
AS

/********************************************************************************************************/
/* sp_pmwrk_users_quick_start_saa selects the Quick Start Tasks for a User. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 23/11/1998 RFC */
/********************************************************************************************************/
BEGIN
    SELECT
        pmwrk_task_group_id,
        pmwrk_task_id
    FROM pmwrk_user_quick_start WITH (NOLOCK)
    WHERE user_id = @user_id
    ORDER BY
        display_sequence_num ASC

END
GO



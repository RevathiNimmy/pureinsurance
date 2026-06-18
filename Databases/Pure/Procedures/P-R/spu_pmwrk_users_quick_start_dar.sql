SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_users_quick_start_dar'
GO


CREATE PROCEDURE spu_pmwrk_users_quick_start_dar
    @user_id integer
AS

/********************************************************************************************************/
/* sp_pmwrk_users_quick_start_dar Deletes All Quick Start Tasks for a User. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 23/11/1998 RFC */
/********************************************************************************************************/
BEGIN
    DELETE
    FROM pmwrk_user_quick_start
    WHERE user_id = @user_id
END
GO



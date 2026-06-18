SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_supervises_groups'
GO


CREATE PROCEDURE spu_pmuser_supervises_groups
    @user_id smallint,
    @effective_date datetime
AS

/********************************************************************************************************/
/* sp_pmuser_supervises_groups returns the Groups that a User Supervises. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 25/11/1998 RFC */
/********************************************************************************************************/
BEGIN
    SELECT ug.pmuser_group_id,
        ug.code
    FROM PMUser_Group_User ugu,
        PMUser_Group ug
    WHERE ugu.user_id = @user_id
    AND ugu.is_supervisor = 1
    AND ugu.pmuser_group_id = ug.pmuser_group_id
    AND ug.effective_date < @effective_date
    AND ug.is_deleted = 0

END
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_is_supervisor'
GO


CREATE PROCEDURE spu_pmuser_is_supervisor
    @user_id smallint,
    @pmuser_group_id integer
AS

/********************************************************************************************************/
/* sp_pmuser_is_supervisor returns supervisor status of the user in the group */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 24/11/1998 Tom */
/********************************************************************************************************/
BEGIN
    SELECT ugu.is_supervisor
    FROM PMUser_Group_User ugu
    WHERE ugu.user_id = @user_id
    AND ugu.pmuser_group_id = @pmuser_group_id

END
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_delete_pmlock'
GO


CREATE PROCEDURE spu_delete_pmlock
    @key_name char(30),
    @key_value int,
    @user_id smallint,
    @key2_value int = 0
AS

/******************************************************************************************/
/* sp_delete_pmlock deletes a lock record in the PMLock table */
/* for the keyname, keyvalue and current user id. */
/* 3 parameters are passed in - @key_name, @key_value, @user_id */
/* no parameters are returned */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 30/06/1997 TF */
/******************************************************************************************/
BEGIN
    /* Delete record from PMLock */
    DELETE FROM PMLock
        WHERE lock_name = @key_name
        AND lock_value = @key_value
        AND locked_by_id = @user_id
        AND lock2_value = @key2_value
	And Is_system_lock = 0
END
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_delete_pmlock_key'
GO


CREATE PROCEDURE spu_delete_pmlock_key
    @key_name char(30),
    @key_value int,
    @key2_value int = 0,
    @delete_System_lock int = 0,
	@bIsExclusiveLock int = 0
AS

/******************************************************************************************/
/* sp_delete_pmlock_key deletes all lock records in the PMLock table */
/* for the given keyname and keyvalue, regardless of user id. */
/* 2 parameters are passed in - @key_name, @key_value */
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
        AND lock2_value = @key2_value
		AND ISNULL(Is_system_lock,0) = @delete_System_lock
		AND ISNULL(is_exclusive_lock,0) = ISNULL(@bIsExclusiveLock,0)

END
GO



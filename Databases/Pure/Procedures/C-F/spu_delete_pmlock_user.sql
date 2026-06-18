SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_delete_pmlock_user'
GO


CREATE PROCEDURE spu_delete_pmlock_user
    @user_id smallint,
	@bIsExclusiveLock tinyint = 0,
	@sLock3Value varchar(250) = ''
AS

/******************************************************************************************/
/* sp_delete_pmlock_user deletes all lock records in the PMLock table */
/* for the current user id. */
/* 1 parameter is passed in - @user_id */
/* no parameters are returned */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 30/06/1997 TF */
/******************************************************************************************/
BEGIN
    /* Delete records from PMLock */
    DELETE FROM PMLock
        WHERE locked_by_id = @user_id
		AND Is_system_lock = 0
		AND is_exclusive_lock = @bIsExclusiveLock
		AND UPPER(RTRIM(LTRIM(ISNULL(lock3_value,'')))) = UPPER(RTRIM(LTRIM(ISNULL(@sLock3Value,''))))

END
GO



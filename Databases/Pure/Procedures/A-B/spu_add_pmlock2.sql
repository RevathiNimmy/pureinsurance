SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_add_pmlock2'
GO


CREATE PROCEDURE spu_add_pmlock2
	@locked_by varchar(255) OUTPUT,
	@locked_by_id Int = Null OUTPUT, 
    @key_name char(30),
    @key_value int,
	@user_id smallint,
	@is_system_lock tinyint = 0,
	@bIsExclusiveLock tinyint = 0,
	@sLock3Value varchar(250) = ''
AS

/******************************************************************************************/
/* sp_add_pmlock creates a lock record for the keyname and keyvalue in the PMLock table */
/* and stores the current user name against it. */
/* 3 parameters are passed in - @key_name, @key_value, @user_id */
/* 1 parameter is returned - @locked_by */
/* If the lock already exists the procedure will fail on a primary key constraint and */
/* the id of the user who holds the lock will be returned in @locked_by. */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 30/06/1997 TF */
/* 1.1 Does not use PK Constraint Error to check. 17/04/1998 RFC */
/******************************************************************************************/
BEGIN
    /* Set return user name to NULL */
    /* This will represent a successful query execution */
    SELECT @locked_by = NULL
	DECLARE @enumEnableExclusiveLocking AS INT = 5174
	DECLARE @enumRemoveExclusiveLockAfter AS INT = 5175
	DECLARE @nOptionValueEnableExclusiveLocking AS INT = 0
	DECLARE @nOptionValueRemoveExclusiveLockAfter AS INT = 0
	DECLARE @dtLockedTime AS DateTime 	
	DECLARE @bIsSystemLock AS TINYINT = 0

	select Top 1 @nOptionValueEnableExclusiveLocking= value from  System_Options where option_number = @enumEnableExclusiveLocking
	select Top 1 @nOptionValueRemoveExclusiveLockAfter= value from  System_Options where option_number = @enumRemoveExclusiveLockAfter

	IF ISNULL(@nOptionValueEnableExclusiveLocking,0) = 1 AND ISNULL(@nOptionValueRemoveExclusiveLockAfter,0) > 0
	BEGIN
		SELECT @dtLockedTime = locked_time,@bIsSystemLock = Is_system_lock  FROM PMLock WHERE lock_name = @key_name AND lock_value = @key_value

		IF DATEADD(minute,@nOptionValueRemoveExclusiveLockAfter,@dtLockedTime) < GETDATE() AND ISNULL(@bIsSystemLock,0) = 0
		BEGIN
			Delete FROM PMLock WHERE  lock_name = @key_name AND lock_value = @key_value
		END
		--If the Record is Locked by Same user then allow to Proceed
		IF EXISTS(SELECT 1 FROM PMLock WHERE lock_name = @key_name AND lock_value = @key_value AND locked_by_id = @user_id)
			RETURN
	END
    /* Add record to PMLock */
    INSERT INTO PMLock
        (lock_name,
        lock_value,
        locked_by_id,
        locked_time,
	is_system_lock,
		is_exclusive_lock,
		lock3_value)
    SELECT
        @key_name,
        @key_value,
        @user_id,
        GETDATE(),
	@is_system_lock,
		@bIsExclusiveLock,
		@sLock3Value
    WHERE
        NOT EXISTS
            (SELECT *
             FROM PMLock
             WHERE lock_name = @key_name
             AND lock_value = @key_value)

    /* If we did not add the lock */
    IF @@ROWCOUNT < 1
    BEGIN
        SELECT @locked_by = u.username,@locked_by_id =u.user_id 
        FROM PMLock l JOIN PMUser u ON l.locked_by_id = u.user_id
        WHERE l.lock_name = @key_name
        AND l.lock_value = @key_value 
    END

END
GO

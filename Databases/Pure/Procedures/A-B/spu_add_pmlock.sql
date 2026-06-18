SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_add_pmlock'
GO


CREATE PROCEDURE spu_add_pmlock
    @locked_by varchar(255) OUTPUT,
    @locked_by_id Int = Null OUTPUT,   
    @key_name char(30),
    @key_value int,
    @user_id smallint,
    @key2_value int = 0,
    @is_system_lock tinyint = 0
AS
BEGIN
    /* Set return user name to NULL */
    /* This will represent a successful query execution */
    SELECT @locked_by = NULL

    /* Add record to PMLock */
    INSERT INTO PMLock
        (lock_name,
        lock_value,
        locked_by_id,
        locked_time,
        lock2_value,
	is_system_lock)
    SELECT
        @key_name,
        @key_value,
        @user_id,
        GETDATE(),
        @key2_value,
	@is_system_lock
    WHERE
        NOT EXISTS
            (SELECT *
             FROM PMLock
             WHERE lock_name = @key_name
             AND lock_value = @key_value 
             AND lock2_value = @key2_value
	     AND is_system_lock =  1)

    /* If we did not add the lock */
    IF @@ROWCOUNT < 1
    BEGIN
		/* No row inserted, verify if a is_system_lock record exist or not, can not be unlocked by same user as well*/
		If Exists(SELECT * FROM PMLock WHERE lock_name = @key_name AND lock_value = @key_value AND lock2_value = @key2_value AND is_system_lock =  1)
		BEGIN
			select @user_id = 0
		END
        /* Select name of another user who has record locked */
       SELECT @locked_by = username,@locked_by_id =PMUser.user_id  
        FROM PMLock, PMUser
        WHERE lock_name = @key_name
            AND lock_value = @key_value
            AND lock2_value = @key2_value
            AND locked_by_id <> @user_id
            AND PMLock.locked_by_id = PMUser.user_id

        /* A user name in the return parameter will */
        /* confirm that a lock already exists. */

    END

END
GO




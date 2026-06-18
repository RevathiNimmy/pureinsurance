

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAN_User_Set_Attempts_and_Lock'
GO
-- Track the number of user authentication failures.
CREATE PROCEDURE spu_SAN_User_Set_Attempts_and_Lock
    @action tinyint,
    @user_id int,
    @incorrect_attempts_allowed int,
    @is_locked tinyint OUTPUT
AS BEGIN
    SET NOCOUNT ON

    -- This sproc must behave correctly even if other connections are checking the user
    -- at the same time. Accordingly, the table is modified atomically first, then the
    -- new state is read back. This should prevent race conditions.

    If @action = 1 BEGIN
        -- Reset the failure counter.
        UPDATE PMUser
        SET incorrect_attempt_count = 0
        WHERE user_id = @user_id
    END ELSE If @action = 2 BEGIN
        -- Increment the failure counter.
        UPDATE PMUser
        SET incorrect_attempt_count = isnull(incorrect_attempt_count,0) + 1
        WHERE user_id = @user_id
        -- Disable the user if failure counter has exceeded the limit.
        -- Do not re-enable the user here, that's a manual action to be performed elsewhere.
        UPDATE PMUser
        SET is_locked = 1
        WHERE user_id = @user_id
        AND isnull(incorrect_attempt_count,0) >= @incorrect_attempts_allowed
    END

    -- Return the current enabled state.
    SELECT @is_locked = is_locked
    FROM PMUser
    WHERE user_id = @user_id
END
GO

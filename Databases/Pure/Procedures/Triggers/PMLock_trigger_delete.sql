DDLDropTrigger 'PMLock_trd'
GO

CREATE TRIGGER PMLock_trd
ON PMLock
FOR DELETE AS
BEGIN

SET NOCOUNT ON

    UPDATE PMLock_Last_Unlock
    SET lock_name = PMLock_Last_Unlock.lock_name,
        lock_value = PMLock_Last_Unlock.lock_value
FROM PMLock_Last_Unlock
INNER JOIN deleted ON PMLock_Last_Unlock.lock_name = deleted.lock_name AND PMLock_Last_Unlock.lock_value = deleted.lock_value

END
GO

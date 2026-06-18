DDLDropProcedure 'spu_pmlock_last_unlock_sel'
GO

CREATE PROCEDURE spu_pmlock_last_unlock_sel
@lock_name char(30),
@lock_value int
AS
BEGIN
    DECLARE @ts timestamp
    DECLARE @locked_by varchar(255)
    DECLARE @locked_by_id integer

SELECT  @ts = tstamp  from pmlock_last_unlock where lock_name = @lock_name and lock_value = @lock_value
    
    IF @ts IS NULL
    BEGIN
        SET NOCOUNT ON
        INSERT INTO  pmlock_last_unlock (lock_name, lock_value) values (@lock_name, @lock_value)
        SET NOCOUNT OFF
        SELECT @ts = tstamp from pmlock_last_unlock where lock_name = @lock_name and lock_value = @lock_value
    END

    SELECT @locked_by = username,
           @locked_by_id = locked_by_id
    FROM   PMlock
    INNER JOIN PMUser on PMLock.locked_by_id = PMUser.user_id
    WHERE  lock_name = @lock_name
      AND  lock_value = @lock_value

    SELECT @ts as tstamp,
           @locked_by as currently_locked_by,
           @locked_by_id as currently_locked_by_id

END
GO

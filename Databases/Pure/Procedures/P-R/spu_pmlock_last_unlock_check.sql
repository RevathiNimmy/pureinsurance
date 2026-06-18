DDLDropProcedure 'spu_pmlock_last_unlock_check'
GO

CREATE PROCEDURE spu_pmlock_last_unlock_check
    @lock_name char(30),
    @lock_value int,
    @tstamp timestamp,
    @tstamp_matches as tinyint OUTPUT,
    @is_system_lock tinyint = 0
AS

BEGIN
    if @tstamp = (select tstamp from pmlock_last_unlock where lock_name = @lock_name and lock_value = @lock_value and ISNULL(is_system_lock,0) = @is_system_lock)
                 select @tstamp_matches = 1
    else
       select @tstamp_matches = 0
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Delete_Allocation_Locks'
GO

CREATE PROCEDURE spu_ACT_Delete_Allocation_Locks
    @user_id INT,
    @transdetail_id INT
AS
BEGIN

    DELETE FROM pmlock
    WHERE lock_name = 'ALLOCATION'
    AND locked_by_id = @user_id
    AND lock_value = @transdetail_id
END
GO
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_pmwrk_check_is_supervisor'
GO

CREATE PROCEDURE spu_pmwrk_check_is_supervisor
	@UserID SMALLINT,
	@UserGroupID INTEGER,
	@IsSupervisor SMALLINT OUTPUT

AS

BEGIN

    SELECT @IsSupervisor = is_supervisor 
    FROM pmuser_group_user 
    WHERE [user_id] = @UserID 
      AND pmuser_group_id = @UserGroupID

END
GO


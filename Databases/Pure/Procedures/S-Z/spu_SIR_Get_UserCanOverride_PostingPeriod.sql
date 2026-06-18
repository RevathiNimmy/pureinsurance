SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_SIR_Get_UserCanOverride_PostingPeriod'
GO

CREATE PROCEDURE spu_SIR_Get_UserCanOverride_PostingPeriod
	@user_id  integer,
	@can_override tinyint OUTPUT
AS

	SELECT @can_override = can_override_posting_period FROM User_Authorities WHERE USER_ID = @user_id

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_SIR_Get_UserCanChange_Reserves'
GO

CREATE PROCEDURE spu_SIR_Get_UserCanChange_Reserves
		@user_id  integer,
		@can_change_reserves tinyint OUTPUT
AS

		SELECT @can_change_reserves = ISNULL(can_change_reserves_on_claim_payments, 0) FROM User_Authorities WHERE USER_ID = @user_id
GO

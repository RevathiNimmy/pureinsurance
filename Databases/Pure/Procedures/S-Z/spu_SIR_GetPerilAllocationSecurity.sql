SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_GetPerilAllocationSecurity'
GO

CREATE PROCEDURE spu_SIR_GetPerilAllocationSecurity
	@risk_cnt INT,
	@user_id SMALLINT,
	@user_allow_ratingsection_adddelete TINYINT OUTPUT,
	@user_allow_ratingsection_editing TINYINT OUTPUT,
	@allow_add_ratingsection TINYINT OUTPUT,
	@allow_edit_ratingsection TINYINT OUTPUT,
	@allow_delete_ratingsection TINYINT OUTPUT,
	@allow_edit_ratingsection_ratetype TINYINT OUTPUT,
	@allow_edit_ratingsection_rate TINYINT OUTPUT,
	@allow_edit_ratingsection_suminsured TINYINT OUTPUT,
	@allow_edit_ratingsection_thispremium TINYINT OUTPUT
AS
	SELECT 	@user_allow_ratingsection_adddelete = allow_ratingsection_adddelete,
		@user_allow_ratingsection_editing = allow_ratingsection_editing
	FROM	User_Authorities
	WHERE	user_id = @user_id

	SELECT 	@allow_add_ratingsection = RT.allow_add_ratingsection,
		@allow_edit_ratingsection = RT.allow_edit_ratingsection,
		@allow_delete_ratingsection = RT.allow_delete_ratingsection,
		@allow_edit_ratingsection_ratetype = RT.allow_edit_ratingsection_ratetype,
		@allow_edit_ratingsection_rate = RT.allow_edit_ratingsection_rate,
		@allow_edit_ratingsection_suminsured = RT.allow_edit_ratingsection_suminsured,
		@allow_edit_ratingsection_thispremium = RT.allow_edit_ratingsection_thispremium
	FROM	Risk_Type RT
	INNER JOIN Risk R ON R.risk_type_id = RT.risk_type_id AND R.risk_cnt = @risk_cnt


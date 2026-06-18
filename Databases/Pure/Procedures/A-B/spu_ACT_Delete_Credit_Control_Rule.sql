SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Delete_Credit_Control_Rule'
GO

CREATE PROCEDURE spu_ACT_Delete_Credit_Control_Rule
    @credit_control_rule_id INT,
	@user_id INT,
	@unique_id VARCHAR(50),
	@screen_hierarchy VARCHAR(500)
AS

BEGIN
    UPDATE  Credit_Control_Rule
			set ScreenHierarchy = @screen_hierarchy,
				UserId = @user_id,
				UniqueId = @unique_id
				WHERE credit_control_rule_id = @credit_control_rule_id

    DELETE
      FROM Credit_Control_Rule
     WHERE credit_control_rule_id = @credit_control_rule_id

END
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Delete_Credit_Control_Step'
GO

CREATE PROCEDURE spu_ACT_Delete_Credit_Control_Step
    @credit_control_step_id INT,
	@nResult INT = 0 OUT,
	@user_id INT,
	@unique_id VARCHAR(50),
	@screen_hierarchy VARCHAR(500)
AS BEGIN

       IF NOT EXISTS(SELECT 1
			      FROM Credit_Control_Item 
			      WHERE credit_control_step_id = @credit_control_step_id)   
       BEGIN

	       Update Credit_Control_Step 
		   set UserId = @user_id,
			   UniqueId = @unique_id,
			   ScreenHierarchy = @screen_hierarchy
			   WHERE credit_control_step_id = @credit_control_step_id

	       DELETE
             FROM Credit_Control_Step
	        WHERE credit_control_step_id = @credit_control_step_id

	       SET @nResult = 1

	   END
   END
GO



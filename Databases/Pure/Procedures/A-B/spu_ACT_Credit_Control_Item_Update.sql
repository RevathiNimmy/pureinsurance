SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Credit_Control_Item_Update'
GO

CREATE PROCEDURE spu_ACT_Credit_Control_Item_Update 

@credit_control_item_id int,
@credit_control_reason varchar(50), 
@credit_control_step_id int, 
@due_date datetime

AS


BEGIN

	UPDATE credit_control_item WITH (ROWLOCK)
	SET 
		credit_control_step_id = @credit_control_step_id, 
		credit_control_reason = @credit_control_reason, 
		due_date = @due_date
	WHERE credit_control_item_id =@credit_control_item_id

END



GO

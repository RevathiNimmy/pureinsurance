SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Credit_Control_Rule_Insurance_File_Status_Delete'
GO

CREATE PROCEDURE spu_ACT_Credit_Control_Rule_Insurance_File_Status_Delete
@credit_control_rule_id INT,
@UserId INT,
@UniqueId VARCHAR(50),
@ScreenHierarchy VARCHAR(500)

AS
UPDATE credit_control_rule_insurance_file_status SET UserId=@UserId,UniqueId=@UniqueId,ScreenHierarchy=@ScreenHierarchy
	WHERE credit_control_rule_id = @credit_control_rule_id

DELETE FROM credit_control_rule_insurance_file_status 
WHERE credit_control_rule_id = @credit_control_rule_id

GO

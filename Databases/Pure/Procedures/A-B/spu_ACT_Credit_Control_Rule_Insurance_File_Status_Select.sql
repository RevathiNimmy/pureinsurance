SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Credit_Control_Rule_Insurance_File_Status_Select'
GO

CREATE PROCEDURE spu_ACT_Credit_Control_Rule_Insurance_File_Status_Select  
  
@credit_control_rule_id int
  
AS  
  
SELECT Insurance_File_Status.Insurance_File_Status_id, Code, Description 

FROM credit_control_rule_insurance_file_status 

	INNER JOIN Insurance_File_Status ON 
		credit_control_rule_insurance_file_status.insurance_file_status_id =
			 insurance_file_Status.insurance_file_status_id

WHERE

credit_control_rule_id = @credit_control_rule_id
 

  


GO

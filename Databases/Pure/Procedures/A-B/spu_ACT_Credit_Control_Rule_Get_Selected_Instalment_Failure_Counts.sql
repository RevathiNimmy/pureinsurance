SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Credit_Control_Rule_Get_Selected_Instalment_Failure_Counts'
GO

CREATE PROCEDURE spu_ACT_Credit_Control_Rule_Get_Selected_Instalment_Failure_Counts 

@credit_control_rule_id int

AS

SELECT instalment_failure_count

FROM credit_control_step 

WHERE credit_control_rule_id = @credit_control_rule_id  

ORDER BY instalment_failure_count





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Set_Policy_True_Monthly_Policy_Status'
GO

CREATE PROCEDURE spu_SAM_Set_Policy_True_Monthly_Policy_Status

@insurance_file_cnt int

AS

UPDATE insurance_file 
SET anniversary_copy = is_true_monthly_policy 
FROM insurance_file
	INNER JOIN product ON 
		product.product_id = insurance_file.product_id

WHERE insurance_file_cnt = @insurance_file_cnt


GO

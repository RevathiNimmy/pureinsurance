SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Get_Selected_Risk_Count'
GO



CREATE PROCEDURE spu_SIR_Policy_Discount_Get_Selected_Risk_Count

@insurance_file_cnt int 


AS

BEGIN

	SELECT Count(*)
	FROM risk r
		INNER JOIN insurance_file_risk_link ifrl ON
			ifrl.risk_cnt = r.risk_cnt
		
	WHERE ifrl.insurance_file_cnt =@insurance_file_cnt
	AND r.is_risk_selected = 1

END 



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

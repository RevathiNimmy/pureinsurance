SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_Discount_Update_Risk_Details'
GO

CREATE PROCEDURE spu_SIR_Policy_Discount_Update_Risk_Details

@insurance_file_cnt int, 
@is_discounted int

AS 

BEGIN
	
	UPDATE risk 
	SET is_discounted = @is_discounted 
	WHERE risk_cnt in (SELECT ifrl.risk_cnt 
			   FROM insurance_file_risk_link ifrl
				INNER JOIN risk r ON
					ifrl.risk_cnt = r.risk_cnt
			   WHERE insurance_file_cnt  = @insurance_file_cnt
		           AND r.is_risk_selected = 1
			   AND r.risk_status_id in (
					SELECT risk_status_id 
					FROM POLICY_DISCOUNT_VALID_RISK_STATUSES))

END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

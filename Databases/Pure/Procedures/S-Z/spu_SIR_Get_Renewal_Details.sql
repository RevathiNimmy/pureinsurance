SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Renewal_Details'
GO

CREATE PROCEDURE spu_SIR_Get_Renewal_Details

@insurance_file_cnt int

AS

BEGIN

	DECLARE @transaction_type_id int

	SELECT @transaction_Type_id = transaction_type_id FROM transaction_type
	WHERE code = 'REN'

	SELECT product_id,r.risk_cnt, @transaction_Type_id as transaction_type FROM insurance_file ifile
	
		INNER JOIN insurance_file_risk_link ifrl ON
			ifrl.insurance_file_cnt = ifile.insurance_file_cnt
		
			INNER JOIN (SELECT risk_cnt FROM risk where is_risk_selected = 1) r ON
				r.risk_cnt = ifrl.risk_cnt

	WHERE ifile.insurance_file_cnt = @insurance_file_cnt
	
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

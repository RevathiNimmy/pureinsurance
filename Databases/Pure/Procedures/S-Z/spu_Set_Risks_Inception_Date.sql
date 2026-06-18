SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Set_Risks_Inception_Date'
GO

CREATE PROCEDURE spu_Set_Risks_Inception_Date  
	@insurance_file_cnt INT,
	@inception_date DATETIME

AS  

	UPDATE risk SET inception_date = @inception_date 
		From risk r
		Inner Join insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt
	WHERE ifrl.insurance_file_cnt = @insurance_file_cnt

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

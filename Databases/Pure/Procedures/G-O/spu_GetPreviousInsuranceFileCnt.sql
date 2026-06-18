
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetPreviousInsuranceFileCnt'
GO

CREATE PROCEDURE spu_GetPreviousInsuranceFileCnt
	
	@new_insurance_file_cnt INT 
	

AS

SELECT irl.insurance_file_cnt FROM insurance_file_risk_link irl
WHERE irl.risk_cnt = (SELECT TOP 1 original_risk_cnt FROM insurance_file_risk_link ifrl
		     WHERE insurance_file_cnt=@new_insurance_file_cnt)



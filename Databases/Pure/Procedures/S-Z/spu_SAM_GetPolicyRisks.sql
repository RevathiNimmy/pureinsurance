SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_SAM_GetPolicyRisks'
GO

CREATE PROCEDURE spu_SAM_GetPolicyRisks  
    @insurance_file_cnt int,
    @risk_cnt INT OUTPUT 
    
AS  

	IF (SELECT COUNT(*)
		FROM    insurance_file_risk_link irl1  
		JOIN    risk rsk                                     ON irl1.risk_cnt = rsk.risk_cnt  
		WHERE irl1.insurance_file_cnt = @insurance_file_cnt
			AND rsk.is_risk_selected = 1) = 1			
	BEGIN
		SELECT @risk_cnt = rsk.risk_cnt
		FROM    insurance_file_risk_link irl1  
		JOIN    risk rsk                                     ON irl1.risk_cnt = rsk.risk_cnt  
		WHERE irl1.insurance_file_cnt = @insurance_file_cnt
			AND rsk.is_risk_selected = 1
	END	
	ELSE
	BEGIN
		SET @risk_cnt = -1
	END		

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


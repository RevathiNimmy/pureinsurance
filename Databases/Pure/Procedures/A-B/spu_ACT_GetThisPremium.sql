SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_GetThisPremium'
GO

CREATE PROCEDURE spu_ACT_GetThisPremium  
    @nInsuranceFileCnt INT 
    
AS    
DECLARE @nThisPremium AS INT 
DECLARE @nRIRowsToPostCnt AS INT

SELECT @nThisPremium = this_premium FROM Insurance_File
		 WHERE insurance_file_cnt = @nInsuranceFileCnt

SELECT  @nRIRowsToPostCnt=COUNT (*) FROM (
						SELECT riarr.risk_cnt  ,
						riarrl.type, CASE  WHEN riarrl.type IN ('F', 'FX') THEN riarrl.party_cnt ELSE riarrl.treaty_id END AS 'Treaty_or_party_id' ,
						SUM(ROUND(riarrl.premium_value,2)) AS 'Premium'
						FROM insurance_file_risk_link insfrl 
						JOIN risk r ON insfrl.risk_cnt = r.risk_cnt
						JOIN RI_Arrangement riarr ON riarr.risk_cnt = r.risk_cnt
						JOIN RI_Arrangement_line riarrl ON riarrl.ri_arrangement_id = riarr.ri_arrangement_id
						WHERE insfrl.insurance_file_cnt = @nInsuranceFileCnt AND insfrl.status_flag <> 'U'
						GROUP BY riarr.risk_cnt, riarrl.type, CASE  WHEN riarrl.type IN ('F', 'FX') THEN riarrl.party_cnt ELSE riarrl.treaty_id END
						HAVING SUM(ROUND(riarrl.premium_value,2))  >0
					  ) AS TempResultSet

SELECT @nThisPremium, @nRIRowsToPostCnt

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

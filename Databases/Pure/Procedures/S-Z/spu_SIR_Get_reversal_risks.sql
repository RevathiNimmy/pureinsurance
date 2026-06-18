SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Get_reversal_risks'
GO

CREATE PROCEDURE spu_SIR_Get_reversal_risks  
  
 @nBase_insurance_file_cnt INT,  
 @nNew_insurance_file_cnt INT
AS  
  
BEGIN  
  
	SELECT ifrl.risk_cnt, ifrl.status_flag, base.is_risk_edited
		FROM Insurance_File_Risk_Link ifrl WITH (NOLOCK) 
			JOIN risk r WITH (NOLOCK) ON r.risk_cnt =ifrl.risk_cnt 
			LEFT JOIN (SELECT r.risk_folder_cnt,ifrl.is_risk_edited FROM risk r  WITH (NOLOCK) 
							JOIN insurance_file_risk_link ifrl  WITH (NOLOCK) ON ifrl.risk_cnt = r.risk_cnt 
								WHERE ifrl.insurance_file_cnt = @nBase_insurance_file_cnt) base ON r.risk_folder_cnt = base.risk_folder_cnt 
	WHERE insurance_file_cnt = @nNew_insurance_file_cnt 
  
END  

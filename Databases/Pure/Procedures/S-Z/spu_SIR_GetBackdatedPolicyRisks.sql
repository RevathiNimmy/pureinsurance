SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_SIR_GetBackdatedPolicyRisks'
GO

CREATE PROCEDURE spu_SIR_GetBackdatedPolicyRisks
	@insurance_file_cnt int  
AS  
BEGIN  

SELECT 	R.risk_cnt,
		R.description [RiskDescription],
		IFRL.status_flag,
    	r.risk_folder_cnt
FROM Insurance_File iFile1 With (nolock)
JOIN mta_insurance_file_link mtaFile With (nolock) ON mtaFile.new_linked_insurance_file_cnt = iFile1.insurance_file_cnt
JOIN Insurance_File iFile2 With (nolock) ON iFile2.insurance_file_cnt = mtaFile.insurance_file_cnt
JOIN Insurance_File_Risk_Link IFRL With (nolock) ON iFile2.insurance_file_cnt = IFRL.insurance_file_cnt
JOIN Risk R With (nolock) ON R.risk_cnt = IFRL.risk_cnt
	WHERE iFile1.insurance_file_cnt = @insurance_file_cnt
END  

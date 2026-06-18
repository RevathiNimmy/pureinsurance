SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Anniv_PriorVersion_InsuranceFileCnt'
GO
CREATE PROCEDURE spu_SAM_Get_Anniv_PriorVersion_InsuranceFileCnt  
 @nInsurance_file_cnt INT ,
 @nInsurance_folder_count INT,
 @r_nOriginal_Insurance_file_cnt INT OUTPUT
 
 As  
 BEGIN  
	SELECT @r_nOriginal_Insurance_file_cnt = max(insurance_file_cnt) from insurance_file  
	WHERE insurance_folder_cnt = @nInsurance_folder_count And  
	renewal_date =  (SELECT cover_start_date  
						FROM insurance_file  
					 WHERE insurance_file_cnt  = @nInsurance_file_cnt)  
	AND insurance_file_type_id IN (2,5,9) 
 END  
    
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

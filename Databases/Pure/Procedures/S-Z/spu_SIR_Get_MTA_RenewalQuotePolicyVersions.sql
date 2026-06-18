SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Get_MTA_RenewalQuotePolicyVersions'
GO

CREATE PROCEDURE spu_SIR_Get_MTA_RenewalQuotePolicyVersions  
 @insurance_file_cnt INT,  
 @insurance_folder_cnt INT  
  
AS  
  
BEGIN  
 SELECT insurance_file_cnt,insurance_folder_cnt,insurance_file_status_id,t.code    
 FROM insurance_file  IFL    
    JOIN Insurance_file_type T ON IFL.insurance_file_type_id = T.insurance_file_type_id    
 WHERE insurance_folder_cnt = @insurance_folder_cnt    
        AND T.Code in ( 'MTAQUOTE' ,'MTAQTETEMP' ,'RENEWAL','MTAQREINS', 'MTAQCAN')  
  AND insurance_file_cnt > @insurance_file_cnt  
  
END  
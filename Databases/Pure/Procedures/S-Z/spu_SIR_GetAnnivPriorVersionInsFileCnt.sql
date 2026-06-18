EXECUTE DDLDropProcedure 'spu_SIR_GetAnnivPriorVersionInsFileCnt'
GO

CREATE PROCEDURE spu_SIR_GetAnnivPriorVersionInsFileCnt
  
@nInsuranceFileCnt INT, 
@nInsuranceFolderCnt INT
  
AS  
  
BEGIN  
 
       SELECT  max(insurance_file_cnt) from insurance_file  
       WHERE insurance_folder_cnt = @nInsuranceFolderCnt And  
       renewal_date =  (SELECT cover_start_date  
                               FROM insurance_file  
                               WHERE insurance_file_cnt  = @nInsuranceFileCnt)  
       AND insurance_file_type_id IN (2,5,9) 

END
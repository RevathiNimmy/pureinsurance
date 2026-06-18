
EXECUTE DDLDropProcedure 'spu_Update_Lapse_Policyversions_Status'
GO

CREATE PROCEDURE spu_Update_Lapse_Policyversions_Status  
    @insurance_file_cnt INT   
AS  
BEGIN
  
  DECLARE @ninsurance_folder_cnt Integer 
  DECLARE @ninsurance_file_status_id Integer 
    
  SELECT  @ninsurance_folder_cnt=insurance_folder_cnt ,@ninsurance_file_status_id = insurance_file_status_id 
		  FROM Insurance_File WHERE insurance_file_cnt = @insurance_file_cnt  
  
 IF ISNULL(@ninsurance_file_status_id,0)=2
  BEGIN
  
   UPDATE insurance_file  
	  SET insurance_file_status_id = (case when IFT.code='POLICY' then NULL else 4 end) 
		FROM  insurance_file INSF  
		JOIN insurance_file_type IFT ON IFT.insurance_file_type_id=INSF.insurance_file_type_id  
        WHERE INSF.insurance_folder_cnt = @ninsurance_folder_cnt  
			AND IFT.code NOT in('MTAQUOTE','MTAQTETEMP','MTAQREINS','MTAQCAN')   
			AND INSF.insurance_file_cnt <> @insurance_file_cnt  
 END
 
END  
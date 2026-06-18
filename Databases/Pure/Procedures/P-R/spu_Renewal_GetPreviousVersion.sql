/* Created by : Vidya Rangdale
Creation Date : 26/02/2014
Description   : This is used to select insurance_file_cnt  from insurance_file  table
Test Code     : Exec spu_Renewal_GetPreviousVersion    
 */

EXECUTE DDLDropProcedure 'spu_Renewal_GetPreviousVersion'
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE spu_Renewal_GetPreviousVersion    
   @nInsurance_folder_cnt INT,    
   @nInsurance_file_cnt INT    
AS    
IF @nInsurance_folder_cnt=0   
BEGIN  
   SELECT @nInsurance_folder_cnt=insurance_folder_cnt FROM insurance_file     
   WHERE insurance_file_cnt = @nInsurance_file_cnt     
END  
	
	SELECT insurance_file_cnt FROM insurance_file     
	WHERE insurance_folder_cnt=@nInsurance_folder_cnt AND insurance_file_cnt < @nInsurance_file_cnt  


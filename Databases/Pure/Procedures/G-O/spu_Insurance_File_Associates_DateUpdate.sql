EXECUTE DDLDropProcedure 'spu_Insurance_File_Associates_DateUpdate'
GO

CREATE PROCEDURE spu_Insurance_File_Associates_DateUpdate    
   @Insurance_file_cnt INT  
AS    
  
BEGIN  
  
UPDATE  Insurance_file_associates   
 SET   
  date_removed= (select cover_start_date as csd from Insurance_File insfile where insfile.insurance_file_cnt=@Insurance_file_cnt),    
  Is_DelUnConfirmed=0  
 WHERE Insurance_File_cnt = @Insurance_file_cnt   
  AND Is_DelUnConfirmed= 1   
  AND Is_Deleted=1  
    
UPDATE  Insurance_file_associates    
 SET    
    date_attached=(select cover_start_date as csd from Insurance_File insfile where insfile.insurance_file_cnt=@Insurance_file_cnt),    
    is_AddUnConfirmed=0  
 WHERE Insurance_File_cnt = @Insurance_file_cnt   
  AND is_AddUnConfirmed= 1  
 
END  
GO
  

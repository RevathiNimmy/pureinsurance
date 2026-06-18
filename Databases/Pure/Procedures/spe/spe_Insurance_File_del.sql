EXECUTE DDLDropProcedure 'spe_Insurance_File_del'
GO

CREATE PROCEDURE spe_Insurance_File_del
    @insurance_file_cnt int
AS

DELETE FROM insurance_file_clone_log  
 WHERE insurance_file_cnt = @insurance_file_cnt  
  
DELETE FROM insurance_file_pt_log  
 WHERE insurance_file_cnt = @insurance_file_cnt  

DELETE FROM Claim_pt_log
 WHERE insurance_file_cnt = @insurance_file_cnt 
   
DELETE FROM Insurance_file_associates
WHERE insurance_file_cnt = @insurance_file_cnt

DELETE FROM Insurance_File
WHERE insurance_file_cnt = @insurance_file_cnt

GO


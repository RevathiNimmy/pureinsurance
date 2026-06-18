  
EXECUTE DDLDropProcedure 'spu_Update_Renewal_Exception'
GO
  
CREATE PROCEDURE spu_Update_Renewal_Exception(  
    @renewal_insurance_file_cnt INT,  
    @exception_reason_id INT,  
    @exception_note VARCHAR(255))  
AS  
BEGIN  
  
  
 UPDATE Renewal_Status Set renewal_exception_reason_id = @exception_reason_id,  
                           renewal_exception_notes = @exception_note  
 WHERE renewal_insurance_file_cnt = @renewal_insurance_file_cnt  
      
END  

GO
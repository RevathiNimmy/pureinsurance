SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Insurance_File_Update'
GO
CREATE PROCEDURE spu_Insurance_File_Update  
 @PolicyCnt INTEGER,  
 @LapseId INTEGER,  
 @LivePolicy INTEGER,  
 @LapseDesc VARCHAR(255),
 @IsMigratedPolicy BIT = 0 
As  
BEGIN  
DECLARE @lapsed_date DATETIME  
DECLARE @insurance_folder_cnt INTEGER   
DECLARE @insurance_file_status_id integer   
     
If @LivePolicy = 1  
SELECT  @lapsed_date=ifi.expiry_date FROM insurance_file ifi WHERE insurance_file_cnt = @PolicyCnt  
Else  
SELECT @lapsed_date=ifi.renewal_date FROM insurance_file ifi WHERE insurance_file_cnt = @PolicyCnt  
     
SELECT @insurance_folder_cnt = insurance_folder_cnt FROM insurance_file  WHERE insurance_file_cnt = @PolicyCnt       
 
IF @IsMigratedPolicy = 1
UPDATE Insurance_File  
SET insurance_file_type_id = 3  
WHERE insurance_file_cnt = @PolicyCnt    
          
UPDATE insurance_file   
SET  lapsed_reason_id = @LapseId ,  
lapsed_date = @lapsed_date,   
lapsed_description = @LapseDesc,  
insurance_file_status_id = 2   
WHERE insurance_file_cnt = @PolicyCnt   
END  
    
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

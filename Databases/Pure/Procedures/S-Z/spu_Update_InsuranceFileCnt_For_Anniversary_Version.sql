--*****************************************************************
--Created By:- Vidya Rangdale
--Date:- 19/09/2014
--Description:-To update insurance_file  table
--*****************************************************************
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Update_InsuranceFileCnt_For_Anniversary_Version'
GO


CREATE PROCEDURE spu_Update_InsuranceFileCnt_For_Anniversary_Version  
@insurance_ref VARCHAR(30),  
@cover_start_date datetime,  
@insurance_file_cnt INT   
AS  
BEGIN    
    
 UPDATE insurance_file SET insurance_file_status_id=3   
 WHERE insurance_file_cnt=@insurance_file_cnt   
   
 UPDATE Renewal_Status SET insurance_file_cnt=@insurance_file_cnt  
 WHERE renewal_insurance_file_cnt= (SELECT top 1 insurance_file_cnt 
                                     FROM  Insurance_File    
                                     WHERE anniversary_copy =1    
                                     AND cover_start_date >=@cover_start_date    
                                     AND insurance_ref = @insurance_ref)    
    
END 
GO
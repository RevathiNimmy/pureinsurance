SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Is_Accept_TMP_Anniversary_Valid_Action'
GO

CREATE PROCEDURE spu_SIR_Get_Is_Accept_TMP_Anniversary_Valid_Action  
  
@insurance_file_cnt int,  
@Renewal_Date datetime
  
AS  
  
BEGIN  
  
 DECLARE @Insurance_Folder_cnt   int  
  
 SELECT top 1 @Insurance_Folder_cnt = insurance_folder_cnt  
 FROM Insurance_File  
 WHERE insurance_file_cnt  = @insurance_file_cnt  
  
-- check whether a monthly version is in renewal prior to anniversary version---    

 SELECT insurance_file_cnt  
 FROM Insurance_File  ifi  
 WHERE insurance_folder_cnt  = @Insurance_Folder_cnt  
 AND renewal_date=@Renewal_Date AND insurance_file_type_id = 2   
  
END  
  
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
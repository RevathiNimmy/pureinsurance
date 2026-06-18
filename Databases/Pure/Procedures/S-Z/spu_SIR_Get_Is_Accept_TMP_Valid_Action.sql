SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Is_Accept_TMP_Valid_Action'
GO

CREATE PROCEDURE spu_SIR_Get_Is_Accept_TMP_Valid_Action  
  
@insurance_file_cnt int,  
@insurance_ref varchar(30)
 
  
AS  
  
BEGIN  
  
 DECLARE @max_insurance_file_cnt 	int  
--Start(Sriram P)PN55579
 DECLARE @Insurance_Folder_cnt 		int  
--End(Sriram P)PN55579
 
 --Start(Sriram P)PN55579
 SELECT top 1 @Insurance_Folder_cnt = insurance_folder_cnt  
 FROM Insurance_File  
 WHERE insurance_file_cnt  = @insurance_file_cnt  
 --End(Sriram P)PN55579

-- get the latest live version of the insurance file cnt  
 SELECT top 1 @max_insurance_file_cnt = insurance_file_cnt  
 FROM Insurance_File  
--Start(Sriram P)PN55579
 WHERE insurance_folder_cnt  = @Insurance_Folder_cnt  
--End(Sriram P)PN55579
 AND insurance_file_cnt <> @insurance_file_cnt  
 AND (insurance_file_status_id IS NULL OR insurance_file_status_id=3)  
 ORDER BY policy_version desc 
 
  
 -- return the insurance file cnt  
 -- of the last live version of the policy  
 -- that is also the last renewal of a true monthly policy batch  
 SELECT insurance_file_cnt  
 FROM Insurance_file  
 WHERE insurance_file_cnt = @max_insurance_file_cnt  
 AND renewal_date = anniversary_date  

END 
 
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
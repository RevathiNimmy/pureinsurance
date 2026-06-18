SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Copy_Risk_Folder'
GO


/*************************************************************************/  
/* Jai Prakash: copy risk folder record and return Identity column                 */  
/* and generate ID column if required.                                   */  
/*************************************************************************/  
/*************************************************************************/  
/* 1.0  21/05/2010 RFC Original (Based on SP Original)                   */  
/*************************************************************************/  
CREATE PROCEDURE [dbo].[spu_Copy_Risk_Folder] 
    @risk_folder_cnt int,  
    @insurance_file_cnt int,
    @new_risk_folder_cnt int OUTPUT     
AS BEGIN  
  
DECLARE  
     @risk_folder_id int ,  
     @source_id smallint ,  
     @risk_folder_type_id int ,  
     @code varchar(40) ,  
     @description varchar(255) ,  
     @table_name varchar(128),  
     @insurance_folder_cnt int  
 SELECT  
     @risk_folder_cnt =0,  
     @risk_folder_id = 0,  
     @source_id = source_id,  
     @risk_folder_type_id = risk_folder_type_id,  
     @code ='',  
     @description = description  
 FROM Risk_Folder  
 WHERE risk_folder_cnt = @risk_folder_cnt  
  
 SELECT @insurance_folder_cnt = insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt  
  
 IF ISNULL(@source_id, 0) = 0 BEGIN  
     SELECT @source_id = 1  
 END  
  
 IF @risk_folder_id IS NULL BEGIN  
     SELECT @risk_folder_id = 0  
 END  
  
 INSERT INTO Risk_Folder (  
     risk_folder_id,  
     source_id,  
     risk_folder_type_id,  
     code,  
     description,  
     insurance_folder_cnt)  
 VALUES (  
     @risk_folder_id,  
     @source_id,  
     @risk_folder_type_id,  
     NEWID(),  
     @description,  
     @insurance_folder_cnt)  
  
 SELECT @new_risk_folder_cnt = SCOPE_IDENTITY() 
 
 UPDATE risk_folder  
     SET Code = 'C' + convert(char(2), @source_id) + convert(char(10), @new_risk_folder_cnt)  
	 WHERE risk_folder_cnt = @new_risk_folder_cnt  
	  
END   

GO



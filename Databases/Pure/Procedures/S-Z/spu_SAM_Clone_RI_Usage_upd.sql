
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Clone_RI_Usage_upd'
GO

CREATE PROCEDURE spu_SAM_Clone_RI_Usage_upd  
(  
 @insurance_file_cnt       int,  
 @Clone_RI_status_type_id     int  
)  
AS  
  
UPDATE  
 Insurance_File_Cloned_RI_Usage 
SET  
 status  = @Clone_RI_status_type_id  
WHERE  
 insurance_file_cnt = @insurance_file_cnt
 

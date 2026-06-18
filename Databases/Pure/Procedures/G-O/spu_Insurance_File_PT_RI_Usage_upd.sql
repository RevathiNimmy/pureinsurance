/* 
		Purpose : E007 
		Stored Precedure name : spu_Insurance_File_PT_RI_Usage_upd
		SQL SERVER 2005 
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Insurance_File_PT_RI_Usage_upd'
GO

CREATE PROCEDURE spu_Insurance_File_PT_RI_Usage_upd  
(  
 @ins_file_PT_RI_usage_id  int,  
 @insurance_file_cnt       int,  
 @PT_RI_status_type_id     int  
)  
AS  
  
UPDATE  
 Insurance_File_PT_RI_Usage  
SET  
 new_insurance_file_cnt = @insurance_file_cnt,  
 status = @PT_RI_status_type_id  
WHERE  
 ins_file_PT_RI_usage_id = @ins_file_PT_RI_usage_id  

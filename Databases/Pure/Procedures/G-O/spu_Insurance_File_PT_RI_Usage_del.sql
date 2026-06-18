/* 
		Purpose : E007 
		Stored Precedure name : spu_Insurance_File_PT_RI_Usage_del
		SQL SERVER 2005 
*/


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Insurance_File_PT_RI_Usage_del'
GO


CREATE PROCEDURE spu_Insurance_File_PT_RI_Usage_del  
	@ins_file_PT_RI_usage_id int    
AS  
  
DELETE FROM  
 Insurance_File_PT_RI_Usage  
WHERE  
 ins_file_PT_RI_usage_id = @ins_file_PT_RI_usage_id  

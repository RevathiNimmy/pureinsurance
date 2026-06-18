SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Do_Not_Merge_Clauses'
GO
CREATE PROCEDURE spu_Get_Do_Not_Merge_Clauses(    
 @insurance_file_cnt INT)  
 AS    
	BEGIN    
	  
	 SELECT  psw.document_template_id, dt.document_type_id,dt.code,dt.[description]  
	 FROM policy_standard_wording psw  
	 JOIN document_template dt  
	 ON dt.document_template_id = psw.document_template_id  
	 WHERE insurance_file_cnt = @insurance_file_cnt  
	 AND do_not_merge=1  
				
	END  
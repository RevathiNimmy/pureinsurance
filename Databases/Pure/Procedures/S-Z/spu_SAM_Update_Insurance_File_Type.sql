SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Update_Insurance_File_Type'
GO

/*******************************************************************************************************/  
/* spu_SAM_Update_Insurance_File_Type                                                                  */  
/*                                                                                        	       */  
/* Update the Insurance file table and sets the insurance file type ID from the given code	       */		
/*******************************************************************************************************/  

CREATE  PROCEDURE spu_SAM_Update_Insurance_File_Type
@insurance_file_cnt INT,
@insurance_file_type_code VARCHAR(255)
    
AS  

UPDATE Insurance_File 
SET insurance_file_type_id = (select insurance_file_type_id 
				FROM insurance_file_type 
				WHERE code = @insurance_file_type_code)
WHERE insurance_file_cnt = @insurance_file_cnt
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO




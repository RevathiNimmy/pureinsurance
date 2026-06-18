SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Check_Valid_Insurance_File_For_Clone'
GO

CREATE  PROCEDURE spu_Check_Valid_Insurance_File_For_Clone    
    @nInsurance_file_cnt integer,    
    @nIs_InValid INT = NULL OUTPUT    
AS    
    
BEGIN    
    
  DECLARE @insurance_folder_cnt INTEGER    
    
  SELECT @insurance_folder_cnt = insurance_folder_cnt from insurance_file where insurance_file_cnt = @nInsurance_file_cnt    
    
  If EXISTS(SELECT MAX(ifcu.insurance_file_cnt )    
   FROM insurance_file ins    
   JOIN Insurance_File_Cloned_RI_Usage  ifcu    
   On ins.insurance_file_cnt = ifcu .insurance_file_cnt    
   WHERE ins.insurance_file_cnt < @nInsurance_file_cnt and ins.insurance_folder_cnt =@insurance_folder_cnt  
   Group By ins.insurance_folder_cnt)    
 SELECT @nIs_InValid = 1    
  ELSE    
 SELECT @nIs_InValid =0    
    
END 

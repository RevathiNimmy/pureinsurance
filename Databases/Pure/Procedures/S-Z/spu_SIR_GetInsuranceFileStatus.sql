SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_GetInsuranceFileStatus'
GO

CREATE PROCEDURE spu_SIR_GetInsuranceFileStatus  
  
@insurance_file_cnt int  
  
AS  
  
SELECT insurance_file_status.code,insurance_file_type.code
FROM insurance_file  
 INNER JOIN insurance_file_type ON  
  insurance_file_type.insurance_file_type_id  = insurance_file.insurance_file_type_id  
  
 INNER JOIN insurance_file_status ON  
  insurance_file_status.insurance_file_status_id  = insurance_file.insurance_file_status_id  
  
WHERE insurance_file_cnt = @insurance_file_cnt  



GO

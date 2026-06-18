SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_GetInsuranceRefForInsuranceFolderCnt'
GO

CREATE PROCEDURE spu_SIR_GetInsuranceRefForInsuranceFolderCnt
    @InsuranceFolderCnt INT  
AS  
  
    SELECT TOP 1 
           ifi.insurance_ref  
    FROM Insurance_File ifi  
	INNER JOIN Insurance_Folder ifo 
	ON 
		ifi.insurance_folder_cnt = ifo.insurance_folder_cnt  
    WHERE 
		ifi.insurance_folder_cnt = @InsuranceFolderCnt  
		AND ifi.insurance_file_type_id NOT IN (
							SELECT insurance_file_type_id 
							FROM insurance_file_type 
							WHERE code IN ( 'RENEWAL','MTAQUOTE','MTAQTETEMP','MTAQREINS','MTAQCAN')  
							)
     
	ORDER BY ifi.insurance_file_cnt DESC

GO
DDLDROPPROCEDURE spu_get_saved_oos_versions
GO
CREATE PROCEDURE spu_get_saved_oos_versions
@insurance_file_cnt INT,
@insurance_folder_cnt INT
AS

SELECT IFI.Base_Insurance_File_Cnt as BaseInsuranceFileKey
       FROM insurance_file IFI
	   INNER JOIN Insurance_File_Type ift ON ift.insurance_file_type_id = ifi.insurance_file_type_id
       Where insurance_folder_cnt = @insurance_folder_cnt 
			 AND ISNull(Base_Insurance_File_Cnt, 0) > 0 
			 AND ift.code in ('RENEWAL', 'MTAQCAN', 'MTAQREINS', 'MTAQUOTE') 
       Group By Base_Insurance_File_Cnt
       Having Count(*) > 1
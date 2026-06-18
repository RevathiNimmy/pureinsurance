SET QUOTED_IDENTIFIER OFF 
GO
Execute DDLDropProcedure 'spu_SAM_Get_Policy_Version_For_Renewal'
GO

CREATE PROCEDURE spu_SAM_Get_Policy_Version_For_Renewal 
@insurance_ref VARCHAR(30)

AS
SELECT 
 Insurance_File.insurance_file_cnt
 
 FROM Insurance_File
 INNER JOIN Insurance_Folder
 ON Insurance_Folder.insurance_folder_cnt = Insurance_File.insurance_folder_cnt
 AND ((Insurance_File.insurance_file_status_id IS NULL)
 OR (Insurance_File.insurance_file_status_id IN
 (SELECT insurance_file_status_id
 FROM Insurance_File_Status
 WHERE code IN ('REN', 'LAP'))))
 AND Insurance_file.policy_ignore IS NULL
 AND
 Insurance_File.insurance_ref Like @insurance_ref
 INNER JOIN Party
 ON Party.party_cnt = Insurance_Folder.insurance_holder_cnt
 INNER JOIN Insurance_File_System
 ON Insurance_File_System.insurance_file_cnt = Insurance_File.insurance_file_cnt
 INNER JOIN Insurance_File_Type
 ON Insurance_File_Type.insurance_file_type_id = Insurance_File.insurance_file_type_id
 AND
(Insurance_File.insurance_file_cnt = (SELECT MAX(ifi.insurance_file_cnt) FROM Insurance_File ifi WHERE ifi.insurance_folder_cnt = Insurance_File.insurance_folder_cnt AND ifi.insurance_file_type_id IN (2,5,3,9)))
 INNER JOIN Product
 ON Product.product_id = Insurance_File.product_id
 INNER JOIN PMCaption
 ON PMCaption.caption_id = Product.caption_id
 LEFT OUTER JOIN Insurance_File_Status
 ON Insurance_File_Status.insurance_file_status_id = Insurance_File.insurance_file_status_id
 ORDER BY Insurance_File.insurance_ref, Insurance_File_System.date_created DESC

GO
SET QUOTED_IDENTIFIER OFF
GO

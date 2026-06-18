EXECUTE DDLDropProcedure 'spu_Get_Latest_Policy_Version'
GO

CREATE PROCEDURE spu_Get_Latest_Policy_Version  
   @insurance_folder_cnt INT

AS
SELECT  TOP 1 ifi.insurance_file_cnt,renewal_date
FROM    insurance_file  ifi JOIN Insurance_File_System ifs
ON ifi.insurance_file_cnt=ifs.insurance_file_cnt
WHERE   insurance_folder_cnt = @insurance_folder_cnt
AND     insurance_file_type_id IN (SELECT insurance_file_type_id FROM insurance_file_type WHERE code IN ('POLICY', 'MTA PERM', 'MTAREINS'))
AND     (insurance_file_status_id IS NULL OR insurance_file_status_id in (3,2,5,6))
ORDER BY ifi.inception_date_tpi DESC,ifi.insurance_file_cnt DESC  


EXEC DDLDropProcedure 'spu_get_underwriting_year_for_nb'
GO

CREATE PROCEDURE spu_get_underwriting_year_for_nb
	@insurance_folder_cnt INT,
	@underwriting_year_id INT OUTPUT
AS BEGIN

SELECT	@underwriting_year_id=iff.underwriting_year_id
FROM	Insurance_File iff
JOIN	Insurance_Folder ifldr ON iff.insurance_folder_cnt=ifldr.insurance_folder_cnt
WHERE	ifldr.insurance_folder_cnt=@insurance_folder_cnt
AND		iff.insurance_file_type_id=2

END
GO
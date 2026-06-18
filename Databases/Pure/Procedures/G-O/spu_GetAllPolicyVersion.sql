
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetAllPolicyVersion'
GO

-- 
CREATE PROCEDURE spu_GetAllPolicyVersion
	@InsuranceRef varchar(30)

AS

BEGIN

	SELECT	ifi.insurance_file_cnt,
			ifi.insurance_ref,
			ift.[description] PolicyType,
			IsNull(ifs.[description],'Live') PolicyStatus,
			ifi.cover_start_date,
			ifi.expiry_date,
			ifi.insured_name,
			ifi.insurance_file_type_id,
			ifi.insurance_file_status_id			
	FROM	Insurance_File ifi LEFT JOIN Insurance_File_Type ift ON ifi.insurance_file_type_id = ift.insurance_file_type_id
			LEFT JOIN Insurance_File_Status ifs ON ifi.insurance_file_status_id = ifs.insurance_file_status_id
	WHERE	ifi.insurance_ref = @InsuranceRef
	ORDER BY ifi.insurance_file_cnt ASC
	
END
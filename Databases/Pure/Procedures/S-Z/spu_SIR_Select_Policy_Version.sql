SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Select_Policy_Version'
GO

CREATE PROCEDURE spu_SIR_Select_Policy_Version 
	@insurance_file_cnt int
AS
	
	SELECT TOP 1 ifi.insurance_file_type_id, ifi.insurance_file_status_id, 
		ifs.created_by_id, created_by.username created_by_username, 
		ifs.modified_by_id, modified_by.username modified_by_username
		FROM Insurance_file ifi
	INNER JOIN Insurance_file_System ifs ON ifi.insurance_file_cnt = ifs.insurance_file_cnt
	INNER JOIN PMUser created_by ON ifs.created_by_id = created_by.User_id
	INNER JOIN PMUser modified_by ON ifs.modified_by_id = modified_by.User_id
	WHERE ifi.insurance_file_cnt = @insurance_file_cnt

GO


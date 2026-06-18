EXECUTE DDLDropProcedure 'spu_reorder_later_policy_versions'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_reorder_later_policy_versions
	@original_insurance_file_cnt	INT
AS

UPDATE isf1 
	SET isf1.policy_version = isf1.policy_version - 1
FROM insurance_file isf1
	INNER JOIN insurance_file isf2 
ON isf1.insurance_folder_cnt = isf2.insurance_folder_cnt
WHERE isf2.insurance_file_cnt = @original_insurance_file_cnt
	AND isf1.policy_version > isf2.policy_version


GO

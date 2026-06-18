EXECUTE DDLDropProcedure 'spu_IsPolicyVersionInAccount'
GO
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_IsPolicyVersionInAccount
	@InsuranceFileCnt Int
AS
BEGIN
	SELECT 	document_ref
	FROM	stats_folder
	WHERE	insurance_file_cnt = @InsuranceFileCnt ORDER BY stats_folder_cnt 
END

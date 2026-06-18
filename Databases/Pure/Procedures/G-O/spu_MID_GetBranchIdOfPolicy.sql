
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

EXEC DDLDropProcedure 'spu_MID_GetBranchIdOfPolicy'
GO

CREATE PROCEDURE spu_MID_GetBranchIdOfPolicy
	@insurance_ref as varchar(30)
AS

BEGIN

	SELECT TOP 1 source_id 
	FROM Insurance_File 
	WHERE insurance_ref = @insurance_ref
	ORDER BY insurance_file_cnt desc

END
GO
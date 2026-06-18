
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

EXEC DDLDropProcedure 'spu_MID_GetActiveRulesForExport'
GO

CREATE PROCEDURE spu_MID_GetActiveRulesForExport
	@mid_type as varchar(4)
AS

BEGIN

	SELECT DISTINCT MID_Rule_id, source_id, FileName, Supplier_id, Insurer_id, Site_Number, Test_Indicator
	FROM MID_Rule 
	WHERE	Is_Deleted <> 1 
		AND MID_Type = @mid_type
		AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(date, Start_Date) AND CONVERT(DATE, Expiry_Date) 

END
GO

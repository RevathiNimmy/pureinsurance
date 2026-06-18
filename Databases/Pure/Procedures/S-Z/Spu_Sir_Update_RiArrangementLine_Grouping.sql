SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'Spu_Sir_Update_RiArrangementLine_Grouping'
GO

CREATE PROCEDURE Spu_Sir_Update_RiArrangementLine_Grouping
@Ri_Arrangement_line_Id AS INT,
@sRiIds AS VARCHAR(MAX),
@ProcessId AS INT

AS

DECLARE @sSql VARCHAR(MAX)

IF @ProcessId = 1 -- Policy Risk
BEGIN

	SET @sSQL= 'Update Ri_arrangement_line Set grouping = ' +  CONVERT(VARCHAR(10),@Ri_Arrangement_line_Id) + 
		' where ri_arrangement_line_id in (' + @sRiIds + ')'  
END
IF @ProcessId = 2 -- Claims 
BEGIN
	SET @sSQL= 'Update Claim_Ri_arrangement_line Set grouping = ' +  CONVERT(VARCHAR(10),@Ri_Arrangement_line_Id) + 
		' where ri_arrangement_line_id in (' + @sRiIds + ')'  
END

EXEC (@sSql)
GO

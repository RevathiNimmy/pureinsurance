SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Secondary_Cause_Sel'
GO

CREATE PROCEDURE spe_Secondary_Cause_Sel
	@mode int=1
AS
IF @mode<>0
	SELECT secondary_cause_id, primary_cause_id,code,description FROM secondary_cause WHERE is_deleted =0
ELSE
	SELECT secondary_cause_id, primary_cause_id,code,description FROM secondary_cause
GO


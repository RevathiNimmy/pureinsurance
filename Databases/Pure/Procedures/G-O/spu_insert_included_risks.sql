SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_insert_included_risks'
GO

CREATE PROCEDURE spu_insert_included_risks
 @session_id int,
 @riskcodeid int

AS
BEGIN
	Insert into Temp_Report_IncludedRisks(session_id, riskcodeid) 
	values(@session_id, @riskcodeid)
END
GO



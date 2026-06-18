SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Delete_Temp_Report_Records'
GO


CREATE PROCEDURE spu_Delete_Temp_Report_Records
	@session_id int
AS
BEGIN
	DELETE FROM Temp_Report_IncludedRisks
	WHERE session_id = @session_id
	DELETE FROM Temp_Report_ExcludedRisks
	WHERE session_id = @session_id
	DELETE FROM Temp_Report_IncludedRiskGroups
	WHERE session_id = @session_id
	DELETE FROM Temp_Report_ExcludedRiskGroups
	WHERE session_id = @session_id
	DELETE FROM Temp_Report_Grouping
	WHERE session_id = @session_id
	DELETE FROM Temp_Report_Users
	WHERE session_id = @session_id

END
GO

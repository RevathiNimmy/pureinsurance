SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_insert_excluded_risk_groups'
GO

CREATE PROCEDURE spu_insert_excluded_risk_groups
 @session_id int,
 @riskgroupid int
AS
BEGIN
	Insert into Temp_Report_ExcludedRiskGroups(session_id, riskgroupid) 
	values(@session_id, @riskgroupid)
END
GO

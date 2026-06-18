SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_insert_report_grouping'
GO

CREATE PROCEDURE spu_insert_report_grouping
 @session_id int,
 @group varchar(50)
AS
BEGIN
	Insert into Temp_Report_Grouping(session_id, [group]) 
	values(@session_id, @group)
END
GO

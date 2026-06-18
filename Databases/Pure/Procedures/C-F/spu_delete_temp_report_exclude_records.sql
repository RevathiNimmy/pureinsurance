SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_delete_temp_report_exclude_records'
GO

CREATE PROCEDURE spu_delete_temp_report_exclude_records
    @unique_report_name varchar(300) --int
AS
BEGIN
    DELETE FROM Temp_Report_Exclude
    WHERE unique_report_name = @unique_report_name
End

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

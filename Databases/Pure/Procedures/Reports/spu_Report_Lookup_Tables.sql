SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Lookup_Tables'
GO
CREATE PROCEDURE spu_Report_Lookup_Tables
    @report_type varchar(60)
AS
BEGIN
    EXECUTE ('SELECT code, description, effective_date FROM ' + @report_type + ' WHERE is_deleted = 0')
END
GO


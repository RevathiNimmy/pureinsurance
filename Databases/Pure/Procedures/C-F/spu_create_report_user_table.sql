SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_create_report_user_table'
GO
CREATE PROCEDURE spu_create_report_user_table
AS
BEGIN
    EXECUTE DDLDropTable 'Report_Users'

    CREATE TABLE Report_Users (
        reportuser varchar(12)
    )
END
GO


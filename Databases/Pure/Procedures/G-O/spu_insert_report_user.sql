SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_insert_report_user'
GO
CREATE PROCEDURE spu_insert_report_user
    @user varchar(12)
AS
    INSERT INTO Report_Users(reportuser)
        VALUES(@user)
GO


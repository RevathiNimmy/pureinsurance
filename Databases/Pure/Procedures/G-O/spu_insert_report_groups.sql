SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_insert_report_groups'
GO
CREATE PROCEDURE spu_insert_report_groups
    @group varchar(50)
AS
    INSERT INTO Report_Groups([group])
        VALUES(@group)
GO


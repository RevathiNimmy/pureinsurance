SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_ReportHeader'
GO


CREATE PROCEDURE spu_ACT_Add_ReportHeader
    @reportheader_id int OUTPUT,
    @company_id smallint,
    @user_id smallint,
    @report_date datetime
AS


BEGIN
INSERT INTO ReportHeader (
    company_id,
    user_id,
    report_date)
VALUES (
    @company_id,
    @user_id,
    @report_date)
END
BEGIN
SELECT @reportheader_id = @@IDENTITY
END
GO



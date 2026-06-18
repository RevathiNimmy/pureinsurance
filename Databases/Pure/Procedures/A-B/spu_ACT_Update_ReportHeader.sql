SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_ReportHeader'
GO


CREATE PROCEDURE spu_ACT_Update_ReportHeader
    @reportheader_id int,
    @company_id smallint,
    @user_id smallint,
    @report_date datetime
AS


BEGIN
UPDATE ReportHeader
    SET
    company_id=@company_id,
    user_id=@user_id,
    report_date=@report_date
WHERE reportheader_id = @reportheader_id
END
GO



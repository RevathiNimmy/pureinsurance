SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_ReportHeader'
GO


CREATE PROCEDURE spu_ACT_Select_ReportHeader
    @reportheader_id int
AS


SELECT
    reportheader_id,
    company_id,
    user_id,
    report_date
FROM ReportHeader
WHERE reportheader_id = @reportheader_id
GO



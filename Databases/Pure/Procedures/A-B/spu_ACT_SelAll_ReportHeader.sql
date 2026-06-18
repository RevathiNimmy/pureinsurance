SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_ReportHeader'
GO


CREATE PROCEDURE spu_ACT_SelAll_ReportHeader
    @company_id smallint
AS


SELECT
    reportheader_id,
    company_id,
    user_id,
    report_date
FROM ReportHeader
     WHERE @company_id = company_id
GO



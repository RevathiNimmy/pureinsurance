SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_ReportHeader'
GO


CREATE PROCEDURE spu_ACT_Check_ReportHeader
    @reportheader_id int OUTPUT
AS


BEGIN
    SELECT @reportheader_id = reportheader_id
    FROM ReportHeader
    WHERE reportheader_id = @reportheader_id
END
BEGIN
IF @reportheader_id = NULL
    SELECT @reportheader_id = -1
END
GO



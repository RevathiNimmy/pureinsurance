SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_ReportHeader'
GO


CREATE PROCEDURE spu_ACT_Delete_ReportHeader
    @reportheader_id int
AS


DELETE FROM ReportHeader
WHERE reportheader_id = @reportheader_id
GO



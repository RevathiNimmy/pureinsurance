SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_document_spooler_upd_pr'
GO


CREATE PROCEDURE spu_document_spooler_upd_pr
    @document_spooler_id int
AS


BEGIN
UPDATE document_spooler
    SET
    times_printed = times_printed + 1
WHERE document_spooler_id = @document_spooler_id
END
GO



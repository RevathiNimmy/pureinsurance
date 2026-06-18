SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_document_spooler_upd_ar'
GO


CREATE PROCEDURE spu_document_spooler_upd_ar
    @document_spooler_id int
AS


BEGIN
UPDATE document_spooler
    SET
    times_archived = times_archived + 1
WHERE document_spooler_id = @document_spooler_id
END
GO



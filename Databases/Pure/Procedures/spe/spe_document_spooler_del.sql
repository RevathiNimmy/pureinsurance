SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_document_spooler_del'
GO

CREATE PROCEDURE spe_document_spooler_del
    @document_spooler_id int
AS
DELETE FROM document_spooler
WHERE document_spooler_id = @document_spooler_id

GO


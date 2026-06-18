SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_document_spooler_dar'
GO

CREATE PROCEDURE spe_document_spooler_dar

AS
DELETE
FROM document_spooler

GO


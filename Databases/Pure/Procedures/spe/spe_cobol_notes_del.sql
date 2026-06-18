SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_cobol_notes_del'
GO

CREATE PROCEDURE spe_cobol_notes_del
    @cobol_notes_code int
AS
DELETE FROM cobol_notes
WHERE cobol_notes_code = @cobol_notes_code

GO


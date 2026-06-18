SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_cobol_notes_sel'
GO

CREATE PROCEDURE spe_cobol_notes_sel
    @cobol_notes_code int
AS
SELECT
    cobol_notes_code,
    cobol_notes_data
 FROM cobol_notes
WHERE cobol_notes_code = @cobol_notes_code

GO


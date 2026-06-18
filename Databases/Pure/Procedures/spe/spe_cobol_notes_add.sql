SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_cobol_notes_add'
GO

CREATE PROCEDURE spe_cobol_notes_add
    @cobol_notes_code int,
    @cobol_notes_data varchar(68)
AS
BEGIN
INSERT INTO cobol_notes (
    cobol_notes_code,
    cobol_notes_data)
VALUES (
    @cobol_notes_code,
    @cobol_notes_data)
END

GO


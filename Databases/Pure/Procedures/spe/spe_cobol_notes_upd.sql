SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_cobol_notes_upd'
GO

CREATE PROCEDURE spe_cobol_notes_upd
    @cobol_notes_code int,
    @cobol_notes_data varchar(68)
AS
BEGIN
UPDATE cobol_notes
    SET
    cobol_notes_data=@cobol_notes_data
WHERE cobol_notes_code = @cobol_notes_code
END

GO


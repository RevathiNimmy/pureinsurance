SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_numbering_scheme_del'
GO

CREATE PROCEDURE spe_numbering_scheme_del
    @numbering_scheme_id int
AS

DELETE FROM numbering_scheme

WHERE numbering_scheme_id = @numbering_scheme_id

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_abandoned_numbers_add'
GO

CREATE PROCEDURE spe_abandoned_numbers_add
    @numbering_scheme_id int,
    @abandoned_number varchar(20)

AS

BEGIN
INSERT INTO abandoned_numbers (
    numbering_scheme_id ,
    abandoned_number )
VALUES (
    @numbering_scheme_id,
    @abandoned_number)
END

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_abandoned_numbers_saa'
GO


CREATE PROCEDURE spu_abandoned_numbers_saa
    @numbering_scheme_id int
AS

/* 250900 */
SELECT abandoned_number
FROM abandoned_numbers
WHERE numbering_scheme_id = @numbering_scheme_id
ORDER BY
    abandoned_number ASC
GO



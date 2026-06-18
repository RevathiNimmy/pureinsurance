SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_abandoned_numbers_del'
GO

/* end of sp_pm_SIR */

CREATE PROCEDURE spe_abandoned_numbers_del
    @numbering_scheme_id int,
    @abandoned_number varchar(20)
AS

DELETE FROM abandoned_numbers

WHERE numbering_scheme_id = @numbering_scheme_id AND abandoned_number = @abandoned_number

GO


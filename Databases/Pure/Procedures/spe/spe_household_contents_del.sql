SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_household_contents_del'
GO

CREATE PROCEDURE spe_household_contents_del
    @insurance_file_cnt int
AS
DELETE FROM household_contents
WHERE insurance_file_cnt = @insurance_file_cnt

GO


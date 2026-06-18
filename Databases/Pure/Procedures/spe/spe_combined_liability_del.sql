SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_combined_liability_del'
GO

CREATE PROCEDURE spe_combined_liability_del
    @insurance_file_cnt int
AS
DELETE FROM combined_liability
WHERE insurance_file_cnt = @insurance_file_cnt

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_commercial_combined_del'
GO

CREATE PROCEDURE spe_commercial_combined_del
    @insurance_file_cnt int
AS
DELETE FROM commercial_combined
WHERE insurance_file_cnt = @insurance_file_cnt

GO


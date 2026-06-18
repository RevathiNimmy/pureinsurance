SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_travel_del'
GO

CREATE PROCEDURE spe_travel_del
    @insurance_file_cnt int
AS
DELETE FROM travel
WHERE insurance_file_cnt = @insurance_file_cnt

GO


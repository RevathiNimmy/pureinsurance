SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_marine_del'
GO

CREATE PROCEDURE spe_marine_del
    @insurance_file_cnt int
AS
DELETE FROM marine
WHERE insurance_file_cnt = @insurance_file_cnt

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_aviation_del'
GO

CREATE PROCEDURE spe_aviation_del
    @insurance_file_cnt int
AS
DELETE FROM aviation
WHERE insurance_file_cnt = @insurance_file_cnt

GO


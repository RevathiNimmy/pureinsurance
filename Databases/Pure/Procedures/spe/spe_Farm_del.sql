SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Farm_del'
GO

CREATE PROCEDURE spe_Farm_del
    @insurance_file_cnt int
AS
DELETE FROM Farm
WHERE insurance_file_cnt = @insurance_file_cnt

GO


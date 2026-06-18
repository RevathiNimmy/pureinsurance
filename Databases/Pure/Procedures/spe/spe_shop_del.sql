SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_shop_del'
GO

CREATE PROCEDURE spe_shop_del
    @insurance_file_cnt int
AS
DELETE FROM shop
WHERE insurance_file_cnt = @insurance_file_cnt

GO


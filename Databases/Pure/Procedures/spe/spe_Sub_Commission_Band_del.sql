SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Band_del'
GO

CREATE PROCEDURE spe_Sub_Commission_Band_del
    @sub_commission_band_id int
AS
DELETE FROM Sub_Commission_Band
WHERE sub_commission_band_id = @sub_commission_band_id

GO


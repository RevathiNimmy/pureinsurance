SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Band_upd'
GO

CREATE PROCEDURE spe_Sub_Commission_Band_upd
    @sub_commission_band_id int,
    @insurance_file_cnt int,
    @commission_band tinyint,
    @premium numeric(19,4)
AS
BEGIN
UPDATE Sub_Commission_Band
    SET
    insurance_file_cnt=@insurance_file_cnt,
    commission_band=@commission_band,
    premium=@premium
WHERE sub_commission_band_id = @sub_commission_band_id
END

GO


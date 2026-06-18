SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Band_sel'
GO

CREATE PROCEDURE spe_Sub_Commission_Band_sel
    @sub_commission_band_id int
AS
SELECT
    sub_commission_band_id,
    insurance_file_cnt,
    commission_band,
    premium
 FROM Sub_Commission_Band
WHERE sub_commission_band_id = @sub_commission_band_id

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Value_sel'
GO

CREATE PROCEDURE spe_Sub_Commission_Value_sel
    @party_cnt int,
    @insurance_file_cnt int,
    @sub_commission_band_id int
AS
SELECT
    party_cnt,
    insurance_file_cnt,
    sub_commission_band_id,
    [percent],
    value
 FROM Sub_Commission_Value
WHERE party_cnt = @party_cnt AND insurance_file_cnt = @insurance_file_cnt AND sub_commission_band_id = @sub_commission_band_id

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Value_del'
GO

CREATE PROCEDURE spe_Sub_Commission_Value_del
    @party_cnt int,
    @insurance_file_cnt int,
    @sub_commission_band_id int
AS
DELETE FROM Sub_Commission_Value
WHERE party_cnt = @party_cnt AND insurance_file_cnt = @insurance_file_cnt AND sub_commission_band_id = @sub_commission_band_id

GO


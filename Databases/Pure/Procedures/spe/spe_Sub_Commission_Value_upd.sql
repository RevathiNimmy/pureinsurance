SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Value_upd'
GO

CREATE PROCEDURE spe_Sub_Commission_Value_upd
    @party_cnt int,
    @insurance_file_cnt int,
    @sub_commission_band_id int,
    @percent numeric(12,8),
    @value numeric(19,4)
AS
BEGIN
UPDATE Sub_Commission_Value
    SET
    [percent]=@percent,
    value=@value
WHERE party_cnt = @party_cnt AND insurance_file_cnt = @insurance_file_cnt AND sub_commission_band_id = @sub_commission_band_id
END

GO


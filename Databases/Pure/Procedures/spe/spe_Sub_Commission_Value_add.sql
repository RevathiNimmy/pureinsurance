SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Value_add'
GO

CREATE PROCEDURE spe_Sub_Commission_Value_add
    @party_cnt int,
    @insurance_file_cnt int,
    @sub_commission_band_id int,
    @percent numeric(12,8),
    @value numeric(19,4)
AS
BEGIN
INSERT INTO Sub_Commission_Value (
    party_cnt ,
    insurance_file_cnt ,
    sub_commission_band_id ,
    [percent] ,
    value )
VALUES (
    @party_cnt,
    @insurance_file_cnt,
    @sub_commission_band_id,
    @percent,
    @value)
END

GO


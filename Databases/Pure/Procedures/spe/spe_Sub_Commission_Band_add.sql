SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Commission_Band_add'
GO

CREATE PROCEDURE spe_Sub_Commission_Band_add
    @sub_commission_band_id int OUTPUT ,
    @insurance_file_cnt int ,
    @commission_band tinyint ,
    @premium numeric(19,4)
AS
BEGIN
INSERT INTO Sub_Commission_Band (
    insurance_file_cnt,
    commission_band,
    premium)
VALUES (
    @insurance_file_cnt,
    @commission_band,
    @premium)
END
BEGIN
SELECT @sub_commission_band_id = @@IDENTITY
END

GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Arc_Coi_Value_sel'
GO

CREATE PROCEDURE spe_Arc_Coi_Value_sel
    @coi_value_id int,
    @insurance_file_cnt int
AS
SELECT
    coi_value_id,
    insurance_file_cnt,
    party_cnt,
    arrangement_ref,
    share_percent,
    share_premium,
    commission_percent,
    commission_value,
    surcharge_percent,
    surcharge_value,
    is_standard_surcharge,
    premium_tax_recovery_percent,
    premium_tax_recovery_value,
    is_manual_premium_tax_rec
 FROM Arc_Coi_Value
WHERE coi_value_id = @coi_value_id AND insurance_file_cnt = @insurance_file_cnt

GO


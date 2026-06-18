SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Value_sel'
GO

CREATE PROCEDURE spe_Coi_Value_sel
    @insurance_file_cnt int,
    @coi_value_id int
AS
SELECT
    insurance_file_cnt,
    coi_value_id,
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
 FROM Coi_Value
WHERE insurance_file_cnt = @insurance_file_cnt AND coi_value_id = @coi_value_id

GO


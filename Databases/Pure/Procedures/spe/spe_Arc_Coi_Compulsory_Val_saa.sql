SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Arc_Coi_Compulsory_Val_saa'
GO

CREATE PROCEDURE spe_Arc_Coi_Compulsory_Val_saa
    @insurance_file_cnt int
AS
SELECT
    coi_compulsory_value_id,
    insurance_file_cnt,
    party_cnt,
    arrangement_ref,
    share_percent,
    share_premium,
    commission_percent,
    commission_value,
    premium_tax_recovery_percent,
    premium_tax_recovery_value,
    is_manual_premium_tax_rec
 FROM Arc_Coi_Compulsory_Value
WHERE insurance_file_cnt = @insurance_file_cnt
ORDER BY coi_compulsory_value_id ASC

GO


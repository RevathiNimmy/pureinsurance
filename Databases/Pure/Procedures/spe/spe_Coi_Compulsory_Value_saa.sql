SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Compulsory_Value_saa'
GO

CREATE PROCEDURE spe_Coi_Compulsory_Value_saa
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    coi_compulsory_value_id,
    party_cnt,
    arrangement_ref,
    share_percent,
    share_premium,
    commission_percent,
    commission_value,
    premium_tax_recovery_percent,
    premium_tax_recovery_value,
    is_manual_premium_tax_rec
 FROM Coi_Compulsory_Value
WHERE insurance_file_cnt = @insurance_file_cnt
ORDER BY coi_compulsory_value_id ASC

GO


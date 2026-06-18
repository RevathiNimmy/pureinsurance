SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Compulsory_Value_upd'
GO

CREATE PROCEDURE spe_Coi_Compulsory_Value_upd
    @insurance_file_cnt int,
    @coi_compulsory_value_id int,
    @party_cnt int,
    @arrangement_ref varchar(30),
    @share_percent numeric(12,8),
    @share_premium numeric(19,4),
    @commission_percent numeric(12,8),
    @commission_value numeric(19,4),
    @premium_tax_recovery_percent numeric(12,8),
    @premium_tax_recovery_value numeric(19,4),
    @is_manual_premium_tax_rec tinyint
AS
BEGIN
UPDATE Coi_Compulsory_Value
    SET
    party_cnt=@party_cnt,
    arrangement_ref=@arrangement_ref,
    share_percent=@share_percent,
    share_premium=@share_premium,
    commission_percent=@commission_percent,
    commission_value=@commission_value,
    premium_tax_recovery_percent=@premium_tax_recovery_percent,
    premium_tax_recovery_value=@premium_tax_recovery_value,
    is_manual_premium_tax_rec=@is_manual_premium_tax_rec
WHERE insurance_file_cnt = @insurance_file_cnt AND coi_compulsory_value_id = @coi_compulsory_value_id
END

GO


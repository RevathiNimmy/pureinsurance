SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Arc_Coi_Value_add'
GO

CREATE PROCEDURE spe_Arc_Coi_Value_add
    @coi_value_id int,
    @insurance_file_cnt int,
    @party_cnt int,
    @arrangement_ref varchar(30),
    @share_percent numeric(12,8),
    @share_premium numeric(19,4),
    @commission_percent numeric(12,8),
    @commission_value numeric(19,4),
    @surcharge_percent numeric(12,8),
    @surcharge_value numeric(19,4),
    @is_standard_surcharge tinyint,
    @premium_tax_recovery_percent numeric(12,8),
    @premium_tax_recovery_value numeric(19,4),
    @is_manual_premium_tax_rec tinyint
AS
BEGIN
INSERT INTO Arc_Coi_Value (
    coi_value_id ,
    insurance_file_cnt ,
    party_cnt ,
    arrangement_ref ,
    share_percent ,
    share_premium ,
    commission_percent ,
    commission_value ,
    surcharge_percent ,
    surcharge_value ,
    is_standard_surcharge ,
    premium_tax_recovery_percent ,
    premium_tax_recovery_value ,
    is_manual_premium_tax_rec )
VALUES (
    @coi_value_id,
    @insurance_file_cnt,
    @party_cnt,
    @arrangement_ref,
    @share_percent,
    @share_premium,
    @commission_percent,
    @commission_value,
    @surcharge_percent,
    @surcharge_value,
    @is_standard_surcharge,
    @premium_tax_recovery_percent,
    @premium_tax_recovery_value,
    @is_manual_premium_tax_rec)
END

GO


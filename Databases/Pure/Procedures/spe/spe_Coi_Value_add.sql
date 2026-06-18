SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Value_add'
GO

CREATE PROCEDURE spe_Coi_Value_add
    @insurance_file_cnt int,
    @coi_value_id int,
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
INSERT INTO Coi_Value (
    insurance_file_cnt ,
    coi_value_id ,
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
    @insurance_file_cnt,
    @coi_value_id,
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


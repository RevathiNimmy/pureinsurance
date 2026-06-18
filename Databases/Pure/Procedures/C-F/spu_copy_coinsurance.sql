SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_copy_coinsurance'
GO


CREATE PROCEDURE spu_copy_coinsurance
    @OldInsuranceFileCnt int,
    @NewInsuranceFileCnt int
AS

/*
    1.0 Created to copy coinsurance for renewal and MTA.        RWH 13/06/01

*/
BEGIN TRAN

INSERT INTO Coi_Arrangement
            (Insurance_File_Cnt,
            is_recovered,
            is_surcharged,
            coi_default_id)
    SELECT  @NewInsuranceFileCnt,
            is_recovered,
            is_surcharged,
            coi_default_id
    FROM        Coi_Arrangement
    WHERE   insurance_file_cnt = @OldInsuranceFileCnt

IF @@error > 0
    ROLLBACK TRAN

INSERT INTO Coi_value
            (Insurance_File_Cnt,
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
            is_manual_premium_tax_rec)
    SELECT  @NewInsuranceFileCnt,
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

    FROM        Coi_Value

    WHERE   insurance_file_cnt = @OldInsuranceFileCnt

IF @@error > 0
    ROLLBACK TRAN
ELSE
    COMMIT TRAN
GO



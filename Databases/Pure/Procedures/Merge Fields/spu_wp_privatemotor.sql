SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_privatemotor'
GO


CREATE PROCEDURE spu_wp_privatemotor
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @driving_restriction Varchar(255),
    @usage Varchar(255),
    @cover Varchar(255),
    @excess NUMERIC,
    @no_claims_discount_years INT OUTPUT,
    @is_ncd_protected TINYINT OUTPUT
AS


SELECT
    @driving_restriction = private_motor.driving_restriction,
    @usage = private_motor.usage,
    @cover = private_motor.cover,
    @excess = private_motor.excess,
    @no_claims_discount_years = private_motor.no_claims_discount_years,
    @is_ncd_protected = private_motor.is_ncd_protected

FROM private_motor
WHERE private_motor.insurance_file_cnt = @insurancefilecnt
GO



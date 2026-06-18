SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_privatepublichire'
GO


CREATE PROCEDURE spu_wp_privatepublichire
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @driving_restriction VARCHAR(255),
    @usage VARCHAR(255),
    @cover VARCHAR(255),
    @excess NUMERIC,
    @no_claim_discount_years INT OUTPUT,
    @is_ncd_protected TINYINT OUTPUT,
    @is_self_employed TINYINT OUTPUT,
    @taxi_firm VARCHAR(255),
    @radio_value NUMERIC
AS


SELECT
    @driving_restriction = private_public_hire.driving_restriction,
    @usage = private_public_hire.usage,
    @cover = private_public_hire.cover,
    @excess = private_public_hire.excess,
    @no_claim_discount_years = private_public_hire.no_claim_discount_years,
    @is_ncd_protected = private_public_hire.is_ncd_protected,
    @is_self_employed = private_public_hire.is_self_employed,
    @taxi_firm = private_public_hire.taxi_firm,
    @radio_value = private_public_hire.radio_value
FROM private_public_hire
WHERE private_public_hire.insurance_file_cnt = @insurancefilecnt
GO



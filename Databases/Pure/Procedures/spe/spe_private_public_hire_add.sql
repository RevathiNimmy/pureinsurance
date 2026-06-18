SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_private_public_hire_add'
GO

CREATE PROCEDURE spe_private_public_hire_add
    @insurance_file_cnt int,
    @driving_restriction varchar(70),
    @usage varchar(70),
    @cover varchar(70),
    @excess numeric(19,4),
    @no_claim_discount_years int,
    @is_ncd_protected tinyint,
    @is_self_employed tinyint,
    @taxi_firm varchar(50),
    @radio_value numeric(19,4)
AS
BEGIN
INSERT INTO private_public_hire (
    insurance_file_cnt ,
    driving_restriction ,
    usage ,
    cover ,
    excess ,
    no_claim_discount_years ,
    is_ncd_protected ,
    is_self_employed ,
    taxi_firm ,
    radio_value )
VALUES (
    @insurance_file_cnt,
    @driving_restriction,
    @usage,
    @cover,
    @excess,
    @no_claim_discount_years,
    @is_ncd_protected,
    @is_self_employed,
    @taxi_firm,
    @radio_value)
END

GO


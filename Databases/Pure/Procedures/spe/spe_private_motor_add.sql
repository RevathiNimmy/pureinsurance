SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_private_motor_add'
GO

CREATE PROCEDURE spe_private_motor_add
    @insurance_file_cnt int,
    @driving_restriction varchar(70),
    @usage varchar(70),
    @cover varchar(70),
    @excess numeric(19,4),
    @no_claims_discount_years int,
    @is_ncd_protected tinyint
AS
BEGIN
INSERT INTO private_motor (
    insurance_file_cnt ,
    driving_restriction ,
    usage ,
    cover ,
    excess ,
    no_claims_discount_years ,
    is_ncd_protected )
VALUES (
    @insurance_file_cnt,
    @driving_restriction,
    @usage,
    @cover,
    @excess,
    @no_claims_discount_years,
    @is_ncd_protected)
END

GO


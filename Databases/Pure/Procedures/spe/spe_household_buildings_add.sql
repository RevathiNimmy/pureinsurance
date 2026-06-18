SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_household_buildings_add'
GO

CREATE PROCEDURE spe_household_buildings_add
    @insurance_file_cnt int,
    @address_cnt int,
    @sum_insured numeric(19,4),
    @rebuild_cost numeric(19,4),
    @cover_type varchar(20),
    @number_of_bedrooms int,
    @type_of_property varchar(70),
    @is_subsidence tinyint,
    @excess numeric(19,4)
AS
BEGIN
INSERT INTO household_buildings (
    insurance_file_cnt ,
    address_cnt ,
    sum_insured ,
    rebuild_cost ,
    cover_type ,
    number_of_bedrooms ,
    type_of_property ,
    is_subsidence ,
    excess )
VALUES (
    @insurance_file_cnt,
    @address_cnt,
    @sum_insured,
    @rebuild_cost,
    @cover_type,
    @number_of_bedrooms,
    @type_of_property,
    @is_subsidence,
    @excess)
END

GO


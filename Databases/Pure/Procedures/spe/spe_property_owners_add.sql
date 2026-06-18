SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_property_owners_add'
GO

CREATE PROCEDURE spe_property_owners_add
    @insurance_file_cnt int,
    @is_buildings tinyint,
    @b_sum_insured numeric(19,4),
    @b_contents_sum_insured numeric(19,4),
    @b_excess numeric(19,4),
    @is_public_liability tinyint,
    @pl_indemnity_limit numeric(19,4),
    @is_employers_liability tinyint,
    @el_indemnity_limit numeric(19,4),
    @is_residential_units tinyint,
    @ru_contents_sum_insured numeric(19,4),
    @is_engineering tinyint,
    @e_property_insured varchar(60),

    @e_contingencies varchar(60),
    @e_indemnity_limit numeric(19,4),
    @e_excess numeric(19,4)
AS
BEGIN
INSERT INTO property_owners (
    insurance_file_cnt ,
    is_buildings ,
    b_sum_insured ,
    b_contents_sum_insured ,
    b_excess ,
    is_public_liability ,
    pl_indemnity_limit ,
    is_employers_liability ,
    el_indemnity_limit ,
    is_residential_units ,
    ru_contents_sum_insured ,
    is_engineering ,
    e_property_insured ,
    e_contingencies ,
    e_indemnity_limit ,
    e_excess )
VALUES (
    @insurance_file_cnt,
    @is_buildings,
    @b_sum_insured,
    @b_contents_sum_insured,
    @b_excess,
    @is_public_liability,
    @pl_indemnity_limit,
    @is_employers_liability,
    @el_indemnity_limit,
    @is_residential_units,
    @ru_contents_sum_insured,
    @is_engineering,
    @e_property_insured,
    @e_contingencies,
    @e_indemnity_limit,
    @e_excess)
END

GO


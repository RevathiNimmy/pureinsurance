SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_property_owners_sel'
GO

CREATE PROCEDURE spe_event_property_owners_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    is_buildings,
    b_sum_insured,
    b_contents_sum_insured,
    b_excess,
    is_public_liability,
    pl_indemnity_limit,
    is_employers_liability,
    el_indemnity_limit,
    is_residential_units,
    ru_contents_sum_insured,
    is_engineering,
    e_property_insured,
    e_contingencies,
    e_indemnity_limit,
    e_excess
FROM event_property_owners
WHERE insurance_file_cnt = @insurance_file_cnt

GO


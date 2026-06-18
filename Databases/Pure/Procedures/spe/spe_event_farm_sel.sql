SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_farm_sel'
GO

CREATE PROCEDURE spe_event_farm_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    address_cnt,
    is_property_damage,
    pd_produce_sum_insured,
    pd_poultry_sum_insured,
    pd_buildings_sum_insured,
    pd_misc_sum_insured,
    pd_liability_sum_insured,
    is_business_interruption,
    bi_description,
    bi_sum_insured,
    bi_indemnity_period,
    is_livestock,
    l_sum_insured,
    l_in_transit_limit,
    l_disease_sum_insured,
    is_farm_liabilities,
    fl_wages_estimate,
    fl_employers_liability_limit,
    fl_public_liability_limit,
    is_money_and_assault,
    ma_money_in_transit,
    ma_money,
    ma_not_in_safe,
    ma_in_dwellings,
    ma_locked,
    ma_any_other,
    is_personal_accident,
    pa_insured_person,
    pa_age,
    pa_duties,
    pa_compensation,
    is_engineering,
    e_property_insured,
    e_cover,
    e_indemnity_limit,
    e_excess,
    is_uncollected_milk,
    um_days_compensation,
    um_compensation,
    um_number_of_cows,
    is_farm_home,
    fh_buildings_sum_insured,
    fh_contents_sum_insured,
    fh_possessions_sum_insured,
    fh_unspecified_sum_insured,
    fh_specified_sum_insured,
    fh_buildings_excess,
    fh_subsidence_excess,
    fh_contents_excess,
    is_farm_motor,
    fm_year,
    fm_registration_number,
    fm_value,
    fm_cover
FROM event_farm
WHERE insurance_file_cnt = @insurance_file_cnt

GO


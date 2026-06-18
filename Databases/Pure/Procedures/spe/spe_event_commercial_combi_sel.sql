SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_commercial_combi_sel'
GO

CREATE PROCEDURE spe_event_commercial_combi_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    is_fire_damage,
    fd_buildings_sum,
    fd_stock_and_materials,
    is_all_risks,
    ar_buildings_sum,
    landlord_f_and_f_sum,
    stock_in_trade_sum,
    property_sum_insured,
    is_burglary,
    machinery_sum_insured,
    b_stock_and_materials,
    s_i_t_tobacco,
    s_i_t_alcohol,
    s_i_t_radio,
    s_i_t_non_ferrous,
    is_business_interruption,
    gross_profit_sum,
    maximum_indemnity_period,
    is_glass,
    glass_type,
    glass_cover_amount,
    is_employers_liability,
    e_indemnity_limit,
    e_description,
    wages_and_salaries,
    is_public_liability,
    pl_indemnity_limit,
    pl_description,
    pl_rating_basis,
    pl_excess,
    is_money_and_assault,
    ma_cards_limit,
    ma_not_locked,
    ma_locked,
    ma_premises,
    ma_any_other,
    ma_insured_persons,
    ma_compensation_amount,
    is_goods_in_transit,
    git_make_of_vehicle,
    git_registration_number,
    git_tools_limit,
    git_liability_limit,
    git_retained_liability,
    is_personal_accident,
    pa_insured_person,
    pa_age,
    pa_duties,
    pa_compensation,
    is_supp_all_risks,
    sar_property_insured,
    sar_sum_insured,
    sar_liability_limit,
    is_engineering,
    e_property_insured,
    e_cover,
    e_liability_limit,
    e_excess,
    is_frozen_food,
    ff_unit_description,
    ff_date_of_make,
    ff_sum_insured,
    ff_liability_limit
FROM event_commercial_combined
WHERE insurance_file_cnt = @insurance_file_cnt

GO


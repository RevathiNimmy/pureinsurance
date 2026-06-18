SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_shop_sel'
GO

CREATE PROCEDURE spe_shop_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    address_cnt,
    is_property_damage,
    buildings_sum_insured,
    improvements_sum_insured,
    contents_sum_insured,
    s_i_t_sum_insured,
    trade_f_a_f_sum_insured,
    stock_of_tobacco_sum_insured,
    stock_of_alcohol_sum_insured,
    is_business_interruption,
    loss_of_income_sum_insured,
    maximum_indemnity_period,
    is_glass,
    glass_type,
    cover_amount,
    is_frozen_foods,
    ff_description,
    ff_sum_insured,
    is_all_risks,
    ar_description,
    ar_sum_insured,
    is_loss_of_licence,
    ll_liability_limit
 FROM shop
WHERE insurance_file_cnt = @insurance_file_cnt

GO


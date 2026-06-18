SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_shop_add'
GO

CREATE PROCEDURE spe_shop_add
    @insurance_file_cnt int,
    @address_cnt int,
    @is_property_damage tinyint,
    @buildings_sum_insured numeric(19,4),
    @improvements_sum_insured numeric(19,4),
    @contents_sum_insured numeric(19,4),
    @s_i_t_sum_insured numeric(19,4),
    @trade_f_a_f_sum_insured numeric(19,4),
    @stock_of_tobacco_sum_insured numeric(19,4),
    @stock_of_alcohol_sum_insured numeric(19,4),
    @is_business_interruption tinyint,
    @loss_of_income_sum_insured numeric(19,4),
    @maximum_indemnity_period int,
    @is_glass tinyint,
    @glass_type int,
    @cover_amount numeric(19,4),
    @is_frozen_foods tinyint,
    @ff_description varchar(60),
    @ff_sum_insured numeric(19,4),
    @is_all_risks tinyint,

    @ar_description varchar(60),
    @ar_sum_insured numeric(19,4),
    @is_loss_of_licence tinyint,
    @ll_liability_limit numeric(19,4)
AS
BEGIN
INSERT INTO shop (
    insurance_file_cnt ,
    address_cnt ,
    is_property_damage ,
    buildings_sum_insured ,
    improvements_sum_insured ,
    contents_sum_insured ,
    s_i_t_sum_insured ,
    trade_f_a_f_sum_insured ,
    stock_of_tobacco_sum_insured ,
    stock_of_alcohol_sum_insured ,
    is_business_interruption ,
    loss_of_income_sum_insured ,
    maximum_indemnity_period ,
    is_glass ,
    glass_type ,
    cover_amount ,
    is_frozen_foods ,
    ff_description ,
    ff_sum_insured ,
    is_all_risks ,
    ar_description ,
    ar_sum_insured ,
    is_loss_of_licence ,
    ll_liability_limit )
VALUES (
    @insurance_file_cnt,
    @address_cnt,
    @is_property_damage,
    @buildings_sum_insured,
    @improvements_sum_insured,
    @contents_sum_insured,
    @s_i_t_sum_insured,
    @trade_f_a_f_sum_insured,
    @stock_of_tobacco_sum_insured,
    @stock_of_alcohol_sum_insured,
    @is_business_interruption,
    @loss_of_income_sum_insured,
    @maximum_indemnity_period,
    @is_glass,
    @glass_type,
    @cover_amount,
    @is_frozen_foods,
    @ff_description,
    @ff_sum_insured,
    @is_all_risks,
    @ar_description,
    @ar_sum_insured,
    @is_loss_of_licence,
    @ll_liability_limit)
END

GO


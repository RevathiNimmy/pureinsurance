SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Offices_add'
GO

CREATE PROCEDURE spe_Offices_add
    @insurance_file_cnt int,
    @is_buildings tinyint,
    @b_buildings_sum_insured numeric(19,4),
    @b_tenants_sum_insured numeric(19,4),
    @b_liability_limit numeric(19,4),
    @is_contents tinyint,
    @c_contents_sum_insured numeric(19,4),
    @c_visitors_sum_insured numeric(19,4),
    @c_art_sum_insured numeric(19,4),
    @c_a_a_t_sum_insured numeric(19,4),
    @c_samples_sum_insured numeric(19,4),
    @c_signs_sum_insured numeric(19,4),
    @c_liability_limit numeric(19,4),
    @is_business_interruption tinyint,
    @bi_loss_of_income_sum_insured numeric(19,4),
    @bi_expenditure_sum_insured numeric(19,4),
    @bi_max_indemnity_period int,
    @is_glass tinyint,
    @g_cover_amount numeric(19,4),
    @is_debit_balances tinyint,
    @db_sum_insured numeric(19,4)
AS
BEGIN
INSERT INTO Offices (
    insurance_file_cnt ,
    is_buildings ,
    b_buildings_sum_insured ,
    b_tenants_sum_insured ,
    b_liability_limit ,
    is_contents ,
    c_contents_sum_insured ,
    c_visitors_sum_insured ,
    c_art_sum_insured ,
    c_a_a_t_sum_insured ,
    c_samples_sum_insured ,
    c_signs_sum_insured ,
    c_liability_limit ,
    is_business_interruption ,
    bi_loss_of_income_sum_insured ,
    bi_expenditure_sum_insured ,
    bi_max_indemnity_period ,
    is_glass ,
    g_cover_amount ,
    is_debit_balances ,
    db_sum_insured )
VALUES (
    @insurance_file_cnt,
    @is_buildings,
    @b_buildings_sum_insured,
    @b_tenants_sum_insured,
    @b_liability_limit,
    @is_contents,
    @c_contents_sum_insured,
    @c_visitors_sum_insured,
    @c_art_sum_insured,
    @c_a_a_t_sum_insured,
    @c_samples_sum_insured,
    @c_signs_sum_insured,
    @c_liability_limit,
    @is_business_interruption,
    @bi_loss_of_income_sum_insured,
    @bi_expenditure_sum_insured,
    @bi_max_indemnity_period,
    @is_glass,
    @g_cover_amount,
    @is_debit_balances,
    @db_sum_insured)
END

GO


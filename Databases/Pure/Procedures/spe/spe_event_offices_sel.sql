SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_offices_sel'
GO

CREATE PROCEDURE spe_event_offices_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    is_buildings,
    b_buildings_sum_insured,
    b_tenants_sum_insured,
    b_liability_limit,
    is_contents,
    c_contents_sum_insured,
    c_visitors_sum_insured,
    c_art_sum_insured,
    c_a_a_t_sum_insured,
    c_samples_sum_insured,
    c_signs_sum_insured,
    c_liability_limit,
    is_business_interruption,
    bi_loss_of_income_sum_insured,
    bi_expenditure_sum_insured,
    bi_max_indemnity_period,
    is_glass,
    g_cover_amount,
    is_debit_balances,
    db_sum_insured
FROM event_offices
WHERE insurance_file_cnt = @insurance_file_cnt

GO


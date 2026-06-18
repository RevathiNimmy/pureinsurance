SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_offices'
GO


CREATE PROCEDURE spu_wp_offices
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT,
    @is_buildings TinyInt OUTPUT,
    @b_buildings_sum_insured Numeric,
    @b_tenants_sum_insured Numeric,
    @b_liability_limit Numeric,
    @is_contents TinyInt OUTPUT,
    @c_contents_sum_insured Numeric,
    @c_visitors_sum_insured Numeric,
    @c_art_sum_insured Numeric,
    @c_a_a_t_sum_insured Numeric,
    @c_samples_sum_insured Numeric,
    @c_signs_sum_insured Numeric,
    @c_liability_limit Numeric,
    @is_business_interruption TinyInt OUTPUT,
    @bi_loss_of_income_sum_insured Numeric,
    @bi_expenditure_sum_insured Numeric,
    @bi_max_indemnity_period Int OUTPUT,
    @is_glass TinyInt OUTPUT,
    @g_cover_amount Numeric,
    @is_debit_balances TinyInt OUTPUT,
    @db_sum_insured Numeric
AS


SELECT
    @is_buildings = offices.is_buildings,
    @b_buildings_sum_insured = offices.b_buildings_sum_insured,
    @b_tenants_sum_insured = offices.b_tenants_sum_insured,
    @b_liability_limit = offices.b_liability_limit,
    @is_contents = offices.is_contents,
    @c_contents_sum_insured = offices.c_contents_sum_insured,
    @c_visitors_sum_insured = offices.c_visitors_sum_insured,
    @c_art_sum_insured = offices.c_art_sum_insured,
    @c_a_a_t_sum_insured = offices.c_a_a_t_sum_insured,
    @c_samples_sum_insured = offices.c_samples_sum_insured,
    @c_signs_sum_insured = offices.c_signs_sum_insured,
    @c_liability_limit = offices.c_liability_limit,
    @is_business_interruption = offices.is_business_interruption,
    @bi_loss_of_income_sum_insured = offices.bi_loss_of_income_sum_insured,
    @bi_expenditure_sum_insured = offices.bi_expenditure_sum_insured,
    @bi_max_indemnity_period = offices.bi_max_indemnity_period,
    @is_glass = offices.is_glass,
    @g_cover_amount = offices.g_cover_amount,
    @is_debit_balances = offices.is_debit_balances,
    @db_sum_insured = offices.db_sum_insured

FROM offices
WHERE offices.insurance_file_cnt = @insurancefilecnt
GO



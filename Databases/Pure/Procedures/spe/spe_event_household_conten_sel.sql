SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_household_conten_sel'
GO

CREATE PROCEDURE spe_event_household_conten_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    address_cnt,
    sum_insured,
    cover_type,
    is_all_risks_cover,
    all_risks_sum_insured,
    is_freezer_cover,
    freezer_sum_insured,
    is_credit_card_cover,
    credit_card_limit,
    is_money_cover,
    money_limit,
    is_personal_possessions,
    personal_possessions_sum,
    number_of_bedrooms,
    type_of_property,
    excess
FROM event_household_contents
WHERE insurance_file_cnt = @insurance_file_cnt

GO


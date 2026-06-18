SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_household_contents_add'
GO

CREATE PROCEDURE spe_household_contents_add
    @insurance_file_cnt int,
    @address_cnt int,
    @sum_insured int,
    @cover_type varchar(20),
    @is_all_risks_cover tinyint,
    @all_risks_sum_insured numeric(19,4),
    @is_freezer_cover tinyint,
    @freezer_sum_insured numeric(19,4),
    @is_credit_card_cover tinyint,
    @credit_card_limit numeric(19,4),
    @is_money_cover tinyint,
    @money_limit numeric(19,4),
    @is_personal_possessions tinyint,
    @personal_possessions_sum numeric(19,4),
    @number_of_bedrooms int,
    @type_of_property varchar(70),
    @excess numeric(19,4)
AS
BEGIN
INSERT INTO household_contents (
    insurance_file_cnt ,
    address_cnt ,
    sum_insured ,
    cover_type ,
    is_all_risks_cover ,
    all_risks_sum_insured ,
    is_freezer_cover ,
    freezer_sum_insured ,
    is_credit_card_cover ,
    credit_card_limit ,
    is_money_cover ,
    money_limit ,
    is_personal_possessions ,
    personal_possessions_sum ,
    number_of_bedrooms ,
    type_of_property ,
    excess )
VALUES (
    @insurance_file_cnt,
    @address_cnt,
    @sum_insured,
    @cover_type,
    @is_all_risks_cover,
    @all_risks_sum_insured,
    @is_freezer_cover,
    @freezer_sum_insured,
    @is_credit_card_cover,
    @credit_card_limit,
    @is_money_cover,
    @money_limit,
    @is_personal_possessions,
    @personal_possessions_sum,
    @number_of_bedrooms,
    @type_of_property,
    @excess)
END

GO


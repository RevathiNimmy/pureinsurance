SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Peril_upd'
GO

CREATE PROCEDURE spe_Peril_upd
    @insurance_file_cnt int,
    @risk_id int,
    @rating_section_id int,
    @peril_id int,
    @peril_type_id int,
    @class_of_business_id int,
    @sequence_number int,
    @description varchar(30),
    @sum_insured numeric(21,6),
    @rating_sum_insured numeric(21,6),
    @rate_type_id int,
    @annual_rate numeric(21,6),
    @annual_premium numeric(21,6),
    @this_premium numeric(21,6),
    @coinsured_this_premium numeric(21,6),
    @coinsured_sum_insured numeric(21,6),
    @coinsured_commission numeric(21,6),
    @retained_this_premium numeric(21,6),
    @retained_sum_insured numeric(21,6),
    @lead_commission_band tinyint,
    @sub_commission_band tinyint,
    @lead_commission_value numeric(21,6),
    @sub_commission_value numeric(21,6),
    @tax_group tinyint,
    @tax_value numeric(21,6),
    @ri_band int,
    @xl_band int

AS
BEGIN

UPDATE Peril
    SET
    peril_type_id = @peril_type_id,
    class_of_business_id = @class_of_business_id,
    sequence_number = @sequence_number,
    description = @description,
    sum_insured = @sum_insured,
    rating_sum_insured = @rating_sum_insured,
    rate_type_id = rate_type_id,
    annual_rate = @annual_rate,
    annual_premium = @annual_premium,
    this_premium = @this_premium,
    coinsured_this_premium = @coinsured_this_premium,
    coinsured_sum_insured = @coinsured_sum_insured,
    coinsured_commission = @coinsured_commission,
    retained_this_premium = @retained_this_premium,
    retained_sum_insured = @retained_sum_insured,
    lead_commission_band = @lead_commission_band,
    sub_commission_band = @sub_commission_band,
    lead_commission_value = @lead_commission_value,
    sub_commission_value = @sub_commission_value,
    tax_group = @tax_group, 
    tax_value = @tax_value,
    ri_band = @ri_band,
    xl_band = @xl_band

WHERE   risk_cnt = @risk_id
AND rating_section_id = @rating_section_id
AND peril_id = @peril_id

END

GO


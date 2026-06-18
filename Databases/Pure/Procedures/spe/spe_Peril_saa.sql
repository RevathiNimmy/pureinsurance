SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Peril_saa'
GO

CREATE PROCEDURE spe_Peril_saa
    @insurance_file_cnt int,
    @risk_id int,
    @rating_section_id int
AS

SELECT
    risk_cnt,
    rating_section_id,
    peril_id,
    peril_type_id,
    class_of_business_id,
    sequence_number,
    description,
    sum_insured,
    rating_sum_insured,
    rate_type_id,
    annual_rate,
    annual_premium,
    this_premium,
    coinsured_this_premium,
    coinsured_sum_insured,
    coinsured_commission,
    retained_this_premium,
    retained_sum_insured,
    lead_commission_band,
    sub_commission_band,
    lead_commission_value,
    sub_commission_value,
    tax_group,
    tax_value,
    ri_band,
    xl_band
 FROM Peril

WHERE   risk_cnt = @risk_id
AND rating_section_id = @rating_section_id
ORDER BY peril_id ASC

GO


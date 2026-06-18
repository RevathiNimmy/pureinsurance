SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Copy_Peril_Reversal'
GO

create PROCEDURE spu_Copy_Peril_Reversal  
    @OldRiskCnt int,
    @NewRiskCnt int
AS

/*******************************************************************************************/
/*  Created by RWH(23/11/2000)  to copy Peril records for one     */
/*                                                    risk to another                                             */
/*******************************************************************************************/
INSERT INTO Peril
SELECT @NewRiskCnt,
    rating_section_id,
    peril_id,
    peril_type_id,
    class_of_business_id,
    sequence_number,
    description,
    -sum_insured,
    -rating_sum_insured,
    rate_type_id,
    annual_rate,
    -annual_premium,
   -this_premium,
    -coinsured_this_premium,
    -coinsured_sum_insured,
    -coinsured_commission,
    -retained_this_premium,
    -retained_sum_insured,
    lead_commission_band,
    sub_commission_band,
    lead_commission_value,
    sub_commission_value,
    tax_group,
    tax_value,
    ri_band,
    xl_band,
    is_premium,
    is_sum_insured,
    is_levy_tax,	-- Thinh Nguyen 15/08/2002
    is_taxed		-- Thinh Nguyen 15/08/2002
FROM Peril
WHERE risk_cnt = @OldRiskCnt
GO



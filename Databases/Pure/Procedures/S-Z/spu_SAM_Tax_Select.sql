SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Tax_Select'
GO
-- =============================================
-- Author:		<Dharmendra Kumar>
-- Create date: <15/06/2012>
-- Description:	<Select colums from Tax_Calculation>
-- =============================================
CREATE PROCEDURE spu_SAM_Tax_Select    
 -- Add the parameters for the stored procedure here    
  @insurance_file_cnt INT    
    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
     DECLARE @effective_date datetime
	 SELECT @effective_date = GetDate()
   SELECT    
       tc.tax_calculation_cnt,    
       isnull(tc.risk_cnt,0) as risk_cnt,    
       tc.tax_band_id,    
       isnull(tc.premium,0) as premium,    
       tc.is_value,    
       isnull(tc.percentage,0) as percentage,    
       isnull(tc.value,0) as value,    
       tc.is_manually_changed,    
       isnull(tc.Calc_Basis,0)as Calc_Basis,    
       isnull(tc.Basis_Value,0) as Basis_Value,    
       isnull(tc.sum_insured,0) as sum_insured,    
       isnull(tc.Sum_Insured_Rounded,0)as Sum_Insured_Rounded,    
       isnull(tc.currency_id,0) as currency_id,    
       tc.allow_tax_credit,    
       isnull(tc.original_sum_insured,0)as original_sum_insured,    
       isnull(tc.country_id,0) as country_id,    
       isnull(tc.state_id,0)as state_id,    
       isnull(tc.class_of_business_id,0) as class_of_business_id,    
       isnull(tc.tax_group_id,0)as tax_group_id,    
       isnull(tc.sequence,0)as sequence,    
       isnull(tc.policy_fee_u_id,0)as policy_fee_u_id,    
       isnull(tc.agent_commission_cnt,0)as agent_commission_cnt,    
       isnull(tc.ri_party_cnt,0)as ri_party_cnt,    
       isnull(tc.ri_arrangement_line_id,0)as ri_arrangement_line_id,    
       isnull(tc.insurance_section_id,0)as insurance_section_id,    
       isnull(tc.policy_fee_id,0)as policy_fee_id,    
       isnull(tc.policy_agents_id,0)as policy_agents_id,    
       isnull(tc.insurer_party_cnt,0)as insurer_party_cnt,    
       isnull(tc.claim_peril_id,0)as claim_peril_id,    
       isnull(tc.claim_payment_id,0)as claim_payment_id,    
       isnull(claim_receipt_id,0)as claim_receipt_id,    
       isnull(tc.claim_payment_item_id,0) as claim_payment_item_id,    
       isnull(tc.claim_receipt_item_id,0) as claim_receipt_item_id,    
       isnull(tc.is_not_applied_to_client,0) as is_not_applied_to_client ,    
       isnull(tc.include_tax_in_instalments,0) as include_tax_in_instalments ,    
       isnull(tc.spread_tax_across_instalments,0) as spread_tax_across_instalments,    
       isnull(tc.base_tax_calculation_cnt,0)as base_tax_calculation_cnt,    
       isnull(version_id,0) as version_id,    
       isnull(tc.pfprem_finance_cnt,0)as pfprem_finance_cnt,    
       isnull(tc.pfprem_finance_version,0)as pfprem_finance_version,    
       isnull(tc.policy_coinsurers_section_id,0)as policy_coinsurers_section_id,    
       isnull(tc.is_commission_tax,0)as is_commission_tax,    
       isnull(tc.apply_tax_by,0) as is_commission_tax,    
       isnull(tc.tax_band_rate_id,0)as tax_band_rate_id,    
       isnull(tc.is_suspended,0) as is_suspended,    
       Case tc.transtype    
       when 'TTR' then    
             'Risk Tax'    
          When 'TTIF' then    
              'Policy Tax'    
           end as transtype,    
       tb.code as taxbandcode,    
       isnull(tb.description,' ') as taxbandDescription,    
       isnull(tg.code,' ') as taxgroupcode,    
       ISNULL(tg.description,' ')as taxgroupDescription,    
       ISNULL(tbr.description,' ') as TaxBandRateDescription  
       from Tax_Calculation tc    
       Join tax_band tb    
       on tb.tax_band_id=tc.tax_band_id    
       Join tax_band_rate tbr  
       on tbr.code=tb.code  
       join Tax_Group tg    
       on tg.tax_group_id=tc.tax_group_id    
       where insurance_file_cnt=@insurance_file_cnt and transtype in('TTIF','TTR')    
       AND     tbr.effective_date =(SELECT  MAX(effective_date)  
                                            FROM    tax_band_rate  
                                            WHERE  
                                                tbr.code=code
												AND  effective_date <= @effective_date  
                                                AND is_deleted = 0)  
END 
Go
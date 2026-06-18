SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Risk_Tax_Copy' 
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date: 16/09/2014
-- Description:	Copies the risk tax between insurance files
-- =============================================
CREATE PROCEDURE spu_Risk_Tax_Copy
	@risk_cnt INT,
	@insurance_File_Cnt INT,
	@source_risk_cnt INT,
	@source_insurance_file_cnt INT
AS
BEGIN
		
	-- DELETE Current Risk Taxes if there are any
	DELETE Tax_Calculation 
        WHERE risk_cnt = @risk_cnt AND insurance_file_cnt = @Insurance_File_Cnt
	
	SELECT @source_insurance_file_cnt= MAX(insurance_file_cnt) 
        FROM   insurance_file_risk_link
        WHERE  risk_cnt = @source_risk_cnt AND  status_flag <> 'U'  
               AND insurance_file_cnt <> @insurance_File_Cnt
	
	-- Copy the taxes between two versions
	INSERT INTO Tax_Calculation
           (risk_cnt,
           tax_band_id,
           premium,
           percentage,
           value,
           is_value,
           is_manually_changed,
           Calc_Basis,
           Basis_Value,
           Sum_Insured,
           Sum_Insured_Rounded,
           currency_id,
           allow_tax_credit,
           original_sum_insured,
           country_id,
           state_id,
           class_of_business_id,
           tax_group_id,
           sequence,
           insurance_file_cnt,
           transtype,
           policy_fee_u_id,
           agent_commission_cnt,
           ri_party_cnt,
           ri_arrangement_line_id,
           insurance_section_id,
           policy_fee_id,
           policy_agents_id,
           insurer_party_cnt,
           claim_peril_id,
           claim_payment_id,
           claim_receipt_id,
           claim_payment_item_id,
           claim_receipt_item_id,
           is_not_applied_to_client,
           include_tax_in_instalments,
           spread_tax_across_instalments,
           base_tax_calculation_cnt,
           version_id,
           pfprem_finance_cnt,
           pfprem_finance_version,
           policy_coinsurers_section_id,
           is_commission_tax,
           apply_tax_by,
           tax_band_rate_id,
           is_suspended)
     SELECT @risk_cnt,
           tax_band_id,
           premium,
           percentage,
           value,
           is_value,
           is_manually_changed,
           Calc_Basis,
           Basis_Value,
           Sum_Insured,
           Sum_Insured_Rounded,
           currency_id,
           allow_tax_credit,
           original_sum_insured,
           country_id,
           state_id,
           class_of_business_id,
           tax_group_id,
           sequence,
           @insurance_File_Cnt,
           transtype,
           policy_fee_u_id,
           agent_commission_cnt,
           ri_party_cnt,
           ri_arrangement_line_id,
           insurance_section_id,
           policy_fee_id,
           policy_agents_id,
           insurer_party_cnt,
           claim_peril_id,
           claim_payment_id,
           claim_receipt_id,
           claim_payment_item_id,
           claim_receipt_item_id,
           is_not_applied_to_client,
           include_tax_in_instalments,
           spread_tax_across_instalments,
           base_tax_calculation_cnt,
           version_id,
           pfprem_finance_cnt,
           pfprem_finance_version,
           policy_coinsurers_section_id,
           is_commission_tax,
           apply_tax_by,
           tax_band_rate_id,
           is_suspended
   FROM    Tax_Calculation
   WHERE   risk_cnt = @source_risk_cnt 
   AND	  insurance_file_cnt = @source_insurance_File_Cnt	
END

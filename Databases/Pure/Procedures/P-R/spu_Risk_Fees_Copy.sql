SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Risk_Fees_Copy' 
GO

-- =============================================  
-- Author: Vidya Rangdale
-- Create date:  12/09/2014
-- Description: Copies the risk fees between insurance files  
-- =============================================  
CREATE PROCEDURE spu_Risk_Fees_Copy
 @risk_cnt      INT,  
 @insurance_File_Cnt    INT,  
 @source_risk_cnt    INT,  
 @source_insurance_file_cnt  INT  
AS  
BEGIN  
   

 DELETE Tax_Calculation  
 FROM   Tax_Calculation  
 INNER  JOIN Policy_Fee_U  
 ON     Policy_Fee_U.policy_fee_u_id = Tax_Calculation.policy_fee_u_id  
 WHERE  Policy_Fee_U.insurance_file_cnt = @Insurance_File_Cnt AND Policy_Fee_U.risk_cnt = @risk_cnt  
  
 DELETE Policy_Fee_U WHERE insurance_file_cnt = @Insurance_File_Cnt AND risk_cnt = @risk_cnt  
  
 --If there was an MTA then copy find the risk before the MTA if it was unchanged  
 SELECT @source_insurance_file_cnt= MAX(insurance_file_cnt) 
 FROM   insurance_file_risk_link where risk_cnt = @source_risk_cnt AND  status_flag <> 'U'  
 AND    insurance_file_cnt <> @insurance_File_Cnt  
  
 DECLARE @policy_fee_u_id INT  
 DECLARE @policy_fee_u_id_new INT  
 DECLARE fee_cursor CURSOR FAST_FORWARD  
  FOR  
    SELECT policy_fee_u_id  
    FROM   Policy_Fee_U 
    WHERE insurance_file_cnt = @source_insurance_file_cnt AND risk_cnt = @source_risk_cnt  
  
 OPEN fee_cursor  
   FETCH NEXT FROM fee_cursor  
   INTO @policy_fee_u_id  
 WHILE @@FETCH_STATUS = 0  
  BEGIN  
   SELECT @policy_fee_u_id_new = NULL  
   INSERT INTO policy_fee_u  
    ([insurance_file_cnt]  
    ,[party_cnt]  
    ,[fee_rate_percentage]  
    ,[fee_rate_amount]  
    ,[currency_id]  
    ,[transaction_type_id]  
    ,[product_id]  
    ,[branch_id]  
    ,[risk_cnt]  
    ,[base_currency_id]  
    ,[base_fee_amount]  
    ,[base_tax_amount]  
    ,[currency_amount]  
    ,[currency_tax_amount]  
    ,[risk_type_group_id]  
    ,[peril_group_id]  
    ,[tax_group_id]  
    ,[transaction_sub_type]  
    ,[is_fee_applied_to_cr]  
    ,[fee_rate_currency_id]  
    ,[Fee_Premium]  
    ,[include_fee_in_instalments]  
    ,[spread_fee_across_instalments]  
    ,[MakeLiveOptions_id]  
    ,[DoPaymentTerms_id]  
    ,[Calculation_Basis]  
    ,[Is_Prorated]  
    ,[Pro_rata_rate]  
    ,[Is_Override]
    ,[FeeTypePercent])  
 SELECT  
    @insurance_File_Cnt  
    ,[party_cnt]  
    ,[fee_rate_percentage]  
    ,[fee_rate_amount]  
    ,[currency_id]  
    ,[transaction_type_id]  
    ,[product_id]  
    ,[branch_id]  
    ,@risk_cnt  
    ,[base_currency_id]  
    ,[base_fee_amount]  
    ,[base_tax_amount]  
    ,[currency_amount]  
    ,[currency_tax_amount]  
    ,[risk_type_group_id]  
    ,[peril_group_id]  
    ,[tax_group_id]  
    ,[transaction_sub_type]  
    ,[is_fee_applied_to_cr]  
    ,[fee_rate_currency_id]  
    ,[Fee_Premium]  
    ,[include_fee_in_instalments]  
    ,[spread_fee_across_instalments]  
    ,[MakeLiveOptions_id]  
    ,[DoPaymentTerms_id]  
    ,[Calculation_Basis]  
    ,[Is_Prorated]  
    ,[Pro_rata_rate]  
    ,[Is_Override]
    ,[FeeTypePercent]
FROM  policy_fee_u  
WHERE policy_fee_u.policy_fee_u_id = @policy_fee_u_id  
  
   --Get the New Identity  
   SELECT @policy_fee_u_id_new = SCOPE_IDENTITY()  
  
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
   SELECT  
      @risk_cnt,  
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
      @policy_fee_u_id_new,  
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
FROM  Tax_Calculation  
WHERE policy_fee_u_id = @policy_fee_u_id 
AND   @policy_fee_u_id IS NOT NULL  
  			
   FETCH NEXT FROM fee_cursor INTO @policy_fee_u_id  
END  
CLOSE fee_cursor  
DEALLOCATE fee_cursor  
  
END  

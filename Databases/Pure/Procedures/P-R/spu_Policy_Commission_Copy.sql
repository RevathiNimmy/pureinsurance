SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Policy_Commission_Copy' 
GO

-- =============================================
-- Author:Vidya Rangdale
-- Create date:  12/09/2014
-- Description:	Copies the risk fees between insurance files
-- =============================================
CREATE PROCEDURE spu_Policy_Commission_Copy
	@insurance_File_Cnt	   INT,
	@source_insurance_file_cnt INT
AS
BEGIN
		
	-- DELETE Current Risk Fees if there are any
	DELETE	Tax_Calculation 
	FROM	Tax_Calculation 
	INNER JOIN Agent_Commission
		ON Agent_Commission.agent_commission_cnt = Tax_Calculation.agent_commission_cnt
	WHERE Agent_Commission.insurance_file_cnt = @Insurance_File_Cnt 
	
	DELETE Lead_Commission WHERE Lead_Commission.insurance_file_cnt = @insurance_File_Cnt
		
	DELETE Agent_Commission WHERE Agent_Commission.insurance_file_cnt = @Insurance_File_Cnt  
	
	DECLARE @agent_commission_cnt INT
	DECLARE @agent_commission_cnt_new INT 
	DECLARE commission_cursor CURSOR FAST_FORWARD FOR
		SELECT agent_commission_cnt
		FROM   Agent_Commission WHERE insurance_file_cnt = @source_insurance_file_cnt  

	OPEN commission_cursor
	FETCH NEXT FROM commission_cursor
		INTO @agent_commission_cnt 
	WHILE @@FETCH_STATUS = 0 
		BEGIN 
			SELECT @agent_commission_cnt_new = NULL
			INSERT INTO  Agent_Commission
			   ([insurance_file_cnt]
			   ,[is_lead_agent]
			   ,[party_cnt]
			   ,[risk_type_id]
			   ,[commission_band_id]
			   ,[premium]
			   ,[commission_percentage]
			   ,[commission_value]
			   ,[is_amended]
			   ,[account_currency_id]
			   ,[account_commission_value]
			   ,[base_currency_id]
			   ,[base_commission_value]
			   ,[tax_group_id]
			   ,[tax_amount]
			   ,[tax_account_amount]
			   ,[tax_base_amount]
			   ,[override_reason]
			   ,[calculated_commission_value]
			   ,[Maximum_rate]
			   ,[is_value]
			   ,[is_tax_amended])
			SELECT
			   @Insurance_File_Cnt
			   ,is_lead_agent
			   ,party_cnt
			   ,risk_type_id
			   ,commission_band_id
			   ,premium
			   ,commission_percentage
			   ,commission_value
			   ,is_amended
			   ,account_currency_id
			   ,account_commission_value
			   ,base_currency_id
			   ,base_commission_value
			   ,tax_group_id
			   ,tax_amount
			   ,tax_account_amount
			   ,tax_base_amount
			   ,override_reason
			   ,calculated_commission_value
			   ,Maximum_rate
			   ,is_value
			   ,is_tax_amended
			FROM Agent_Commission
			WHERE Agent_Commission.agent_commission_cnt= @agent_commission_cnt		
			
			--Get the New Identity
			SELECT @agent_commission_cnt_new = SCOPE_IDENTITY()
			
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
			   risk_cnt,
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
			   @agent_commission_cnt_new,
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
		   FROM   Tax_Calculation
		   WHERE  agent_commission_cnt = @agent_commission_cnt
		   AND	  @agent_commission_cnt IS NOT NULL

			
			FETCH NEXT FROM commission_cursor INTO @agent_commission_cnt 
		END 
	CLOSE commission_cursor 
	DEALLOCATE commission_cursor

	INSERT INTO Lead_Commission
                    ([insurance_file_cnt]
                    ,[commission_band]
                    ,[premium]
                    ,[percent]
                    ,[value]
                    ,[risk_type_id])
       SELECT
               @insurance_File_Cnt
               ,commission_band
               ,premium
               ,[percent]
               ,[value]
              ,risk_type_id
      FROM    Lead_Commission
      WHERE   [insurance_file_cnt] = @source_insurance_file_cnt
	
END

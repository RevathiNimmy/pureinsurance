SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_copy_void_agent_commission'
GO

Create PROCEDURE spu_copy_void_agent_commission  
    @OldInsuranceFileCnt int,  
    @NewInsuranceFileCnt int  
AS  
  
INSERT INTO agent_commission (
				insurance_file_cnt,
				is_lead_agent, 
				party_cnt, 
				risk_type_id,	
				commission_band_id,	
				premium, 
				commission_percentage,	
				commission_value,
				is_amended, 
				account_currency_id,
				account_commission_value, 
				base_currency_id,
				base_commission_value,
				tax_group_id, 
				tax_amount, 
				tax_account_amount, 
				tax_base_amount, 
				override_reason,
				calculated_commission_value,
				maximum_rate, 
				is_value,	
				is_tax_amended,
				peril_type_id, 
				class_of_business_id)

				SELECT  @NewInsuranceFileCnt, 
				is_lead_agent, 
				party_cnt,	
				risk_type_id, 
				commission_band_id, 
				isnull(premium,0) * -1,	
				commission_percentage,
				commission_value * -1,
				is_amended,	
				account_currency_id,
				isnull(account_commission_value,0) * -1,  -- establish xRate here only as was applied originally
				base_currency_id,
				isnull(base_commission_value,0) * -1,
				tax_group_id, 
				tax_amount * -1, 
				tax_account_amount * -1,	
				tax_base_amount * -1,
				override_reason,
				isnull(calculated_commission_value,0) * -1,
				maximum_rate, 
				is_value,	
				is_tax_amended, 
				peril_type_id,
				class_of_business_id
FROM    agent_commission    
    WHERE   insurance_file_cnt = @OldInsuranceFileCnt  
 
    
INSERT INTO lead_commission (    
        insurance_file_cnt,    
        commission_band,    
        premium,    
        [percent],    
        value,    
        risk_type_id)    
    SELECT  @NewInsuranceFileCnt,    
        commission_band,    
        premium *-1,    
        [percent],    
        value * -1,    
        risk_type_id    
    FROM    lead_commission    
    WHERE   insurance_file_cnt = @OldInsuranceFileCnt    
    

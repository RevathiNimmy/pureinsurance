SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Event_SIR_Calculate_Agent_Tax_Amounts_SFB'
GO


CREATE PROCEDURE spu_Event_SIR_Calculate_Agent_Tax_Amounts_SFB    
 @policy_agents_id int,
 @return_taxes tinyint    
    
AS    
    
BEGIN    
    
 DECLARE @tax_group_id int    
 DECLARE @total_tax_amount money    
 DECLARE @tax_base_amount money    
 DECLARE @agent_currency_id int    
 DECLARE @company_id int    
 DECLARE @agent_amount money    
 DECLARE @insurance_file_cnt int
 DECLARE @risk_cnt int
 DECLARE @RateEffectiveDate datetime 
 DECLARE @EffectiveDate datetime
-- Get the required policy agent details
SELECT @RateEffectiveDate=max(agr.effective_date) 
	FROM event_policy_agents pa
	JOIN event_insurance_file i on i.insurance_file_cnt  = pa.insurance_file_cnt
	JOIN risk_code rc on rc.risk_code_id = i.risk_code_id
	JOIN agent_group_rate agr on agr.party_cnt = pa.agent_cnt
			  and agr.risk_group_id = rc.risk_group_id   
WHERE policy_agents_id = @policy_agents_Id   
SELECT    
    @tax_group_id = agr.tax_group_id,    
    @agent_currency_id = pa.base_currency_id,    
    @company_id = i.source_id,    
    @agent_amount = pa.agent_commission_value,
    @insurance_file_cnt = pa.insurance_file_cnt, 
    @risk_cnt = NULL,
    @effectiveDate = i.cover_start_date    
FROM event_policy_agents pa
JOIN event_insurance_file i on i.insurance_file_cnt  = pa.insurance_file_cnt
JOIN risk_code rc on rc.risk_code_id = i.risk_code_id
JOIN agent_group_rate agr on agr.party_cnt = pa.agent_cnt
			  and agr.risk_group_id = rc.risk_group_id   
WHERE policy_agents_id = @policy_agents_Id
AND agr.effective_date = @RateEffectiveDate    
  
 -- delete any existing tax calculations for this item  
 -- delete any existing tax calculations for this item  
 DELETE FROM event_tax_calculation   
 WHERE policy_agents_id = @policy_agents_id    
   
-- Calculate the Tax and regenerate the tax_calculations    
EXEC spu_Event_SIR_Calculate_Tax_Amounts_SFB    
 @company_id=@company_id,    
 @tax_group_id=@tax_group_id,    
 @transtype='TTAC',    
 @currency_id=@agent_currency_id,    
 @amount=@agent_amount,    
 @tax_currency_amount=@total_tax_amount OUTPUT,    
 @tax_base_amount=@tax_base_amount OUTPUT,  
 @associated_key_id =@policy_agents_id,
 @insurance_file_cnt = @insurance_file_cnt, 
 @effective_date = @EffectiveDate
 

IF @return_taxes = 1
BEGIN
     SELECT  t.insurance_file_cnt,  
            t.tax_band_id,  
            t.premium,  
            t.percentage,  
            t.value,  
            t.is_value,  
            t.is_manually_changed,  
            tb.description,  
            tt.is_not_applied_to_client,  
        0, -- is deleted  
            t.basis_value,  
            t.calc_basis,  
            t.sum_insured,  
            t.sum_insured_rounded,  
            t.currency_id,  
            c.description,  
            t.allow_tax_credit,  
            t.original_sum_insured,  
            t.sum_insured - t.original_sum_insured,  
            tg.tax_group_id,  
            tg.description,  
            t.sequence,  
            ct.country_id,  
            ct.description,  
          s.state_id,  
            s.description,  
            cob.class_of_business_id,  
            cob.description,  
            0, -- running total  
      t.tax_calculation_cnt,  
      t.transtype  
    FROM    Event_Tax_Calculation t  
    JOIN    tax_band tb ON tb.tax_band_id = t.tax_band_id  
    JOIN    tax_type tt ON tt.tax_type_id = tb.tax_type_id  
    JOIN    currency c  ON c.currency_id = t.currency_id  
    LEFT JOIN  
            tax_group tg ON tg.tax_group_id = t.tax_group_id  
    LEFT JOIN  
            country ct ON ct.country_id = t.country_id  
    LEFT JOIN  
            state s ON s.state_id = t.state_id  
    LEFT JOIN  
            class_of_business cob ON cob.class_of_business_id = t.class_of_business_id  
    WHERE   t.policy_agents_id = @policy_agents_id  
    AND     tt.tax_basis = 2  
    AND     t.transtype = 'TTAC'
    ORDER BY  
            t.tax_group_id,  
            t.sequence  
END           
  
    
END
 
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

 
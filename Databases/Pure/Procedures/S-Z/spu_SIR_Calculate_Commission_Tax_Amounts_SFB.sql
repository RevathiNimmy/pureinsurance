SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Calculate_Commission_Tax_Amounts_SFB'
GO


CREATE PROCEDURE spu_SIR_Calculate_Commission_Tax_Amounts_SFB    
 @insurance_section_id  int,
 @return_taxes tinyint,
 @insurer_cnt int = 0    
    
AS    
    
BEGIN    
    
 DECLARE @tax_group_id int    
 DECLARE @total_tax_amount money    
 DECLARE @tax_base_amount money    
 DECLARE @commission_currency_id int    
 DECLARE @company_id int    
 DECLARE @commission_amount money    
 DECLARE @insurance_file_cnt int
 DECLARE @risk_cnt int
 DECLARE @effective_date datetime

/* New for CoInsurers to get tax group */
DECLARE @InsurerCnt as Integer	
DECLARE @EffectiveDate as datetime 
DECLARE @RiskSectionId integer
DECLARE @RiskCodeId integer
DECLARE @RiskGroupId integer
DECLARE @SchemeId integer


DECLARE @Rate1 numeric(19,4) 
DECLARE @Value1 numeric(19,4)
DECLARE @MinimumTotal1 numeric(19,4)
DECLARE @Rate2 numeric(19,4) 
DECLARE @Value2 numeric(19,4)
DECLARE @MinimumTotal2 numeric(19,4)
DECLARE @Rate3 numeric(19,4) 
DECLARE @Value3 numeric(19,4)
DECLARE @MinimumTotal3 numeric(19,4)
DECLARE @CommissionTaxGroupId integer


-- Get the required policy commission details 
IF @insurer_cnt = 0
	BEGIN   
	SELECT    
    	@tax_group_id = iCOBS.commission_tax_group_id,    
    	@commission_currency_id = i.currency_id,    
    	@company_id = i.source_id,    
    	@commission_amount = iCOBS.commission_net,
    	@insurance_file_cnt = iCOBS.insurance_file_cnt, 
    	@risk_cnt = NULL,
    	@effective_date = i.cover_start_date     
	FROM insurance_COB_section iCOBS
	JOIN insurance_file i on i.insurance_file_cnt  = iCOBS.insurance_file_cnt   
	WHERE insurance_section_id = @insurance_section_id
	END
ELSE
	BEGIN
	SELECT    
    	@risksectionid = iCOBs.COB_rating_section_id,    
    	@commission_currency_id = i.currency_id,    
    	@company_id = i.source_id,    
    	@commission_amount = po.coinsurer_net_commission,
    	@insurance_file_cnt = iCOBs.insurance_file_cnt, 
    	@risk_cnt = NULL,
	@effective_date = i.cover_start_date     
	FROM insurance_COB_section iCOBs
	JOIN insurance_file i on i.insurance_file_cnt  = iCOBs.insurance_file_cnt
	JOIN policy_coinsurers po on po.insurance_section_id = iCOBS.insurance_section_id   
	WHERE iCOBS.insurance_section_id = @insurance_section_id
	AND po.party_cnt = @insurer_cnt

	/* Get Tax Group Id */
	 
	SELECT @InsurerCnt=@Insurer_cnt,@SchemeId=I.scheme,@RiskGroupId =RC.risk_group_id,@RiskCodeId=I.risk_code_id  
			FROM risk_code RC
			JOIN Insurance_File I ON I.risk_code_id = RC.risk_code_id
			WHERE I.Insurance_File_Cnt = @Insurance_File_Cnt 
 
 
 
 	SELECT @EffectiveDate=@Effective_date
	exec spu_get_commissionrates @insurerCnt,@SchemeId,@RiskGroupId,@RiskCodeId,@RiskSectionId,@EffectiveDate,
	@Rate1 OUTPUT,@Value1 OUTPUT,@MinimumTotal1 OUTPUT,@Rate2 OUTPUT,@Value2 OUTPUT,@MinimumTotal2 OUTPUT,
	@Rate3 OUTPUT,@Value3 OUTPUT,@MinimumTotal3 OUTPUT,@CommissionTaxGroupId OUTPUT
	    
  	SELECT @tax_group_id=@CommissionTaxGroupId
 END
 -- delete any existing tax calculations for this item  
 DELETE FROM tax_calculation   
 WHERE insurance_section_id = @insurance_section_id
 AND transtype = 'TTIC'
 AND (insurer_party_cnt = @insurer_cnt OR @insurer_cnt = 0)
   
-- Calculate the Tax and regenerate the tax_calculations    
EXEC spu_SIR_Calculate_Tax_Amounts_SFB    
 @company_id=@company_id,    
 @tax_group_id=@tax_group_id,    
 @transtype='TTIC',    
 @currency_id=@Commission_currency_id,    
 @amount=@commission_amount,    
 @tax_currency_amount=@total_tax_amount OUTPUT,    
 @tax_base_amount=@tax_base_amount OUTPUT,  
 @associated_key_id =@insurance_section_id, 
 @insurance_file_cnt = @insurance_file_cnt,
 @insurer_cnt = @insurer_cnt,
 @effective_date = @effective_date 
        

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
    FROM    Tax_Calculation t  
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
    WHERE   t.Insurance_File_Cnt = @Insurance_File_Cnt  
    AND     tt.tax_basis = 2  
    AND     t.transtype = 'TTIC'
    AND     (t.insurer_party_cnt = @Insurer_cnt OR @insurer_cnt = 0)
    AND	    t.insurance_section_id = @insurance_section_id      
    ORDER BY  
            t.tax_group_id,  
            t.sequence  
END    
END   
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

 
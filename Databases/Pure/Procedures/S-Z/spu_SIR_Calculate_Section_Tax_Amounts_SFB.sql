SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Calculate_Section_Tax_Amounts_SFB'
GO


CREATE PROCEDURE spu_SIR_Calculate_Section_Tax_Amounts_SFB

@insurance_section_id  int,
@return_taxes tinyint 

AS    
    
BEGIN    
    
 DECLARE @tax_group_id int    
 DECLARE @total_tax_amount money    
 DECLARE @tax_base_amount money    
 DECLARE @premium_currency_id int    
 DECLARE @company_id int    
 DECLARE @premium_amount money    
 DECLARE @insurance_file_cnt int
 DECLARE @risk_cnt int
 Declare @effective_date datetime
 Declare @insurer_cnt int
   
-- Get the required section premium details
--If not multi
IF Not Exists(Select * from policy_coinsurers where insurance_Section_id = @insurance_section_id)
BEGIN 
	SELECT    
    	@tax_group_id = iCOBs.tax_group_id,    
    	@premium_currency_id = i.currency_id,    
    	@company_id = i.source_id,    
    	@premium_amount = iCOBs.premium_excluding_tax,
    	@insurance_file_cnt = iCOBs.insurance_file_cnt, 
    	@risk_cnt = NULL,
    	@effective_date = i.cover_start_date 
	FROM insurance_COB_section iCOBs
	JOIN insurance_file i on i.insurance_file_cnt  = iCOBs.insurance_file_cnt   
	WHERE insurance_section_id = @insurance_section_id 
 
 
	DELETE FROM tax_calculation   
	WHERE insurance_section_id = @insurance_section_id
	AND transtype = 'TTIF'   
	 
	-- Calculate the Tax and regenerate the tax_calculations    
	EXEC spu_SIR_Calculate_Tax_Amounts_SFB    
 	@company_id=@company_id,    
 	@tax_group_id=@tax_group_id,    
 	@transtype='TTIF',    
 	@currency_id=@premium_currency_id,    
 	@amount=@premium_amount,    
 	@tax_currency_amount=@total_tax_amount OUTPUT,    
 	@tax_base_amount=@tax_base_amount OUTPUT,  
 	@associated_key_id =@insurance_section_id, 
 	@insurance_file_cnt = @insurance_file_cnt,
 	@effective_date = @effective_date
END
ELSE
BEGIN 
	DECLARE c_taxes CURSOR FAST_FORWARD FOR      
	SELECT   
    	iCOBs.tax_group_id,    
    	i.currency_id,    
    	i.source_id,    
    	PO.coinsurer_value,
    	iCOBs.insurance_file_cnt, 
    	NULL,
    	i.cover_start_date,
	PO.party_cnt    
	FROM insurance_COB_section iCOBs
        JOIN policy_coinsurers PO on PO.insurance_section_id = iCOBS.insurance_section_id 
	JOIN insurance_file i on i.insurance_file_cnt  = iCOBs.insurance_file_cnt   
	WHERE ICOBS.insurance_section_id = @insurance_section_id  
 
 
	DELETE FROM tax_calculation   
	WHERE insurance_section_id = @insurance_section_id
	AND transtype = 'TTIF'   
	
	OPEN c_taxes
	FETCH NEXT FROM c_taxes INTO
    		@tax_group_id,    
    		@premium_currency_id,    
    		@company_id,    
    		@premium_amount,
    		@insurance_file_cnt, 
    		@risk_cnt,
    		@effective_date,
		@insurer_cnt 

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
	-- Calculate the Tax and regenerate the tax_calculations    
		EXEC spu_SIR_Calculate_Tax_Amounts_SFB    
 		@company_id=@company_id,    
 		@tax_group_id=@tax_group_id,    
 		@transtype='TTIF',    
 		@currency_id=@premium_currency_id,    
 		@amount=@premium_amount,    
 		@tax_currency_amount=@total_tax_amount OUTPUT,    
 		@tax_base_amount=@tax_base_amount OUTPUT,  
 		@associated_key_id =@insurance_section_id, 
 		@insurance_file_cnt = @insurance_file_cnt,
		@insurer_cnt = @insurer_cnt,
 		@effective_date = @effective_date
		FETCH NEXT FROM c_taxes INTO
    			@tax_group_id,    
    			@premium_currency_id,    
    			@company_id,    
    			@premium_amount,
    			@insurance_file_cnt, 
    			@risk_cnt,
    			@effective_date,
			@insurer_cnt 
	END
	CLOSE c_taxes
	DEALLOCATE c_taxes
  
  
    
END 
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
    		AND	    t.insurance_section_id = @insurance_section_id  
    		AND     tt.tax_basis = 2  
    		AND     t.transtype = 'TTIF'
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

 
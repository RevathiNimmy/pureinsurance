SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Event_SIR_Calculate_Tax_Amounts_SFB'
GO

CREATE PROCEDURE spu_Event_SIR_Calculate_Tax_Amounts_SFB      
 @company_id int,      
 @tax_group_id int,      
 @transtype VARCHAR(20),      
 @currency_id int,      
 @amount Money,      
 @tax_currency_amount Money OUTPUT,      
 @tax_base_amount Money OUTPUT,     
 @associated_key_id int = NULL,   
 @insurance_file_cnt int,  
 @calculate_only tinyint = 0,
 @insurer_cnt int = 0,
 @effective_date datetime = '1899-01-01'
      
AS      
      
BEGIN      
      
 DECLARE @tax_rate_is_value int      
 DECLARE @tax_rate money      
 DECLARE @tax_currency_id int         
 DECLARE @individual_tax_amount money      
 DECLARE @tax_type_id int    
 DECLARE @tax_band_id int    
 DECLARE @tax_calculation_cnt int    
 DECLARE @tax_sequence int      
 DECLARE @tax_rate_allow_tax_credit int  
 DECLARE @tax_rate_country_id int  
 DECLARE @tax_rate_state_id int  
 DECLARE @tax_rate_class_of_business_id int  
  
 SELECT @tax_currency_amount = 0      
 SELECT @tax_base_amount = 0 

--Datasure running totals
DECLARE @calc_basis int
DECLARE @running_total money
SELECT @running_total = @amount    
      
 -- cannot calculate tax when there is no tax group so just exit...      
 IF ISNULL(@tax_group_id, 0) =0    
  RETURN      
       
 -- get the effective date 
IF @effective_date = '1899-01-01'     
 	SELECT @effective_date = GetDate()      
       
 --- create temporary table to hold rates      
 CREATE TABLE #Rates      
  (tax_type_id int,      
  description varchar(255),      
  tax_band_id int,      
  description1 varchar(255),      
  is_value int,      
  rate money,      
  currency_id int,      
  code varchar(10),      
  sequence int,   
  allow_tax_credit tinyint,  
  country_id smallint ,  
  state_id smallint,  
  class_of_business_id int,
  calc_basis int)      
      
 -- save rates into temporary table      
 INSERT INTO #Rates      
  EXEC spu_Get_Tax_Types_and_Bands_SFB      
   @tax_group_id =  @tax_group_id,      
   @effective_date = @effective_date,      
   @transtype = @transtype      
 
      
 DECLARE CURSOR_Tax_Rates  CURSOR FOR      
 SELECT   
  tax_type_id,   
  tax_band_id,     
  is_value,   
  rate,   
  currency_id,  
  sequence,   
  allow_tax_credit,   
  country_id,   
  state_id,   
  class_of_business_id,
  calc_basis  
  
 FROM #Rates      
      
 OPEN CURSOR_Tax_Rates      
      
 FETCH NEXT FROM CURSOR_Tax_Rates INTO      
  @tax_type_id,     
  @tax_band_id,     
  @tax_rate_is_value,      
  @tax_rate,      
  @tax_currency_id,      
  @tax_sequence,  
  @tax_rate_allow_tax_credit,  
  @tax_rate_country_id,  
  @tax_rate_state_id,   
  @tax_rate_class_of_business_id,  
  @calc_basis   
  
 WHILE @@FETCH_STATUS = 0      
  BEGIN      
      
   -- if this is a value rather than a percentage      
   IF @tax_rate_is_value = 1      
    BEGIN      
    -- the routine needs to convert the tax value      
    -- into the same currency as the fee      
    -- before it is added to the total tax amount      
      
     EXEC spu_ACT_Do_Currency_To_Currency_Conversion      
      @currency_id_from =  @tax_currency_id,      
      @currency_amount_from = @tax_rate,      
      @company_id =  @company_id,      
      @currency_id_to=  @currency_id,      
      @currency_amount_to = @individual_tax_amount OUTPUT
  
    END      
    ELSE      
    BEGIN
  
     -- the tax amount is simply a percentage of the passed in amount      
     	SET @individual_tax_amount = @amount * (@tax_rate/100)
	
     -- Keep the running total
    	SET @running_total = @running_total + @individual_tax_amount      
    END      
   
   SET @tax_currency_amount = @tax_currency_amount + @individual_tax_amount      
    
  
   -- writing back to the database  
   IF (@calculate_only=0) 
   BEGIN   
   IF @individual_tax_amount <> 0 
	BEGIN
        -- insert an entry into tax_calculation for each item...    
        -- @associatedkeyid    
        INSERT INTO event_tax_calculation 
        (    
        insurance_file_cnt,  
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
        transtype,   
        policy_fee_u_id,  
        agent_commission_cnt,   
        ri_party_cnt,
        insurance_section_id,
        policy_fee_id,
        policy_agents_id,
        insurer_party_cnt
	)    
    
        VALUES 
	(    
        @insurance_file_cnt,   
        NULL,  
        @tax_band_id,     
        @amount,     
        CASE  WHEN @tax_rate_is_value = 0 THEN @tax_rate    
        ELSE @individual_tax_amount    
        END,
        @individual_tax_amount,   
        @tax_rate_is_value,     
        0,     
        @calc_basis,     
        0,    
        0,      
        0,     
        @currency_id,     
        @tax_rate_allow_tax_credit,     
        0,     
        @tax_rate_country_id,     
        @tax_rate_state_id,     
        @tax_rate_class_of_business_id,     
        @tax_group_id,     
        @tax_sequence,     
        @transtype,   
        NULL,  
        NULL,  
        CASE WHEN @transtype ='TTRITP' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
             WHEN @transtype ='TTRITC' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
             WHEN @transtype ='TTRIFP' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
             WHEN @transtype ='TTRITC' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
        ELSE NULL      
        END,
        CASE  WHEN @transtype = 'TTIF' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
              WHEN @transtype = 'TTIC' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
	ELSE NULL  
        END,
        CASE  WHEN @transtype = 'TTF' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
              WHEN @transtype = 'TTFC' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
       	ELSE NULL  
        END,
        CASE  WHEN @transtype = 'TTAC' and ISNULL(@associated_key_id, 0) <> 0 THEN @associated_key_id  
        ELSE NULL  
        END,
	CASE WHEN  ISNULL(@insurer_cnt, 0) <> 0 THEN @insurer_cnt 
        ELSE NULL
	END
     )  
     END  
  END  
  
  FETCH NEXT FROM CURSOR_Tax_Rates INTO      
    @tax_type_id,     
    @tax_band_id,     
    @tax_rate_is_value,      
    @tax_rate,      
    @tax_currency_id,      
    @tax_sequence,  
    @tax_rate_allow_tax_credit,  
    @tax_rate_country_id,  
    @tax_rate_state_id,   
    @tax_rate_class_of_business_id,
    @calc_basis  
  END      
      
 CLOSE CURSOR_Tax_Rates      
 DEALLOCATE CURSOR_Tax_Rates      
      
 DROP TABLE #Rates      
      
 EXEC spu_ACT_Do_Currency_Conversion      
  @company_id = @company_id,      
  @currency_id = @currency_id,      
  @currency_amount_unrounded = @tax_currency_amount,      
  @currency_base_date = @effective_date,      
  @mode ='1',      
  @base_amount_unrounded = @tax_base_amount OUTPUT,      
  @return_status = 1      
      
END     
  
  
  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIfIER OFF SET ANSI_NullS On
GO


Execute DDLDropProcedure 'spu_RI_Arrangement_duplicate'
GO

CREATE  PROCEDURE spu_RI_Arrangement_duplicate  
    @insurance_file_cnt int,    
    @risk_cnt int
AS    
    
    Declare      
        @original_risk_cnt int,      
        @pro_rata_rate float,      
        -- RI Arrangement Fields      
        @old_ri_arrangement_id int,      
        @new_ri_arrangement_id int,      
        @has_fac tinyint,      
        @original_flag tinyint,      
        -- RI Arrangement Line Fields      
        @old_ri_arrangement_line_id int,      
        @new_ri_arrangement_line_id int,      
        -- Currency Fields      
        @original_currency_id smallint,      
        @original_source_id smallint,      
        @original_currency_rate float,      
        @original_date datetime,      
        @new_currency_id smallint,      
        @new_source_id smallint,      
        @new_currency_rate float,      
        @new_date datetime,      
        @combined_rate float,      
        @old_expiry_date datetime,      
        @new_expiry_date datetime,    
        @Date_for_Treaty_XOL_Calculation  int,    
        @old_treaty_id int,    
        @Replaced_by_treaty_id int,    
        @Replaced_by_effective_date datetime,  
        @RI2007Enabled int,  
        @old_grouping_id int,  
        @new_grouping_id int,
		@effective_date datetime
      
 	Select @RI2007Enabled=ISNull(value,0) from hidden_options where option_number=88
  
	-- Check if we have already copied      
    If Exists (Select * From ri_arrangement Where risk_cnt = @risk_cnt And original_flag = 1)      
        Return      
      
    -- Select and validate original risk      
    Select  @original_risk_cnt = Null      
    Select  @original_risk_cnt = ifrl.original_risk_cnt      
    From    insurance_file_risk_link ifrl      
    Join    insurance_file ifi      
            On ifrl.insurance_file_cnt = ifi.insurance_file_cnt      
    Join    risk r      
            On r.risk_cnt = ifrl.risk_cnt      
    Where   ifrl.insurance_file_cnt = @insurance_file_cnt      
    And     ifrl.risk_cnt = @risk_cnt      
            -- Don't copy original ri on an mta reinstatement      
--  And     ifi.insurance_file_type_id <> 10      
            -- Don't pick up an original count where this is just a copied risk      
            -- i.e. the original version of this risk was created on this policy      
    And     Not Exists (      
            Select  *      
            From    insurance_file_risk_link ifrl2      
            Where   ifrl2.insurance_file_cnt = @insurance_file_cnt      
            And     ifrl2.risk_cnt = ifrl.original_risk_cnt)      
      
    -- If we have no original risk we have nothing to copy      
    If @original_risk_cnt Is Null      
        Return      
      
    -- Get new policy details      
    Select  @new_currency_id = currency_id,      
            @new_source_id = source_id,      
            @new_currency_rate = currency_base_xrate,      
            @new_date = cover_start_date,      
          	@new_expiry_date = expiry_date      
    From    insurance_file      
    Where   insurance_file_cnt = @insurance_file_cnt      
      
    -- Get original policy details      
    Select  @original_currency_id = currency_id,      
            @original_source_id = source_id,      
            @original_currency_rate = currency_base_xrate,      
            @original_date = cover_start_date,      
            @old_expiry_date = expiry_date      
    From    insurance_file      
    Where   insurance_file_cnt = (      
            Select  Max(ifi.insurance_file_cnt)      
            From    insurance_file_risk_link ifrl      
            Join    insurance_file ifi      
                    On ifi.insurance_file_cnt = ifrl.insurance_file_cnt      
            Where   ifrl.risk_cnt = @original_risk_cnt      
            And     IsNull(ifi.insurance_file_status_id, 3) In (3, 4, 5, 6)      
            And     ifi.insurance_file_type_id In (2, 5))      
      
    -- Calculate single rate to go From old currency to new currency      
    If @new_currency_id = @original_currency_id      
        -- Don't convert amounts if both policy versions are in they same currency, even if they have different rates.      
        Select @combined_rate = 1      
    Else Begin      
        -- If new rate wasn't overridden then get the rate From currencyrate table      
        If IsNull(@new_currency_rate,0) = 0      
            Execute spu_ACT_Get_Currency_Rate      
           @new_currency_id,      
                @new_source_id,      
                @new_date,      
                @new_currency_rate Output      
      
        -- If original rate wasn't overridden then get the rate From currencyrate table      
        If IsNull(@original_currency_rate,0) = 0      
            Execute spu_ACT_Get_Currency_Rate      
                @original_currency_id,      
                @original_source_id,      
                @original_date,      
                @original_currency_rate Output      
      
        Select @combined_rate = @original_currency_rate / @new_currency_rate      
    End      
      
    -- Ensure non null      
    Select @combined_rate = IsNull(@combined_rate, 1)      
      
	Select @pro_rata_rate=1  

    -- Reset the reinsurance flags on the risk      
    Update  r      
    Set     is_ri_at_risk_level = rt.is_ri_at_risk_level,      
            is_auto_reinsured = rt.is_auto_reinsured      
    From    risk r      
    Join    risk_type rt On rt.risk_type_id = r.risk_type_id      
    Where   r.risk_cnt = @risk_cnt      
      
    -- Arrangement cursor      
    Declare Arrangement_Cursor Cursor Fast_Forward For      
        Select  ri_arrangement_id,      
                Case When Exists (      
                    Select  *      
                    From    ri_arrangement_line ral      
                    Where   ral.ri_arrangement_id = ra.ri_arrangement_id) Then 1 Else 0 End has_fac,      
                1 original_flag      
        From    ri_arrangement ra      
        Where   risk_cnt = @original_risk_cnt      
        And     original_flag = 0      
      
    Open Arrangement_Cursor      
    Fetch Next From Arrangement_Cursor Into @old_ri_arrangement_id, @has_fac, @original_flag      
      
    -- For each of the old arrangements      
    While (@@Fetch_Status = 0) Or (@original_flag = 0) Begin      
        -- If this is the original flag then combined_rate is negative      
        If @original_flag = 1      
            Select  @combined_rate = -Abs(@combined_rate)      
        Else      
            Select  @combined_rate = Abs(@combined_rate)      
    
        -- Copy arrangement      
        Insert Into ri_arrangement (      
                risk_cnt,      
                ri_band_id,      
                ri_model_id,      
                sum_insured,      
                premium,      
                original_flag,  
                  is_modified,  
                           version_id,  
                           RI_Version_Type_id,  
                           Effective_Date
              ,cloned,xol_ri_model_id,
				ri_override_reason_id)       
        Select  @risk_cnt,      
                ri_band_id,      
                ri_model_id,      
                Round(sum_insured * @combined_rate, 2),      
                Round(premium * @combined_rate * @pro_rata_rate, 2),      
                @original_flag,      
               	is_modified,
                  version_id,  
                           RI_Version_Type_id,  
                           Effective_Date
              ,cloned,xol_ri_model_id,
				ri_override_reason_id
        From    ri_arrangement      
        Where   risk_cnt = @original_risk_cnt      
        And     ri_arrangement_id = @old_ri_arrangement_id      
        And     original_flag = 0      
      
        -- Get new id      
        Select  @new_ri_arrangement_id = @@Identity      
              
		SET @new_grouping_id = NULL  

        -- Arrangement Line cursor      
        Declare Arrangement_Line_Cursor Cursor Fast_Forward For      
            Select  ri_arrangement_line_id,grouping     
            From    ri_arrangement_line      
            Where   ri_arrangement_id = @old_ri_arrangement_id 
            Order By grouping, ri_arrangement_line_id 
         
        Open Arrangement_Line_Cursor      
        Fetch Next From Arrangement_Line_Cursor Into @old_ri_arrangement_line_id,@old_grouping_id      
    
        -- For each of the old arrangements      
        While (@@Fetch_Status = 0) Begin      
            -- Copy arrangement line first      
    
	   	 	Insert Into ri_arrangement_line (      
	                      ri_arrangement_id,      
	                      type,      
	                      treaty_id,      
	                      party_cnt,      
	                      default_share_percent,      
	                      this_share_percent,      
	                      premium_percent,      
	                      commission_percent,      
	                      agreement_code,      
	                      priority,      
	                      number_of_lines,      
	                      line_limit,      
	                      lower_limit,      
	                      sum_insured,      
	                      premium_value,      
	                      commission_value,      
	                      premium_tax,      
	                      commission_tax,      
	                      is_commission_modified,      
	                      Retained,participation_percent,grouping)      
	              Select  @new_ri_arrangement_id,      
	                      type,      
	                      treaty_id,      
	                      party_cnt,      
	                      default_share_percent,      
	                      this_share_percent,      
	                      premium_percent,      
	                      commission_percent,      
	                      agreement_code,      
	   		      		  priority,      
	                      number_of_lines,      
	                      Round(line_limit * Abs(@combined_rate), 2),      
	                      Round(lower_limit * Abs(@combined_rate), 2),      
	                      Round(sum_insured * @combined_rate, 2),      
	                      Round(premium_value * @combined_rate * @pro_rata_rate, 2),    
	                      Round(commission_value * @combined_rate * @pro_rata_rate, 2),    
	                      Round(premium_tax * @combined_rate * @pro_rata_rate, 2),    
	                      Round(commission_tax * @combined_rate * @pro_rata_rate, 2),    
	                      is_commission_modified,Retained,participation_percent,grouping      
	              From    ri_arrangement_line      
	              Where   ri_arrangement_line_id = @old_ri_arrangement_line_id
     
			-- Get new id      
			Select  @new_ri_arrangement_line_id = @@Identity   
  
   IF EXISTS (select * from Ri_Arrangement_line_Broker_Participants where ri_arrangement_line_id=@old_ri_arrangement_line_id)  
   BEGIN  
         DECLARE Brokers_FAC CURSOR FOR      
           Select ri_party_cnt,participation_percent From      
            Ri_Arrangement_line_Broker_Participants      
            Where ri_arrangement_line_id=@old_ri_arrangement_line_id  
          
     DECLARE @Party_Cnt INT,  
       @Part_percent FLOAT   
       
     OPEN Brokers_FAC      
          
        FETCH NEXT FROM Brokers_FAC    
          INTO @Party_Cnt,@Part_percent      
          
           WHILE @@FETCH_STATUS = 0      
           BEGIN      
            
            Insert Into Ri_Arrangement_line_Broker_Participants(ri_arrangement_line_id,      
               ri_party_cnt,      
               participation_percent)      
            Values (@new_ri_arrangement_line_id,      
              @Party_Cnt,      
              @Part_percent)      
            
            FETCH NEXT FROM Brokers_FAC
             INTO @Party_Cnt,@Part_percent      
           END      
           CLOSE Brokers_FAC     
           DEALLOCATE Brokers_FAC     
   END  
  
   IF  @old_ri_arrangement_line_id = @old_grouping_id  
     set @new_grouping_id = @new_ri_arrangement_line_id  
       
   IF @old_grouping_id IS NOT NULL  
    Update ri_arrangement_line set grouping = @new_grouping_id   
        Where ri_arrangement_line_id = @new_ri_arrangement_line_id  
   ELSE  
    SET @new_grouping_id = NULL  
  
            -- Copy taxes (only for originals)      
            If @original_flag = 1 Begin      
                Insert Into tax_calculation (      
                        risk_cnt,      
                        tax_band_id,      
                        premium,      
                        percentage,      
                        value,      
                        is_value,      
                        is_manually_changed,      
                        calc_basis,      
                        basis_value,      
                        sum_insured,      
                        sum_insured_rounded,      
                        allow_tax_credit,      
                        original_sum_insured,      
                        currency_id,      
                        country_id,      
                        state_id,      
                        class_of_business_id,      
                        tax_group_id,      
                        sequence,      
                        insurance_file_cnt,      
                        transtype,      
                        ri_party_cnt,      
                        ri_arrangement_line_id)      
                Select  @risk_cnt,      
                        tax_band_id,      
                        Round(-premium * @combined_rate * @pro_rata_rate, 2),      
                        percentage,      
                        Round(-value * @combined_rate * @pro_rata_rate, 2),      
                        is_value,      
                        is_manually_changed,      
                        calc_basis,      
                        Round(-basis_value * @combined_rate, 2),      
                        Round(-sum_insured * @combined_rate, 2),      
                        sum_insured_rounded,      
                        allow_tax_credit,      
                        Round(-original_sum_insured * @combined_rate, 2),      
                        @new_currency_id,      
                        country_id,      
                        state_id,      
                        class_of_business_id,      
                        tax_group_id,      
                        sequence,      
                        @insurance_file_cnt,      
                        transtype,      
                        ri_party_cnt,      
                        @new_ri_arrangement_line_id      
                From    tax_calculation      
                Where   risk_cnt = @original_risk_cnt      
                And     ri_arrangement_line_id = @old_ri_arrangement_line_id      
            End Else Begin      
                -- We should recalc all taxes, just to be safe      
                -- Note, this will also refresh the premium & comm shares      
                -- just in case the si/premium ratio has changed      
                Execute spu_RI_Arrangement_taxes      
                 @insurance_file_cnt = @insurance_file_cnt,      
                 @risk_cnt = @risk_cnt,      
                    @ri_arrangement_id = @new_ri_arrangement_id,      
                    @band_premium = 0      
            End      
      
            -- Get next arrangement line      
            Fetch Next From Arrangement_Line_Cursor Into @old_ri_arrangement_line_id,@old_grouping_id      
        End      
      
        -- Close and release cursor      
        Close Arrangement_Line_Cursor      
        Deallocate Arrangement_Line_Cursor      
      
        -- If we have fac we need to copy it to new arrangements      
        If @original_flag = 1 And @has_fac = 1 Begin      
            -- Set original flag to 0 and reprocess arrangement for fac      
            Select @original_flag = 0      
        End Else Begin      
            -- If this is the last record we'll go into an infinite loop if      
            -- we don't manually reset the original_flag      
            Select @original_flag = 1      
      
            -- Get next arrangement      
            Fetch Next From Arrangement_Cursor Into @old_ri_arrangement_id, @has_fac, @original_flag      
        End      
    End      
      
    -- Close and release cursor      
    Close Arrangement_Cursor      
    Deallocate Arrangement_Cursor      
    
  
GO  


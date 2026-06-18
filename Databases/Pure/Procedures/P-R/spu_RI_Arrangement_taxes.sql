SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RI_Arrangement_taxes'
GO


CREATE PROCEDURE spu_RI_Arrangement_taxes
    @insurance_file_cnt int,
    @risk_cnt int,
    @ri_arrangement_id int,
    @band_premium money
AS

    Declare
        @ri_arrangement_line_id int,
        @type char(1),
        @treaty_id int,
        @party_cnt int,
        @premium_percent float,
        @commission_percent float,
        @premium money,
        @commission money,
        @premium_tax money,
        @commission_tax money,
        @b_isObligatory bit,	--PN 71440
        @RI2007Enabled tinyint,
        @is_premium_edited bit  -- preserve user-edited premium
  SELECT @RI2007Enabled=ISNull(value,0) FROM hidden_options WHERE option_number=88  


    -- Delete any existing values for temporary rows
    Execute spu_SIR_Delete_Tax_Calculations
        @insurance_file_cnt = @insurance_file_cnt , 
        @risk_cnt = @risk_cnt,
        @transtype_premium = 'TTRITP',
        @transtype_commission = 'TTRITC'

    Execute spu_SIR_Delete_Tax_Calculations
        @insurance_file_cnt = @insurance_file_cnt , 
        @risk_cnt = @risk_cnt,
        @transtype_premium = 'TTRIFP',
        @transtype_commission = 'TTRIFC'

    -- Recalc taxes on all active ri lines (including manually added)
    -- Task 2.7: Include manually added treaties in tax calculations
    Declare RILine_Cursor Cursor Fast_Forward For
        Select  ri_arrangement_line_id,
                type,
                treaty_id,
                party_cnt,
                premium_percent,
                commission_percent,
                Is_Obligatory,		-- PN 71440
                ISNULL(is_premium_edited, 0)
        From    RI_Arrangement_Line
        Where   ri_arrangement_id = @ri_arrangement_id
                -- No filter on manually_added - include ALL treaties

    -- Open and get first row
    Open RILine_Cursor
    Fetch Next From RILine_Cursor Into
        @ri_arrangement_line_id, @type, @treaty_id, @party_cnt, @premium_percent, @commission_percent, @b_isObligatory, @is_premium_edited

    While (@@Fetch_Status = 0) Begin
        -- Recalc premium and comm values
        Select  @premium = @band_premium * (@premium_percent / 100),
                @commission = @premium * (@commission_percent / 100)

        -- Check type and calc tax
        If @type = 'F' Begin    
            -- Delete any existing values
            Execute spu_SIR_Delete_Tax_Calculations
                @insurance_file_cnt = @insurance_file_cnt , 
                @risk_cnt = @risk_cnt,
                @transtype_premium = 'TTRIFP',
                @transtype_commission = 'TTRIFC', 
                @ri_arrangement_line_id = @ri_arrangement_line_id
    
            Execute spu_SIR_Calculate_Treaty_Party_Tax_Amounts
                @insurance_file_cnt = @insurance_file_cnt , 
                @risk_cnt = @risk_cnt,
                @ri_arrangement_line_id = @ri_arrangement_line_id,
                @party_cnt = @party_cnt,
                @premium = @premium, 
                @commission = @commission, 
                @premium_transtype = 'TTRIFP',
                @commission_transtype = 'TTRIFC', 
                @premium_tax = @premium_tax output, 
                @commission_tax = @commission_tax output
        End Else Begin
            -- Delete any existing values
            Execute spu_SIR_Delete_Tax_Calculations
                @insurance_file_cnt = @insurance_file_cnt , 
                @risk_cnt = @risk_cnt,
                @transtype_premium = 'TTRITP',
                @transtype_commission = 'TTRITC', 
                @ri_arrangement_line_id = @ri_arrangement_line_id

            Execute spu_SIR_Calculate_Treaty_Tax_Amounts
                @insurance_file_cnt = @insurance_file_cnt , 
                @risk_cnt = @risk_cnt,
                @ri_arrangement_line_id = @ri_arrangement_line_id,
                @treaty_id = @treaty_id,
                @premium = @premium, 
                @commission = @commission, 
                @premium_transtype = 'TTRITP',
                @commission_transtype = 'TTRITC', 
                @premium_tax = @premium_tax output, 
                @commission_tax = @commission_tax output
        End

        -- Update new values — skip premium_value/commission_value for manually added lines (user-set, must be preserved)
        IF(Select Type From RI_Arrangement_line WHERE ri_arrangement_line_id = @ri_arrangement_line_id)<> 'FX'   
        Update  ri_arrangement_line
        Set     premium_value    = CASE WHEN ISNULL(manually_added,   0) = 1 THEN premium_value
                                        WHEN @b_isObligatory = 1 THEN premium_value
                                        WHEN ISNULL(is_premium_edited, 0) = 1 THEN premium_value
                                        ELSE @premium END,
                commission_value = CASE WHEN ISNULL(manually_added,   0) = 1 THEN commission_value
                                        WHEN @b_isObligatory = 1 THEN commission_value
                                        ELSE @commission END,
                premium_tax = @premium_tax,
                commission_tax = @commission_tax
        Where   ri_arrangement_line_id = @ri_arrangement_line_id
      ELSE   
           IF  (@Premium<> 0 )   
                Update  ri_arrangement_line  
                Set     premium_value = @premium,  
                commission_value = @commission,  
                premium_tax = @premium_tax,  
                commission_tax = @commission_tax  
                Where   ri_arrangement_line_id = @ri_arrangement_line_id 
                
           IF @b_isObligatory = 1  and @RI2007Enabled=0
                BEGIN  
                    SET @band_premium = @band_premium - @premium  
                END   
        -- Get next row
		--Start PN 71440
		--No need to subtract the obligatory premium as premium % is in accordance with the base premium.
        --if (@b_isObligatory=1)
			--Select @band_premium = @band_premium - premium_value From ri_arrangement_line Where ri_arrangement_line_id=@ri_arrangement_line_id
			
        Fetch Next From RILine_Cursor Into
            @ri_arrangement_line_id, @type, @treaty_id, @party_cnt, @premium_percent, @commission_percent, @b_isObligatory, @is_premium_edited
		--End of PN 71440
    End

    Close RILine_Cursor
    Deallocate RILine_Cursor


Go




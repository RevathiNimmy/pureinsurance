SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RI2007Disabled_Arrangement_refresh'
GO


CREATE PROCEDURE spu_RI2007Disabled_Arrangement_refresh
    @insurance_file_cnt integer,
    @risk_cnt integer,
    @transtype varchar(10) = ''
AS

    DECLARE
        @effective_date datetime,
        @is_coinsured tinyint,
        @retained_percent float,
        @source_id int,
        @policy_currency_id smallint,
        @policy_currency_rate float,
        @is_auto_reinsured tinyint,
        @risk_type_id int,
        @eml_percent float,
        @allow_deferred tinyint,
        @line_limit money,
        @ri_band int,
        @premium money,
        @sum_insured money,
               @premium_PT money,  
        @sum_insured_PT money,  
        @ri_arrangement_id int,  
        @is_modified tinyint,  
        @bContinue tinyint ,
        @insuranceFileType tinyint,
        @FAC_type Varchar(10),
        @IsDeletedRiskInPT tinyint,
        @ri_override_reason_id int

        Set @transtype = IsNull(@transtype, '')  
        Select @insuranceFileType = Insurance_File_Type_id from insurance_file where insurance_file_cnt = @insurance_file_cnt 

    -- Don't use the supplied effective date. 
    -- Note: For an MTA this call may return an MTA date or today's date
    -- depending on the system option "Use MTA date for reinsurance"
       
    Set @transtype = IsNull(@transtype, '')  
    Select @insuranceFileType = Insurance_File_Type_id from insurance_file where insurance_file_cnt = @insurance_file_cnt 
    Set @IsDeletedRiskInPT = 0  
    
    If @transtype = 'PT' OR @transtype = 'DRI'
        SELECT @effective_date = cover_start_date  
        FROM insurance_file  
        WHERE insurance_file_cnt = @insurance_file_cnt  
    ELSE  
     Execute spu_get_effective_date  
          @insurance_file_cnt = @insurance_file_cnt,  
          @risk_cnt = @risk_cnt,  
          @effective_date = @effective_date output  

      IF Exists(Select risk_cnt From insurance_file_risk_link 
              Where risk_cnt = @risk_cnt And status_flag = 'D' And @transtype = 'PT')   
         Set @IsDeletedRiskInPT = 1

    -- Check coinsurance and rate
    Execute spu_Insurance_File_is_coinsured 
        @insurance_file_cnt, 
        @is_coinsured output, 
        @retained_percent output


    -- Copy any original reinsurance
    If NOT(@insuranceFileType = 3 AND (@transtype = 'DRI' OR @transtype = 'PT'))  
    Execute spu_RI2007Disabled_Arrangement_copy
        @insurance_file_cnt = @insurance_file_cnt,
        @risk_cnt = @risk_cnt,
        @effective_date = @effective_date,  
        @Trans_type = @transtype,
        @IsDeletedRiskInPT = @IsDeletedRiskInPT  

    --In case of PT, just copy from original RI arrangement,
    --where (RI model and band) is not being transferred
    If @transtype = 'PT' and ISNULL (@insuranceFileType ,0) <> 8  
    Execute spu_RI2007Disabled_Arrangement_copy_from_original_PT  
	        @risk_cnt = @risk_cnt,  
	        @risk_type_id = @risk_type_id,  
            @effective_date = @effective_date,  
            @allow_deferred = @allow_deferred  


    -- Get currency of policy, and therefore the currency of new ri_arrangement
    Select  @policy_currency_id = currency_id,
            @policy_currency_rate = currency_base_xrate,
            @source_id = source_id
    From    insurance_file
    Where   insurance_file_cnt = @insurance_file_cnt
    
    -- If policy rate wasn't overridden then get the rate from currencyrate table
    If IsNull(@policy_currency_rate, 0) = 0
        Execute spu_ACT_Get_Currency_Rate
            @policy_currency_id,
            @source_id,
            @effective_date,
            @policy_currency_rate output


    -- Get config values from risk and risk_type
    Select  @is_auto_reinsured = rt.is_auto_reinsured,
            @risk_type_id = rt.risk_type_id,
            @eml_percent = IsNull(r.eml_percentage, 100) / 100,
            @allow_deferred = rt.is_deferred_ri_permitted
    From    risk r
    Join    risk_type rt On r.risk_type_id = rt.risk_type_id
    Where   r.risk_cnt = @risk_cnt


    -- Check for ri_limits from DM
    -- Note: This call returns an single row of Null
    If @is_auto_reinsured = 1 
        Execute spu_get_ri_values
            @insurance_file_cnt = @insurance_file_cnt,
            @risk_cnt = @risk_cnt,
            @value = @line_limit Output


    -- Declare active ri_band cursor and get premiums
   IF @IsDeletedRiskInPT <> 1   
    Declare RI_Band_Cursor Cursor Fast_Forward For
        Select  p.ri_band,
                sum_insured = IsNull((
                    Select  Sum(rs2.sum_insured)        
                    From    rating_section rs2
                    Where   rs2.risk_cnt = @risk_cnt
                    And     rs2.rating_section_id In (
                            Select  rating_section_id        
                            From    peril p2        
                            Where   p2.risk_cnt = @risk_cnt
                            And     p2.is_sum_insured = 1
                            And     p2.ri_band = p.ri_band)
                    And     rs2.original_flag = 0), 0),
                premium = IsNull(Sum(Case When p.is_premium = 1 Then cast(p.this_premium as money) Else 0 End), 0)
        From    Peril p
        Join    Rating_Section rs
                On  rs.risk_cnt = p.risk_cnt
                And rs.rating_section_id = p.rating_section_id
        Where   p.risk_cnt = @risk_cnt
        And    (p.is_premium = 1 Or p.is_sum_insured = 1)
        And     rs.original_flag = 0
        Group By
                p.ri_band
     Else
      Declare RI_Band_Cursor Cursor Fast_Forward For 
         Select ri_band_id, sum_insured * (-1) , premium * (-1) 
         From RI_Arrangement  
         Where risk_cnt = @risk_cnt And original_flag = 1

    -- Open the RI Bands Cursor and get first row
    Open RI_Band_Cursor
    Fetch Next From RI_Band_Cursor Into @ri_band, @sum_insured, @premium

    -- Start processing
    While (@@Fetch_Status = 0)
    Begin
        Set @bContinue = 0 
        -- Apply the EML Percentage and Coinsurance
        Select  @sum_insured = @sum_insured * @retained_percent * @eml_percent,
                @premium = @premium * @retained_percent

        -- Check for existing arrangement
        Select  @ri_arrangement_id = null
        Select  @ri_arrangement_id = ri_arrangement_id,
                @is_modified = IsNull(is_modified, 1),
				@ri_override_reason_id=isnull(@ri_override_reason_id,0)
        From    ri_arrangement 
        Where   risk_cnt = @risk_cnt 
        And     ri_band_id = @ri_band 
        And     original_flag = 0

	Set @FAC_type = ''
        Select @FAC_type = isnull(type, '') from ri_arrangement_line where ri_arrangement_id = @ri_arrangement_id and [type] = 'F'

        -- Either insert or update our arrangement
        If IsNull(@ri_arrangement_id, 0) > 0
          If @transtype = 'PT'  And @FAC_type <> 'F'
             --In case of PT and this RI arrangement(RI model and band) is not being transferred
             Set @bContinue = 1  
          Else  
            -- Update totals on current arrangement
            Update  ri_arrangement
            Set     sum_insured = @sum_insured,
                    premium = @premium
            Where   ri_arrangement_id = @ri_arrangement_id
        Else Begin
            -- Insert new arrangement
            Insert Into ri_arrangement (
                    risk_cnt,
                    ri_band_id,
                    sum_insured,
                    premium,
                    original_flag,
                    is_modified,
					ri_override_reason_id)
            Values (@risk_cnt,
                    @ri_band,
                    @sum_insured,
                    @premium,
                    0,
                    1^@is_auto_reinsured,
					@ri_override_reason_id)

            -- Get new id and assume we have not modified
            Select  @ri_arrangement_id = @@Identity,
                    @is_modified = 0
        End

       If @bContinue = 0  
       Begin  
        
        -- Check if the band has been modified
        If (@is_modified = 0)  
            -- It hasn't, or it's new so refresh it
            If (RTRIM(LTRIM(@transtype)) = 'PT')  
            Begin  
    Select @premium_PT = @premium - IsNull(Sum(premium_value), 0),  
           @sum_insured_PT = @sum_insured - IsNull(Sum(sum_insured), 0)  
    From RI_Arrangement_Line  
    Where ri_arrangement_id = @ri_arrangement_id And [type] In ('F', 'FX')  
            
            Execute spu_RI2007Disabled_Arrangement_make  
                @ri_arrangement_id = @ri_arrangement_id,  
                @risk_type_id = @risk_type_id,  
                @ri_band_id = @ri_band,  
                @effective_date = @effective_date,  
                @allow_deferred = @allow_deferred,  
                @sum_insured = @sum_insured_PT,  
                @premium = @premium_PT,  
                @line_limit = @line_limit,  
                @is_auto_reinsured = @is_auto_reinsured,  
                @source_id = @source_id,  
                @policy_currency_id = @policy_currency_id,  
                @policy_currency_rate = @policy_currency_rate,  
                @transtype = @transtype  
            End  
            Else  
            Execute spu_RI2007Disabled_Arrangement_make
                @ri_arrangement_id = @ri_arrangement_id,
                @risk_type_id = @risk_type_id,
                @ri_band_id = @ri_band,
                @effective_date = @effective_date,
                @allow_deferred = @allow_deferred,
                @sum_insured = @sum_insured,
                @premium = @premium,
                @line_limit = @line_limit,
                @is_auto_reinsured = @is_auto_reinsured,
                @source_id = @source_id,
                @policy_currency_id = @policy_currency_id,
                @policy_currency_rate = @policy_currency_rate
        

	--ReCalc this_share_percent & premium_percent 
/*	UPDATE ri_arrangement_line
	   SET this_share_percent = (sum_insured / convert(float, @sum_insured)) * 100.0000, 
	       premium_percent = (sum_insured / convert(float, @sum_insured)) * 100.0000 
           WHERE ri_arrangement_id = @ri_arrangement_id*/

UPDATE ri_arrangement_line  
    SET this_share_percent = CASE WHEN (convert(float, @sum_insured) =0 or sum_insured=0) 
                             THEN CASE WHEN premium_value=0 then 0 else default_share_percent end else (sum_insured / convert(float, @sum_insured)) * 100.0000 END,  
        premium_percent =  CASE WHEN (convert(float, @sum_insured) =0 or sum_insured=0) 
                             then CASE WHEN premium_value=0 then 0 else default_share_percent end else (sum_insured / convert(float, @sum_insured)) * 100.0000 END
           WHERE ri_arrangement_id =  @ri_arrangement_id


        -- We should recalc all taxes, just to be safe
        -- Note, this will also refresh the premium & comm shares
        -- just in case the si/premium ratio has changed
        If (RTRIM(LTRIM(@transtype)) <> 'PT')  
        Execute spu_RI_Arrangement_taxes  
         @insurance_file_cnt = @insurance_file_cnt,  
         @risk_cnt = @risk_cnt,  
            @ri_arrangement_id = @ri_arrangement_id,  
            @band_premium = @premium  
       End  

        -- Get next record
        Fetch Next From RI_Band_Cursor Into @ri_band, @sum_insured, @premium
    End

    -- Close the cursor
    Close RI_Band_Cursor
    Deallocate RI_Band_Cursor

     UPDATE ral 
	 SET    ral.premium_percent = CASE WHEN (convert(float, ra.premium) =0 or ral.premium_value=0)
			  then CASE WHEN ral.premium_value=0 then 0 else ral.default_share_percent end else (ral.premium_value / convert(float, ra.premium)) * 100.0000 END
	 FROM ri_arrangement ra INNER JOIN ri_arrangement_line ral 
	 ON ra.ri_arrangement_id = ral.ri_arrangement_id 
	 WHERE ra.risk_cnt =  @risk_cnt



    -- Delete any tax on arrangements on bands that are no longer in use
    Delete  tax_calculation
    Where   ri_arrangement_line_id In (
                Select  rl.ri_arrangement_line_id
                From    ri_arrangement_line rl
				Inner Join ri_arrangement ri ON ri.ri_arrangement_id = rl.ri_arrangement_id
                Where   ri.risk_cnt = @risk_cnt
                And     ri.ri_band_id Not In (
                            Select  p.ri_band
                            From    Peril p
                            Join    Rating_Section rs
                                    On  rs.risk_cnt = p.risk_cnt
                                    And rs.rating_section_id = p.rating_section_id
                            Where   p.risk_cnt = @risk_cnt
                            And    (p.is_premium = 1 Or p.is_sum_insured = 1)
                            And     rs.original_flag = 0)
                And     ri.original_flag = 0)

    -- Delete any arrangements on bands that are no longer in use
    Delete  ri_arrangement_line
    Where   ri_arrangement_id In (
                Select  ri_arrangement_id
                From    ri_arrangement
                Where   risk_cnt = @risk_cnt
                And     ri_band_id Not In (
                            Select  p.ri_band
                            From    Peril p
                            Join    Rating_Section rs
                                    On  rs.risk_cnt = p.risk_cnt
                                    And rs.rating_section_id = p.rating_section_id
                            Where   p.risk_cnt = @risk_cnt
                            And    (p.is_premium = 1 Or p.is_sum_insured = 1)
                            And     rs.original_flag = 0)
                And     original_flag = 0
                And @IsDeletedRiskInPT <> 1 ) 



    Delete  ri_arrangement
    Where   risk_cnt = @risk_cnt
    And     ri_band_id Not In (
                Select  p.ri_band
                From    Peril p
                Join    Rating_Section rs
                        On  rs.risk_cnt = p.risk_cnt
                        And rs.rating_section_id = p.rating_section_id
                Where   p.risk_cnt = @risk_cnt
                And    (p.is_premium = 1 Or p.is_sum_insured = 1)
                And     rs.original_flag = 0)
    And     original_flag = 0
    And @IsDeletedRiskInPT <> 1  

Go



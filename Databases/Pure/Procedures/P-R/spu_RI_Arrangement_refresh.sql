SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_RI_Arrangement_refresh'
GO
CREATE PROCEDURE spu_RI_Arrangement_refresh  
    @insurance_file_cnt integer,  
    @risk_cnt integer,  
    @transtype varchar(10) = '',  
    @TMPRisk_cnt_under_renewal integer = NULL  
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
        @ri_arrangement_id int,  
        @is_modified tinyint,  
   @ri_override_reason_id int,
  @temp_premium Money 
  
    -- Don't use the supplied effective date.  
    -- Note: For an MTA this call may return an MTA date or today's date  
    -- depending on the system option "Use MTA date for reinsurance"  
    Execute spu_get_effective_date  
     @insurance_file_cnt = @insurance_file_cnt,  
     @risk_cnt = @risk_cnt,  
     @effective_date = @effective_date output  
  
    -- Check coinsurance and rate  
    Execute spu_Insurance_File_is_coinsured  
        @insurance_file_cnt,  
        @is_coinsured output,  
        @retained_percent output  
  

    -- Copy any original reinsurance  
    Execute spu_RI_Arrangement_copy  
        @insurance_file_cnt = @insurance_file_cnt,  
        @risk_cnt = @risk_cnt,  
        @effective_date = @effective_date,  
  @Trans_type=@transtype  
  

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
  Left Join Peril_type PT on P.Peril_Type_ID=PT.Peril_Type_ID     -- PN:61917 : Added By Upendra : The Premimum should not reflect on RI-Insurance Screen when the Levy Tax is selected.  
        Where   p.risk_cnt = @risk_cnt  
        And    (p.is_premium = 1 Or p.is_sum_insured = 1)  
 And Isnull(PT.is_levy_tax,0)=0						-- PN:61917 : Added By Upendra : The Premimum should not reflect on RI-Insurance Screen when the Levy Tax is selected.      
        And     rs.original_flag = 0  
        Group By  
                p.ri_band  
  

    -- Open the RI Bands Cursor and get first row  
    Open RI_Band_Cursor  
    Fetch Next From RI_Band_Cursor Into @ri_band, @sum_insured, @premium  
  
    -- Start processing  
    While (@@Fetch_Status = 0)  
    Begin  
        -- Apply the EML Percentage and Coinsurance  
        Select  @sum_insured = @sum_insured * @retained_percent * @eml_percent,  
                @premium = @premium * @retained_percent  
  
        -- Check for existing arrangement  
        Select  @ri_arrangement_id = null  
        Select  @ri_arrangement_id = ri_arrangement_id,  
                @is_modified = IsNull(is_modified, 1),  
    @ri_override_reason_id=isnull(ri_override_reason_id,0)  
        From    ri_arrangement  
        Where   risk_cnt = @risk_cnt  
        And     ri_band_id = @ri_band  
        And     original_flag = 0  
  
  -- Either insert or update our arrangement  
        If IsNull(@ri_arrangement_id, 0) > 0  
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
                    is_modified,version_id,  
     ri_override_reason_id)  
            Values (@risk_cnt,  
                    @ri_band,  
                    @sum_insured,  
                    @premium,  
                    0,  
                    1^@is_auto_reinsured,1,  
     @ri_override_reason_id)  
  
            -- Get new id and assume we have not modified  
            Select  @ri_arrangement_id = @@Identity,  
                    @is_modified = 0  
        End  
  
        -- Check if the band has been modified  
        If (@is_modified = 0)  
   If @transtype='REN'  And @TMPRisk_cnt_under_renewal IS NOT NULL  
    Begin  
     Execute spu_RI_Arrangement_copy_TMPFAC  
      @TMPRisk_cnt_under_renewal,  
      @ri_arrangement_id  
    End  
            -- It hasn't, or it's new so refresh it  
            Execute spu_RI_Arrangement_make  
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
                @policy_currency_rate = @policy_currency_rate,  
    @transtype = @transtype  
  

 --ReCalc this_share_percent & premium_percent  
/*  UPDATE ri_arrangement_line  
    SET this_share_percent = (sum_insured / convert(float, @sum_insured)) * 100.0000,  
        premium_percent = (sum_insured / convert(float, @sum_insured)) * 100.0000  
           WHERE ri_arrangement_id = @ri_arrangement_id*/  
  
SET @temp_premium = (
    SELECT SUM(premium_value)
    FROM RI_Arrangement_Line
    WHERE ri_arrangement_id = @ri_arrangement_id
    AND Is_Obligatory = 1
);
SET @sum_insured = (
    SELECT SUM(sum_insured)
    FROM RI_Arrangement_Line
    WHERE ri_arrangement_id = @ri_arrangement_id
    AND Is_Obligatory = 0
);

SET @premium = ISNULL(@premium,0) - ISNULL(@temp_premium,0)

UPDATE ri_arrangement_line  
SET 
    this_share_percent = 
        CASE 
            WHEN CONVERT(FLOAT, @sum_insured) = 0 OR sum_insured = 0 
                THEN CASE 
                    WHEN premium_value = 0 THEN 0 
                    ELSE (premium_value / CONVERT(FLOAT, @premium)) * 100.0000 
                END 
            ELSE  (sum_insured / CONVERT(FLOAT, @sum_insured)) * 100.0000 
        END,  
    premium_percent = 
        CASE 
            WHEN CONVERT(FLOAT, @premium) = 0 OR premium_value = 0 
                THEN CASE 
                    WHEN premium_value = 0 AND type <> 'F' THEN 0  
                    WHEN premium_value = 0 AND type = 'F' AND @sum_insured <> 0  
                        THEN (sum_insured / CONVERT(FLOAT, @sum_insured)) * 100.0000  
                    ELSE default_share_percent 
                END  
            ELSE 
                CASE 
                    WHEN CONVERT(FLOAT, @sum_insured) = 0 
                        THEN (premium_value / NULLIF(CONVERT(FLOAT, @premium), 0)) * 100.0000
                    ELSE (sum_insured / NULLIF(CONVERT(FLOAT, @sum_insured), 0)) * 100.0000
                END 
        END  
WHERE ri_arrangement_id = @ri_arrangement_id AND ISNULL(Is_Obligatory,0)=0;

        -- We should recalc all taxes, just to be safe  
        -- Note, this will also refresh the premium & comm shares  
        -- just in case the si/premium ratio has changed  
        -- If (RTRIM(LTRIM(@transtype)) <> 'PT')  
SET @premium = (
    SELECT SUM(premium)
    FROM RI_Arrangement
    WHERE ri_arrangement_id = @ri_arrangement_id
);

        Execute spu_RI_Arrangement_taxes  
         @insurance_file_cnt = @insurance_file_cnt,  
         @risk_cnt = @risk_cnt,  
            @ri_arrangement_id = @ri_arrangement_id,  
            @band_premium = @premium  
  
        -- Get next record  
        Fetch Next From RI_Band_Cursor Into @ri_band, @sum_insured, @premium  
    End  
  
    -- Close the cursor  
    Close RI_Band_Cursor  
    Deallocate RI_Band_Cursor  
  
    -- Delete any tax on arrangements on bands that are no longer in use  
    Delete  tax_calculation  
    Where   ri_arrangement_line_id In (  
                Select  rl.ri_arrangement_line_id  
                From    ri_arrangement_line rl  
    Inner Join ri_arrangement ri ON ri.ri_arrangement_id = rl.ri_arrangement_id  
                Where   ri.risk_cnt = @risk_cnt  
                And     (ri.ri_band_id Not In (  
                            Select  p.ri_band  
                            From    Peril p  
                            Join    Rating_Section rs  
                                    On  rs.risk_cnt = p.risk_cnt  
                                    And rs.rating_section_id = p.rating_section_id  
                            Where   p.risk_cnt = @risk_cnt  
                            And    (p.is_premium = 1 Or p.is_sum_insured = 1)  
                            And     rs.original_flag = 0 and ri_band is not null) or ri_band_id is null )
                And     ri.original_flag = 0)  
  
    -- Delete any arrangements on bands that are no longer in use  
    Delete  ri_arrangement_line  
    Where   ri_arrangement_id In (  
                Select  ri_arrangement_id  
                From    ri_arrangement  
                Where   risk_cnt = @risk_cnt  
                And     (ri_band_id Not In (  
                            Select  p.ri_band  
                            From    Peril p  
                            Join    Rating_Section rs  
                                    On  rs.risk_cnt = p.risk_cnt  
                                    And rs.rating_section_id = p.rating_section_id  
                            Where   p.risk_cnt = @risk_cnt  
                            And    (p.is_premium = 1 Or p.is_sum_insured = 1)  
                            And     rs.original_flag = 0 and ri_band is not null) or ri_band_id is null) 
                And     original_flag = 0)  
  
    Delete  ri_arrangement  
    Where   risk_cnt = @risk_cnt  
    And     (ri_band_id Not In (  
                Select  p.ri_band  
                From    Peril p  
                Join    Rating_Section rs  
                        On  rs.risk_cnt = p.risk_cnt  
                        And rs.rating_section_id = p.rating_section_id  
                Where   p.risk_cnt = @risk_cnt  
                And    (p.is_premium = 1 Or p.is_sum_insured = 1)  
                And     rs.original_flag = 0 and ri_band is not null)  or ri_band_id is null)
    And     original_flag = 0  


Go

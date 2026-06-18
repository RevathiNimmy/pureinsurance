SET QUOTED_IDENTIfIER OFF SET ANSI_NullS ON
GO


Execute DDLDropProcedure 'spu_RI2007Disabled_Arrangement_copy_from_original_PT'
GO
CREATE  PROCEDURE spu_RI2007Disabled_Arrangement_copy_from_original_PT  
    @risk_cnt int,
    @risk_type_id int,
    @effective_date datetime,
    @allow_deferred tinyint 
AS    
    
    Declare      
           
        @ri_arrangement_id int,      
        @new_ri_arrangement_id int,      
        @original_flag tinyint,      
              
        @old_ri_arrangement_line_id int,      
        @new_ri_arrangement_line_id int,      
        @ri_band_id int,
        @ri_model_id int,
        @ri_model_id_new int,
        @total_this_premium Money 
        
    -- Arrangement cursor      
    Declare Arrangement_Cursor Cursor Fast_Forward For      
        Select  ri_arrangement_id, ri_band_id, ri_model_id     
        From    ri_arrangement ra      
        Where   risk_cnt = @risk_cnt      
        And     original_flag = 1      
      
    Open Arrangement_Cursor      
    Fetch Next From Arrangement_Cursor Into @ri_arrangement_id, @ri_band_id, @ri_model_id      
      
    -- For each of the original arrangements      
    While @@Fetch_Status = 0
      Begin  
    
     -- Get the best RI Model for this ri_band and risk_type
    Set  @ri_model_id_new = Null
    Select  @ri_model_id_new = rmu.ri_model_id
    From    risk_type_ri_model_usage rmu
    Join    ri_model rm
            On rm.ri_model_id = rmu.ri_model_id
    Where   rmu.risk_type_id = @risk_type_id
    And     rmu.ri_band = @ri_band_id
    And     rmu.is_deleted = 0
    And     rmu.effective_date <= @effective_date
    And    (rmu.expiry_date >= @effective_date or IsNull(rmu.expiry_date, '1899.12.29') = '1899.12.29')
    And    (rm.ri_model_type = 0
        Or (rm.ri_model_type = 2 And @allow_deferred = 1))
    And     rm.is_deleted = 0
    And     rm.effective_date <= @effective_date
    And    (rm.expiry_date >= @effective_date or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')
    Order By
            rm.ri_model_type Desc,    -- give priority to none-deferred models
            rmu.effective_date Asc   -- give priority to newer models
 --Here we need to select total this premium for the risk_cnt 
              select @total_this_premium = sum(ra.premium)  from ri_arrangement ra
  inner join insurance_file_risk_link rl on ra.risk_cnt = rl.original_risk_cnt 
       inner join  insurance_file ifl  
       on ifl.insurance_file_cnt = rl.insurance_file_cnt  
       where ra.ri_band_id = @ri_band_id and rl.risk_cnt = @risk_cnt       
 --Now only move to new model when total_this_premium is not 0
 --if @total_this_premium <> 0
    Set  @ri_model_id_new = ISNULL(@ri_model_id_new, 0) 
   -- else
    --Set  @ri_model_id_new = @ri_model_id

 If @ri_model_id_new = @ri_model_id
  	Begin 
  	    --Delete if any 
  	    DELETE ral 
  	    FROM RI_Arrangement ra INNER JOIN ri_arrangement_line ral 
  	    ON ra.ri_arrangement_id = ral.ri_arrangement_id 
  	    WHERE ra.risk_cnt = @risk_cnt AND ra.ri_band_id = @ri_band_id 
  	          AND ra.ri_model_id = @ri_model_id AND ra.original_flag = 0
  	    
  	    DELETE RI_Arrangement 
  	    WHERE risk_cnt = @risk_cnt AND ri_band_id = @ri_band_id 
  	          AND ri_model_id = @ri_model_id AND original_flag = 0
  	           
        -- Copy arrangement from original     
        Insert Into ri_arrangement (      
                risk_cnt,      
                ri_band_id,      
                ri_model_id,      
                sum_insured,      
                premium,      
                original_flag,      
                is_modified,
				ri_override_reason_id)      
        Select  @risk_cnt,      
                ri_band_id,      
                ri_model_id,      
                0,  
                0,  
                0,      
                is_modified,
				ri_override_reason_id
        From    ri_arrangement      
        Where   risk_cnt = @risk_cnt      
        And     ri_arrangement_id = @ri_arrangement_id      
        
        If @@ROWCOUNT > 0 
			-- Get new id      
			Select  @new_ri_arrangement_id = @@Identity      
        Else 
			Set @new_ri_arrangement_id = 0
      
        Delete from ri_arrangement_line where ri_arrangement_id=@new_ri_arrangement_id      
    
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
                      line_limit,      
                      lower_limit,      
                      sum_insured * (-1),      
                      premium_value * (-1),  
                      commission_value * (-1),    
                      premium_tax * (-1),      
                      commission_tax * (-1),      
                      is_commission_modified,Retained,participation_percent,grouping      
              From    ri_arrangement_line  ril
              Where   ri_arrangement_id = @ri_arrangement_id      
              
              --update the RI arrangement since it is which origanally calculated
              Update ri_arrangement     
              Set sum_insured = (Select Sum(rl.sum_insured) From ri_arrangement_line rl 
                                    Where rl.ri_arrangement_id = @new_ri_arrangement_id),      
                  premium = (Select Sum(rl.premium_value) From ri_arrangement_line rl 
                                    Where rl.ri_arrangement_id = @new_ri_arrangement_id) 
              Where   risk_cnt = @risk_cnt      
              And ri_arrangement_id = @new_ri_arrangement_id 
              --PM025950(1 cent issue) Update old RI arrangement premium
              Update ri_arrangement     
              Set sum_insured = (Select Sum(rl.sum_insured) From ri_arrangement_line rl 
                                    Where rl.ri_arrangement_id = @ri_arrangement_id),      
                  premium = (Select Sum(rl.premium_value) From ri_arrangement_line rl 
                                    Where rl.ri_arrangement_id = @ri_arrangement_id) 
              Where   risk_cnt = @risk_cnt      
              And ri_arrangement_id = @ri_arrangement_id 
          End    
            -- Get next arrangement      
            Fetch Next From Arrangement_Cursor Into @ri_arrangement_id, @ri_band_id, @ri_model_id          
        End      
    -- Close and release cursor      
    Close Arrangement_Cursor      
    Deallocate Arrangement_Cursor      

GO  

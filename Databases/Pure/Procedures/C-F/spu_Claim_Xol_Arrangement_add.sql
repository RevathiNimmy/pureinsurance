SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_Xol_Arrangement_add'
GO

CREATE PROCEDURE spu_Claim_Xol_Arrangement_add      
    @claim_id int,      
    @ri_arrangement_id int,      
    @catastrophe_code_id int,      
    @layer int,      
    @ri_model_id int,      
    @trigger_limit money,      
    @xol_arrangement_id int output      
AS      
      
 Declare @base_priority int,  
  @base_arrangement_line_id int  
 DECLARE @version_id int  
 DECLARE @claim_xol_arrangement_id int  
  
 -- Check if this model is already in use for this claim, band and xol type, we don't allow recursive!  
 If Exists (  
  Select * From claim_xol_arrangement  
  Where   claim_id = @claim_id  
  And     ri_arrangement_id = @ri_arrangement_id  
  And     IsNull(catastrophe_code_id, 0) = IsNull(@catastrophe_code_id, 0)  
  And     ri_model_id = @ri_model_id)  
 Return  

   DECLARE  @model_currency_id INT,
    @claim_currency_id INT,
    @source_id INT, @model_currency_rate numeric(22,18),@effective_date datetime, @claim_currency_rate numeric(22,18)

      SELECT @claim_currency_id=c.currency_id, @source_id=source_id, @effective_date= create_date, @claim_currency_rate=c.currency_base_xrate 
      FROM claim c join insurance_file i on insurance_file_cnt=policy_id
      WHERE claim_id=@claim_id  

      SELECT @model_currency_id = RIM.currency_id
      FROM   RI_MODEL RIM  
      WHERE  RI_MODEL_ID = @ri_model_id

  If @model_currency_id <> @claim_currency_id Begin
        Execute spu_ACT_Get_Currency_Rate
            @model_currency_id,
            @source_id,
            @effective_date,
            @model_currency_rate Output
       
       If IsNull(@claim_currency_rate, 0) = 0
	Execute spu_ACT_Get_Currency_Rate
            @claim_currency_id,
            @source_id,
            @effective_date,
            @claim_currency_rate Output

        Select @model_currency_rate = @model_currency_rate / @claim_currency_rate
  End Else
        Select @model_currency_rate = 1

  
 -- We have excess ret payments to allocate so create new model  
 Select  @xol_arrangement_id = IsNull(Max(xol_arrangement_id), 0) + 1  
 From    claim_xol_arrangement  
  
 EXEC spu_CLM_get_claim_version  
   @claim_id = @claim_id,  
   @version_id = @version_id OUTPUT  
  
 Insert Into claim_xol_arrangement (  
  xol_arrangement_id,  
  claim_id,  
  ri_arrangement_id,  
  catastrophe_code_id,  
  layer,  
  ri_model_id,  
  trigger_limit,  
  version_id  
  )  
 Values (  
  @xol_arrangement_id,  
  @claim_id,  
  @ri_arrangement_id,  
  @catastrophe_code_id,  
  @layer,  
  @ri_model_id,  
  @trigger_limit,  
  @version_id  
  )  
  
 SELECT @claim_xol_arrangement_id = @@IDENTITY  
  
 UPDATE claim_xol_arrangement  
 SET base_claim_xol_arrangement_id = @claim_xol_arrangement_id,  
     xol_arrangement_id = @claim_xol_arrangement_id  
 WHERE claim_xol_arrangement_id = @claim_xol_arrangement_id  
  
 -- explicity change the return value to match with the newly  
 -- added xol arrangement id updated above....  
 SET  @xol_arrangement_id = @claim_xol_arrangement_id  
  
    -- Get base priority value, dependent on xol type and layer,  
    -- Note:  
    --    This formula leaves room for almost a million fac lines and  
    -- allows up to 768 layers of XOL with 4096 priorities per layer.  
    Select  @base_priority =  
            Case When @catastrophe_code_id Is Null Then  
                Power(2, 20) + 1  
            Else  
                Power(2, 22) + 1  
            End + (Power(2, 12) * (@layer - 1))  
  
    -- Get base id value  
    -- NB: this is overriden by the update later on in the process  
    Select  @base_arrangement_line_id = IsNull(Max(ri_arrangement_line_id), 0) + 1  
    From    claim_ri_arrangement_line  
    Where   claim_id = @claim_id  
    And     ri_arrangement_id = @ri_arrangement_id  
  
    -- Insert the arrangement lines  
    Insert Into claim_ri_arrangement_line (  
            claim_id,  
            ri_arrangement_line_id,  
            ri_arrangement_id,  
            type,  
            treaty_id,  
            xol_arrangement_id,  
            default_share_percent,  
            this_share_percent,  
            agreement_code,  
            priority,  
            number_of_lines,  
            line_limit,  
            sum_insured,  
            reserve, payment, salvage, recovery,  
            this_reserve, this_payment, this_salvage, this_recovery,  
          version_id)  
    Select  @claim_id,  
            @base_arrangement_line_id + ri_model_line_id, -- Get safe line_id  
            @ri_arrangement_id,  
            'X',  
            rml.treaty_id,  
            @claim_xol_arrangement_id,  
            rml.share_percent,  
            0,  
            t.agreement_code,  
            @base_priority + rml.priority,  
            rml.number_of_lines,  
            rml.line_limit* @model_currency_rate,  
            -- We need to calculate a max SI limit from the line config  
	        0,  
	        0, 0, 0, 0,  
            0, 0, 0, 0,  
          @version_id  
    From    ri_model_line rml  
    Join    Treaty t  
            On t.treaty_id = rml.treaty_id  
    Where   ri_model_id = @ri_model_id  
  
    -- update the claim ri arrangement line base_claim_ri_arrangement_line_id's  
    UPDATE claim_ri_arrangement_line  
    SET base_claim_ri_arrangement_line_id = claim_ri_arrangement_line_id,  
 ri_arrangement_line_id = claim_ri_arrangement_line_id  
    WHERE claim_id = @claim_id  
    AND version_id = @version_id  
    AND base_claim_Ri_arrangement_line_id IS NULL      
    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claims_recovery_reins_allocate'
GO

CREATE PROCEDURE spu_claims_recovery_reins_allocate  
    @peril_id int  
AS  
    Declare  
        @is_post_taxes tinyint,  
        @payment_date datetime  
  
    -- If we are not posting tax seperately we will need to add it  
    -- back on for the reinsurance amounts  
    Select  @is_post_taxes = IsNull(claims_is_post_taxes, 1)  
    From    risk_type rt  
    Join    risk r                 On r.risk_type_id = rt.risk_type_id  
    Join    claim wc          On wc.risk_type_id = r.risk_cnt  
    Join    claim_peril wcp   On wcp.claim_id = wc.claim_id  
    Where   wcp.claim_peril_id = @peril_id  
  
    -- We only want information from the last payment!!  
    Select  @payment_date = Max(date_of_payment)  
    From    claim_payment  
    Where   claim_peril_id = @peril_id  
    and     base_claim_payment_id = claim_payment_id
  
    -- Update treaty reinsurance  
    Update  cral  
    Set     this_salvage = payments.salvage,  
            this_recovery = payments.recovery  
    From   (Select  cp.claim_id,  
                    ral.ri_arrangement_line_id,  
      Sum(Case When rt.is_salvage = 1 Then  
    Case When @is_post_taxes = 0 Then wcpi.this_payment + wcpi.tax_amount  
                  Else wcpi.this_payment  
    End * wcpi.payment_loss_xrate  
   Else 0 End) salvage,  
                    Sum(Case When rt.is_salvage = 0 Then  
                         Case When @is_post_taxes = 0 Then wcpi.this_payment + wcpi.tax_amount  
                         Else wcpi.this_payment  
                         End * wcpi.payment_loss_xrate  
   Else 0 End) recovery  
            From    claim_peril cp  
            Join    recovery r  
                    On  r.claim_peril_id = cp.claim_peril_id  
            Join    recovery_type rt  
                    On  rt.recovery_type_id = r.recovery_type_id  
            Join    claim_ri_arrangement ra  
                    On  ra.ri_band_id = cp.ri_band  
                    And ra.claim_id = cp.claim_id  
            Join    claim_ri_arrangement_line ral  
                    On  ral.ri_arrangement_id = ra.ri_arrangement_id  
                    And ral.claim_id = cp.claim_id  
            Join    claim_payment p  
                    ON  p.claim_peril_id = cp.claim_peril_id  
  
                    And (p.treaty_id = ral.treaty_id  
                      Or p.party_cnt = ral.party_cnt)  
  
  Join claim_payment_item wcpi ON  
       p.claim_payment_id = wcpi.claim_payment_id  
   And wcpi.recovery_id = r.recovery_id  
  
            Where   cp.claim_peril_id = @peril_id  
                And p.date_of_payment = @payment_date  
		And wcpi.base_claim_payment_item_id = claim_payment_item_id
          Group By  
                    cp.claim_id,  
                    ral.ri_arrangement_line_id) payments  
    Join    claim_ri_arrangement_line cral  
            On  cral.claim_id = payments.claim_id  
            And cral.ri_arrangement_line_id = payments.ri_arrangement_line_id  
  
    -- Rollup reinsurance lines to arrangement  
    Update  ra  
    Set     this_salvage = ri.this_salvage,  
            this_recovery = ri.this_recovery  
    From   (Select  cp.claim_id,  
                    ra.risk_cnt,  
                    ra.ri_band_id,  
                    Sum(ral.this_salvage) this_salvage,  
                    Sum(ral.this_recovery) this_recovery  
            From    claim_peril cp  
            Join    claim_ri_arrangement ra  
                    On  ra.ri_band_id = cp.ri_band  
                    And ra.claim_id = cp.claim_id  
            Join    claim_ri_arrangement_line ral  
                    On  ral.ri_arrangement_id = ra.ri_arrangement_id  
                    And ral.claim_id = cp.claim_id  
            Where   cp.claim_peril_id = @peril_id  
            Group By  
                    cp.claim_id,  
                    ra.risk_cnt,  
                    ra.ri_band_id) ri  
    Join    claim_ri_arrangement ra  
            On  ra.claim_id = ri.claim_id  
            And ra.risk_cnt = ri.risk_cnt  
            And ra.ri_band_id = ri.ri_band_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

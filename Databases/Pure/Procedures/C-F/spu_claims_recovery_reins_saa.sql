SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claims_recovery_reins_saa'
GO

CREATE PROCEDURE spu_claims_recovery_reins_saa
    @peril_id int,    
    @is_salvage tinyint    
AS    
    
    Declare @is_share tinyint    
    
    -- Do we share taxes with reinsurers    
    Select  @is_share = is_share_with_re_insurers    
    From    risk_type rt    
    Join    risk r    
            On r.risk_type_id = rt.risk_type_id    
    Join    claim wc    
            On wc.risk_type_id = r.risk_cnt    
    Join    claim_peril wcp    
            On wcp.claim_id = wc.claim_id    
    Where   wcp.claim_peril_id = @peril_id    
    
    -- treaty reinsurance    
    Select  r.recovery_id,    
            ral.ri_arrangement_line_id,    
            ral.treaty_id,    
            ral.party_cnt,    
            Case When ral.type = 'F' Then p.name Else t.description End,    
            IsNull(ral.this_share_percent, ral.default_share_percent) this_share_percent, 
           (Select  Sum(wcpi.this_payment * wcpi.payment_loss_xrate)    
            From    claim_payment p    
      Join    claim_payment_item wcpi    
                    ON p.claim_payment_id = wcpi.claim_payment_id    
            Where   p.claim_peril_id = cp.claim_peril_id    
            And     wcpi.recovery_id = r.recovery_id    
            And    (p.treaty_id = ral.treaty_id    
                Or  p.party_cnt = ral.party_cnt)) to_date,    
            @is_share is_tax_shared,    
   ral.this_payment,    
   ral.salvage-ral.this_salvage,    
   ral.this_salvage,    
   ral.recovery-ral.this_recovery,    
   ral.this_recovery    
    From    claim_peril cp    
    Join    recovery r    
            On  r.claim_peril_id = cp.claim_peril_id    
    Join    recovery_type rt    
            On  rt.recovery_type_id = r.recovery_type_id    
    Join    claim_ri_arrangement rra    
            On  rra.ri_band_id = cp.ri_band    
            And rra.claim_id = cp.claim_id    
    Join    claim_ri_arrangement_line ral    
            On  ral.ri_arrangement_id = rra.ri_arrangement_id    
            And ral.claim_id = cp.claim_id    
    Left Join    
            treaty t    
            On  t.treaty_id = ral.treaty_id    
    Left Join    
            party p    
            On p.party_cnt = ral.party_cnt    
    Where   cp.claim_peril_id = @peril_id    
        And rt.is_salvage = @is_salvage    
    
    Order By    
            r.recovery_id 



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

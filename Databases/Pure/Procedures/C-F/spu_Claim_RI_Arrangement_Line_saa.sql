SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Line_saa'
GO

CREATE PROCEDURE spu_Claim_RI_Arrangement_Line_saa  
    @claim_id int,  
    @ri_arrangement_id int,
	@Mode int=1  
AS  
  
    Select  -- RI Grid fields  
            Case When type = 'F' Then p.resolved_name Else t.description End [ri_name],  
            ral.default_share_percent / 100,  
            ral.this_share_percent / 100,  
            ral.sum_insured,  
            Case @Mode          
                  When 0 Then  ral.reserve - ral.this_reserve          
                  Else ral.reserve          
            End as Reserve, 
            ral.this_reserve,  
            Case       
            When   @Mode=0  Then   (ral.payment) - (ral.this_Payment)    
                  Else  ral.payment    
            End as Payment,   
            ral.this_payment,  
            Case @Mode          
                  When 0 Then (ral.reserve ) - (ral.payment)    
                  Else (ral.reserve + ral.this_reserve) - (ral.payment + ral.this_Payment)    
            End balance,  
            ral.agreement_code,  
            -- Supporting fields  
            ral.ri_arrangement_line_id,  
            ral.type,  
            ral.treaty_id,  
            ral.party_cnt,  
            ral.xol_arrangement_id,  
            ral.priority,  
            ral.number_of_lines,  
            ral.line_limit,  
            -- Xol fields  
            xa.layer,  
            xa.catastrophe_code_id,  
            cc.code,  
            xa.ri_model_id,  
            Case When xa.catastrophe_code_id Is Null Then -- next layer model id  
                rm.xol_clm_ri_model_id Else rm.xol_cat_ri_model_Id End xol_ri_model_id,  
            Case When xa.catastrophe_code_id Is Null Then -- next layer trigger amount  
                rm.xol_clm_limit Else rm.xol_cat_limit End xol_limit,
		--(WPR2 - Reinsurance Obligatory)
		ral.is_obligatory
 
    From    Claim_RI_Arrangement_Line ral  
    Left Join  
            Treaty t  
            On t.treaty_id = ral.treaty_id  
    Left Join  
            Reinsurance_type rt  
            On rt.reinsurance_type_id = t.reinsurance_type_id  
    Left Join  
            Party p  
            On p.party_cnt = ral.party_cnt  
    Left Join  
            claim_xol_arrangement xa  
            On xa.xol_arrangement_id = ral.xol_arrangement_id  
            And xa.claim_id = ral.claim_id  
    Left Join  
            ri_model rm  
            On rm.ri_model_id = xa.ri_model_id  
    Left Join  
            catastrophe_code cc  
            On cc.catastrophe_code_id = xa.catastrophe_code_id  
    Where   ral.claim_id = @claim_id  
    And     ral.ri_arrangement_id = @ri_arrangement_id  
    Order By  
            ral.ri_arrangement_id, ral.priority, ral.default_share_percent  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

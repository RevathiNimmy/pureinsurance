SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claim_get_clientagent'
GO

CREATE PROCEDURE spu_claim_get_clientagent  
    @claim_id int  
AS  
  
    Select  c.party_cnt client_party_cnt,  
            c.resolved_name client_resolved_name,  
            a.party_cnt agent_party_cnt,  
            a.resolved_name agent_resolved_name,  
            i.product_id product_id  
    From    claim wc  
    Join    insurance_file i  
            On i.insurance_file_cnt = wc.policy_id  
    Join    party c -- client details  
            On c.party_cnt = i.insured_cnt  
    Left Join  
            party a -- agent details  
            On a.party_cnt = i.lead_agent_cnt  
    Where   wc.claim_id = @claim_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

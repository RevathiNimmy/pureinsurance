--Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(6.2.1)
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Attached_Parties_On_Claim'
GO
CREATE PROCEDURE spu_Get_Attached_Parties_On_Claim 
@Claim_Id INT
AS

SELECT      c.party_cnt client_party_cnt,
            c.ShortName Client_Code,  
            c.resolved_name client_resolved_name,  
            a.party_cnt agent_party_cnt,  
            a.resolved_name agent_resolved_name,
            a.ShortName AgentCode  
             
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

--End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(6.2.1)
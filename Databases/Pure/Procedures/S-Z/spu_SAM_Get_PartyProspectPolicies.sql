SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_PartyProspectPolicies'
GO
--Start (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.21) 
CREATE procedure spu_SAM_Get_PartyProspectPolicies  
@party_cnt int  
as  
Begin  
select  
 party_cnt ,  
 prospect_policy_id ,   
 risk_group_id ,  
 renewal_date ,  
 no_of_times_quoted ,  
 target_premium   
from Prospect_Policy  
where party_cnt=@party_cnt 
ORDER BY prospect_policy_id ASC  
End  
--End (girija) - (UIIC WR27 - MTA Amend Client.doc) - (7.7.3.1.21) 

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


  

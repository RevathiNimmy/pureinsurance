SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_Get_Agent_Detail_ForAccount
GO

CREATE PROCEDURE spu_Get_Agent_Detail_ForAccount    
@account_id INT    
AS    
SELECT pat.code,pa.is_gross_agent,ac.currency_id,pt.code,p.source_id,pa.binder_indicator FROM party p    
INNER JOIN account ac ON ac.account_key=p.party_cnt    
INNER JOIN party_type pt ON p.party_type_id=pt.party_type_id    
LEFT JOIN party_agent pa ON pa.party_cnt=p.party_cnt    
LEFT JOIN party_agent_type pat ON pat.party_agent_type_id=pa.party_agent_type_id    
WHERE ac.account_id=@account_id 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
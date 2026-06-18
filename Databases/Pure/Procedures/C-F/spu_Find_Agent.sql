SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Find_Agent'
GO


CREATE PROCEDURE spu_Find_Agent    
@shortname varchar(200) = NULL,    
@name varchar(200) = NULL,    
@partyAgentDesc varchar(200) = NULL,    
@CurrCode varchar(50) = NULL,    
@SubBraDesc varchar(100) = NULL,    
@IsGrossAgent varchar(100)=Null    

As    

BEGIN    

SELECT DISTINCT p1.party_cnt,    
shortname,    
name,      
party_agent_type.description,    
Currency.code,    
Sub_Branch.description     
 
FROM Party p1
join Party_Type on Party_Type.party_type_id = p1.party_type_id 
join Party_Agent on Party_Agent.party_cnt = p1.party_cnt
join Party_Agent_Type on Party_Agent_Type.party_agent_type_id = Party_Agent.party_agent_type_id 
join Currency on p1.currency_id = currency.currency_id
join Sub_Branch on p1.source_id = Sub_Branch.source_id
WHERE 
p1.is_deleted = 0  
AND Party_type.code = 'AG'  
AND Party_Type.is_deleted = 0  
AND Party_Agent_Type.is_visible = 1
And p1.shortname = ISNULL(@shortname,shortname)  
And ( @Name IS NULL OR p1.name LIKE @name)
And party_agent_type.description =ISNULL(@partyAgentDesc,party_agent_type.description )  
And Currency.code=ISNULL(@CurrCode,currency.code)  
And Sub_Branch.description =ISNULL(@SubBraDesc,Sub_Branch.description)  
And (Party_agent.is_gross_agent= ISNULL(@IsGrossAgent,Party_agent.is_gross_agent) or Party_Agent_Type.party_agent_type_id<>1)  
ORDER BY Shortname    
END   
GO



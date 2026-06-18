SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Account_Details_For_InsuranceFile'
GO

CREATE PROCEDURE spu_SAM_Get_Account_Details_For_InsuranceFile   
    
@insurance_file_cnt int,
@debit_against_client int=0    
    
AS      
  
 DECLARE @business_type_code varchar(20)    
 DECLARE @agent_account_id int    
 DECLARE @client_account_id int    
 DECLARE @account_id_to_use int    
 DECLARE @agent_type_code varchar(20)  
  
    
 -- get the account from the insurance file    
 Select    
 @business_type_code = business_type.code,    
 @agent_account_id = agent_account.account_id,    
 @client_account_id = client_account.account_id,   
 @agent_type_code = party_agent_type.code    
  
 FROM insurance_file    
    
  LEFT JOIN business_type ON    
   business_type.business_type_id = insurance_file.business_type_id    
    
  LEFT JOIN Account client_account ON    
   client_account.account_key = insurance_file.insured_cnt    
    
  LEFT JOIN Account agent_account ON    
   agent_account.account_key = insurance_file.lead_agent_cnt    
  
  LEFT JOIN Party agent ON  
 agent.party_cnt = insurance_file.lead_agent_cnt    
  
   LEFT JOIN party_agent ON   
 party_agent.party_cnt = agent.party_cnt  
  
  LEFT JOIN party_agent_type ON   
 party_agent_type.party_agent_type_id = party_agent.party_agent_type_id  
  
 WHERE insurance_file_cnt = @insurance_file_cnt    
    
 IF LTRIM(RTRIM(@business_type_code)) = 'DIRECT'    
 SELECT @account_id_to_use = @client_account_id    
 ELSE    
 -- if the lead agent is a commission account use the insured   
 IF LTRIM(RTRIM(@agent_type_code)) = 'Comm Acc'  
 BEGIN  
  SELECT @account_id_to_use = @client_account_id      
 END 

 ELSE	--if agent type in Intermediary and debit is againts client 
   IF LTRIM(RTRIM(@agent_type_code)) = 'Intermed' AND  @debit_against_client=1
   BEGIN
	SELECT @account_id_to_use = @client_account_id  	
   END    
 ELSE  
 BEGIN  
  SELECT @account_id_to_use = @agent_account_id    
 END  
  
SELECT  account_id,   
 account_name,   
 short_code    
    
FROM account    
    
WHERE account_id = @account_id_to_use    
  
  



GO

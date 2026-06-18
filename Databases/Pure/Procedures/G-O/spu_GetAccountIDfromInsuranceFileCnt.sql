SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GetAccountIDfromInsuranceFileCnt'
GO

CREATE Procedure spu_GetAccountIDfromInsuranceFileCnt    
@insurance_file_cnt BIGINT ,   
@CheckforAccountType int=0  
AS    
DECLARE @lead_agent_cnt BIGINT,    
@Client_cnt BIGINT,  
@Party_type_code varchar(12)  
  
SELECT @lead_agent_cnt=lead_agent_cnt,@Client_cnt=Insured_cnt FROM    
insurance_file WHERE insurance_file_cnt=@insurance_file_cnt    
  
Select @Party_type_code=pt.code   
FROM party_agent_type pt join party_agent pa   
ON pt.party_agent_type_id=pa.party_agent_type_id  
WHERE party_cnt=@lead_agent_cnt  
  
  
IF @lead_agent_cnt IS NOT NULL   
BEGIN  
 IF @CheckforAccountType =1 AND @Party_type_code='Comm Acc'  
 BEGIN  
   SELECT Account_id FROM Account    
   Join Party ON    
  Account.Account_key=Party.Party_cnt    
   JOIN Insurance_file    
   ON insurance_file.insured_cnt=party.Party_cnt    
   WHERE insurance_file.Insurance_file_cnt=@insurance_file_cnt    
 END  
 ELSE  
 BEGIN  
  SELECT Account_id FROM Account    
  Join Party ON    
  Account.Account_key=Party.Party_cnt    
  JOIN Insurance_file    
  ON insurance_file.lead_agent_cnt=party.Party_cnt    
  WHERE insurance_file.Insurance_file_cnt=@insurance_file_cnt     
 END  
   
END    
ELSE    
BEGIN    
 SELECT Account_id FROM Account    
 Join Party ON    
 Account.Account_key=Party.Party_cnt    
 JOIN Insurance_file    
 ON insurance_file.insured_cnt=party.Party_cnt    
 WHERE insurance_file.Insurance_file_cnt=@insurance_file_cnt    
END    
  

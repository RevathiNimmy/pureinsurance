SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Is_Policy_Paid'
GO

CREATE PROCEDURE spu_ACT_Is_Policy_Paid      
      
@insurance_file_cnt int      
      
AS BEGIN      
      
 DECLARE @business_type_code varchar(20)      
 DECLARE @agent_account_id int      
 DECLARE @client_account_id int      
 DECLARE @account_id_to_use int      
 DECLARE @lead_agent_cnt int   

 -- get the account from the insurance file      
 SELECT  @business_type_code = business_type.code,      
  @agent_account_id = agent_account.account_id,      
  @client_account_id = client_account.account_id,      
  @lead_agent_cnt  = insurance_file.lead_agent_cnt
 FROM insurance_file      
      
  LEFT JOIN business_type ON      
   business_type.business_type_id = insurance_file.business_type_id      
      
  LEFT JOIN Account client_account ON      
   client_account.account_key = insurance_file.insured_cnt      
      
  LEFT JOIN Account agent_account ON      
   agent_account.account_key = insurance_file.lead_agent_cnt      
      
 WHERE insurance_file_cnt = @insurance_file_cnt      
      
      
 IF LTRIM(RTRIM(@business_type_code)) = 'DIRECT'      
  SELECT @account_id_to_use = @client_account_id      
 ELSE BEGIN
	-- Premium never gets posted to Agency Commission Account so we need to pick Client Account id        
	 IF LTRIM(RTRIM(@business_type_code)) = 'AGENCY' AND EXISTS(SELECT NULL 
																 FROM PARTY_AGENT 
																WHERE PARTY_CNT = @lead_agent_cnt AND PARTY_AGENT_TYPE_ID IN(3))
	   SELECT @account_id_to_use = @client_account_id
	   ELSE IF LTRIM(RTRIM(@business_type_code)) = 'AGENCY' AND EXISTS(SELECT * FROM PARTY_AGENT WHERE PARTY_CNT = @lead_agent_cnt AND PARTY_AGENT_TYPE_ID IN(5))  
    SELECT TOP 1 @account_id_to_use =account_id FROM transdetail td 
							INNER JOIN Document d on d.document_id=td.document_id 
								WHERE d.insurance_file_cnt=@insurance_file_cnt AND td.spare='GROSS' order by td.document_sequence
	ELSE  
	   SELECT @account_id_to_use = @agent_account_id  
 END           
    
 SELECT ISNULL(Sum(amount),0) as amount, ISNULL(Sum(outstanding_amount),0) as outstanding_amount    
 FROM TransDetail      
 INNER JOIN Document d ON TransDetail.document_id = d.document_id      
 WHERE  d.insurance_file_cnt = @insurance_file_cnt      
 AND  account_id = @account_id_to_use      
      
END      


GO

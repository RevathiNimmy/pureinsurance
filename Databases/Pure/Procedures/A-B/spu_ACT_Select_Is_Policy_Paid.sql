SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_Is_Policy_Paid'
GO

CREATE PROCEDURE spu_ACT_Select_Is_Policy_Paid  
  
@insurance_file_cnt int,  
@policy_is_paid tinyint OUTPUT,  
@base_balance money OUTPUT  
  
AS  
  
BEGIN  
  
 DECLARE @business_type_code varchar(20)  
 DECLARE @agent_account_id int  
 DECLARE @client_account_id int  
 DECLARE @account_id_to_use int  
  
 -- get the account from the insurance file  
 Select  @business_type_code = business_type.code,  
  @agent_account_id = agent_account.account_id,  
  @client_account_id = client_account.account_id  
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
 ELSE  
  SELECT @account_id_to_use = @agent_account_id  
  
 SELECT  @base_balance= ISNULL(SUM(outstanding_amount) ,0)  
 FROM TransDetail  
 INNER JOIN Document d ON TransDetail.document_id = d.document_id  
 WHERE  d.insurance_file_cnt = @insurance_file_cnt  
 AND  account_id = @account_id_to_use  
  
 -- set the policy is paid indicator  
 IF @base_balance <> 0  
  SET @policy_is_paid = 0  
 ELSE  
  SET @policy_is_paid = 1  
  
  
END  



GO

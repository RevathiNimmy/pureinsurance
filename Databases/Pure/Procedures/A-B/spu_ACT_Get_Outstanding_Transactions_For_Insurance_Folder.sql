SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Outstanding_Transactions_For_Insurance_Folder'
GO

CREATE PROCEDURE spu_ACT_Get_Outstanding_Transactions_For_Insurance_Folder  
  
@insurance_file_cnt int,  
@outstandingbalance money OUTPUT,
@nViaCreditControl tinyint = 0

AS  
  
 DECLARE @insurance_ref varchar(30)  
 DECLARE @business_type_code varchar(20)      
 DECLARE @agent_account_id int      
 DECLARE @client_account_id int      
 DECLARE @account_id_to_use int
 Declare @agent_type char(10)      
      
 -- get the account from the insurance file      
 Select  @business_type_code = business_type.code,      
  @agent_account_id = agent_account.account_id,      
  @client_account_id = client_account.account_id, 
  @insurance_ref = insurance_file.insurance_ref,
  @agent_type = party_agent_type.code        
 FROM insurance_file      
      
  LEFT JOIN business_type ON      
   business_type.business_type_id = insurance_file.business_type_id      
      
  LEFT JOIN Account client_account ON      
   client_account.account_key = insurance_file.insured_cnt      
      
  LEFT JOIN Account agent_account ON      
   agent_account.account_key = insurance_file.lead_agent_cnt

   LEFT JOIN party_agent ON 
	   agent_account.account_key = party_agent.party_cnt

	left join party_agent_type on
	party_agent.party_agent_type_id = party_agent_type.party_agent_type_id
      
 WHERE insurance_file_cnt = @insurance_file_cnt      
      
  SELECT @account_id_to_use = @client_account_id      
  
  if @business_type_code <> 'DIRECT' and @agent_type <> 'Comm Acc'
	BEGIN
		if @agent_type = 'Intermed'
			select @account_id_to_use = intermediary_agent_account_id from Insurance_File where insurance_file_cnt = @insurance_file_cnt
		else
			select @account_id_to_use = @agent_account_id    
	END      


-- get transdetail records  
 SELECT @outstandingbalance = ISNULL(SUM(outstanding_amount),0)
 FROM Transdetail  
  
 INNER JOIN Document ON  
  transdetail.document_id = document.document_id  
  
  INNER JOIN DocumentType On  
   document.documenttype_id = DocumentType.documenttype_id  
  
 WHERE insurance_ref = @insurance_ref  
 AND account_id = @account_id_to_use  
 AND outstanding_amount <> 0  
  
 -- dont include claims transactions  
 AND DocumentType.code not in ('CLA','CLO','CLP','CLR')  
 AND (@nViaCreditControl =0 OR (@nViaCreditControl=1 AND document.insurance_file_cnt>=@insurance_file_cnt))
 
-- get transdetail records  
 SELECT account_id, transdetail_id, outstanding_amount, RTRIM(DocumentType.code)
 FROM Transdetail  
  
 INNER JOIN Document ON  
  transdetail.document_id = document.document_id  
  
  INNER JOIN DocumentType On  
   document.documenttype_id = DocumentType.documenttype_id  
  
 WHERE insurance_ref = @insurance_ref  
 AND account_id = @account_id_to_use  
 AND outstanding_amount <> 0  
  
 -- dont include claims transactions  
 AND DocumentType.code not in ('CLA','CLO','CLP','CLR')  
 AND (@nViaCreditControl =0 OR (@nViaCreditControl=1 AND document.insurance_file_cnt>=@insurance_file_cnt))


GO

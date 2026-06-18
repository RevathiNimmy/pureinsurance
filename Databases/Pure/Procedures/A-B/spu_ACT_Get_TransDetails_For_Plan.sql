

SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_ACT_Get_TransDetails_For_Plan'
GO

CREATE PROCEDURE spu_ACT_Get_TransDetails_For_Plan  
 @pfprem_finance_cnt int  
AS  
 
BEGIN
	DECLARE @Account_Id_Agent INT
	DECLARE @Account_Id_Client INT

	SELECT @Account_Id_Agent = account_id FROM account WHERE account_key IN 
    (SELECT TOP 1 agent_cnt FROM pfpremiumfinance WHERE pfprem_finance_cnt  = @pfprem_finance_cnt)

	SELECT @Account_Id_Client = account_id FROM account WHERE account_key IN 
    (SELECT TOP 1 clientid FROM pfpremiumfinance WHERE pfprem_finance_cnt  = @pfprem_finance_cnt)
 
	Select t.transdetail_id,t.currency_amount,d.document_id,d.document_ref,
    d.documenttype_id,d.insurance_file_cnt,t.account_id
  	from transdetail t Left Join document d
	On t.document_id = d.document_id 
    Where t.account_id IN  (@Account_Id_Agent,@Account_Id_Client)
	AND d.documenttype_id in(17,18)
        AND t.spare <>'COMM'
	AND  d.insurance_file_cnt in(
	Select insurance_file_cnt from insurance_file where insurance_folder_cnt in
	(Select insurance_folder_cnt From insurance_file where insurance_file_cnt in 
	(Select Insurance_file_cnt from pfpremiumfinance Where pfprem_finance_cnt = @pfprem_finance_cnt)))
 
END
GO


 

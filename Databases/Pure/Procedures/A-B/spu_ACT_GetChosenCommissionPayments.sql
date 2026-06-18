SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_ACT_GetChosenCommissionPayments'
GO

CREATE PROCEDURE spu_ACT_GetChosenCommissionPayments
(
 @batch_Id INT
)  
AS  
SELECT	td.transdetail_id,
	td.account_id,
	td.currency_id,
	td.outstanding_amount,
	td.account_currency_id,
	td.outstanding_account_amount,
	MIN(currency.description) currency_description,  
	MIN(currency.format_string) currency_format_string 
FROM	TransDetail td
INNER JOIN
(
 SELECT DISTINCT account.* 
 FROM account 
 INNER JOIN TransDetail td1
 ON account.account_id = td1.account_id --@account_id  
 WHERE td1.commission_payment_batch_id = @batch_Id
 UNION  
 SELECT * 
 FROM account 
 WHERE account_key IN
 (  
  SELECT pa.party_cnt 
  FROM Party_agent pa 
  JOIN party ON pa.party_cnt=party.party_cnt  
  WHERE linked_account_group = 
  (
   Select DISTINCT account_key 
   FROM Account 
   INNER JOIN TransDetail td2 
   ON Account.account_id = td2.account_id 
   WHERE td2.commission_payment_batch_id = @batch_Id
  )
  And ISNULL(is_grouped_claim_settlement,0) = 1
 )
) 
AS	account ON  td.account_id = account.account_id 	
INNER JOIN Currency ON td.currency_id = currency.currency_id  
INNER JOIN Currency Base ON td.amount_currency_id = base.currency_id  
WHERE td.commission_payment_batch_id = @batch_Id 
GROUP BY td.transdetail_id,
td.account_id,
td.currency_id,
td.outstanding_amount,
td.account_currency_id,
td.outstanding_account_amount
GO

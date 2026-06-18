SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_CLM_Get_Claim_Payment_Accounts_Details' 
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Payment_Accounts_Details  
 @claim_id int = 0,  
 @claim_payment_id int = 0  
AS  
  
BEGIN  
  
 -- if a claim payment id isnt passed  
 IF @claim_payment_id = 0  
 BEGIN  
  -- get the claim payment id from the claim id  
  SELECT @claim_payment_id = claim_payment_id  
  FROM claim_payment  
  WHERE claim_payment_id = base_claim_payment_id  
  AND claim_id = @claim_id  
 END  
  
 -- if we dont have a claim payment id now  
 If ISNULL(@claim_payment_id, 0) = 0  
 BEGIN  
  -- quit  
  RETURN  
 END  
  
  DECLARE @CLMPAYABLE_account_id int  
  
  --retrieve the CLMPAYABLE account id  
 SELECT @CLMPAYABLE_account_id = account_id FROM account WHERE short_code = 'CLMPAYABLE'  
  
  -- return claim payment details  
 SELECT  
  claim_payment_id,  
  -(cp.amount + cp.tax_amount + cp.tax_amount_WHT)as total_payment_amount,  
  CASE WHEN cp.party_cnt IS NULL THEN @CLMPAYABLE_account_id  
  ELSE a.account_id  
  END account_id,  
  cp.currency_id,  
  payeemediatype,  
  payeename,  
  payeebankname,  
  payeesortcode,  
  payeeaccountno,  
  payeecountry,  
  payeecomments,  
  media_ref,  
  payeeaddress1,  
  payeeaddress2,  
  payeeaddress3,  
  payeeaddress4,  
  payeepostalcode,  
  thirdpartyreference,  
  document_id,  
  ifile.source_id,  
  our_ref  ,
  party_bank_id,
  business_identifier_code,
  international_bank_account_number

  
FROM claim_payment cp  
  
 INNER JOIN claim c ON  
  cp.claim_id = c.claim_id  
  
  INNER JOIN insurance_file ifile ON  
   ifile.insurance_file_cnt = c.policy_id  
  
  LEFT JOIN account a ON  
   cp.party_cnt = a.account_key  
  WHERE claim_payment_id = @claim_payment_id  
  
END 



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
 

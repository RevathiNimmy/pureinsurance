SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Payment_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Payment_Details  
 @claim_id int,  
 @claim_payment_id int = 0  
AS  

BEGIN  
--**********************************************
--  Decide whether we are Underwriter or Broker
--**********************************************
   
DECLARE @AgentUnderwriter varchar(1)  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'    
IF @AgentUnderwriter = 'U' 
BEGIN  

 SELECT  

  claim_payment.claim_payment_id,  
  claim_payment.claim_peril_id,  
  claim_payment.date_of_payment,  
  claim_payment.amount,  
  claim_payment.tax_amount,  
  claim_payment.party_cnt,  
  claim_payment.comments,  
  claim_payment.is_referred,  
  claim_payment.created_by,  
  claim_payment.PayeeMediaType,  
  claim_payment.PayeeName,  
  claim_payment.PayeeBankName,  
  claim_payment.PayeeSortCode,  
  claim_payment.PayeeAccountNo,  
  claim_payment.PayeeCountry,  
  claim_payment.PayeeComments,  
  claim_payment.SequenceNo,  
  claim_payment.treaty_id,  
  claim_payment.claim_payment_to_id,  
  claim_payment.payment_party_to,  
  claim_payment.Insured_domiciled,  
  claim_payment.insured_percentage,  
  claim_payment.insured_tax_number,  
  claim_payment.payee_domiciled,  
  claim_payment.payee_percentage,  
  claim_payment.payee_tax_number,  
  claim_payment.safe_harbour_id,  
  claim_payment.safe_harbour_percentage,  
  claim_payment.is_tax_exempt,  
  claim_payment.is_wht_exempt,  
  claim_payment.is_settlement,  
  claim_payment.document_id,  
  party.resolved_name,  
  claim_payment.payment_party_to & 1 as Claim_Payable,  
  claim_payment.payment_party_to & 2 as Party,  
  claim_payment.payment_party_to & 4 as Agent,  
  claim_payment.payment_party_to & 8 as client,  
  claim_payment.media_ref,  
  claim_payment_to.description,  
  safe_harbour.description,  
  mediatype.description ,  
  country.description, 
  claim_payment.excess_amount,  
  claim_payment.PayeeAddress1,
  claim_payment.PayeeAddress2,
  claim_payment.PayeeAddress3,
  claim_payment.PayeeAddress4,
  claim_payment.PayeePostalCode, 
  claim_payment.ThirdPartyReference,
  claim_payment.cheque_date,
  claim_payment.Party_Bank_Id,
--Start(Saurabh Agrawal) Tech Spec LOA010 Claim payment Improvements
  claim_payment.our_ref,
--End(Saurabh Agrawal) Tech Spec LOA010 Claim payment Improvements	
  claim_payment.is_ex_gratia,
  claim_payment.business_identifier_code,
  claim_payment.international_bank_account_number
  FROM claim_payment  
  
  INNER JOIN claim ON  
   claim.claim_id  = claim_payment.claim_id  
  
   INNER JOIN currency ON  
   claim.currency_id = currency.currency_id  
  
   INNER JOIN risk ON  
    claim.risk_type_id = risk.risk_cnt  
  
    INNER JOIN risk_type ON  
     risk.risk_type_id = risk_type.risk_type_id  
  
  LEFT JOIN party ON  
  party.party_cnt = claim_payment.party_cnt  
  
  LEFT JOIN claim_payment_to ON  
  claim_payment.claim_payment_to_id = claim_payment_to.claim_payment_to_id  
  
  LEFT JOIN safe_harbour ON  
  claim_payment.safe_harbour_id = safe_harbour.safe_harbour_id  
  
  LEFT JOIN mediatype ON  
  claim_payment.payeemediatype = mediatype.mediatype_id  
  
  LEFT JOIN country ON  
  claim_payment.PayeeCountry = Country.Country_id  
  
 WHERE claim.claim_id = @claim_id  
  
 -- this procedure should return  
 -- either the latest work payment that is being processed (claim_payment_id is set to 0 and is_live is set to 0)  
 -- or historic payments specified by claim_payment_id  
 AND (  
 ((@claim_payment_id = 0)  
   AND  
 (claim_payment.base_claim_payment_id = claim_payment.claim_payment_id))  
  
 OR  
  
 (claim_Payment.claim_payment_id = @claim_payment_id)  
     )  
 -- AND ISNULL(claim.transaction_type_id, 0) not in (5,6) -- to remove any recovery receipt records  
END
ELSE
BEGIN

 SELECT  
  claim_payment.claim_payment_id,  
  claim_payment.claim_peril_id,  
  claim_payment.date_of_payment,  
  claim_payment.amount,  
  claim_payment.tax_amount,  
  claim_payment.party_cnt,  
  claim_payment.comments,  
  claim_payment.is_referred,  
  claim_payment.created_by,  
  claim_payment.PayeeMediaType,  
  claim_payment.PayeeName,  
  claim_payment.PayeeBankName,  
  claim_payment.PayeeSortCode,  
  claim_payment.PayeeAccountNo,  
  claim_payment.PayeeCountry,  
  claim_payment.PayeeComments,  
  claim_payment.SequenceNo,  
  claim_payment.treaty_id,  
  claim_payment.claim_payment_to_id,  
  claim_payment.payment_party_to,  
  claim_payment.Insured_domiciled,  
  claim_payment.insured_percentage,  
  claim_payment.insured_tax_number,  
  claim_payment.payee_domiciled,  
  claim_payment.payee_percentage,  
  claim_payment.payee_tax_number,  
  claim_payment.safe_harbour_id,  
  claim_payment.safe_harbour_percentage,  
  claim_payment.is_tax_exempt,  
  claim_payment.is_wht_exempt,  
  claim_payment.is_settlement,  
  claim_payment.document_id,  
  party.resolved_name,  
  claim_payment.payment_party_to & 1 as Claim_Payable,  
  claim_payment.payment_party_to & 2 as Party,  
  claim_payment.payment_party_to & 4 as Agent,  
  claim_payment.payment_party_to & 8 as client,  
  claim_payment.media_ref,  
  claim_payment_to.description,  
  safe_harbour.description,  
  mediatype.description ,  
  country.description, 
  claim_payment.excess_amount, 
  claim_payment.PayeeAddress1,
  claim_payment.PayeeAddress2,
  claim_payment.PayeeAddress3,
  claim_payment.PayeeAddress4,
  claim_payment.PayeePostalCode, 
  claim_payment.ThirdPartyReference,
  claim_payment.cheque_date,  
  claim_payment.party_bank_id,
  --Start(Saurabh Agrawal) Tech Spec LOA010 Claim payment Improvements
  claim_payment.our_ref,
--End(Saurabh Agrawal) Tech Spec LOA010 Claim payment Improvements	
  claim_payment.is_ex_gratia
  FROM claim_payment  
  
  INNER JOIN claim ON  
   claim.claim_id  = claim_payment.claim_id  
  
   INNER JOIN currency ON  
   claim.currency_id = currency.currency_id  
  
   LEFT JOIN risk ON  
    claim.risk_type_id = risk.risk_cnt  
  
   LEFT JOIN risk_type ON  
     risk.risk_type_id = risk_type.risk_type_id  
  
  LEFT JOIN party ON  
  party.party_cnt = claim_payment.party_cnt  
  
  LEFT JOIN claim_payment_to ON  
  claim_payment.claim_payment_to_id = claim_payment_to.claim_payment_to_id  
  
  LEFT JOIN safe_harbour ON  
  claim_payment.safe_harbour_id = safe_harbour.safe_harbour_id  
  
  LEFT JOIN mediatype ON  
  claim_payment.payeemediatype = mediatype.mediatype_id  
  
  LEFT JOIN country ON  
  claim_payment.PayeeCountry = Country.Country_id  
  
 WHERE claim.claim_id = @claim_id  
  
 -- this procedure should return  
 -- either the latest work payment that is being processed (claim_payment_id is set to 0 and is_live is set to 0)  
 -- or historic payments specified by claim_payment_id  
 AND (  
  (@claim_payment_id = 0)  
  
  
 OR  
  
 (claim_Payment.claim_payment_id = @claim_payment_id)  
     )  
 END  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

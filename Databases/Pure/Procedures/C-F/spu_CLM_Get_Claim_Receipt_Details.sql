--Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claims Recovery Reinsurance
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CLM_Get_Claim_Receipt_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Receipt_Details      
 @claim_id int,      
 @claim_Receipt_id int = 0      
AS      
BEGIN       
SELECT      
      claim_Receipt.claim_Receipt_id,      
      claim_Receipt.claim_peril_id,      
      claim_Receipt.date_of_Receipt,      
      claim_Receipt.amount,      
      claim_Receipt.tax_amount,      
      claim_Receipt.party_cnt,      
      claim_Receipt.comments,      
      claim_Receipt.created_by,      
      claim_Receipt.PayeeMediaType,      
      claim_Receipt.PayeeName,      
      claim_Receipt.PayeeBankName,      
      claim_Receipt.PayeeSortCode,      
      claim_Receipt.PayeeAccountNo,      
      claim_Receipt.PayeeCountry,      
      claim_Receipt.PayeeComments,      
      claim_Receipt.PayeeMediaRef,  
      claim_Receipt.Insured_domiciled,      
      claim_Receipt.insured_percentage,      
      claim_Receipt.insured_tax_number,      
      claim_Receipt.is_tax_exempt,      
      claim_Receipt.is_settlement,      
      claim_Receipt.document_id,      
      party.resolved_name,      
      mediatype.description ,      
      country.description,      
      claim_Receipt.ThirdPartyReference      
      
     FROM claim_Receipt      
    
     INNER JOIN claim ON      
      claim.claim_id  = claim_Receipt.claim_id      
    
     INNER JOIN currency ON      
      claim.currency_id = currency.currency_id      
    
      LEFT JOIN risk ON      
       claim.risk_type_id = risk.risk_cnt      
    
      LEFT JOIN risk_type ON      
        risk.risk_type_id = risk_type.risk_type_id      
    
     LEFT JOIN party ON      
     party.party_cnt = claim_Receipt.party_cnt      
    
     LEFT JOIN mediatype ON      
     claim_Receipt.payeemediatype = mediatype.mediatype_id      
    
     LEFT JOIN country ON      
     claim_Receipt.PayeeCountry = Country.Country_id     
    
    WHERE claim.claim_id = @Claim_Id  
    AND (      
     (@claim_Receipt_id = 0)      
    
    OR      
    
    (claim_Receipt.claim_Receipt_id = @claim_Receipt_id)      
        )      
   END      
--End(Saurabh Agrawal) Tech Spec QBENZCR004 Claims Recovery Reinsurance  
EXECUTE DDLDropProcedure 'spu_SAM_Get_Claim_Payments_Details'  
GO

CREATE PROCEDURE spu_SAM_Get_Claim_Payments_Details  
@claim_id INT,
@iFetchAllVersionAmounts TINYINT = 0    
AS  
SET NOCOUNT ON;

SELECT  
 cp.claim_payment_id,  
 cp.claim_id,  
 cp.claim_peril_id,  
 cp.date_of_payment,  
 cp.amount,  
 ISNULL(cp.tax_amount,0) + ISNULL(cp.tax_amount_WHT,0) as tax_amount,  
 cp.party_cnt,  
 p.shortname,  
 cp.comments,  
 cp.is_referred,
 mt.code 'PayeeMediaType',  
 cp.PayeeName,  
 cp.PayeeBankName,  
 cp.PayeeSortCode,  
 cp.PayeeAccountNo,  
 cou.code 'PayeeCountry',  
 cp.PayeeComments,  
 cp.payment_party_to,  
 cp.insured_domiciled,  
 cp.insured_percentage,  
 cp.insured_tax_number,  
 cp.payee_domiciled,  
 cp.payee_percentage,
 cp.payee_tax_number,
 cp.safe_harbour_id,
 sh.code as safe_harbour_code,
 cp.safe_harbour_percentage,
 cp.is_tax_exempt,
 cp.is_wht_exempt,
 cp.is_settlement,
 cp.media_ref 'PayeeMediaRef',
 cp.currency_id,
 cur.code as currency_code,
 cp.excess_amount,
 cp.PayeeAddress1,
 cp.PayeeAddress2,
 cp.PayeeAddress3,
 cp.PayeeAddress4,
 cp.PayeePostalCode,
 cp.ThirdPartyReference,
 cp.base_claim_payment_id,
 cp.version_id,
 CASE WHEN cp.is_referred = 2 THEN 0  
 WHEN (cp.claim_payment_id <> cp.base_claim_payment_id AND @iFetchAllVersionAmounts=0) THEN 0   
 ELSE cpa.LossAmount END AS LossAmount,  
 CASE WHEN cp.is_referred = 2 THEN 0  
 WHEN (cp.claim_payment_id <> cp.base_claim_payment_id AND @iFetchAllVersionAmounts=0) THEN 0   
 ELSE cpa.BaseAmount END AS BaseAmount,  
 cur.[Description] as CurrencyDescription,
 ISNULL(p.resolved_name,'Claim Payable') as resolved_name,
 CASE WHEN cp.is_referred = 2 THEN 0  
 WHEN (cp.claim_payment_id <> cp.base_claim_payment_id AND @iFetchAllVersionAmounts=0) THEN 0   
 ELSE cpa.LossTaxAmount END AS LossTaxAmount, 
 ISNULL(cp.party_bank_id,0) party_bank_id,
 ISNULL(cp.our_ref,'') our_ref,
 cur1.code as 'LossCurrencyCode',
 cp.ultimate_payee,
 cp.is_ex_gratia,
 cp.business_identifier_code,
 cp.international_bank_account_number,
 CASE WHEN cp.claim_payment_id=cp.base_claim_payment_id AND cp.amount <> 0 THEN 1 ELSE 0 END AS 'IsThisPayment',
 mt.description 'PayeeMediaTypeDesc', 
 sf.document_ref 'Document_Reference', 
 CASE WHEN spy.document_ref IS NOT NULL THEN 'Paid'
	 WHEN ISNULL(cp.document_id,0) > 0 THEN 'Authorise'
	 WHEN ISNULL(cp.recommended_by, 0) > 0 THEN 'Recommend'
	 ELSE 'Pending' END 'Payment_Status',
 ISNULL(spy.their_ref,'') as SPYTheirRef,
 ISNULL(spy.document_ref,'') as SPYDocRef,
 cp.Payee_Account_Type
FROM Claim_Payment cp WITH (NOLOCK)
INNER JOIN (
	Select claim_payment_id,  
		SUM(ISNULL(this_payment,0)*ISNULL(payment_loss_xrate,1)) as LossAmount,  
		SUM(ISNULL(this_payment,0)*ISNULL(currency_base_xrate,1)) as BaseAmount,  
		SUM((ISNULL(tax_amount,0) + ISNULL(tax_amount_WHT,0))*ISNULL(payment_loss_xrate,1)) as LossTaxAmount  
	FROM claim_payment_item WITH (NOLOCK) 
	WHERE reserve_id>0 
	GROUP BY claim_payment_id  
 )cpa ON cp.claim_payment_id=cpa.claim_payment_id  
 LEFT JOIN Currency cur WITH (NOLOCK) ON cur.currency_id = cp.currency_id  
 LEFT JOIN Safe_Harbour sh WITH (NOLOCK) ON sh.safe_harbour_id = cp.safe_harbour_id  
 LEFT JOIN Party p WITH (NOLOCK) ON p.party_cnt = cp.party_cnt  
 LEFT JOIN Country cou WITH (NOLOCK) ON cou.country_id = cp.PayeeCountry  
 LEFT JOIN MediaType mt WITH (NOLOCK) ON mt.MediaType_id = cp.PayeeMediaType  
 LEFT JOIN claim c WITH (NOLOCK) ON c.Claim_id = cp.claim_id 
 LEFT JOIN Currency cur1 WITH (NOLOCK) ON cur1.currency_id = c.Currency_id   
 LEFT JOIN Stats_folder sf WITH (NOLOCK) ON sf.payment_id = cp.base_claim_payment_id
  
 OUTER APPLY (
	SELECT TOP 1 ad.document_ref, cli.their_ref
	FROM AllocationDetail ad WITH (NOLOCK)
	INNER JOIN AllocationDetail ad2 WITH (NOLOCK) ON ad2.allocation_id = ad.allocation_id AND ad2.document_ref = sf.document_ref
	LEFT JOIN cashlistitem cli WITH (NOLOCK) ON cli.transdetail_id = ad.transdetail_id
	WHERE ad.document_ref LIKE 'SPY%'
 ) spy
WHERE cp.Claim_id = @claim_id  
ORDER BY cp.claim_payment_id ASC  
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO


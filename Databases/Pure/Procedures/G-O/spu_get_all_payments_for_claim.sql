SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_get_all_payments_for_claim
GO

CREATE   PROCEDURE spu_get_all_payments_for_claim
 @claim_id INT,
 @reserve_id INT
AS
BEGIN

	SELECT
		cp.claim_payment_id,
		cp.date_of_payment,
		ISNULL(pty.resolved_name,'Claim Payable'),
		cp.PayeeName,
		p.this_payment,
		c.description,
		p.this_payment*ISNULL(p.payment_loss_xrate,1) [loss amount],
		p.this_payment*ISNULL(p.currency_base_xrate,1) [base amount],
		p.currency_id [payment currency],
		cl.currency_id [loss currency],
		i.base_currency_id,
		ISNULL(p.tax_amount,0) + ISNULL(p.tax_amount_WHT,0) ,
  		claim_peril_id , 
  		 cp.media_ref, 
		ISNULL(cp.PayeeBankName,'') AS PayeeBankName,
		ISNULL(cp.PayeeAccountNo,'') AS PayeeAccountNo,
		ISNULL(cp.PayeeSortCode,'') AS PayeeSortCode,
		Case   WHEN cp.claim_payment_id <> cp. base_claim_payment_id THEN 
		(
		 Select Case    WHEN is_referred = 0 then 'Approved'
						WHEN is_referred = 1 THEN 'Pending'  
						WHEN ISNULL(is_referred,0) = 0 AND Amount = 0 THEN 'Declined'  
						WHEN ISNULL(is_referred,0) = 2 THEN 'Declined'							  
						Else 'Approved' 
		 		 End
		  from Claim_Payment where claim_payment_id=cp. base_claim_payment_id 
		 )
		Else Case  When  cp.claim_payment_id = cp. base_claim_payment_id THEN  
		CASE  
		   WHEN is_referred = 0 then 'Approved'
		   WHEN is_referred = 1 THEN 'Pending'  
		   WHEN ISNULL(is_referred,0) = 0 AND Amount = 0 THEN 'Declined'  
		   WHEN ISNULL(is_referred,0) = 2 THEN 'Declined'  
		ELSE 'Approved' 
		End 
	 End 
  END AS Status,
  cp.business_identifier_code As 'BIC', 
  cp.international_bank_account_number As 'IBAN'		

	FROM  Claim_Payment_Item p

		INNER JOIN Claim_Payment cp ON
			p.claim_payment_id = cp.claim_payment_id

		JOIN Currency c ON
			c.currency_id=p.currency_id

		LEFT JOIN Party pty ON
			pty.party_cnt=cp.party_cnt

		JOIN Claim cl ON
			cl.claim_id=cp.claim_id

		JOIN Insurance_File i ON
			i.insurance_file_cnt= cl.policy_id

	WHERE cp.claim_id= @claim_id
	AND p.reserve_id= ISNULL(@reserve_id,p.reserve_id)
	AND p.this_payment <> 0

END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


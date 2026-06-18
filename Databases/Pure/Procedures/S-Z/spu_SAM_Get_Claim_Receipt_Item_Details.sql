SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Claim_Receipt_Item_Details'
GO

CREATE PROCEDURE spu_SAM_Get_Claim_Receipt_Item_Details

@claim_id integer

AS

	SELECT 

		cri.claim_receipt_item_id,
		cri.claim_receipt_id,
		cri.recovery_id,
		cri.reserve_id,
		cri.this_receipt,
		cri.tax_group_id,
		tg.code as tax_group_code,
		cri.tax_amount,
		cri.base_claim_receipt_item_id,
		cri.version_id,
		(ISNULL(cri.this_receipt,0)*ISNULL(cri.receipt_loss_xrate,1)) as LossAmount,
 	    (ISNULL(cri.this_receipt,0)*ISNULL(cri.currency_base_xrate,1)) as BaseAmount
		,rtrim(ltrim(ISNULL((select code from recovery_type where recovery_type_id  = cri.recovery_type_id),'')))  as recovery_type_code    


		-- claim receipt items fields not required
		--recovery_type_id,
		--currency_id,
		--exchange_rate_override_reason_id,
		--currency_base_xrate,
		--currency_base_date,
		--account_base_xrate,
		--account_base_date,
		--system_base_xrate,
		--system_base_date,
		--receipt_loss_xrate,

	FROM claim_receipt_item cri

		LEFT outer JOIN tax_group tg ON
			(tg.tax_group_id = cri.tax_group_id)
	
	WHERE cri.claim_receipt_id in (select claim_Receipt_id from claim_receipt where claim_id = @claim_id)

	Order by cri.claim_receipt_id, cri.claim_receipt_item_id



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

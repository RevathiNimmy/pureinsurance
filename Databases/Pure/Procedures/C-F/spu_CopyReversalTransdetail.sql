SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_CopyReversalTransdetail'

GO
CREATE PROCEDURE spu_CopyReversalTransdetail  
    @nNewDocumentId  INT  ,
    @sOldDocumentRef  varchar(25),
	@user_id		INT
    
AS  

DECLARE @posting_period_number int 

 
SELECT @posting_period_number=posting_period_number 
FROM Stats_Folder sd join Document d ON sd.document_ref=d.document_ref
 WHERE d.document_id=@nNewDocumentId
 
 
INSERT INTO  transdetail
(	
	account_id,
	postingstatus_id,
	company_id ,
	sub_branch_id,
	currency_id,
	period_id ,
	document_id,
	document_sequence,
	accounting_date,
	amount,
	base_amount_unrounded ,
	fully_matched,
	currency_amount,
	currency_amount_unrounded,
	currency_base_xrate,
	euro_currency_id ,
	euro_amount,
	euro_base_xrate,
	euro_ccy_xrate,
	comment,
	insurance_ref,
	operator_id,
	purchase_order_no,
	purchase_invoice_no,
	department,
	spare,
	ref_date,
	ref_amount,
	ref_quantity,
	ref_units,
	insurance_ref_index,
	department_id ,
	not_reported ,
	underwriting_year_id,
	amount_currency_id,
	account_currency_id,
	account_amount ,
    account_amount_unrounded ,
	account_base_xrate  ,
	system_currency_id ,
	system_amount   ,
    system_amount_unrounded ,
	system_base_xrate  ,
    outstanding_amount ,
    outstanding_currency_amount,
	outstanding_account_amount,
	outstanding_system_amount ,
	amount_updated  ,
    reference ,
	type_code ,
	transdetail_type_id,
	tax_group_id ,
	tax_band_id ,
batch_id
)

SELECT 	
	account_id,
	postingstatus_id,
	company_id ,
	sub_branch_id,
	currency_id,
	@posting_period_number ,
	@nNewDocumentId,
	document_sequence,
	GETDATE(),
	-1*amount,
	-1*base_amount_unrounded ,
	0,
	-1*currency_amount,
	-1* currency_amount_unrounded,
	currency_base_xrate,
	euro_currency_id ,
	euro_amount,
	euro_base_xrate,
	euro_ccy_xrate,
	'',
	insurance_ref,
	@user_id,
	purchase_order_no,
	purchase_invoice_no,
	department,
	spare,
	ref_date,
	ref_amount,
	ref_quantity,
	ref_units,
	insurance_ref_index,
	department_id ,
	not_reported ,
	underwriting_year_id,
	amount_currency_id,
	account_currency_id,
	-1*account_amount ,
    -1*account_amount_unrounded ,
	account_base_xrate  ,
	 system_currency_id ,
	-1*system_amount   ,
    -1*system_amount_unrounded,
	system_base_xrate  ,
    -1*amount ,
    -1*currency_amount,
	-1*account_amount,
	-1*system_amount ,
	amount_updated  ,
    reference ,
	type_code ,
	transdetail_type_id,
	tax_group_id ,
	tax_band_id,
 batch_id
 FROM transdetail
WHERE document_id =(SELECT document_id from document where document_ref =@sOldDocumentRef)

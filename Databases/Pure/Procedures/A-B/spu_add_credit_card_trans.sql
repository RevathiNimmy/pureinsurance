SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_add_credit_card_trans'
GO

CREATE PROCEDURE spu_add_credit_card_trans

        @document_ref varchar(20),
        @credit_card_account_code varchar(20),
        @orig_client_transdetail_id int OUTPUT,
        @new_client_transdetail_id int OUTPUT,
        @client_account_id int OUTPUT,	
        @amount numeric (19, 4) OUTPUT
AS

DECLARE 
        @credit_card_account_id int,
        @document_id int,
        @document_sequence int

--Get credit card account id
SELECT @credit_card_account_id = account_id
FROM   Account
WHERE  short_code = @credit_card_account_code 

--Get document_id
SELECT @document_id = D.document_id,
       @client_account_id = T.account_id,
       @orig_client_transdetail_id = T.transdetail_id,
       @amount = T.amount
FROM   Document D,
       Transdetail T
WHERE  T.document_id = D.document_id
AND    D.document_ref = @document_ref
AND    T.document_sequence = 1

--Get next document_sequence
SELECT @document_sequence = MAX(document_sequence) + 1
FROM   Transdetail
WHERE  document_id = @document_id

--Copy & reverse client transaction
INSERT INTO Transdetail
(
account_id,
postingstatus_id,
company_id,
currency_id,
period_id,
document_id,
document_sequence,
accounting_date,
amount,
base_amount_unrounded,
fully_matched,
currency_amount,
currency_amount_unrounded,
currency_base_xrate,
euro_currency_id,
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
department_id,
sub_branch_id
)
SELECT  
account_id,
postingstatus_id,
company_id,
currency_id,
period_id,
document_id,
@document_sequence,
accounting_date,
amount * -1,
base_amount_unrounded * -1,
fully_matched,
currency_amount * -1,
currency_amount_unrounded * -1,
currency_base_xrate,
euro_currency_id,
euro_amount * -1,
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
department_id,
sub_branch_id 
FROM    Transdetail
WHERE   document_id = @document_id
AND     document_sequence = 1

SELECT @new_client_transdetail_id = @@IDENTITY

--Create credit card transaction
INSERT INTO Transdetail
(
account_id,
postingstatus_id,
company_id,
currency_id,
period_id,
document_id,
document_sequence,
accounting_date,
amount,
base_amount_unrounded,
fully_matched,
currency_amount,
currency_amount_unrounded,
currency_base_xrate,
euro_currency_id,
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
department_id,
sub_branch_id
)
SELECT  
@credit_card_account_id,
postingstatus_id,
company_id,
currency_id,
period_id,
document_id,
@document_sequence + 1,
accounting_date,
amount,
base_amount_unrounded,
fully_matched,
currency_amount,
currency_amount_unrounded,
currency_base_xrate,
euro_currency_id,
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
department_id,
sub_branch_id 
FROM    Transdetail
WHERE   document_id = @document_id
AND     document_sequence = 1

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO






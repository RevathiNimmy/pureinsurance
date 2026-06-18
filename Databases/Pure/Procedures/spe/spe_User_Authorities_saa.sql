SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Execute DDLDropProcedure 'spe_User_Authorities_saa'
GO

CREATE PROCEDURE spe_User_Authorities_saa
AS
SELECT
	user_id ,
	has_write_off_authority ,
	write_off_amount,
	has_unrestricted_enquiry,
	has_unrestricted_update,
	fee_discount,
	has_transaction_write_off_authority,
	transaction_write_off_amount,
	has_refund_authority,
	has_transfer_authority,
	has_payments_authority, 
	payments_amount, 
	has_claim_payments_authority, 
	claim_payments_amount,
	write_off_currency_id,
	transaction_write_off_currency_id,
	payments_currency_id,
	claims_payments_currency_id,
	can_change_exchange_date,
	can_change_exchange_rate,
	can_change_prepolicy_exchange_date,
	can_change_prepolicy_exchange_rate,
	is_view_client,
	is_edit_client,
	is_edit_policy,
	is_edit_claim,
	is_edit_finance_plan,
	is_raise_debit,
	is_raise_credit,
	is_raise_fee,
	is_raise_cash,
	is_reverse_transactions,
	is_reverse_allocations,
	is_raise_manual_DID,
	is_delete_client,
	is_perform_allocations,
	can_perform_broker_transfer,
    is_delete_policy,
    is_edit_scheme_policy,
	can_user_debug_dynamic_logic_scripts,
	user_server_scripts_run_in_debug,can_change_instalment_default_currency
FROM User_Authorities
ORDER BY user_id ASC

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


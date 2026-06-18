SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_UserAuthorities'
GO


CREATE PROCEDURE spu_ACT_SelAll_UserAuthorities

AS

SELECT
    	ua.user_id,
    	ua.has_write_off_authority,
    	ua.write_off_amount,
    	ua.has_unrestricted_enquiry,
    	ua.has_unrestricted_update,
    	ua.fee_discount, 
	ua.has_transaction_write_off_authority,
	ua.transaction_write_off_amount,
	ua.has_refund_authority,
	ua.has_transfer_authority,
	ua.write_off_currency_id,
	ua.transaction_write_off_currency_id,
	ua.payments_currency_id,
	ua.claims_payments_currency_id,
	ua.can_change_exchange_date,
	ua.can_change_exchange_rate,
	ua.can_change_prepolicy_exchange_date,
	ua.can_change_prepolicy_exchange_rate,
	ua.is_view_client,
	ua.is_edit_client,
	ua.is_edit_policy,
	ua.is_edit_claim,
	ua.is_edit_finance_plan,
	ua.is_raise_debit,
	ua.is_raise_credit,
	ua.is_raise_fee,
	ua.is_raise_cash,
	ua.is_reverse_transactions,
	ua.is_reverse_allocations,
	ua.is_raise_manual_DID,
	ua.is_delete_client,
	ua.is_perform_allocations,
	ua.is_delete_policy,
	ua.can_reverse_and_replace_transactions

FROM PMUser u 
-- join to show users that are not marked as deleted...
INNER JOIN User_Authorities ua ON u.user_id = ua.user_id
WHERE u.Is_Deleted = 0  

ORDER BY ua.user_id ASC
EXECUTE DDLDropProcedure 'spu_ACT_SelAll_AllocationDetail'
GO

CREATE PROCEDURE spu_ACT_SelAll_AllocationDetail
    @allocation_id INT
AS

SELECT
    allocationdetail_id,
     cashlistitem_id,
     allocation_id,
     original_currency,
     transdetail_id,
     documenttype_id,
     document_ref,
     accounting_date,
     original_date,
     allocate_to_base,
     orig_base_amount,
     orig_base_amount_unrounded,
     orig_ccy_amount,
     orig_ccy_amount_unrounded,
     orig_xrate,
     effective_xrate,
     os_base_amount,
     os_ccy_amount,
     alloc_base_amount,
     alloc_ccy_amount,
     fully_matched,
     write_off_amount,
     write_off_reason_id,
     new_os_ccy_amount,
     new_os_base_amount,
     loss_gain_amount,
     is_primary,
     euro_currency_id,
     euro_amount,
     euro_base_xrate,
     euro_ccy_xrate,
     round_off_amount,
     alloc_account_amount,
     alloc_system_amount       
FROM AllocationDetail
     WHERE @allocation_id = allocation_id
ORDER BY is_primary DESC

GO



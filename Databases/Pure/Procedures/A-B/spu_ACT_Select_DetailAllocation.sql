SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_DetailAllocation'
GO


CREATE PROCEDURE spu_ACT_Select_DetailAllocation
    @allocation_id int
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
    euro_ccy_xrate
FROM AllocationDetail
WHERE allocation_id = @allocation_id
GO



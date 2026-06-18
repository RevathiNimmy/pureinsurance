SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_allocate_transdetail_ids'
GO
CREATE PROCEDURE spu_allocate_transdetail_ids
@TransdetailId1 INT,
@Transdetailid2 INT,
@amount Numeric(19,4)

AS 
BEGIN
DECLARE @Allocation_id INT,
@company_id INT,
@account_Id INT,
@kAllocationStatusAllocated INT

UPDATE transdetail SET outstanding_amount=outstanding_amount-@amount,outstanding_currency_amount=outstanding_currency_amount-@amount,
                           outstanding_account_amount=outstanding_account_amount-@amount,outstanding_system_amount=outstanding_system_amount-@amount
                           WHERE transdetail_id=@TransdetailId1

UPDATE transdetail SET outstanding_amount=outstanding_amount-@amount*-1,outstanding_currency_amount=outstanding_currency_amount-@amount*-1,
                           outstanding_account_amount=outstanding_account_amount-@amount*-1,outstanding_system_amount=outstanding_system_amount-@amount*-1
                           WHERE transdetail_id=@Transdetailid2

UPDATE transdetail SET fully_matched=1 WHERE transdetail_id IN (@TransdetailId1,@Transdetailid2) AND outstanding_amount=0

SELECT @company_id=company_id,@account_Id=account_id from transdetail WHERE transdetail_id=@TransdetailId1

DECLARE @Today DATETIME
SET @Today = GETDATE()
EXEC spu_ACT_add_Allocation
   @company_id=@company_id,
   @account_id=@account_Id,
   @user_id=1,
   @allocation_date=@Today,
   @allocationstatus_id=3,
   @Allocation_id=@Allocation_id OUTPUT

INSERT INTO AllocationDetail
  (
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
   orig_xrate,    effective_xrate,
   os_base_amount,
   os_ccy_amount,
   alloc_base_amount,
   alloc_ccy_amount,
   fully_matched,
   new_os_ccy_amount,
   new_os_base_amount,
   loss_gain_amount,
   is_primary,
   euro_currency_id,
   euro_amount,
   euro_base_xrate,
   euro_ccy_xrate,
   alloc_account_amount,
   alloc_system_amount,
   transdetailex_id)

  SELECT
    @Allocation_id,
   T.Currency_id ,
   T.transdetail_id,
   d.documentType_id,
   d.Document_ref,
   @Today,
   d.document_date,
   1,
   T.amount,
   T.amount,
   currency_amount,
   currency_amount_unrounded,
   currency_base_xrate ,
   currency_base_xrate,
   t.outstanding_amount + @amount,
   t.outstanding_currency_amount + @amount,
   @amount,--@amount AllocBaseAmount,
   @amount,-- @amount AllocCCyAmount,
   1 fully_matched,
   t.outstanding_currency_amount,--@amount OSCcyAmount, 
   t.outstanding_amount,-- @amount OSBaseAmount, 
   0,
   fully_matched,
   0,
   0.0,
   1,
   1,
   @amount,
   @amount,
   0
   
   FROM transdetail T INNER JOIN document d ON d.document_id=t.document_id
   WHERE transdetail_id = @TransdetailId1

INSERT INTO AllocationDetail
  (
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
   orig_xrate,    effective_xrate,
   os_base_amount,
   os_ccy_amount,
   alloc_base_amount,
   alloc_ccy_amount,
   fully_matched,
   new_os_ccy_amount,
   new_os_base_amount,
   loss_gain_amount,
   is_primary,
   euro_currency_id,
   euro_amount,
   euro_base_xrate,
   euro_ccy_xrate,
   alloc_account_amount,
   alloc_system_amount,
   transdetailex_id)

  SELECT
    @Allocation_id,
   T.Currency_id ,
   T.transdetail_id,
   d.documentType_id,
   d.Document_ref,
   @Today,
   d.document_date,
   1,
   T.amount,
   T.amount,
   currency_amount,
   currency_amount_unrounded,
   currency_base_xrate ,
   currency_base_xrate,
   -1 * @amount,
   -1 * @amount,
   -1 * @amount,--@amount AllocBaseAmount,
   -1 * @amount,-- @amount AllocCCyAmount,
   1 fully_matched,
   t.outstanding_currency_amount,--@amount OSCcyAmount, 
   t.outstanding_amount,-- @amount OSBaseAmount, 
   0,
   fully_matched,
   0,
   0.0,
   1,
   1,
   -1 * @amount,
   -1 * @amount,
   0
   
   FROM transdetail T INNER JOIN document d ON d.document_id=t.document_id
   WHERE transdetail_id  = @TransdetailId2


END

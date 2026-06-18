EXECUTE DDLDropProcedure 'spu_ACT_Add_AllocationDetail'
GO

CREATE PROCEDURE spu_ACT_Add_AllocationDetail
    @allocationdetail_id INT OUTPUT,
    @cashlistitem_id INT,
    @allocation_id INT,
    @original_currency  SMALLINT,
    @transdetail_id INT,
    @documenttype_id  SMALLINT,
    @document_ref varchar(25),
    @accounting_date  DATETIME,
    @original_date  DATETIME,
    @allocate_to_base TINYINT,
    @orig_base_amount NUMERIC(19,4),
    @orig_base_amount_unrounded NUMERIC(19,4),
    @orig_ccy_amount NUMERIC(19,4),
    @orig_ccy_amount_unrounded NUMERIC(19,4),
    @orig_xrate NUMERIC(19,10),
    @effective_xrate NUMERIC(19,10),
    @os_base_amount NUMERIC(19,4),
    @os_ccy_amount NUMERIC(19,4),
    @alloc_base_amount NUMERIC(19,4),
    @alloc_ccy_amount NUMERIC(19,4),
    @fully_matched TINYINT,
    @write_off_amount NUMERIC(19,4),
    @write_off_reason_id INT,
    @new_os_ccy_amount NUMERIC(19,4),
    @new_os_base_amount NUMERIC(19,4),
    @loss_gain_amount NUMERIC(19,4),
    @is_primary TINYINT,
    @euro_currency_id  SMALLINT,
    @euro_amount NUMERIC(19,4),
    @euro_base_xrate NUMERIC(12,8),
    @euro_ccy_xrate NUMERIC(12,8),
	@crAlloc_account_amount NUMERIC(19,4),
    @crAlloc_system_amount NUMERIC(19,4),
    @transdetailex_id INT
AS

BEGIN
INSERT INTO AllocationDetail (
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
	alloc_account_amount,
    alloc_system_amount,
    transdetailex_id)
VALUES (
    @cashlistitem_id,
    @allocation_id,
    @original_currency,
    @transdetail_id,
    @documenttype_id,
    @document_ref,
    @accounting_date,
    @original_date,
    @allocate_to_base,
    @orig_base_amount,
    @orig_base_amount_unrounded,
    @orig_ccy_amount,
    @orig_ccy_amount_unrounded,
    @orig_xrate,
    @effective_xrate,
    @os_base_amount,
    @os_ccy_amount,
    @alloc_base_amount,
    @alloc_ccy_amount,
    @fully_matched,
    @write_off_amount,
    @write_off_reason_id,
    @new_os_ccy_amount,
    @new_os_base_amount,
    @loss_gain_amount,
    @is_primary,
    @euro_currency_id,
    @euro_amount,
    @euro_base_xrate,
    @euro_ccy_xrate,
	@crAlloc_account_amount,
    @crAlloc_system_amount,
    @transdetailex_id)
END
BEGIN
SELECT @allocationdetail_id = @@IDENTITY

-- Don't need to add write_off_amount in case of "SWD" and "SCD"
--IF @documenttype_id NOT IN (SELECT documenttype_id FROM DocumentType WHERE code IN ('SWD' , 'SCD', 'SND'))
--BEGIN
--	SELECT @alloc_base_amount = @alloc_base_amount + ISNULL(@write_off_amount,0) * @effective_xrate + ISNULL(@loss_gain_amount,0) * @effective_xrate
--	SELECT @crAlloc_account_amount = @crAlloc_account_amount + ISNULL(@write_off_amount,0) * @effective_xrate + ISNULL(@loss_gain_amount,0) * @effective_xrate
--	SELECT @alloc_ccy_amount = @alloc_ccy_amount + ISNULL(@write_off_amount,0) + ISNULL(@loss_gain_amount,0)
--END
--ELSE
--BEGIN
	--SELECT @alloc_base_amount = @alloc_base_amount + (ISNULL(@loss_gain_amount,0) * @effective_xrate)
	--SELECT @crAlloc_account_amount = @crAlloc_account_amount + (ISNULL(@loss_gain_amount,0) * @effective_xrate)
	--SELECT @alloc_ccy_amount = @alloc_ccy_amount + ISNULL(@loss_gain_amount,0)

	--SELECT @write_off_amount = ISNULL(@write_off_amount, 0) * @effective_xrate

--END

EXEC spu_ACT_Update_TransDetailOutstanding @transdetail_id,
			@alloc_ccy_amount, @alloc_base_amount, @crAlloc_account_amount, @crAlloc_system_amount, @transdetailex_id
END

GO



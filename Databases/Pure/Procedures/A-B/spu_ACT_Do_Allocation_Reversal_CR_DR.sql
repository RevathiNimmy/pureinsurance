EXECUTE DDLDropProcedure 'spu_ACT_Do_Allocation_Reversal_CR_DR'
GO

CREATE PROCEDURE spu_ACT_Do_Allocation_Reversal_CR_DR 
(
	@v_iAllocationID INT
	,@v_iAllocationDetailID_CR INT = NULL
	,@v_iAllocationDetailID_DR INT = NULL
	,@v_iUserID INT = NULL
)
	/*
		CreatedBy		Date		Description
		---------------------------------------------------------------------------------------------------------------------------------------------------------------
		GHarris			15/08/18	This proc is called from the spu_ACT_Do_Allocation_Reversal proc if the CR and DR params are passed.  This as to cut doewn on the time taken on the proc of just
									the allocation id was passed as all the where clauses with if and or statements were taking far too long to execute 
		
*/
AS
DECLARE @iAllocationID INT
DECLARE @iAllocationDetailID INT
DECLARE @iTransDetailID INT
DECLARE @iWriteOffDocID INT
DECLARE @iWriteOffDocTypeID INT
DECLARE @iAllocationBatchId INT = 0
DECLARE @iReversedAllocationBatchId INT
DECLARE @iNewAllocationID INT
DECLARE @nAllocCurrencyAmount_CR NUMERIC(19, 4) -- RAW 13/05/2003 : CQ954 : added
DECLARE @nAllocCurrencyAmount_DR NUMERIC(19, 4) -- RAW 13/05/2003 : CQ954 : added
DECLARE @nAllocCurrencyAmount NUMERIC(19, 4)
DECLARE @nAllocBaseAmount_CR NUMERIC(19, 4) -- RAW 13/05/2003 : CQ954 : added   
DECLARE @nAllocBaseAmount_DR NUMERIC(19, 4) -- RAW 13/05/2003 : CQ954 : added   
DECLARE @nAllocBaseAmount NUMERIC(19, 4)
DECLARE @nWriteOffCurrencyAmount NUMERIC(19, 4)
DECLARE @nWriteOffBaseAmount NUMERIC(19, 4)
DECLARE @nADStartOSCurrencyAmount NUMERIC(19, 4)
DECLARE @nADStartOSBaseAmount NUMERIC(19, 4)
DECLARE @nCurrDiffCurrencyAmount NUMERIC(19, 4)
DECLARE @nCurrDiffBaseAmount NUMERIC(19, 4)
DECLARE @nAllocAccountAmount_CR NUMERIC(19, 4)
DECLARE @nAllocSystemAmount_DR NUMERIC(19, 4)
DECLARE @nAllocAccountAmount_DR NUMERIC(19, 4)
DECLARE @nAllocSystemAmount_CR NUMERIC(19, 4)
DECLARE @nAllocSystemAmount NUMERIC(19, 4)
DECLARE @nAllocAccountAmount NUMERIC(19, 4)
DECLARE @nTransDetailExID INT
DECLARE @iReturnCode INT
DECLARE @iErrorCode INT

DECLARE @k_iErrorCode_AllocationIdMissing INT = - 1
DECLARE @k_iErrorCode_AllocationDetailIdMismatch INT = - 2
DECLARE @k_iErrorCode_AllocationIdInvalid INT = - 3
DECLARE @k_iErrorCode_AllocationDetailIdInvalid INT = - 4
DECLARE @k_iErrorCode_WriteOffDocNotFound INT = - 5
DECLARE @k_iErrorCode_Imbalance INT = - 6
DECLARE @k_iErrorCodeOffset_WriteOff INT = - 100
DECLARE @k_iErrorCodeOffset_AllocationDetail INT = - 200

SET @iAllocationID = @v_iAllocationID -- from now on @v_iAllocationID should not be referenced

IF (@iAllocationID IS NULL)
BEGIN
	-- we must have an allocation id
	IF (@v_iAllocationDetailID_CR IS NOT NULL)
	BEGIN
		SET @iAllocationID = (
				SELECT allocation_id
				FROM allocationdetail
				WHERE allocationdetail_id = @v_iAllocationDetailID_CR
				)
	END
	ELSE
	BEGIN
		IF (@v_iAllocationDetailID_DR IS NOT NULL)
		BEGIN
			SET @iAllocationID = (
					SELECT allocation_id
					FROM allocationdetail
					WHERE allocationdetail_id = @v_iAllocationDetailID_DR
					)
		END
	END
END

IF (@iAllocationID IS NULL)
BEGIN
	PRINT 'AllocationID has not been provided and cannot be derived'
	RETURN @k_iErrorCode_AllocationIdMissing
END

SELECT @iAllocationBatchId = (allocationbatch_id)
FROM Allocation
WHERE allocation_id = @iAllocationID

SET @iReversedAllocationBatchId = 0

SELECT @iReversedAllocationBatchId = BatchWO.allocationbatch_id
FROM Allocation A
JOIN Allocation Batch ON batch.allocationbatch_id = a.allocationbatch_id
JOIN AllocationDetail AD ON AD.allocation_id = batch.allocation_id
JOIN AllocationDetail WO ON WO.transdetail_id = AD.transdetail_id
JOIN Allocation BatchWO ON BatchWO.allocation_id = wo.allocation_id
WHERE A.allocation_id = @iAllocationID
	AND (
		ad.document_ref LIKE 'SWD%'
		OR ad.document_ref LIKE 'SCD%'
		)
	AND a.allocationbatch_id <> BatchWO.allocationbatch_id

IF @iReversedAllocationBatchId = 0
BEGIN
	SELECT @iReversedAllocationBatchId = ISNULL(allocationbatch_id, 0)
	FROM AllocationBatch
	WHERE reversed_allocation_batch_id = @iAllocationBatchId
END
ELSE
BEGIN
	UPDATE AllocationBatch
	SET reversed_allocation_batch_id = @iAllocationBatchId
		,is_reversed = 1
	WHERE allocationbatch_id = @iReversedAllocationBatchId
END

IF @iReversedAllocationBatchId = 0
BEGIN
	EXEC spu_ACT_Add_AllocationBatch @iReversedAllocationBatchId OUTPUT
		,@iAllocationBatchId

	IF @iReversedAllocationBatchId = 0
		RETURN
END

INSERT INTO Allocation (
	company_id
	,account_id
	,user_id
	,allocation_date
	,allocationstatus_id
	,allocationbatch_id
	)
SELECT company_id
	,account_id
	,CASE 
		WHEN @v_iUserID IS NULL
			THEN user_id
		ELSE @v_iUserID
		END
	,allocation_date
	,allocationstatus_id
	,@iReversedAllocationBatchId
FROM Allocation
WHERE allocation_id = @iAllocationID

SELECT @iNewAllocationID = @@IDENTITY

-- Is AllocationID valid
SELECT 1
FROM allocationDetail ad
WHERE ad.allocation_id = @iAllocationID

IF (@@ROWCOUNT = 0)
BEGIN
	PRINT 'AllocationID not found'

	RETURN @k_iErrorCode_AllocationIdInvalid
END

-- if AllocationDetailID_CR has been specified it must be within the same allocationID as that specified
IF (@v_iAllocationDetailID_CR IS NOT NULL)
BEGIN
	SELECT @iErrorCode = CASE 
			WHEN ad.allocation_id = @iAllocationID
				THEN 0
			ELSE @k_iErrorCode_AllocationDetailIdMismatch
			END
		,@nAllocCurrencyAmount_CR = alloc_ccy_amount
		,@nAllocBaseAmount_CR = alloc_base_amount
		,@nAllocAccountAmount_CR = alloc_account_amount
		,@nAllocSystemAmount_CR = alloc_system_amount
	FROM allocationDetail ad
	WHERE ad.allocationdetail_id = @v_iAllocationDetailID_CR

	IF (@@ROWCOUNT = 0)
	BEGIN
		PRINT 'AllocationDetailID_CR not found'

		RETURN @k_iErrorCode_AllocationDetailIdInvalid
	END

	IF (@iErrorCode <> 0)
	BEGIN
		PRINT 'AllocationDetailID_CR is not part of the same allocation'

		RETURN @iErrorCode
	END
END

-- if AllocationDetailID_DR has been specified it must be within the same allocationID as that specified
IF (@v_iAllocationDetailID_DR IS NOT NULL)
BEGIN
	SELECT @iErrorCode = CASE 
			WHEN ad.allocation_id = @iAllocationID
				THEN 0
			ELSE @k_iErrorCode_AllocationDetailIdMismatch
			END
		,@nAllocCurrencyAmount_DR = alloc_ccy_amount
		,@nAllocBaseAmount_DR = alloc_base_amount
		,@nAllocAccountAmount_DR = alloc_account_amount
		,@nAllocSystemAmount_DR = alloc_system_amount
	FROM allocationDetail ad
	WHERE ad.allocationdetail_id = @v_iAllocationDetailID_DR

	IF (@@ROWCOUNT = 0)
	BEGIN
		PRINT 'AllocationDetailID_DR not found'

		RETURN @k_iErrorCode_AllocationDetailIdInvalid
	END

	IF (@iErrorCode <> 0)
	BEGIN
		PRINT 'AllocationDetailID_DR is not part of the same allocation'

		RETURN @iErrorCode
	END
END

BEGIN TRANSACTION

-- ===================================================================
-- PROCESS EACH AllocationDetail THAT IS TO BE REVERSED
-- ===================================================================
-- if the credit has been allocated to more than one debit in this allocation then there will only be one allocationdetail row for the credit and it will contain the full amount.
-- If specific credit and debit allocation detail ids have been requested then, for the credit row,we need to get the amount that was allocated to the debt concerned rather that the full amount
-- that was allocated to all of the debts.The reverse situation where multiple credits are allocated to the same debt within the same allocation will result in a similar problem
--alloc_ccy_amount to be reversed
DECLARE c_Temp CURSOR FAST_FORWARD
FOR
SELECT ad1.allocationdetail_id
	,ad1.transdetail_id
	,ad1.alloc_ccy_amount
	,ad1.alloc_base_amount
	,ad1.alloc_account_amount
	,ad1.alloc_system_amount
	,ad1.transdetailex_id 
FROM allocationdetail ad1
WHERE ad1.alloc_ccy_amount <> 0 -- ignore if nothing allocated (eg already reversed)
	AND ad1.allocation_id = @iAllocationID
ORDER BY allocationdetail_id DESC -- in reverse chronological sequence

OPEN c_Temp

BEGIN TRANSACTION

FETCH NEXT
FROM c_Temp
INTO @iAllocationDetailID
	,@iTransDetailID
	,@nAllocCurrencyAmount
	,@nAllocBaseAmount
	,@nAllocAccountAmount
	,@nAllocSystemAmount
	,@nTransDetailexID

WHILE (@@FETCH_STATUS = 0)
BEGIN
	INSERT INTO AllocationDetail (
		cashlistitem_id
		,allocation_id
		,original_currency
		,transdetail_id
		,documenttype_id
		,accounting_date
		,document_ref
		,original_date
		,allocate_to_base
		,orig_base_amount
		,orig_ccy_amount
		,orig_base_amount_unrounded
		,orig_xrate
		,effective_xrate
		,orig_ccy_amount_unrounded
		,os_base_amount
		,os_ccy_amount
		,alloc_base_amount
		,alloc_ccy_amount
		,fully_matched
		,write_off_reason_id
		,write_off_amount
		,new_os_ccy_amount
		,new_os_base_amount
		,loss_gain_amount
		,is_primary
		,euro_currency_id
		,euro_amount
		,euro_base_xrate
		,euro_ccy_xrate
		,Round_Off_Amount
		,alloc_account_amount
		,alloc_system_amount
		,is_reversed
		,allocation_reversed_date
		,MarkedForCollection_Type
		,transdetailex_id
		)
	SELECT cashlistitem_id
		,@iNewAllocationID
		,original_currency
		,transdetail_id
		,documenttype_id
		,GETDATE()
		,document_ref
		,original_date
		,allocate_to_base
		,orig_base_amount
		,orig_ccy_amount
		,orig_base_amount_unrounded
		,orig_xrate
		,effective_xrate
		,orig_ccy_amount_unrounded
		,new_os_base_amount
		,new_os_ccy_amount
		,alloc_base_amount * - 1
		,alloc_ccy_amount * - 1
		,fully_matched
		,write_off_reason_id
		,write_off_amount * - 1
		,new_os_ccy_amount + alloc_ccy_amount
		,new_os_base_amount + alloc_base_amount
		,loss_gain_amount * - 1
		,is_primary
		,euro_currency_id
		,euro_amount
		,euro_base_xrate
		,euro_ccy_xrate
		,Round_Off_Amount * - 1
		,alloc_account_amount * - 1
		,alloc_system_amount * - 1
		,1
		,GETDATE()
		,MarkedForCollection_Type
		,transdetailex_id
	FROM AllocationDetail
	WHERE allocationdetail_id = @iAllocationDetailID

	UPDATE AllocationDetail
	SET is_reversed = 1
		,allocation_reversed_date = GETDATE()
	WHERE allocation_id = @iAllocationID

	-- ===================================================================
	-- UPDATE TransDetail
	-- ===================================================================
	PRINT 'updating transdetail'

	UPDATE transdetail
	SET risk_transfer = 1
	WHERE transdetail_id = @iTransDetailID
		AND isnull(risk_transfer, 0) > 1

	UPDATE TransDetail
	SET fully_matched = 0
		,outstanding_amount = outstanding_amount + @nAllocBaseAmount
		,outstanding_currency_amount = outstanding_currency_amount + @nAllocCurrencyAmount
		,outstanding_account_amount = outstanding_account_amount + @nAllocAccountAmount
		,outstanding_system_amount = outstanding_system_amount + @nAllocSystemAmount
		,amount_updated = GetDate()
	WHERE transdetail_id = @iTransDetailID

	IF ISNULL(@nTransDetailExID, 0) > 0
		UPDATE TransDetailEx
		SET outstanding_currency_amount = outstanding_currency_amount + @nAllocCurrencyAmount
			,outstanding_account_amount = outstanding_account_amount + @nAllocAccountAmount
			,outstanding_system_amount = outstanding_system_amount + @nAllocSystemAmount
		WHERE transdetail_id = @iTransDetailID
			AND transdetailex_id = @nTransDetailExID

	FETCH NEXT
	FROM c_Temp
	INTO @iAllocationDetailID
		,@iTransDetailID
		,@nAllocCurrencyAmount
		,@nAllocBaseAmount
		,@nAllocAccountAmount
		,@nAllocSystemAmount
		,@nTransDetailExID
END

CLOSE c_Temp
DEALLOCATE c_Temp

-- ===================================================================	
-- UPDATE CashListItem
-- ===================================================================	
-- update the allocationstatus for the cashlistitem matched to the debit - if one exists
-- This must be executed AFTER the allocation detail table amounts have been adjusted    
PRINT 'updating cashlistitem'

UPDATE cashlistitem
SET allocationstatus_id = CASE 
		WHEN ad2.new_os_ccy_amount = (- 1 * cashlistitem.amount)
			THEN 1 -- fully unallocated
		WHEN ad2.new_os_ccy_amount <> 0
			THEN 4 -- only partially unallocated
		ELSE allocationstatus_id -- no change
		END
FROM
	-- this represents the credit rows that are being reversed
	AllocationDetail ad1
	,
	-- this is used to link to the most recent allocationId that applies for the 
	-- cash list item transaction which is having this particular allocation reversed.
	-- we need this to get the latest up-to-date os balance - regardless of allocationid
	AllocationDetail ad2
WHERE cashlistitem.transdetail_id = ad1.transdetail_id
	AND cashlistitem.transdetail_id = ad2.transdetail_id
	-- get the last entry from allocationdetail for the cash list item transaction
	-- to get the most recent os balance 
	-- - regardless of which allocation id it is assigned to
	AND ad2.allocation_id = (
		SELECT MAX(ad3.allocation_id)
		FROM allocationdetail ad3
		WHERE ad3.transdetail_id = cashlistitem.transdetail_id
		)
	-- filter by AllocationID
	-- =============================== 
	AND (@iAllocationID IS NULL	OR ad1.allocation_id = @iAllocationID)

	-- filter by AllocationDetailID_CR / AllocationDetailID_DR
	-- =======================================================
	AND (
		 (
			-- filter by the credit AllocationDetailID if one was supplied 
			@v_iAllocationDetailID_CR IS NOT NULL
			AND ad1.allocationdetail_id = @v_iAllocationDetailID_CR
			)
		OR (
			-- filter by the debit AllocationDetailID if one was supplied 
			@v_iAllocationDetailID_DR IS NOT NULL
			AND EXISTS (
				SELECT 1
				FROM allocationdetail ad3 -- this represents the debit transaction
				WHERE ad3.allocationdetail_id = @v_iAllocationDetailID_DR
					AND cashlistitem.cashlistitem_id = ad3.cashlistitem_id
					AND ad1.transdetail_id = cashlistitem.transdetail_id
					AND ad1.allocation_id = ad3.allocation_id
				)
			)
		)

COMMIT TRANSACTION
GO


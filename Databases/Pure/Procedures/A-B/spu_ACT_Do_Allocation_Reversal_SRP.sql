EXECUTE DDLDropProcedure 'spu_ACT_Do_Allocation_Reversal_SRP'
GO
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_ACT_Do_Allocation_Reversal_SRP
 @v_iAllocationID int,
    @v_iAllocationDetailID_CR int = null,
    @v_iAllocationDetailID_DR int = null,
    @v_iUserID int = null
AS
    declare @iAllocationID int
    declare @iAllocationDetailID int
    declare @iTransDetailID int
    declare @iWriteOffDocID int
    declare @iWriteOffDocTypeID int
    declare @iTransDetailExID int
    declare @iAllocationBatchId int
    declare @iReversedAllocationBatchId int
    declare @iNewAllocationID int
    declare @nAllocCurrencyAmount_CR numeric (19,4)
    declare @nAllocCurrencyAmount_DR numeric (19,4)
    declare @nAllocCurrencyAmount    numeric (19,4)
    declare @nAllocBaseAmount_CR     numeric (19,4)
    declare @nAllocBaseAmount_DR     numeric (19,4)
    declare @nAllocBaseAmount        numeric (19,4)
    declare @nWriteOffCurrencyAmount numeric (19,4)
    declare @nWriteOffBaseAmount     numeric (19,4)
    declare @nADStartOSCurrencyAmount    numeric (19,4)
    declare @nADStartOSBaseAmount        numeric (19,4)
    declare @nCurrDiffCurrencyAmount numeric (19,4)
 declare @nCurrDiffBaseAmount     numeric (19,4)
    DECLARE @nAllocAccountAmount_CR  numeric (19,4)
    DECLARE @nAllocSystemAmount_DR   numeric (19,4)
    DECLARE @nAllocAccountAmount_DR  numeric (19,4)
    DECLARE @nAllocSystemAmount_CR   numeric (19,4)
    DECLARE @nAllocSystemAmount   numeric (19,4)
    DECLARE @nAllocAccountAmount  numeric (19,4)
    declare @iReturnCode int
    declare @iErrorCode int
    declare @k_iErrorCode_AllocationIdMissing int
    SET     @k_iErrorCode_AllocationIdMissing = -1
    declare @k_iErrorCode_AllocationDetailIdMismatch int
    SET     @k_iErrorCode_AllocationDetailIdMismatch = -2
    declare @k_iErrorCode_AllocationIdInvalid int
    SET     @k_iErrorCode_AllocationIdInvalid = -3
    declare @k_iErrorCode_AllocationDetailIdInvalid int
    SET     @k_iErrorCode_AllocationDetailIdInvalid = -4
    declare @k_iErrorCode_WriteOffDocNotFound int
    SET     @k_iErrorCode_WriteOffDocNotFound = -5
    declare @k_iErrorCode_Imbalance int
    SET     @k_iErrorCode_Imbalance = -6
    declare @k_iErrorCodeOffset_WriteOff int
    SET     @k_iErrorCodeOffset_WriteOff = -100
    declare @k_iErrorCodeOffset_AllocationDetail int
    SET     @k_iErrorCodeOffset_AllocationDetail = -200
    declare @k_iErrorCodeOffset_TransMatch int
    SET     @k_iErrorCodeOffset_TransMatch = -200
    SET @iAllocationID = @v_iAllocationID
    IF (@iAllocationID IS NULL) BEGIN
        IF (@v_iAllocationDetailID_CR IS NOT NULL) BEGIN
            SET @iAllocationID = (
                SELECT  allocation_id
                FROM    allocationdetail
                WHERE   allocationdetail_id = @v_iAllocationDetailID_CR  )
        END ELSE BEGIN
            IF (@v_iAllocationDetailID_DR IS NOT NULL) BEGIN
                SET @iAllocationID = (
                    SELECT  allocation_id
                    FROM    allocationdetail
                    WHERE   allocationdetail_id = @v_iAllocationDetailID_DR  )
            END
        END
    END
    IF (@iAllocationID IS NULL) BEGIN
        print 'AllocationID has not been provided and cannot be derived'
        RETURN @k_iErrorCode_AllocationIdMissing
    END
 SET @iAllocationBatchId = 0
 SELECT @iAllocationBatchId = ISNULL(allocationbatch_id, 0)
   FROM Allocation WHERE allocation_id = @iAllocationID
 SET @iReversedAllocationBatchId = 0
 SELECT @iReversedAllocationBatchId = BatchWO.allocationbatch_id
   FROM Allocation A
   JOIN Allocation Batch ON batch.allocationbatch_id = a.allocationbatch_id
   JOIN AllocationDetail AD ON AD.allocation_id = batch.allocation_id
   JOIN AllocationDetail WO ON WO.transdetail_id = AD.transdetail_id
   JOIN Allocation BatchWO on BatchWO.allocation_id = wo.allocation_id
   WHERE A.allocation_id = @iAllocationID and (ad.document_ref Like 'SWD%' OR ad.document_ref Like 'SCD%')
    AND a.allocationbatch_id <> BatchWO.allocationbatch_id
 IF @iReversedAllocationBatchId = 0
  SELECT @iReversedAllocationBatchId = ISNULL(allocationbatch_id, 0)
   FROM AllocationBatch WHERE reversed_allocation_batch_id = @iAllocationBatchId
 ELSE
  UPDATE AllocationBatch SET reversed_allocation_batch_id = @iAllocationBatchId, is_reversed = 1
   WHERE allocationbatch_id = @iReversedAllocationBatchId
  --If @iReversedAllocationBatchId = 0
 --BEGIN
  EXEC spu_ACT_Add_AllocationBatch @iReversedAllocationBatchId OUTPUT, @iAllocationBatchId
  If @iReversedAllocationBatchId = 0
   Return
 --END
 INSERT INTO Allocation (
    company_id,
    account_id,
    user_id,
    allocation_date,
    allocationstatus_id,
    allocationbatch_id)
   SELECT
    company_id,
    account_id,
    CASE WHEN @v_iUserID IS NULL THEN user_id ELSE @v_iUserID END,
    GETDATE(),
    allocationstatus_id,
    @iReversedAllocationBatchId
   FROM Allocation
   WHERE allocation_id = @iAllocationID
   SELECT @iNewAllocationID = @@IDENTITY
  
    IF not exists(SELECT  1
    FROM    allocationDetail ad
    WHERE   ad.allocation_id = @iAllocationID) BEGIN
        --print 'AllocationID not found'
        RETURN @k_iErrorCode_AllocationIdInvalid
    END
    IF (@v_iAllocationDetailID_CR IS NOT NULL) BEGIN
        SELECT  @iErrorCode =
                    CASE
                        WHEN ad.allocation_id = @iAllocationID THEN
                            0
                        ELSE
                            @k_iErrorCode_AllocationDetailIdMismatch
                    END,
                @nAllocCurrencyAmount_CR = alloc_ccy_amount,
                @nAllocBaseAmount_CR     = alloc_base_amount,
                @nAllocAccountAmount_CR = alloc_account_amount,
                @nAllocSystemAmount_CR = alloc_system_amount
        FROM    allocationDetail ad
        WHERE   ad.allocationdetail_id = @v_iAllocationDetailID_CR
        IF (@@ROWCOUNT = 0) BEGIN
            print 'AllocationDetailID_CR not found'
            RETURN @k_iErrorCode_AllocationDetailIdInvalid
        END
        IF (@iErrorCode <> 0) BEGIN
            print 'AllocationDetailID_CR is not part of the same allocation'
            RETURN @iErrorCode
        END
    END
    IF (@v_iAllocationDetailID_DR IS NOT NULL) BEGIN
        SELECT  @iErrorCode =
                    CASE
                        WHEN ad.allocation_id = @iAllocationID THEN
                            0
                        ELSE
                            @k_iErrorCode_AllocationDetailIdMismatch
                    END,
                @nAllocCurrencyAmount_DR = alloc_ccy_amount,
                @nAllocBaseAmount_DR     = alloc_base_amount,
                @nAllocAccountAmount_DR = alloc_account_amount,
                @nAllocSystemAmount_DR = alloc_system_amount
        FROM    allocationDetail ad
        WHERE   ad.allocationdetail_id = @v_iAllocationDetailID_DR
        IF (@@ROWCOUNT = 0) BEGIN
            print 'AllocationDetailID_DR not found'
            RETURN @k_iErrorCode_AllocationDetailIdInvalid
        END
        IF (@iErrorCode <> 0) BEGIN
            print 'AllocationDetailID_DR is not part of the same allocation'
            RETURN @iErrorCode
        END
    END
    BEGIN TRANSACTION
    DECLARE c_Temp CURSOR FAST_FORWARD FOR
        SELECT  ad1.allocationdetail_id,
                ad1.transdetail_id,
                CASE
                    WHEN @v_iAllocationDetailID_CR IS NOT NULL AND
                         @v_iAllocationDetailID_DR IS NOT NULL THEN
                        CASE
                            WHEN ad1.allocationdetail_id = @v_iAllocationDetailID_CR THEN
                                CASE
                                    WHEN ABS(ad1.alloc_ccy_amount) <= ABS(@nAllocCurrencyAmount_DR) THEN
                                         ad1.alloc_ccy_amount
                                    ELSE
-1 * @nAllocCurrencyAmount_DR
                      END
                            ELSE
                                CASE
                                    WHEN ABS(ad1.alloc_ccy_amount) <= ABS(@nAllocCurrencyAmount_CR) THEN
                                         ad1.alloc_ccy_amount
                                    ELSE
                                         -1 * @nAllocCurrencyAmount_CR
                                END
                        END
                    ELSE
                         ad1.alloc_ccy_amount
                END,
                CASE
                    WHEN @v_iAllocationDetailID_CR IS NOT NULL AND
                         @v_iAllocationDetailID_DR IS NOT NULL THEN
                        CASE
                            WHEN ad1.allocationdetail_id = @v_iAllocationDetailID_CR THEN
                                CASE
                                    WHEN ABS(ad1.alloc_base_amount) <= ABS(@nAllocBaseAmount_DR) THEN
                                         ad1.alloc_base_amount
                                    ELSE
                                         -1 * @nAllocBaseAmount_DR
                                END
                            ELSE
                                CASE
                                    WHEN ABS(ad1.alloc_base_amount) <= ABS(@nAllocBaseAmount_CR) THEN
                                         ad1.alloc_base_amount
                                    ELSE
                                         -1 * @nAllocBaseAmount_CR
                                END
                        END
                    ELSE
                         ad1.alloc_base_amount
                END,
                (ISNULL(ad1.os_ccy_amount, 0) -
                 ISNULL(ad1.new_os_ccy_amount, 0) -
                 ISNULL(ad1.alloc_ccy_amount, 0)),
                ISNULL(ad1.write_off_amount, 0),
                ad1.os_ccy_amount,
                ad1.os_base_amount,
    (ISNULL(ad1.os_ccy_amount, 0) -
     ISNULL(ad1.new_os_ccy_amount, 0) -
     ISNULL(ad1.alloc_ccy_amount, 0)),
                ISNULL(ad1.loss_gain_amount, 0),
                CASE
                    WHEN @v_iAllocationDetailID_CR IS NOT NULL AND
                         @v_iAllocationDetailID_DR IS NOT NULL THEN
                        CASE
                            WHEN ad1.allocationdetail_id = @v_iAllocationDetailID_CR THEN
                                CASE
                                    WHEN ABS(ad1.alloc_account_amount) <= ABS(@nAllocAccountAmount_DR) THEN
                                         ad1.alloc_account_amount
                                    ELSE
                                         -1 * @nAllocAccountAmount_DR
                                END
                            ELSE
                                CASE
                                    WHEN ABS(ad1.alloc_account_amount) <= ABS(@nAllocAccountAmount_CR) THEN
                                         ad1.alloc_account_amount
                                    ELSE
                                         -1 * @nAllocAccountAmount_CR
                                END
                        END
                    ELSE
                         ad1.alloc_account_amount
                END,
                CASE
                    WHEN @v_iAllocationDetailID_CR IS NOT NULL AND
                         @v_iAllocationDetailID_DR IS NOT NULL THEN
                        CASE
                            WHEN ad1.allocationdetail_id = @v_iAllocationDetailID_CR THEN
                                CASE
                                    WHEN ABS(ad1.alloc_system_amount) <= ABS(@nAllocSystemAmount_DR) THEN
                                         ad1.alloc_system_amount
                                    ELSE
                                         -1 * @nAllocSystemAmount_DR
                                END
          ELSE
                                CASE
                               WHEN ABS(ad1.alloc_system_amount) <= ABS(@nAllocSystemAmount_CR) THEN
                                         ad1.alloc_system_amount
                                    ELSE
                                         -1 * @nAllocSystemAmount_CR
                                END
                        END
                    ELSE
                         ad1.alloc_system_amount
                END,
    ad1.transdetailex_id
        FROM    allocationdetail ad1
        WHERE
                ad1.alloc_ccy_amount <> 0
          AND   ad1.allocation_id = @iAllocationID
          AND   (
                    (
                        @v_iAllocationDetailID_CR IS NULL
                    AND @v_iAllocationDetailID_DR IS NULL
                    )
                OR
                   (
                        @v_iAllocationDetailID_CR IS NOT NULL
                    AND ad1.allocationdetail_id = @v_iAllocationDetailID_CR
                    )
                OR
                   (
                        @v_iAllocationDetailID_DR IS NOT NULL
                    AND ad1.allocationdetail_id = @v_iAllocationDetailID_DR
                    )
                )
        ORDER BY allocationdetail_id DESC
    OPEN c_Temp
    FETCH NEXT FROM c_Temp INTO
        @iAllocationDetailID,
        @iTransDetailID,
        @nAllocCurrencyAmount,
        @nAllocBaseAmount,
        @nWriteOffCurrencyAmount,
        @nWriteOffBaseAmount,
        @nADStartOSCurrencyAmount,
        @nADStartOSBaseAmount,
        @nCurrDiffCurrencyAmount,
        @nCurrDiffBaseAmount,
        @nAllocAccountAmount,
        @nAllocSystemAmount,
  @iTransDetailexID
    WHILE (@@FETCH_STATUS = 0 )
    BEGIN
  INSERT INTO AllocationDetail(
   cashlistitem_id,
   allocation_id,
   original_currency,
   transdetail_id,
   documenttype_id,
   accounting_date,
   document_ref,
   original_date,
   allocate_to_base,
   orig_base_amount,
   orig_ccy_amount,
   orig_base_amount_unrounded,
   orig_xrate,
   effective_xrate,
   orig_ccy_amount_unrounded,
   os_base_amount,
   os_ccy_amount,
   alloc_base_amount,
   alloc_ccy_amount,
   fully_matched,
   write_off_reason_id,
   write_off_amount,
   new_os_ccy_amount,
   new_os_base_amount,
   loss_gain_amount,
   is_primary,
   euro_currency_id,
   euro_amount,
   euro_base_xrate,
   euro_ccy_xrate,
   Round_Off_Amount,
   alloc_account_amount,
   alloc_system_amount,
   is_reversed,
   allocation_reversed_date,
   MarkedForCollection_Type,
   transdetailex_id
  )
  SELECT cashlistitem_id,
    @iNewAllocationID,
    original_currency,
    transdetail_id,
    documenttype_id,
    GETDATE(),
    document_ref,
    original_date,
    allocate_to_base,
    orig_base_amount,
    orig_ccy_amount,
    orig_base_amount_unrounded,
    orig_xrate,
    effective_xrate,
    orig_ccy_amount_unrounded,
    new_os_base_amount,
    new_os_ccy_amount,
    alloc_base_amount * -1,
    alloc_ccy_amount * -1,
    fully_matched,
    write_off_reason_id,
    write_off_amount * -1,
    new_os_ccy_amount + alloc_ccy_amount,
    new_os_base_amount + alloc_base_amount,
    loss_gain_amount * -1,
    is_primary,
    euro_currency_id,
    euro_amount,
    euro_base_xrate,
    euro_ccy_xrate,
    Round_Off_Amount * -1,
    alloc_account_amount * -1,
    alloc_system_amount * -1,
    1,
    GETDATE(),
    MarkedForCollection_Type,
    transdetailex_id
  FROM AllocationDetail
  WHERE allocationdetail_id = @iAllocationDetailID
  Update AllocationDetail
   Set is_reversed = 1,
    allocation_reversed_date = GETDATE()
   Where allocation_id =@iAllocationID
        print 'updating transdetail'
        UPDATE transdetail
        SET     fully_matched = 0
        WHERE   transdetail_id = @iTransDetailID
 UPDATE transdetail
 SET     risk_transfer = 1
 WHERE   transdetail_id = @iTransDetailID AND isnull(risk_transfer,0)> 1
   UPDATE TransDetail
   SET outstanding_amount = outstanding_amount + @nAllocBaseAmount,
    outstanding_currency_amount = outstanding_currency_amount + @nAllocCurrencyAmount,
    outstanding_account_amount = outstanding_account_amount + @nAllocAccountAmount,
    outstanding_system_amount = outstanding_system_amount + @nAllocSystemAmount,
    amount_updated=GetDate()
   WHERE transdetail_id = @iTransDetailID
   If ISNULL(@iTransDetailExID, 0) > 0
    UPDATE TransDetailEx
    SET outstanding_currency_amount = outstanding_currency_amount + @nAllocCurrencyAmount,
  outstanding_account_amount = outstanding_account_amount + @nAllocAccountAmount,
  outstanding_system_amount = outstanding_system_amount + @nAllocSystemAmount
    WHERE transdetail_id = @iTransDetailID and transdetailex_id = @iTransDetailExID
        FETCH NEXT FROM c_Temp INTO
            @iAllocationDetailID,
            @iTransDetailID,
            @nAllocCurrencyAmount,
            @nAllocBaseAmount,
            @nWriteOffCurrencyAmount,
            @nWriteOffBaseAmount,
            @nADStartOSCurrencyAmount,
            @nADStartOSBaseAmount,
   @nCurrDiffCurrencyAmount,
   @nCurrDiffBaseAmount,
   @nAllocAccountAmount,
   @nAllocSystemAmount,
   @iTransDetailExID
    END
    CLOSE c_Temp
    DEALLOCATE c_Temp
    print 'updating cashlistitem'
IF @iAllocationID IS NULL AND @v_iAllocationDetailID_CR IS NULL AND @v_iAllocationDetailID_DR IS NULL
BEGIN
	UPDATE cashlistitem
		SET     allocationstatus_id =
					CASE
						WHEN ad2.new_os_ccy_amount = (-1 * cashlistitem.amount) THEN
							1
						WHEN ad2.new_os_ccy_amount <> 0 THEN
							4
						ELSE
							allocationstatus_id
					END
		FROM
				AllocationDetail ad1,
				AllocationDetail ad2
		WHERE   cashlistitem.transdetail_id = ad1.transdetail_id
		  AND   cashlistitem.transdetail_id = ad2.transdetail_id
		  AND   ad2.allocation_id =
				(
					SELECT  MAX(ad3.allocation_id)
					FROM    allocationdetail ad3
					WHERE   ad3.transdetail_id = cashlistitem.transdetail_id
				)
	END
ELSE
IF @iAllocationID IS NOT NULL AND (@v_iAllocationDetailID_CR IS NULL OR @v_iAllocationDetailID_DR IS NULL  )
BEGIN
 IF @v_iAllocationDetailID_CR IS NULL AND @v_iAllocationDetailID_DR IS NULL
	BEGIN
	UPDATE cashlistitem
		SET     allocationstatus_id =
					CASE
						WHEN ad2.new_os_ccy_amount = (-1 * cashlistitem.amount) THEN
							1
						WHEN ad2.new_os_ccy_amount <> 0 THEN
							4
						ELSE
							allocationstatus_id
					END
		FROM
				AllocationDetail ad1,
				AllocationDetail ad2
		WHERE   cashlistitem.transdetail_id = ad1.transdetail_id
		  AND   cashlistitem.transdetail_id = ad2.transdetail_id
		  AND   ad2.allocation_id =
				(
					SELECT  MAX(ad3.allocation_id)
					FROM    allocationdetail ad3
					WHERE   ad3.transdetail_id = cashlistitem.transdetail_id
				)
		  AND
					 ad1.allocation_id = @iAllocationID
	END
   ELSE
   BEGIN
	   UPDATE cashlistitem
		SET     allocationstatus_id =
					CASE
						WHEN ad2.new_os_ccy_amount = (-1 * cashlistitem.amount) THEN
							1
						WHEN ad2.new_os_ccy_amount <> 0 THEN
							4
						ELSE
							allocationstatus_id
					END
		FROM
				AllocationDetail ad1,
				AllocationDetail ad2
		WHERE   cashlistitem.transdetail_id = ad1.transdetail_id
		  AND   cashlistitem.transdetail_id = ad2.transdetail_id
		  AND   ad2.allocation_id =
				(
					SELECT  MAX(ad3.allocation_id)
					FROM    allocationdetail ad3
					WHERE   ad3.transdetail_id = cashlistitem.transdetail_id
				)
		  AND   (
					 ad1.allocation_id = @iAllocationID
				 )
		  AND   (
					 ad1.allocationdetail_id = @v_iAllocationDetailID_CR
					)
				OR
				   (
					 EXISTS (
							SELECT 1
							FROM    allocationdetail ad3
							WHERE   ad3.allocationdetail_id = @v_iAllocationDetailID_DR
								AND cashlistitem.cashlistitem_id = ad3.cashlistitem_id
								AND ad1.transdetail_id = cashlistitem.transdetail_id
								AND ad1.allocation_id = ad3.allocation_id )
					)
	END
END
ELSE
IF @iAllocationID IS NULL AND (@v_iAllocationDetailID_CR IS NOT NULL OR @v_iAllocationDetailID_DR IS NOT NULL)
BEGIN
UPDATE cashlistitem
    SET     allocationstatus_id =
                CASE
                    WHEN ad2.new_os_ccy_amount = (-1 * cashlistitem.amount) THEN
                        1
                    WHEN ad2.new_os_ccy_amount <> 0 THEN
                        4
                    ELSE
                        allocationstatus_id
                END
    FROM
            AllocationDetail ad1,
            AllocationDetail ad2
    WHERE   cashlistitem.transdetail_id = ad1.transdetail_id
      AND   cashlistitem.transdetail_id = ad2.transdetail_id
      AND   ad2.allocation_id =
            (
                SELECT  MAX(ad3.allocation_id)
                FROM    allocationdetail ad3
                WHERE   ad3.transdetail_id = cashlistitem.transdetail_id
            )
      AND   (
                 ad1.allocationdetail_id = @v_iAllocationDetailID_CR
                )
            OR
               (
                 EXISTS (
                        SELECT 1
                        FROM    allocationdetail ad3
                        WHERE   ad3.allocationdetail_id = @v_iAllocationDetailID_DR
                            AND cashlistitem.cashlistitem_id = ad3.cashlistitem_id
                            AND ad1.transdetail_id = cashlistitem.transdetail_id
                            AND ad1.allocation_id = ad3.allocation_id )
                )
END
    COMMIT TRANSACTION

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_Allocation_Reversal_AllocationDetail'
GO



CREATE PROCEDURE spu_ACT_Do_Allocation_Reversal_AllocationDetail

                                        @v_iAllocationDetailID int,
                                        @v_iTransDetailID int,
                                        @v_nAllocCurrencyAmount    numeric (19,4),
                                        @v_nAllocBaseAmount        numeric (19,4),   
                                        @v_nWriteOffCurrencyAmount numeric (19,4) = 0,   
                                        @v_nWriteOffBaseAmount     numeric (19,4) = 0  
AS

    declare @iReturnCode int



    BEGIN TRANSACTION


    -- ===================================================================	
    -- UPDATE AllocationDetail
    -- ===================================================================	

    -- update the row in question 
    UPDATE	ad
    SET		ad.alloc_base_amount = ad.alloc_base_amount - @v_nAllocBaseAmount,
            ad.alloc_ccy_amount = ad.alloc_ccy_amount - @v_nAllocCurrencyAmount,
            ad.write_off_amount = ad.write_off_amount - @v_nWriteOffBaseAmount
	FROM	allocationdetail ad
    WHERE	ad.allocationdetail_id = @v_iAllocationDetailID

	-- plus change the balance on all later allocations for the same TransDetailId
    UPDATE	ad
    SET     new_os_base_amount = orig_base_amount - 
            	(
            		SELECT ISNULL(SUM(ISNULL(alloc_base_amount,0)),0)
            		FROM allocationdetail
            		WHERE allocationdetail_id <= ad.allocationdetail_id
            		AND transdetail_id = @v_iTransDetailID
            	),
            new_os_ccy_amount = orig_ccy_amount - 
				(
					SELECT ISNULL(SUM(ISNULL(alloc_ccy_amount,0)),0)
					FROM allocationdetail
					WHERE allocationdetail_id <= ad.allocationdetail_id
					AND transdetail_id = @v_iTransDetailID
            	),
            os_base_amount = orig_base_amount - 
            	(
            		SELECT ISNULL(SUM(ISNULL(alloc_base_amount,0)),0)
            		FROM allocationdetail
            		WHERE allocationdetail_id < ad.allocationdetail_id
            		AND transdetail_id = @v_iTransDetailID
            	),
            os_ccy_amount = orig_ccy_amount - 
				(
					SELECT ISNULL(SUM(ISNULL(alloc_ccy_amount,0)),0)
					FROM allocationdetail
					WHERE allocationdetail_id < ad.allocationdetail_id
					AND transdetail_id = @v_iTransDetailID
            	),
            fully_matched = 0
	FROM	allocationdetail ad
    WHERE   -- choose all allocationdetail rows for this TransDetailId
            transdetail_id = @v_iTransDetailID
            -- ignore all rows that have been added before the one being reversed
	AND		allocationdetail_id >= @v_iAllocationDetailID

    -- ===================================================================	
    -- ALL DONE
    -- ===================================================================	
    COMMIT TRANSACTION
GO
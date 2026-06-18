SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_ReverseAllocation'
GO

                    
CREATE PROCEDURE spu_ACT_Select_ReverseAllocation
	@v_iAllocationId INT = null,
	@v_iCashListItemId INT = null,
	@v_iTransDetailId INT = null,
	@bSelectSplit BIT = 0

AS

    


    -- Validate parameters
    -- ===================
    IF (@v_iAllocationId IS NULL AND
        @v_iCashListItemId IS NULL AND
        @v_iTransDetailId IS NULL ) BEGIN
        PRINT 'ERROR - at least one parameter must be set'
        RETURN -1        -- get out of here
    END

	IF EXISTS (SELECT Null FROM Hidden_options WHERE option_number = 107 AND branch_id = 1 AND ISNull(Value, 0) = 1)
		AND ISNULL(@v_iAllocationID, 0) > 0
			-- return this allocation and associated write-offs\currency gain-loss only
			SELECT DISTINCT
				AD.allocation_id, 
				A.account_id as account_id_CR,
				0 as account_id_DR,
				0 as allocationdetail_id_CR ,
				0 as allocationdetail_id_DR,
				0 as alloc_ccy_amount	 
			FROM Allocation A 
			JOIN Allocation batch ON batch.allocationbatch_id = a.allocationbatch_id
			JOIN AllocationDetail AD ON AD.allocation_id = batch.allocation_id
			WHERE A.allocation_id = @v_iAllocationID AND ISNULL(ad.is_reversed, 0) = 0 	
			AND (
			EXISTS (SELECT NULL FROM AllocationDetail 
							WHERE (document_ref Like 'SWD%' OR document_ref Like 'SCD%') 
									And AllocationDetail.allocation_id = ad.allocation_id)
			OR
			AD.allocation_id = @v_iAllocationID)		
	ELSE 
	BEGIN
	IF (@v_iAllocationID IS NULL AND
		@v_iCashListItemId IS NOT NULL) BEGIN

		SELECT DISTINCT
			AD.allocation_id, 
			A.account_id as account_id_CR,
			0 as account_id_DR,
			0 as allocationdetail_id_CR ,
			0 as allocationdetail_id_DR,
			0 as alloc_ccy_amount	
		FROM AllocationDetail AD
		
		JOIN Allocation A ON A.allocation_id=AD.allocation_id
		WHERE AD.cashlistitem_id = @v_iCashListItemId AND ISNULL(ad.is_reversed, 0) = 0

	    AND (Exists (SELECT NULL FROM AllocationDetail AD1 
                        Left Join CashListItem cli ON cli.transdetail_id = ad1.transdetail_id
                        WHERE ad1.allocation_id = ad.allocation_id AND ad1.documenttype_id != 22
                        GROUP BY AD1.allocation_id HAVING COUNT(*) >= 1)
        OR @bSelectSplit = 1)
	END
	ELSE 
	BEGIN

		SELECT DISTINCT
			AD.allocation_id, 
			A.account_id as account_id_CR,
			0 as account_id_DR,
			0 as allocationdetail_id_CR ,
			0 as allocationdetail_id_DR,
			0 as alloc_ccy_amount	
		FROM AllocationDetail AD
		
		JOIN Allocation A ON A.allocation_id=AD.allocation_id
		WHERE AD.transdetail_id = @v_iTransDetailId AND ISNULL(ad.is_reversed, 0) = 0
		AND (Exists (SELECT NULL FROM AllocationDetail AD1 
                        Left Join CashListItem cli ON cli.transdetail_id = ad1.transdetail_id
                        WHERE ad1.allocation_id = ad.allocation_id AND ad1.documenttype_id != 22
                        GROUP BY AD1.allocation_id HAVING COUNT(*) >= 1)
        OR @bSelectSplit = 1)

	END
	END



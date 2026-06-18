SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_DoAllocationDetailPairsExist'
GO



CREATE PROCEDURE spu_ACT_Get_DoAllocationDetailPairsExist

                            @v_iAllocationId int = null,
                            @v_iCashListItemId int = null,
                            @v_iTransDetailId int = null, 
                            @r_bDoAllocationDetailPairsExist int OUTPUT
AS

    DECLARE @bDoAllocationDetailPairsExist int
            
    SET     @bDoAllocationDetailPairsExist = 0    


    -- Validate parameters
    -- ===================
    IF (@v_iAllocationId IS NULL AND
        @v_iCashListItemId IS NULL AND
        @v_iTransDetailId IS NULL ) BEGIN
        PRINT 'ERROR - at least one parameter must be set'
        RETURN -1        -- get out of here
    END

    -- We can only be certain which credit transaction is linked to which 
    -- debit transaction when the credit is a cash list item allocated to a specific debit (via cashlistitemid) 
    -- or when there is only 1 cr and 1 dr in the allocation.
    -- For other types of credit there is no link to the actual debit transaction - particularly in 
    -- a many:many credit:debit relationship.
    -- In such cases we can only only reverse the complete allocation - ie not partial


    -- identify whether we can reverse parts of an allocation
    -- ======================================================

    SELECT 
        @bDoAllocationDetailPairsExist = 
            MIN(
                CASE
                    WHEN Q1.allow_partial_reversal = 1 THEN
                        -- sub query did not find any problems 
                        1 
                    WHEN Q1.allocation_detail_count_CR = 1 AND
                         Q1.allocation_detail_count_DR = 1 THEN
                        -- if there is only one credit and one debit in the allocation then we can pair them up 
                        -- regardless of what the sub query detected
                        1
                    ELSE 
                        -- we have not been able to match every cr to a dr for one of the allocations selected
                        0
                END
            ) 

    FROM 
        (
            -- run a subquery to return details for each allocation id that matches the search citeria
            
            SELECT
                    ad1.allocation_id AS allocation_id,
        
                    MIN(
                        CASE 
                            WHEN ISNULL(ad1.cashlistitem_id, 0) = 0 AND
                                 ISNULL(cli1.cashlistitem_id, 0) = 0 THEN
                                -- at least one of the transactions within the allocation is neither a cashlistitem itself 
                                -- nor has a link to a cashlistitem so not all debits can be matched to a credit 
                                -- therefore partial reversal not allowed
                                0
                            ELSE
                                1
                        END
                    ) AS allow_partial_reversal, 
        
                    -- count number of credit transactions
                    SUM(
                        CASE
                            WHEN ad1.orig_base_amount < 0 THEN
                                1
                            ELSE
                                0
                        END
                    ) AS allocation_detail_count_CR, 
        
                    -- count number of debit transactions
                    SUM(
                        CASE
                            WHEN ad1.orig_base_amount >= 0 THEN
                                1
                            ELSE
                                0
                        END
                    ) AS allocation_detail_count_DR
        
            FROM
                    allocationdetail ad1
                    -- join to determine whether a transaction is also a cash list item
                    LEFT OUTER JOIN cashlistitem cli1 ON ad1.transdetail_id = cli1.transdetail_id
            WHERE
                    -- identify whether this transaction is part of one of the allocation ids being searched for
                    EXISTS
                    (
                        SELECT  1 
                        FROM 
                                allocationdetail ad2
                                LEFT OUTER JOIN cashlistitem cli2 ON ad2.transdetail_id = cli2.transdetail_id
                        WHERE   
                                ad1.allocation_id = ad2.allocation_id
        
                                -- filter by allocation id
                                -- ======================= 
                          AND   (
                                    (
                                        -- select all - if a specific allocationId was not supplied
                                        @v_iAllocationId IS NULL
                                    )
                                OR
                                    (
                                        -- filter by allocationId if one was supplied 
                                        @v_iAllocationId IS NOT NULL
                                    AND ad2.allocation_id = @v_iAllocationId
                                    )
                                )
                    
                                -- filter by TransDetailID
                                -- ======================= 
                          AND   (
                                    (
                                        -- select all - if a specific TransDetailID was not supplied
                                        @v_iTransDetailId IS NULL
                                    )
                                OR
                                   (
                                        -- filter by TransDetailID if one was supplied 
                                        @v_iTransDetailId IS NOT NULL
                                    AND ad2.transdetail_id = @v_iTransDetailId
                                    )
                                )
                    
                                -- filter by CashListItemId
                                -- ======================== 
                          AND   (
                                    (
                                        -- select all - if a specific CashListItemId was not supplied
                                        @v_iCashListItemId IS NULL
                                    )
                                OR
                                    (
                                        -- filter by CashListItemId if one was supplied 
                                        @v_iCashListItemId IS NOT NULL
                                    AND 
                                        (
                                            ISNULL(ad2.cashlistitem_id, 0)  = @v_iCashListItemId 
                                         OR ISNULL(cli2.cashlistitem_id, 0) = @v_iCashListItemId
                                        )
                                    )
                                )
                        )
            GROUP BY 
                    ad1.allocation_id
        ) AS Q1
        -- end of subquery
        -- end of main query


    IF (@@ROWCOUNT <> 1) BEGIN
        PRINT 'ERROR - no allocations have been found'
        RETURN -2        -- get out of here
    END


	--DJM 28/11/2003 : If allocation did not match up correctly then only allow the complete reversal of it.
	IF @v_iAllocationId IS NOT NULL
	BEGIN
		
		IF 0 <>
		(
			SELECT ISNULL(SUM(ROUND(orig_base_amount,0)),0)
			FROM allocationdetail
			WHERE allocation_id = @v_iAllocationId
		)
		BEGIN
			SET @bDoAllocationDetailPairsExist = 0
		END
	END

    print @bDoAllocationDetailPairsExist

    SET @r_bDoAllocationDetailPairsExist = @bDoAllocationDetailPairsExist
GO
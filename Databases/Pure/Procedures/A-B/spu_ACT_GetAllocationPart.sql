EXECUTE DDLDropProcedure 'spu_ACT_GetAllocationPart'
GO

CREATE PROCEDURE spu_ACT_GetAllocationPart
    @AllocationId INT,
    @LinkedTransdetailId INT,
    @suspendedTransdetailId INT
AS

DECLARE @crAmount MONEY
DECLARE @crPercentage FLOAT
DECLARE @crMatchedAmount MONEY
DECLARE @crDIDAmount MONEY
DECLARE @nBranch_id INT
DECLARE @nSysOption16 INT
DECLARE @nSysOption94 INT

SELECT 
    @nBranch_id = company_id 
FROM transdetail 
WHERE transdetail_id = @LinkedTransdetailId

SELECT 
    @nSysOption16 = RTRIM(value)
FROM system_options
WHERE option_number = 16
AND branch_id = @nBranch_id
   
IF @nSysOption16 = 1
BEGIN
    SELECT 
        @nSysOption94 = RTRIM(value)
    FROM system_options
    WHERE option_number = 94
    AND branch_id = @nBranch_id
END
ELSE
BEGIN
    SELECT @nSysOption94 = 0
END
 

SELECT @crAmount = 0
SELECT @crPercentage = 1

    /*Get the amount that this allocation is allocating of the linked transaction*/
  	SELECT
        	@crMatchedAmount = ISNULL(ad.alloc_ccy_amount, 0)  
    	FROM allocationdetail ad 
    	WHERE ad.transdetail_id = @LinkedTransdetailId  
    	AND ad.allocation_id = @AllocationId

    SELECT 
        @crDIDAmount = ISNULL(MAX(ad.alloc_ccy_amount), 0)
    FROM transdetail td
    JOIN account a
        ON a.account_id = td.account_id
    JOIN ledger l
        ON l.ledger_id = a.ledger_id
        AND l.ledger_short_name = 'SA'
    JOIN AllocationDetail ad
        ON ad.transdetail_id = td.transdetail_id
        AND ISNULL(ad.is_reversed, 0) = 0
    JOIN AllocationDetail ad2
        ON ad2.allocation_id = ad.allocation_id
        AND ad2.allocationdetail_id <> ad.allocationdetail_id
    JOIN transdetail td2
        ON td2.transdetail_id = ad2.transdetail_id
    JOIN transdetail_type tt2
        ON tt2.transdetail_type_id = td2.transdetail_type_id
        AND tt2.code = 'DIRECTDEBIT'
    WHERE td.transdetail_id = @LinkedTransdetailId
    AND EXISTS
        (
            SELECT
                NULL
            FROM AllocationDetail
            WHERE allocation_id = ad.allocation_id
            HAVING SUM(1) = 2
        )
        
    /*Gets outstanding amount of all linked transactions for the suspended transaction*/
    IF EXISTS
        (
            SELECT 
                NULL 
            FROM transdetail td
            JOIN suspended_accounts_transactions sat 
                ON sat.linked_transdetail_id = td.transdetail_id               
            WHERE sat.suspended_transdetail_id = @SuspendedTransdetailId
            AND sat.is_deleted = 0
            HAVING SUM(td.outstanding_currency_amount) = 0
        )
    BEGIN
        /*As any linked transactions are now fully paid the last amount needs to move*/
        SELECT 
            @crAmount = outstanding_currency_amount 
        FROM transdetail 
        WHERE transdetail_id = @SuspendedTransdetailId
    END        
    ELSE
    BEGIN
        /*If the matched amount does not equal the full amount of the linked transaction then it is a partial payment*/ 
        IF @crMatchedAmount <> 
            (
                SELECT 
                    currency_amount 
                FROM transdetail 
                WHERE transdetail_id = @LinkedTransdetailId
            )
        BEGIN

            /*Work out the percentage of the commission to move for this part payment*/
            SELECT 
                @crPercentage = @crMatchedAmount / (currency_amount - @crDIDAmount)
            FROM transdetail 
            WHERE transdetail_id = @LinkedTransdetailId

        END
    END 

SELECT 
    @crAmount,
    @crPercentage 
 
GO
 

 

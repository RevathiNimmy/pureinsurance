SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_FindAllocation'
GO


CREATE PROCEDURE spu_ACT_Do_FindAllocation
    @CompanyID smallint = NULL,
    @AccountID int = NULL,
    @DateFrom datetime = NULL,
    @DateTo datetime = NULL,
    @AllocationStatusId int = NULL,
    @DocumentRef varchar(25) = NULL,
    @UserId smallint = NULL,
    @CurrencyID smallint = NULL,
    @CurrencyAmount numeric(19,4) = NULL,
    @Tolerance smallint = NULL
AS

DECLARE 
    @lower numeric(19,4),
    @upper numeric(19,4)

SELECT
    @lower = abs(@CurrencyAmount) - (abs(@CurrencyAmount) * @Tolerance /100), 
    @upper = abs(@CurrencyAmount) + (abs(@CurrencyAmount) * @Tolerance /100) 

SELECT DISTINCT
       ac.short_code,
       al.allocation_date,
       us.username,
       al.allocationstatus_id,
       al.allocation_id,
       ad2.document_ref,
       ad2.alloc_ccy_amount,
       ad2.original_currency,
       @lower AS ' lower',
       @upper AS ' upper'
FROM   allocation al 
LEFT JOIN   
       PMUser us ON al.user_id = us.user_id
JOIN   allocationdetail ad ON al.allocation_id = ad.allocation_id
JOIN   allocationdetail ad2 ON al.allocation_id = ad2.allocation_id AND ad2.is_primary = 1
JOIN   account ac ON al.account_id = ac.account_id
WHERE (al.company_id = @CompanyID OR @CompanyID = NULL)
AND   (al.account_id = @AccountId OR @AccountId = NULL)
AND   (al.allocation_date >= @DateFrom OR @DateFrom IS NULL)
AND   (al.allocation_date <= @DateTo OR @DateTo IS NULL)
AND   (al.allocationstatus_id = @AllocationStatusId OR @AllocationStatusId IS NULL)
AND   (ad.document_ref like @DocumentRef OR @DocumentRef IS NULL)
AND   (al.user_id = @UserId OR @UserId = NULL)
AND   (ad.original_currency = @CurrencyID OR @CurrencyID IS NULL)
AND  ((abs(ad.alloc_ccy_amount) BETWEEN @lower AND @upper) OR @CurrencyAmount IS NULL)
ORDER BY 
       ac.short_code, 
       al.allocation_date


GO



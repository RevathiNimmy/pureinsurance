SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_perilreserve'
GO


CREATE PROCEDURE spu_wp_perilreserve
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE
    @description AS VARCHAR(50),
    @initial_reserve AS MONEY,
    @paid_to_date AS MONEY,
    @revised_reserve AS MONEY,
    @current_reserve AS MONEY,
    @sum_insured AS MONEY,
    @incurred AS MONEY,
    @average AS INT,
    @revised_reserve_entered AS TINYINT
    
SELECT
	@description = ISNULL(rt.description,0),
	@initial_reserve = ISNULL(r.initial_reserve,0),
  	@paid_to_date = ISNULL(r.paid_to_date,0),
    	@revised_reserve = ISNULL(r.revised_reserve,0),
   	@sum_insured = ISNULL(r.sum_insured,0),
   	@average = ISNULL(r.average,0),
   	@revised_reserve_entered = ISNULL(r.revised_reserve_entered,0)
FROM claim_peril cp
JOIN reserve r
ON r.claim_peril_id = cp.claim_peril_id
JOIN reserve_type rt
ON rt.reserve_type_id = r.reserve_type_id
WHERE   cp.claim_id = @ClaimCnt
AND cp.claim_peril_id = @Instance2
AND r.reserve_id = @Instance3

IF @revised_reserve_entered = 0 AND @revised_reserve = 0
BEGIN
	SELECT @current_reserve = @initial_reserve - @paid_to_date 
	SELECT @incurred = @initial_reserve 
END
ELSE
BEGIN
	SELECT @current_reserve = (@initial_reserve + @revised_reserve) - @paid_to_date
	SELECT @incurred = @initial_reserve + @revised_reserve
END

    SELECT
	@description peril_reserve_description,
	@initial_reserve Peril_Initial_Reserve,
	@paid_to_date Peril_Paid_To_Date,
	@revised_reserve Peril_Revised_Reserve,
	@sum_insured Peril_Sum_Insured,
	@average Peril_Average,
	@current_reserve Peril_Current_Reserve,
	@incurred Peril_Incurred
	
GO



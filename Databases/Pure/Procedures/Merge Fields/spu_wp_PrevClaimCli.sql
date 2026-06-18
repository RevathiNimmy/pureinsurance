SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PrevClaimCli'
GO


CREATE PROCEDURE spu_wp_PrevClaimCli
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  claim_number Prev_claim_num,
    policy_number prev_pol_num,
    (
    SELECT  sum(Initial_reserve + Revised_reserve)
    FROM    reserve r,
        claim_peril cp,
        reserve_type rt
    WHERE   cp.claim_id = @Instance2
    AND cp.claim_peril_id = r.claim_peril_id
    AND r.reserve_type_id = rt.reserve_type_id
    AND rt.Include_in_Total = 1
    ) Total_Reserve,
    (
    SELECT sum(amount) FROM Claim_payment WHERE claim_id = @Instance2
    ) Total_Paid,
    (
    SELECT  sum(Initial_reserve + Revised_reserve - Paid_to_date)
    FROM    reserve r,
        claim_peril cp,
        reserve_type rt
    WHERE   cp.claim_id = @Instance2
    AND cp.claim_peril_id = r.claim_peril_id
    AND r.reserve_type_id = rt.reserve_type_id
    AND rt.Include_in_Total = 1
    ) Current_Reserve

FROM    claim
WHERE   claim_id = @Instance2
GO



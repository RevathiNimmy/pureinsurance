SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PerilReserve_get_keys'
GO


CREATE PROCEDURE spu_wp_PerilReserve_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT       r.reserve_id

FROM         reserve r,
         claim_peril cp

WHERE        cp.claim_id = @ClaimCnt
AND      cp.claim_peril_id = @Instance2
AND      r.claim_peril_id = cp.claim_peril_id
GO



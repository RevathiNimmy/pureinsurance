SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_claimperilcount'
GO


CREATE PROCEDURE spu_wp_claimperilcount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT      count(cp.claim_peril_id) "how_many"

FROM        claim_peril cp
WHERE       cp.claim_id = @ClaimCnt
GO



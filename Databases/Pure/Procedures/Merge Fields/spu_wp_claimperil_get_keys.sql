SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_claimperil_get_keys'
GO


CREATE PROCEDURE spu_wp_claimperil_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT       cp.claim_peril_id

FROM         claim_peril cp

WHERE        cp.claim_id = @ClaimCnt
GO



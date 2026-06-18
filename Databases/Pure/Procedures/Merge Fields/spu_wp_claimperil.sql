SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_claimperil'
GO


CREATE PROCEDURE spu_wp_claimperil
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    SELECT  cp.description peril_description,
            cp.comments peril_comments
    FROM    claim_peril cp
    WHERE   cp.claim_id = @ClaimCnt
    AND     cp.claim_peril_id = @Instance2

GO



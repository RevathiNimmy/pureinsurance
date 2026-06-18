SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ClmCoinsurance'
GO


CREATE PROCEDURE spu_wp_ClmCoinsurance
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    SELECT  p.resolved_name Coinsurer_Name,
            cp.share_value Share_Value,
            cp.share Share_Percent
    FROM    claim_party cp,
            party p
    WHERE   cp.claim_party_id = @Instance1
    AND     cp.party_id = p.party_cnt

GO



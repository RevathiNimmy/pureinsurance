SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PrevClaimPolCount'
GO


CREATE PROCEDURE spu_wp_PrevClaimPolCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  COUNT(*) AS 'how_many'
FROM    claim
WHERE   policy_id = (SELECT policy_id FROM claim WHERE claim_id = @ClaimCnt)
AND     claim_version_status_id = 1 -- LIVE (Alix)
GROUP BY policy_id
GO



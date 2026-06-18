SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PrevClaimPol_get_keys'
GO


CREATE PROCEDURE spu_wp_PrevClaimPol_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT claim_id
FROM    claim
WHERE   policy_id = (SELECT policy_id FROM claim WHERE claim_id = @ClaimCnt)
GO



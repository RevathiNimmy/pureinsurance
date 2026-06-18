SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ClmCoinsurance_get_keys'
GO


CREATE PROCEDURE spu_wp_ClmCoinsurance_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  Claim_Party_id
FROM    claim_party
WHERE   claim_id = @ClaimCnt
AND insurer_type = 0
GO



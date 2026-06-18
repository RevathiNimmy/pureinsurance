SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ClmCoinsuranceCount'
GO


CREATE PROCEDURE spu_wp_ClmCoinsuranceCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  COUNT(*) how_many
FROM    claim_party
WHERE   claim_id = @ClaimCnt
AND insurer_type = 0
GO



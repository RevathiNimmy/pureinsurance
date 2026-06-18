SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ThisClaimCount'
GO


CREATE PROCEDURE spu_wp_ThisClaimCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT  COUNT(*) AS 'how_many'
FROM    claim
WHERE   claim_id = @ClaimCnt
GO



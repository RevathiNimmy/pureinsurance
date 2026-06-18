SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_clmperilotherpartyCount'
GO

CREATE PROCEDURE spu_wp_clmperilotherpartyCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT count(cpl.party_cnt) 'how_many'
FROM claim_party_link cpl
JOIN party p
ON p.party_cnt = cpl.party_cnt
WHERE cpl.claim_id = @ClaimCnt
AND ISNULL(cpl.peril_type_id,0) > 0

GO



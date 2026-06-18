SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_OthPtyTPConvCount'
GO


CREATE PROCEDURE spu_wp_OthPtyTPConvCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT       count(pc.party_conviction_id) 'how_many'

FROM         party_conviction pc,
         claim_party_link cpl

WHERE        cpl.claim_id = @ClaimCnt
AND      cpl.party_cnt = @instance2
AND      pc.party_cnt = @instance2
GO



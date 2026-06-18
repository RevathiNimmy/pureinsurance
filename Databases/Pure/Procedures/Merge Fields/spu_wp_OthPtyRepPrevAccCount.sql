SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_OthPtyRepPrevAccCount'
GO


CREATE PROCEDURE spu_wp_OthPtyRepPrevAccCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT      count(pa.previous_accidents_id) "how_many"

FROM        claim_party_link cpl,
        previous_accidents pa

WHERE       cpl.claim_id = @ClaimCnt
AND     cpl.party_cnt = @instance2
AND     pa.party_cnt = @instance2
GO



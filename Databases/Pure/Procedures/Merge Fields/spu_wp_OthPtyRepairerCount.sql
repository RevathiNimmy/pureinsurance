SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_OthPtyRepairerCount'
GO


CREATE PROCEDURE spu_wp_OthPtyRepairerCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT      count(cpl.party_cnt) "how_many"

FROM        claim_party_link cpl,
        party p

WHERE       cpl.claim_id = @ClaimCnt
AND     cpl.party_cnt = p.party_cnt
AND     p.party_type_id in (SELECT party_type_id
                    FROM   party_type
                    WHERE  code = 'OTREPAIRER')
GO



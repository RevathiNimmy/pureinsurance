SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_OthPtyWitAddress_get_keys'
GO


CREATE PROCEDURE spu_wp_OthPtyWitAddress_get_keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT       pau.address_cnt

FROM         party_address_usage pau,
         claim_party_link cpl

WHERE        cpl.claim_id = @ClaimCnt
AND      cpl.party_cnt = @instance2
AND      pau.party_cnt = @instance2
GO



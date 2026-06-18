SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_OthPtyWitAddress'
GO


CREATE PROCEDURE spu_wp_OthPtyWitAddress
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT       aut.description oth_pty_adduse,
         a.address1 oth_pty_add1,
         a.address2 oth_pty_add2,
         a.address3 oth_pty_add3,
         a.address4 oth_pty_add4,
         a.postal_code oth_pty_pc

FROM         party_address_usage pau,
         claim_party_link cpl,
         address_usage_type aut,
         address a

WHERE        cpl.claim_id = @ClaimCnt
AND      cpl.party_cnt = @instance2
AND      pau.party_cnt = @instance2
AND      pau.address_cnt = @instance3
AND      pau.address_cnt = a.address_cnt
AND      pau.address_usage_type_id = aut.address_usage_type_id
GO



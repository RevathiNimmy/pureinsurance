SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PartyAddressCount'
GO


CREATE PROCEDURE spu_wp_PartyAddressCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT	Count(address_cnt) 'how_many'
FROM	Party_Address_Usage
WHERE	party_cnt = @PartyCnt

GO

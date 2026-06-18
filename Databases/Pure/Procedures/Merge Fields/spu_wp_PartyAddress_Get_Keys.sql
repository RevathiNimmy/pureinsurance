SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PartyAddress_Get_Keys'
GO


CREATE PROCEDURE spu_wp_PartyAddress_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT	address_cnt
FROM	Party_Address_Usage
WHERE	party_cnt = @PartyCnt

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PartyLoyalityScheme_Get_Keys'
GO
CREATE  PROCEDURE spu_wp_PartyLoyalityScheme_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT 	Party_loyalty_scheme_id
FROM PARTY_LOYALTY_SCHEME 
WHERE 	party_cnt = @PartyCnt
GO
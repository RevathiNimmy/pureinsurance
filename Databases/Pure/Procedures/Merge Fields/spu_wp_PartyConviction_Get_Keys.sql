SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PartyConviction_Get_Keys'
GO
CREATE  PROCEDURE spu_wp_PartyConviction_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT 	Party_Conviction_id
FROM PARTY_Conviction 
WHERE 	party_cnt = @PartyCnt
GO
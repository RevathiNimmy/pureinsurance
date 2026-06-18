SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_Party_AE_Contact_Get_Keys'
GO


CREATE PROCEDURE spu_wp_Party_AE_Contact_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT	contact_cnt
FROM	Party_Contact_Usage
WHERE	party_cnt = @Instance1

GO

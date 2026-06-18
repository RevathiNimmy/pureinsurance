SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PartyContactCount'
GO


CREATE PROCEDURE spu_wp_PartyContactCount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT	Count(contact_cnt) 'How_Many'
FROM	Party_Contact_Usage
WHERE	party_cnt = @PartyCnt
GO

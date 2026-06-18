
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PartyRelationship'
GO

CREATE PROCEDURE spu_wp_PartyRelationship
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT 	p.shortname,
		p.[name],
		p.resolved_name,
		pr.description 'Relationship',
        ppc.forename,
        ppc.party_title_code
FROM	Party_Relationship pr JOIN Party p ON pr.relation_cnt = p.party_cnt
LEFT JOIN Party_Personal_Client ppc ON ppc.party_cnt = p.party_cnt
WHERE	pr.party_cnt = @PartyCnt
AND		pr.relation_cnt = @Instance2
GO
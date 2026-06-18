SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_Party_AE_Contact'
GO


CREATE PROCEDURE spu_wp_Party_AE_Contact
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT	c.area_code,
		c.number,
		c.extension,
		c.[description],
		ct.code,
		ct.[description] 'CTDescription'
FROM	party_contact_usage pcu JOIN Contact c ON pcu.contact_cnt = c.contact_cnt
JOIN 	Contact_Type ct ON c.contact_type_id = ct.contact_type_id
WHERE	pcu.party_cnt = @Instance1
AND		pcu.contact_cnt = @Instance2


GO

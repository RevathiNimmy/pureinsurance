SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ContactAddress'
GO


CREATE PROCEDURE spu_wp_ContactAddress
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
FROM	Party_Address_Usage pau JOIN Contact_Address_Usage cau ON pau.address_cnt = cau.address_cnt
JOIN 	Contact c ON cau.contact_cnt = c.contact_cnt
JOIN 	Contact_Type ct ON c.contact_type_id = ct.contact_type_id
WHERE	pau.party_cnt = @PartyCnt
AND		pau.address_cnt = @Instance2
AND		cau.contact_cnt = @Instance3

GO

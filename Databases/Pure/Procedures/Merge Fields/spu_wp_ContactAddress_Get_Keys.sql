SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_ContactAddress_Get_Keys'
GO


CREATE PROCEDURE spu_wp_ContactAddress_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


SELECT	cau.contact_cnt
FROM	Party_Address_Usage pau JOIN Contact_Address_Usage cau ON pau.address_cnt = cau.address_cnt
JOIN 	Contact c ON cau.contact_cnt = c.contact_cnt
WHERE	pau.party_cnt = @PartyCnt
AND		pau.address_cnt = @Instance2


GO

DDLDROPPROCEDURE 'spu_wp_party_contact_email_Get_Keys'
GO
CREATE PROCEDURE spu_wp_party_contact_email_Get_Keys
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
SELECT c.contact_cnt
FROM Party P INNER JOIN Party_Contact_Usage PCU ON P.party_cnt=PCU.party_cnt  
INNER JOIN Contact C ON PCU.contact_cnt=C.contact_cnt  
INNER JOIN Contact_Type CT ON C.contact_type_id=CT.contact_type_id  
WHERE P.party_cnt=@PartyCnt AND CT.Code in ('MEMAIL','E-MAIL')  

GO
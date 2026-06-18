SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_wp_consultant_contact_email'
GO

CREATE PROCEDURE spu_wp_consultant_contact_email  
  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
  
AS  
  
DECLARE @contact_id INT  
DECLARE @area_code varchar(10)  
DECLARE @number varchar(255)  
DECLARE @extension varchar(6)  
DECLARE @complete_number varchar(278)  
DECLARE @ConsultantCnt INT
  
SELECT @complete_number=NULL  
SELECT @contact_id=NULL  

SELECT @ConsultantCnt = p.consultant_cnt
FROM     party p
WHERE p.party_cnt=@PartyCnt  
  
DECLARE ContactCursor CURSOR SCROLL KEYSET READ_ONLY FOR  
SELECT  
C.contact_cnt,  
isnull(C.area_code,''),  
isnull(C.number,''),  
isnull(c.extension,'')  
FROM Party P INNER JOIN Party_Contact_Usage PCU ON P.party_cnt=PCU.party_cnt  
INNER JOIN Contact C ON PCU.contact_cnt=C.contact_cnt  
INNER JOIN Contact_Type CT ON C.contact_type_id=CT.contact_type_id  
WHERE P.party_cnt = @ConsultantCnt AND CT.Code='E-MAIL'  
  
OPEN ContactCursor  
FETCH ABSOLUTE @instance1 FROM ContactCursor INTO @contact_id,@area_code,@number,@extension  
CLOSE ContactCursor  
DEALLOCATE ContactCursor  
  
IF @contact_id IS NOT NULL  
BEGIN  
 SELECT @complete_number=rtrim(ltrim(@number))  
END  
  
SELECT @complete_number AS contact_number  

GO

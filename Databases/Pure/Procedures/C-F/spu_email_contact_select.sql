
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_email_contact_select'
GO


CREATE PROCEDURE spu_email_contact_select  
 @party_cnt int,  
 @contact_type VARCHAR(255)= NULL  
AS   
  
BEGIN  
  
--**********************************************************************************************  
-- Author : Amit Kumar   
--   
-- History: 30/01/2008    
--**********************************************************************************************  
SELECT	c.contact_cnt, c.area_code, c.number, c.extension, c.description, ct.contact_type_id,p.resolved_name  
FROM    Contact c INNER JOIN  
		Contact_Type ct ON c.contact_type_id = ct.contact_type_id INNER JOIN  
		Contact_Address_Usage cau ON c.contact_cnt = cau.contact_cnt INNER JOIN  
		Party_Address_Usage pau ON cau.address_cnt = pau.address_cnt  INNER JOIN
		Party p ON p.party_cnt = pau.party_cnt

WHERE   (pau.party_cnt = @party_cnt) AND (ct.code = ISNULL(@contact_type,ct.code))   
UNION  
SELECT  c.contact_cnt, c.area_code, c.number, c.extension, c.description, ct.contact_type_id, p.resolved_name  
FROM    Contact c INNER JOIN  
        Contact_Type ct ON c.contact_type_id = ct.contact_type_id INNER JOIN  
        Party_Contact_Usage pcu ON c.contact_cnt = pcu.contact_cnt INNER JOIN
        Party p ON p.party_cnt = pcu.party_cnt
WHERE   (pcu.party_cnt = @party_cnt) AND (ct.code = ISNULL(@contact_type,ct.code))   
  
END  

GO


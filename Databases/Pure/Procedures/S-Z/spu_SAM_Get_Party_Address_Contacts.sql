SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Party_Address_Contacts'
GO

CREATE PROCEDURE spu_SAM_Get_Party_Address_Contacts    
    
@party_cnt integer    
    
AS    
    
SELECT     
    
cau.address_cnt,
contact_type.code as contact_type_code,     
area_code,     
number,     
extension    
    
FROM party_address_usage pau  
  
 INNER JOIN contact_Address_usage cau ON  
   cau.address_Cnt = pau.address_cnt  
    
  INNER JOIN contact ON    
   contact.contact_cnt = cau.contact_cnt    
    
   INNER JOIN contact_type ON    
    contact_type.contact_type_id = contact.contact_type_id    
    
WHERE party_cnt = @party_cnt  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

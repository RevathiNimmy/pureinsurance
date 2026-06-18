SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Party_Contacts'
GO

CREATE PROCEDURE spu_SAM_Get_Party_Contacts      
      
@party_cnt integer  
 
AS  
DECLARE @party_type_code CHAR(10)
 
SELECT @party_type_code= Party_Type.code 
FROM Party 
		INNER JOIN Party_Type WITH(NOLOCK) ON
			Party.party_type_id = Party_Type.party_type_id 
WHERE party_cnt=@party_cnt

IF(@party_type_code like 'OT%')
	BEGIN
  		 
		SELECT   
		contact_type.code as contact_type_code,  
		area_code,  
		number,  
		extension,
		contact.Contact_Type_id,  
		contact.description,  
		contact_type.description As 'ContactTypeDescription'
  
		FROM party_address_usage pau  
  
		 INNER JOIN contact_Address_usage cau ON  
		   cau.address_Cnt = pau.address_cnt  
  
		  INNER JOIN contact ON  
		   contact.contact_cnt = cau.contact_cnt  
  
		   INNER JOIN contact_type ON  
			contact_type.contact_type_id = contact.contact_type_id
			
		WHERE party_cnt = @party_cnt  
		UNION
		SELECT    
		contact_type.code as contact_type_code,  
		area_code,  
		number,  
		extension,  
		contact.Contact_Type_id,  
		contact.description,  
		contact_type.description As 'ContactTypeDescription'  
  
		FROM party_contact_usage pau  
  
		  INNER JOIN contact ON  
		   contact.contact_cnt = pau.contact_cnt  
  
		   INNER JOIN contact_type ON  
			contact_type.contact_type_id = contact.contact_type_id  
  
		WHERE party_cnt = @party_cnt  
	END
ELSE
	BEGIN
	
		SELECT    
			contact_type.code as contact_type_code,  
			area_code,  
			number,  
			extension,  
			contact.Contact_Type_id,  
			contact.description,  
			contact_type.description As 'ContactTypeDescription'  
	  
			FROM party_contact_usage pau  
	  
			  INNER JOIN contact ON  
			   contact.contact_cnt = pau.contact_cnt  
	  
			   INNER JOIN contact_type ON  
				contact_type.contact_type_id = contact.contact_type_id  
	  
			WHERE party_cnt = @party_cnt  
	END    
    



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

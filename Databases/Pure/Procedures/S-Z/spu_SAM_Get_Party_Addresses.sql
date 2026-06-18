SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Party_Addresses'
GO

CREATE PROCEDURE spu_SAM_Get_Party_Addresses  

  
@party_cnt INTEGER  
  
AS  
  
SELECT   
  
address.address_cnt,
address_id,   
address1,   
address2,   
address3,   
address4,   
postal_code,  
aut.code as address_usage_type_code,   
		country.code as country_code,
		address5,
		address6,
		address7,
		address8,
		address9,
		address10
  
FROM party_address_usage pau  
  
INNER JOIN address ON  
 pau.address_cnt = address.address_cnt  
  
 INNER JOIN country ON  
  address.country_id = country.country_id  
  
INNER JOIN address_usage_type aut ON  
 aut.address_usage_type_id = pau.address_usage_type_id  
  
WHERE party_cnt = @party_cnt  
  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_Address_sel'
GO

CREATE PROCEDURE spu_Claim_Address_sel  
    @address_cnt int  
AS  
  
SELECT  
 address_cnt,  
 source_id,  
 address_id,  
 address1,  
 address2,  
 address3,  
 address4,  
 postal_code,  
 claim_address.country_id,  
 created_by_id,  
 date_created,  
 modified_by_id,  
 last_modified,  
 address_usage_type_id, 
 Country.code country_code
 FROM Claim_Address  

	LEFT JOIN Country ON 
		Country.country_id = claim_address.country_id

WHERE address_cnt = @address_cnt  




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

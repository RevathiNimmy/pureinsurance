SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Address_sel'
GO


CREATE PROCEDURE spe_Address_sel  
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
    address.country_id,  
    created_by_id,  
    date_created,  
    modified_by_id,  
    last_modified,  
    ExternalId,
    Country.Description country_description, 
    Country.Code country_code,
	address5,
	address6,
	address7,
	address8,
	address9,
	address10 
 FROM Address  

	LEFT OUTER JOIN Country ON 
		Country.Country_id = Address.Country_Id

WHERE address_cnt = @address_cnt  





GO

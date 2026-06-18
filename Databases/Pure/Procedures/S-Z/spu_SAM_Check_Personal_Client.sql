SET QUOTED_IDENTIFIER OFF 
GO

Execute DDLDropProcedure 'spu_SAM_Check_Personal_Client'
GO
 

CREATE PROC spu_SAM_Check_Personal_Client  
@Surname VARCHAR(255)= '',  
@ResolvedName VARCHAR(387)= '',  
@AddressLine1 VARCHAR(60)= '' 
 
AS  
  
SELECT Party.party_cnt,Party.resolved_name  
FROM Party WITH(NOLOCK)  
INNER join Party_Type WITH(NOLOCK) ON Party.party_type_id = Party_Type.party_type_id  
LEFT OUTER join (Party_Address_Usage INNER JOIN Address WITH(NOLOCK)ON Party_Address_Usage.address_cnt = Address.address_cnt INNER JOIN Address_Usage_Type WITH(NOLOCK) ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id
 AND Address_Usage_Type.code = '3131 XCO') ON Party.party_cnt = Party_Address_Usage.party_cnt  
WHERE Party.is_deleted = 0  
AND (  
       (  
			Party.resolved_name = @ResolvedName  AND ISNULL(party.resolved_name,'') <> ''  
		)   
	)   
      
AND (
		Address.address1 = @AddressLine1 AND ISNULL(Address.address1,'') <> ''
	)   
AND Party_Type.code = 'PC'  
ORDER BY Party.shortname  
  
GO
SET QUOTED_IDENTIFIER OFF 
GO

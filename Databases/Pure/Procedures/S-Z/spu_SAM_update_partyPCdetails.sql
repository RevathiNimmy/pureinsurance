SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_update_partyPCdetails'
GO
--Start (girija) - (UIIC WR27 - MTA Amend Client.doc)  
create PROCEDURE spu_SAM_update_partyPCdetails  
    	@party_cnt int,  
    	@SecEmployersBusinessCode varchar(70),  
    	@SecEmploymentStatusCode varchar(70),  
    	@NationalityCode int,
	@AccommodationCode varchar(70),
	@Salutation varchar(255),
	@TPS tinyint,
	@MPS tinyint,
	@eMPS tinyint,
	@Source varchar(255),
	@PetOwner tinyint,
	@Trading_Name varchar(255)
AS  

BEGIN  
UPDATE Party_Personal_Client  
    SET  
    	secondary_employer_business=@SecEmployersBusinessCode,
    	secondary_employment_status_co=@SecEmploymentStatusCode,
    	Nationality_id=@NationalityCode,
	accommodation_type_code=@AccommodationCode,
	salutation=@Salutation,
	tpsind=@TPS,
	mailshot=@MPS,
	empsind=@eMPS,
	source=@Source,
	is_pet_owner=@PetOwner 

WHERE 	party_cnt = @party_cnt  

Update party 
	SET trading_name=@Trading_Name
WHERE 	party_cnt = @party_cnt  
END  
--End (girija) - (UIIC WR27 - MTA Amend Client.doc) 

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


  

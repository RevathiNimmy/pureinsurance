SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Personal_Client_upd'
GO

CREATE PROCEDURE spe_Party_Personal_Client_upd  
    @party_cnt int,  
    @party_title_code varchar(70),  
    @forename varchar(60),  
    @initials varchar(20),  
    @employment_status_code varchar(70),  
    @employer_cnt int,  
    @employer_business varchar(70),  
    @secondary_employer_business varchar(70),  
    @secondary_employment_status_c varchar(70),  
    @marital_status_code varchar(70),  
    @number_of_children int,  
    @Nationality_id int,  
    @country_of_origin_code varchar(70),  
    @mailshot tinyint,  
    @is_pet_owner tinyint,  
    @accommodation_type_code varchar(70),  
    @salutation varchar(255),  
    @source varchar(255),  
    @tpsind tinyint,  
    @empsind tinyint,  
    @tp_password varchar(255),
    @is_fee_client bit
AS  
BEGIN  
UPDATE Party_Personal_Client  
    SET  
    party_title_code=@party_title_code,  
    forename=@forename,  
    initials=@initials,  
    employment_status_code=@employment_status_code,  
    employer_cnt=@employer_cnt,  
    employer_business=@employer_business,  
    secondary_employer_business=@secondary_employer_business,  
    secondary_employment_status_co=@secondary_employment_status_c,  
    marital_status_code=@marital_status_code,  
    number_of_children=@number_of_children,  
    Nationality_id=@Nationality_id,  
    country_of_origin_code=@country_of_origin_code,  
    mailshot=@mailshot,  
    is_pet_owner=@is_pet_owner,  
    accommodation_type_code=@accommodation_type_code,  
    salutation=@salutation,  
    source=@source,  
    tpsind=@tpsind,  
    empsind=@empsind,  
    tp_password=@tp_password,
    is_fee_client=@is_fee_client
WHERE party_cnt = @party_cnt  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

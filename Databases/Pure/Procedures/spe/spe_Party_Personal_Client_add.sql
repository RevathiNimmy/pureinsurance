SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Personal_Client_add'
GO

CREATE PROCEDURE spe_Party_Personal_Client_add  
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
  
INSERT INTO Party_Personal_Client (  
    party_cnt ,  
    party_title_code ,  
    forename ,  
    initials ,  
    employment_status_code ,  
    employer_cnt ,  
    employer_business ,  
    secondary_employer_business ,  
    secondary_employment_status_co ,  
    marital_status_code ,  
    number_of_children ,  
    Nationality_id ,  
    country_of_origin_code ,  
    mailshot ,  
    is_pet_owner ,  
    accommodation_type_code ,  
    salutation,  
    source,  
    tpsind,  
    empsind,  
    tp_password,
    is_fee_client)  
VALUES (  
    @party_cnt,  
    @party_title_code,  
    @forename,  
    @initials,  
    @employment_status_code,  
    @employer_cnt,  
    @employer_business,  
    @secondary_employer_business,  
    @secondary_employment_status_c,  
    @marital_status_code,  
    @number_of_children,  
    @Nationality_id,  
    @country_of_origin_code,  
    @mailshot,  
    @is_pet_owner,  
    @accommodation_type_code,  
    @salutation,  
    @source,  
    @tpsind,  
    @empsind,  
    @tp_password,
    @is_fee_client)  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

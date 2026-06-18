SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_Party_Personal_Client_sel'
GO

CREATE PROCEDURE spe_Party_Personal_Client_sel  
    @party_cnt int  
AS  
SELECT  
    party_cnt,  
    party_title_code,  
    forename,  
    initials,  
    employment_status_code,  
    employer_cnt,  
    employer_business,  
    secondary_employer_business,  
    secondary_employment_status_co,  
    marital_status_code,  
    number_of_children,  
    Nationality_id,  
    country_of_origin_code,  
    mailshot,  
    is_pet_owner,  
    accommodation_type_code,  
    salutation,  
    source,  
    tpsind,  
    empsind,  
    tp_password,
    is_fee_client
FROM Party_Personal_Client  
WHERE party_cnt = @party_cnt  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_party_personal_client'
GO


CREATE PROCEDURE spu_wp_party_personal_client

    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @party_title_code VARCHAR(70) OUTPUT,
    @forename VARCHAR(60) OUTPUT,
    @initials VARCHAR(20) OUTPUT,
    @surname VARCHAR(60) OUTPUT,
    @employment_status_code VARCHAR(70) OUTPUT,
    @employer_name VARCHAR(100) OUTPUT,
    @employer_business VARCHAR(255) OUTPUT,
    @secondary_employment_status VARCHAR(70) OUTPUT,
    @secondary_employer_business VARCHAR(255) OUTPUT,
    @marital_status_code VARCHAR(70) OUTPUT,
    @number_of_children INT OUTPUT,
    @nationality VARCHAR(255) OUTPUT,
    @mailshot TINYINT OUTPUT,
    @is_pet_owner TINYINT OUTPUT,
    @accommodation_type_code VARCHAR(70) OUTPUT,
    @salutation VARCHAR(255) OUTPUT,
    @trading_name_party VARCHAR(255) OUTPUT,
    @tps TINYINT OUTPUT,
    @emps TINYINT OUTPUT,
    @is_smoker TINYINT OUTPUT
    
AS


SELECT 
    @party_title_code = ppc.party_title_code,
    @forename = ppc.forename,
    @initials = ppc.initials,
    @surname = p2.name,
    @employment_status_code = ppc.employment_status_code,
    @employer_name = p1.resolved_name,
    @employer_business = ppc.employer_business,
    @secondary_employment_status = ppc.secondary_employment_status_co,
    @secondary_employer_business = ppc.secondary_employer_business,
    @marital_status_code = ppc.marital_status_code,
    @number_of_children = ppc.number_of_children,
    @nationality = n.description,
    @mailshot = ppc.mailshot,
    @is_pet_owner = ppc.is_pet_owner,
    @accommodation_type_code = ppc.accommodation_type_code,
    @salutation = ppc.salutation,
    @trading_name_party = p2.trading_name,
    @tps = ppc.tpsind,
    @emps = ppc.empsind,
    @is_smoker = pl.is_smoker
FROM party_personal_client ppc
JOIN party p2
    ON p2.party_cnt = ppc.party_cnt
LEFT JOIN party p1
    ON p1.party_cnt = ppc.employer_cnt
LEFT JOIN nationality n
    ON n.nationality_id = ppc.nationality_id 
LEFT JOIN party_lifestyle pl
    ON pl.party_cnt = p2.party_cnt
WHERE ppc.party_cnt = @PartyCnt

 
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Event_Party_PC_sel'
GO


CREATE PROCEDURE spu_Event_Party_PC_sel
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
    secondary_employment_status_co,
    secondary_employer_business,
    marital_status_code,
    number_of_children,
    Nationality_id,
    country_of_origin_code,
    mailshot,
    is_pet_owner,
    accommodation_type_code
 FROM Event_Party_Personal_Client
WHERE party_cnt = @party_cnt
GO



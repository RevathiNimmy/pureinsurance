SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_copy_party_pc_to_event'
GO


CREATE PROCEDURE spu_copy_party_pc_to_event
    @event_cnt int,
    @party_cnt int
AS


BEGIN
INSERT INTO event_party_personal_client (
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
    accommodation_type_code)
select @event_cnt,
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
from    party_personal_client
where   party_cnt = @party_cnt
END
GO



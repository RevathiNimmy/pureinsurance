set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientsPersonal'
go
create view ClientsPersonal as select
    Party.party_cnt                                         ClientID,
    Party.name                                              LastName,
    Party_Personal_Client.party_title_code                  Title,
    Party_Personal_Client.forename                          FirstNames,
    Party_Personal_Client.initials                          Initials,
    (case Party.is_prospect
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                                IsProspect,
    Party_Lifestyle.occupation_code                         Occupation1,
    Party_Personal_Client.employer_business                 EmployersBusiness1,
    Party_Personal_Client.employment_status_code            EmploymentStatus1,
    Party_Lifestyle.secondary_occupation_code               Occupation2,
    Party_Personal_Client.secondary_employer_business       EmployersBusiness2,
    Party_Personal_Client.secondary_employment_status_co    EmploymentStatus2,
    Party_Lifestyle.date_of_birth                           DateOfBirth,
    Party_Personal_Client.marital_status_code               MaritalStatus,
    Party_Lifestyle.gender_code                             Gender,
    Nationality.code                                        NationalityCode,
    Nationality.description                                 NationalityDescription,
    Party_Personal_Client.accommodation_type_code           AccommodationTypeCode,
    (case Party_Lifestyle.is_smoker
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                                IsSmoker
    from Party
    inner join Party_Lifestyle on Party.party_cnt = Party_Lifestyle.party_cnt
    inner join Party_Personal_Client on Party.party_cnt = Party_Personal_Client.party_cnt
    inner join Party_Type on Party.party_type_id = Party_Type.party_type_id
    left outer join Nationality on Party_Personal_Client.nationality_id = Nationality.nationality_id
    where Party_Type.code = 'PC'
go

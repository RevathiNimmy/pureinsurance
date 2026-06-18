set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientDependants'
go
create view ClientDependants as select
    Party_Lifestyle.party_cnt                   ClientID,
    Party_Lifestyle.name                        Name,
    Party_Lifestyle.date_of_birth               DateOfBirth,
    Lifestyle_Category.code                     CategoryCode,
    Lifestyle_Category.description              CategoryDescription,
    Party_Lifestyle.occupation_code             Occupation1,
    Party_Lifestyle.secondary_occupation_code   Occupation2,
    (case Party_Lifestyle.is_smoker
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                    IsSmoker
    from Party_Lifestyle
    inner join Lifestyle_Category on Party_Lifestyle.category = Lifestyle_Category.lifestyle_category_id
    where Party_Lifestyle.party_lifestyle_id = 2
go

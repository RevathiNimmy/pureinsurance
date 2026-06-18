set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientsGroup'
go
create view ClientsGroup as select
    Party.party_cnt                         ClientID,
    Party.name                              GroupName,
    (select Contact.description
        from Contact
        inner join Contact_Type on Contact.contact_type_id = Contact_Type.contact_type_id
        inner join Party_Contact_Usage on Contact.contact_cnt = Party_Contact_Usage.contact_cnt
        where Party_Contact_Usage.party_cnt = Party.party_cnt
        and Contact_Type.code = 'MAIN')     MainContact,
    (case Party.is_also_agent
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                IsAgent,
    (case Party.is_prospect
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                IsProspect,
    (case Party_Group_Client.is_registered_charity
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                IsRegisteredCharity,
    Party_Group_Client.charity_number       CharityNumber,
    Party_Group_Client.number_of_members    NumberOfMembers,
    Party.credit_card_code                  CreditCardType
    from Party
    inner join Party_Type on Party.party_type_id = Party_Type.party_type_id
    inner join Party_Group_Client on Party.party_cnt = Party_Group_Client.party_cnt
    where Party_Type.code = 'GC'
go

set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientContacts'
go
create view ClientContacts as select
    Party_Contact_Usage.party_cnt   ClientID,
    Contact_Type.code               TypeCode,
    Contact_Type.description        TypeDescription,
    Contact.description             Description,
    Contact.area_code               AreaCode,
    Contact.number                  NumberOrEmailOrWeb,
    Contact.extension               Extension
    from Party_Contact_Usage
    inner join Contact on Party_Contact_Usage.contact_cnt = Contact.contact_cnt
    inner join Contact_Type on Contact.contact_type_id = Contact_Type.contact_type_id
    where Contact_Type.is_contact_type = 1 or Contact_Type.is_correspondence_type = 1
go

set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientAddresses'
go
create view ClientAddresses as select
    Party_Address_Usage.party_cnt   ClientID,
    Address_Usage_Type.code         TypeCode,
    Address_Usage_Type.description  TypeDescription,
    Address.address1                Address1,
    Address.address2                Address2,
    Address.address3                Address3,
    Address.address4                Address4,
    Address.postal_code             PostCode
    from Party_Address_Usage
    inner join Address_Usage_Type on Address_Usage_Type.address_usage_type_id = Party_Address_Usage.address_usage_type_id
    inner join Address on Party_Address_Usage.address_cnt = Address.address_cnt
go

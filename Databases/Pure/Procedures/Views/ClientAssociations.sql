set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientAssociations'
go
create view ClientAssociations as select
    Party_Relationship.party_cnt    ClientID,
    Party.shortname                 AssociatedCode,
    Party.resolved_name             AssociatedName,
    Party_Relationship.description  Relationship
    from Party_relationship
    inner join Party on Party_Relationship.relation_cnt = Party.party_cnt
go

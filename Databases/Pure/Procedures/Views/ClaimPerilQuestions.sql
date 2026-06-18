set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClaimPerilQuestions'
go
create view ClaimPerilQuestions as select
    User_Defined_Peril_Data.claim_id    ClaimID,
    Peril_Data_Definition.caption       Question,
    User_Defined_Peril_Data.value       Answer,
    Peril_Data_Definition.type          Type
    from User_Defined_Peril_Data
    inner join Peril_Data_Definition on User_Defined_Peril_Data.peril_data_defn_id = Peril_Data_Definition.peril_data_defn_id
go

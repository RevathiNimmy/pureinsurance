set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'PolicyWordings'
go
create view PolicyWordings as select
    Policy_Narrative.insurance_file_cnt     PolicyID,
    Narrative_Code.code                     Code,
    Narrative_Code.description              Text
    from Policy_Narrative
    inner join Narrative_Code on Policy_Narrative.Narrative_code_id = Narrative_Code.Narrative_code_id
go

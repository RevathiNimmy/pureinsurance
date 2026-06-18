set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'PolicyCoInsurers'
go
create view PolicyCoInsurers as select
    Policy_Coinsurers.insurance_file_cnt    PolicyID,
    Party.shortname                         Code,
    Party.resolved_name                     Name,
    Policy_Coinsurers.coinsurer_percentage  PercentTaken,
    Policy_Coinsurers.coinsurer_value       Value
    from Policy_Coinsurers
    inner join Party on Policy_Coinsurers.party_cnt = Party.party_cnt
go

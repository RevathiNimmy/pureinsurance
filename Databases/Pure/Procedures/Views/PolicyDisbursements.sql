set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'PolicyDisbursements'
go
create view PolicyDisbursements as select
    Policy_Fee.insurance_file_cnt       PolicyID,
    Party_Type.code                     TypeCode,
    Party_Type.description              TypeDescription,
    Party.shortname                     Code,
    Party.resolved_name                 Name,
    Policy_Fee.fee_percentage           PremiumPercentage,
    Policy_Fee.fee_amount               PremiumAmount,
    Policy_Fee.commission_percentage    CommissionPercentage,
    Policy_Fee.commission_amount        CommissionAmount,
    Policy_Fee.isIPTable                IsPremiumSubjectToIPT
    from Policy_Fee
    inner join Party on Policy_Fee.party_cnt = Party.party_cnt
    inner join Party_Type on Party.party_type_id = Party_Type.party_type_id
go

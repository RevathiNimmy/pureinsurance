set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientProspectPolicies'
go
create view ClientProspectPolicies as select
    Prospect_Policy.party_cnt           ClientID,
    Risk_Group.code                     RiskTypeCode,
    Risk_Group.description              RiskTypeDescription,
    Prospect_Policy.renewal_date        RenewalDate,
    Prospect_Policy.no_of_times_quoted  TimesQuoted,
    Prospect_Policy.target_premium      Premium
    from Prospect_Policy
    inner join Risk_Group on Prospect_Policy.risk_group_id = Risk_Group.risk_group_id
go

set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientsProspect'
go
create view ClientsProspect as select
    Party.party_cnt                 ClientID,
    Party_Prospect.agent_reference  AgentReference,
    CurrentAgent.shortname          CurrentAgentCode,
    CurrentAgent.resolved_name      CurrentAgentName,
    Prospect_Status.code            StatusCode,
    Prospect_Status.description     StatusDescription,
    Strength_Code.code              StrengthCode,
    Strength_Code.description       StrengthDescription,
    PreviousInsurer.shortname       PreviousInsurerCode,
    PreviousInsurer.resolved_name   PreviousInsurerName,
    PreviousBroker.shortname        PreviousBrokerCode,
    PreviousBroker.resolved_name    PreviousBrokerName
    from Party
    inner join Party_Prospect on Party.party_cnt = Party_Prospect.party_cnt
    inner join Party_Type on Party.party_type_id = Party_Type.party_type_id
    left outer join Party as CurrentAgent on Party_Prospect.current_intermediary = CurrentAgent.party_cnt
    left outer join Party as PreviousBroker on Party_Prospect.previous_broker_cnt = PreviousBroker.party_cnt
    left outer join Party as PreviousInsurer on Party_Prospect.previous_insurer_cnt = PreviousInsurer.party_cnt
    left outer join Prospect_Status on Party_Prospect.prospect_status_id = Prospect_Status.prospect_status_id
    left outer join Strength_Code on Party_Prospect.strength_code_id = Strength_Code.strength_code_id
    where Party_Type.code in ('PC', 'GC', 'CC')
    and Party.is_prospect = 1
go

set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'Clients'
go
create view Clients as select
    Party.party_cnt                 ID,
    Party.party_cnt                 OrionAccountRef, -- This is different to Broking 1.6
    Party.shortname                 Code,
    Party.resolved_name             Name,
    Party_Type.code                 TypeCode,
    Party_Type.description          TypeDescription,
    Source.code                     BranchCode,
    Source.description              BranchName,
    Sub_Branch.code                 SubBranchCode,
    Sub_Branch.description          SubBranchName,
    LeadAgent.shortname             LeadAgentCode,
    LeadAgent.resolved_name         LeadAgentName,
    AccountExec.shortname           AccountExecCode,
    AccountExec.resolved_name       AccountExecName,
    Currency.code                   CurrencyCode,
    Currency.description            CurrencyDescription,
    Party.payment_method_code       PaymentMethod,
    Reminder_Type.code              ReminderTypeCode,
    Reminder_Type.description       ReminderTypeDescription,
    Service_Level.code              ServiceLevelCode,
    Service_Level.description       ServiceLevelDescription,
    Party.payment_term_code         TermsOfPayment,
    Renewal_Stop_Code.code          RenewalStopReasonCode,
    Renewal_Stop_Code.description   RenewalStopReasonDescription,
    Seasonal_Gift.code              SeasonalGiftCode,
    Seasonal_Gift.description       SeasonalGiftDescription,
    Area.code                       AreaCode,
    Area.description                AreaDescription,
    Party.file_code                 FileCode,
    Party.CCJs                      CCJs,
    Address.address1                CorrespondenceAddress1,
    Address.address2                CorrespondenceAddress2,
    Address.address3                CorrespondenceAddress3,
    Address.address4                CorrespondenceAddress4,
    Address.postal_code             CorrespondencePostCode,
    Contact_Type.code               PreferredCorrespondenceTypeCode,
    Contact_Type.description        PreferredCorrespondenceTypeDescription
    from Party
    inner join Party_Address_Usage on Party.party_cnt = Party_Address_Usage.party_cnt
    inner join Address on Party_Address_Usage.address_cnt = Address.address_cnt
    inner join Address_Usage_Type on Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id
    inner join Currency on Party.currency_id = Currency.currency_id
    inner join Party_Type on Party.party_type_id = Party_Type.party_type_id
    inner join Source on Party.source_id = Source.source_id
    left outer join Area on Party.area_id = Area.area_id
    left outer join Contact_Type on Party.correspondence_type_id = Contact_Type.contact_type_id
    left outer join Party as AccountExec on Party.consultant_cnt = AccountExec.party_cnt
    left outer join Party as LeadAgent on Party.agent_cnt = LeadAgent.party_cnt
    left outer join Reminder_Type on Party.reminder_type_id = Reminder_Type.reminder_type_id
    left outer join Renewal_Stop_Code on Party.renewal_stop_code_id = Renewal_Stop_Code.renewal_stop_code_id
    left outer join Seasonal_Gift on Party.seasonal_gift_id = Seasonal_Gift.seasonal_gift_id
    left outer join Service_Level on Party.service_level_id = Service_Level.service_level_id
    left outer join Sub_Branch on Party.sub_branch_id = Sub_Branch.sub_branch_id
    where Address_Usage_Type.code = '3131 XCO'
    and Party_Type.code in ('PC', 'CC', 'GC')
    and Party.is_deleted = 0
go

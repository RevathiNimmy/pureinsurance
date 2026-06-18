set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'Policy2Files'
go
create view Policy2Files as select
    Insurance_File.insurance_file_cnt               ID,
    Insurance_File.insurance_folder_cnt             PolicyFolderID,
    Insurance_File.insured_cnt                      InsuredClientID,
    Insurance_File.policy_version                   Version,
    Insurance_File.insurance_ref                    Number,
    Source.code                                     BranchCode,
    Source.description                              BranchName,
    Policy_Type.code                                PolicyTypeCode,
    Policy_Type.description                         PolicyTypeDescription,
    Insurance_File_Type.code                        InsFileTypeCode,
    Insurance_File_Type.description                 InsFileTypeDescription,
    Insurance_File_Status.code                      InsFileStatusCode,
    Insurance_File_Status.description               InsFileStatusDescription,
    Risk_Code.code                                  RiskCode,
    Risk_Code.description                           RiskDescription,
    LeadInsurer.shortname                           LeadInsurerCode,
    LeadInsurer.resolved_name                       LeadInsurerName,
    Insurance_File_System.last_trans_description    Regarding,
    Analysis_Code.code                              BusinessSourceCode,
    Analysis_Code.description                       BusinessSourceDescription,
    Business_Type.code                              BusinessTypeCode,
    Business_Type.description                       BusinessTypeDescription,
    Insurance_File.cover_start_date                 CoverFromDate,
    Insurance_File.expiry_date                      CoverToDate,
    Insurance_File.renewal_date                     RenewalDate,
    Insurance_File.date_issued                      IssuedDate,
    AccountHandler.shortname                        AccountHandlerCode,
    AccountHandler.resolved_name                    AccountHandlerName,
    Insurance_File.payment_method                   PaymentMethod,
    GIS_Scheme.scheme_desc                          SchemeName,
    Insurance_File.this_premium                     PremiumIncludingTax,
    Insurance_File.net_premium                      PremiumExcludingTax,
    Insurance_File.ipt_percentage                   IPTPercent,
    Insurance_File.tax_amount                       IPTAmount,
    Insurance_File.vat_percentage                   VATPercent,
    Insurance_File.vat_amount                       VATAmount,
    (isnull(Insurance_File.this_premium, 0) +
        isnull((select sum(fee_amount)
            from Policy_Fee
            where insurance_file_cnt = Insurance_File.insurance_file_cnt), 0)) TotalPremium,
    Currency.code                                   CurrencyCode,
    Currency.description                            CurrencyDescription,
    Insurance_File.annual_premium                   FuturePremium,
    LeadAgent.shortname                             LeadAgentCode,
    LeadAgent.resolved_name                         LeadAgentName,
    Insurance_File.commission_percentage            CommissionPercent,
    Insurance_File.brokerage_amount                 CommissionCharge,
    Insurance_File.commission_amount                CommissionPayable,
    (case Insurance_File.is_retained_documents
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                        RetainedDocuments,
    Renewal_Frequency.code                          RenewalFrequencyCode,
    Renewal_Frequency.description                   RenewalFrequencyDescription,
    Insurance_File.long_term_undertaking_date       LTUExpiryDate,
    Renewal_Stop_Code.code                          RenewalStopReasonCode,
    Renewal_Stop_Code.description                   RenewalStopReasonDescription,
    Renewal_Method.code                             RenewalMethodCode,
    Renewal_Method.description                      RenewalMethodDescription,
    Lapsed_Reason.code                              LapsedReasonCode,
    Lapsed_Reason.description                       LapsedReasonDescription,
    Insurance_File.lapsed_date                      LapsedDate,
    (case Insurance_File.is_referred_at_renewal
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                        ReferredAtRenewal,
    (case Insurance_File.is_referred_on_mta
        when 1 then 'Yes'
        when 0 then 'No'
        else null
        end)                                        ReferredOnMTA
    from Insurance_File
    inner join Business_Type on Insurance_File.business_type_id = Business_Type.business_type_id
    inner join Currency on Insurance_File.currency_id = Currency.currency_id
    inner join Party as LeadInsurer on Insurance_File.lead_insurer_cnt = LeadInsurer.party_cnt
    inner join Policy_Type on Insurance_File.policy_type_id = Policy_Type.policy_type_id
    inner join Insurance_File_Type on Insurance_File.insurance_file_type_id = Insurance_File_Type.insurance_file_type_id
    inner join Risk_Code on Insurance_File.risk_code_id = Risk_Code.risk_code_id
    left outer join Analysis_Code on Insurance_File.Analysis_code_id = Analysis_Code.Analysis_code_id
    left outer join GIS_Policy_Link on Insurance_File.insurance_file_cnt = GIS_Policy_Link.insurance_file_cnt
    left outer join GIS_Scheme on GIS_Policy_Link.GIS_scheme_id = GIS_Scheme.GIS_scheme_id
    left outer join Insurance_File_Status on Insurance_File.insurance_file_status_id = Insurance_File_Status.insurance_file_status_id
    left outer join Insurance_File_System on Insurance_File.insurance_file_cnt = Insurance_File_System.insurance_file_cnt
    left outer join Lapsed_Reason on Insurance_File.lapsed_reason_id = Lapsed_Reason.lapsed_reason_id
    left outer join Party as AccountHandler on Insurance_File.account_handler_cnt = AccountHandler.party_cnt
    left outer join Party as LeadAgent on Insurance_File.lead_agent_cnt = LeadAgent.party_cnt
    left outer join Renewal_Frequency on Insurance_File.renewal_frequency_id = Renewal_Frequency.renewal_frequency_id
    left outer join Renewal_Method on Insurance_File.renewal_method_id = Renewal_Method.renewal_method_id
    left outer join Renewal_Stop_Code on Insurance_File.renewal_stop_code_id = Renewal_Stop_Code.renewal_stop_code_id
    left outer join Source on Insurance_File.source_id = Source.source_id
    where isnull(Insurance_File.policy_ignore, 0) = 0
go

set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'Policy2CurrentFiles'
go
-- needs Policy2Folders, Policy2Files
create view Policy2CurrentFiles as select
    Policy2Folders.OwnerClientID,
    Policy2Files.InsuredClientID,
    Policy2Folders.ID as PolicyFolderID,
    Policy2Files.ID as PolicyFileID,
    Policy2Files.Number,
    Policy2Files.BranchCode,
    Policy2Files.BranchName,
    Policy2Files.PolicyTypeCode,
    Policy2Files.PolicyTypeDescription,
    Policy2Files.InsFileTypeCode,
    Policy2Files.InsFileTypeDescription,
    Policy2Files.InsFileStatusCode,
    Policy2Files.InsFileStatusDescription,
    Policy2Files.RiskCode,
    Policy2Files.RiskDescription,
    Policy2Files.LeadInsurerCode,
    Policy2Files.LeadInsurerName,
    Policy2Files.Regarding,
    Policy2Files.BusinessSourceCode,
    Policy2Files.BusinessSourceDescription,
    Policy2Files.BusinessTypeCode,
    Policy2Files.BusinessTypeDescription,
    Policy2Files.CoverFromDate,
    Policy2Files.CoverToDate,
    Policy2Files.RenewalDate,
    Policy2Files.IssuedDate,
    Policy2Files.AccountHandlerCode,
    Policy2Files.AccountHandlerName,
    Policy2Files.PaymentMethod,
    Policy2Files.SchemeName,
    Policy2Files.PremiumIncludingTax,
    Policy2Files.PremiumExcludingTax,
    Policy2Files.IPTPercent,
    Policy2Files.IPTAmount,
    Policy2Files.VATPercent,
    Policy2Files.VATAmount,
    Policy2Files.TotalPremium,
    Policy2Files.CurrencyCode,
    Policy2Files.CurrencyDescription,
    Policy2Files.FuturePremium,
    Policy2Files.LeadAgentCode,
    Policy2Files.LeadAgentName,
    Policy2Files.CommissionPercent,
    Policy2Files.CommissionCharge,
    Policy2Files.CommissionPayable,
    Policy2Files.RetainedDocuments,
    Policy2Files.RenewalFrequencyCode,
    Policy2Files.RenewalFrequencyDescription,
    Policy2Files.LTUExpiryDate,
    Policy2Files.RenewalStopReasonCode,
    Policy2Files.RenewalStopReasonDescription,
    Policy2Files.RenewalMethodCode,
    Policy2Files.RenewalMethodDescription,
    Policy2Files.LapsedReasonCode,
    Policy2Files.LapsedReasonDescription,
    Policy2Files.LapsedDate,
    Policy2Files.ReferredAtRenewal,
    Policy2Files.ReferredOnMTA,
    Policy2Folders.InceptionDate,
    Policy2Folders.TimesRenewed
    from Policy2Folders
    inner join Policy2Files on Policy2Folders.ID = Policy2Files.PolicyFolderID
    and Policy2Files.Version = (
        select max(policy_version)
        from Insurance_File
        inner join Insurance_File_Type on Insurance_File.insurance_file_type_id = Insurance_File_Type.insurance_file_type_id
        where insurance_folder_cnt = Policy2Folders.ID
        and insurance_file_status_id is null
        and Insurance_File_Type.code in ('POLICY', 'MTA PERM', 'MTA TEMP')
    )
go

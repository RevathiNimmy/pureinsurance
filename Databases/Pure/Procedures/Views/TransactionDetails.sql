set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'TransactionDetails'
go
-- NON-GENERIC REPORT FOR SPECIFIC CUSTOMER. DO NOT USE THIS AS A BASIS
-- FOR LINKING OR CREATING NEW VIEWS.
/* ECK 150802 Modify Gross Values so as to exclude tax */
/* ECK 030902 As tax amount is always positive dedult when doing a credit*/
create view TransactionDetails as select distinct
    Consultant.shortname                        AccountExecCode,
    Consultant.resolved_name                    AccountExec,
    AccountHandler.shortname                    AccountHandlerCode,
    AccountHandler.resolved_name                AccountHandler,
    LeadInsurer.shortname                       LeadInsurerCode,
    LeadInsurer.resolved_name                   LeadInsurer,
    TransDetail.insurance_ref                   PolicyReference,
    Party.party_cnt                             PartyID,
    Party.shortname                             ClientCode,
    Account.company_id                          CompanyID,
    Account.sub_branch_id                       SubBranchID,
    Account.account_id                          AccountID,
    Account.Account_Name                        AccountName,
    Account.short_code                          AccountShortCode,
    Document.document_ref                       TransactionReference,
    Document.documenttype_id                    DocumentTypeID,
    (case
        when documenttype_id in (2,4,7,15,17,31,33,35) then
            1
        when documenttype_id in (3,5,6,16,18,32,34,36) then
            2
        else
            null
        end)                                    DocumentType,
    Document.document_date                      InvoiceDate,
    TransDetail.ref_date                        CoverDate,
    TransDetail.comment                         TransactionType,
    Risk_Code.code                              RiskTypeCode,
    Risk_Code.description                       RiskTypeDescription,
    Broker.shortname                            Brokerage,

    -- Sum up all TD records for a specific Document (Invoice)
    --eck150802
    --eck030902
    (case when (
        select sum(amount)
        from TransDetail as TD
        where spare = 'GROSS'
        and TD.document_id = TransDetail.document_id
    ) < 0 then (
        select sum(amount) + sum(ref_amount)
        from TransDetail as TD
        where spare = 'GROSS'
        and TD.document_id = TransDetail.document_id
    ) else (
        select sum(amount) - sum(ref_amount)
        from TransDetail as TD
        where spare = 'GROSS'
        and TD.document_id = TransDetail.document_id
    ) end)                                                  GROSS,

    (select sum(amount)
        from TransDetail as TD
        where spare IN ('BROK', 'BROK ADJ')
        and TD.document_id = TransDetail.document_id)       BKG,
    (select sum(amount)
        from TransDetail as TD
        where spare IN ('AGENT', 'AGENT ADJ')
        and TD.document_id = TransDetail.document_id)       AGENT,
    (select sum(amount)
        from TransDetail as TD
        where spare IN ('COMM', 'COMM ADJ')
        and TD.document_id = TransDetail.document_id)       Commission,
    (select sum(amount)
        from TransDetail as TD
        where spare = ''
        and TD.document_id = TransDetail.document_id)       CLIENT,
    (select sum(amount)
        from TransDetail as TD
        where spare not in ('COMM', 'COMM ADJ', 'BROK', 'BROK ADJ', 'GROSS', 'AGENT', 'AGENT ADJ', '')
        and TD.document_id = TransDetail.document_id)       OTHER,
    (select sum(Base_Match_Amount)
        from TransMatch
        inner join TransDetail as TD on TransMatch.transdetail_id = TD.transdetail_id
        where TransMatch.base_match_amount <> TD.amount
        and account_id = Account.account_id
        and TD.document_id = TransDetail.document_id
        and TransMatch.allocationdetail_id is not null)     Unallocated,
    PSP.split_percentage

    from ((((((((((((TransDetail
    inner join Document on TransDetail.document_id = Document.document_id)
    right outer join Account on TransDetail.account_id = Account.account_id)
    inner join Party on Account.short_code = Party.shortname)
    inner join Insurance_File as InsFile on TransDetail.insurance_ref = InsFile.Insurance_ref )
    left outer join Policy_Shared_Premiums as PSP on InsFile.Insurance_file_cnt = PSP.Insurance_file_cnt and Party.Party_cnt = PSP.Party_cnt)
    inner join Risk_Code on InsFile.risk_code_id = Risk_Code.risk_code_id)
    inner join Risk_Group on risk_Code.Risk_Group_ID = risk_group.risk_group_id)
    inner join Risk_By_Source on Risk_Group.Risk_Group_id = Risk_By_Source.risk_group_id)
    inner join Party as Broker on Risk_By_Source.Commission_cnt = Broker.Party_cnt)
    left outer join Party as Consultant on Party.consultant_cnt = Consultant.party_cnt)
    left outer join Party as AccountHandler on InsFile.Account_Handler_cnt = AccountHandler.Party_cnt)
    left outer join Party as LeadInsurer on InsFile.Lead_Insurer_cnt = LeadInsurer.Party_cnt)

    where InsFile.policy_version = (
        select max(policy_version)
        from Insurance_File as InsFileVersions
        where InsFileVersions.Insurance_folder_cnt = InsFile.Insurance_Folder_cnt
    )
    and Account.ledger_id = 2
    and PSP.Split_Percentage is null

    group by
        TransDetail.document_id,
        Document.document_ref,
        Document.document_date,
        TransDetail.ref_Date,
        Account.Account_Name,
        Account.account_id,
        Account.short_code,
        Consultant.shortname,
        TransDetail.Insurance_ref,
        AccountHandler.shortname ,
        LeadInsurer.shortname,
        Party.shortname,
        TransDetail.comment,
        Risk_Code.code,
        Risk_Code.description,
        Broker.Shortname,
        Account.company_id,
        Account.sub_branch_id,
        Party.party_cnt,
        Document.documenttype_id,
        AccountHandler.resolved_name,
        Consultant.resolved_name,
        LeadInsurer.resolved_name,
        PSP.Split_percentage
go

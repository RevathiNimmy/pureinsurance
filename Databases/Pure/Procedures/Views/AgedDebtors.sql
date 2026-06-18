set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'AgedDebtors'
go
-- NON-GENERIC REPORT FOR SPECIFIC CUSTOMER. DO NOT USE THIS AS A BASIS
-- FOR LINKING OR CREATING NEW VIEWS.
--eck 130802 changed calculation of unallocated and added document type
create view AgedDebtors as select
    Account.account_id          AccountID,
    Account.account_name        AccountName,
    Account.short_code          AccountShortCode,
    AccExec.shortname           AccountExecShortName,
    Party.party_type_id         PartyType,
    Document.document_date      InvDate,
    Document.documenttype_id    DocType,        --eck130802
    TransDetail.amount          InvAmount,
    isnull((
        select sum(base_match_amount)          --ECK130802 New Field
        from TransMatch
        where TransMatch.transdetail_id = TransDetail.transdetail_id
        and TransMatch.allocationdetail_id is not null
    ), 0)                       MatchedAmount,
    TransDetail.ref_date        EffectiveDate,
    (
        select sum(amount)
        from TransDetail
        where account_id = Account.account_id
    )                           AccountBalance,
    -- eck130802 Replaced calculation
    --  (select sum(Base_Match_Amount)
    --              from TransMatch
    --              where TransMatch.transdetail_id = TransDetail.transdetail_id
    --      and TransMatch.base_match_amount <> TransDetail.amount
    --          and TransMatch.allocationdetail_id is not null
    --      )                           Unallocated
    isnull((
        select sum(td.amount) -- sum of cash reciepts & Payments
        from TransDetail td,document d
        where td.account_id = Account.account_id
        and td.document_id = d.document_id
        and d.Documenttype_id in (22,23,28,29)
    ), 0) +
    isnull((
        select sum(tm.base_match_amount) -- sum of Allocated Debits & Credits
        from Transmatch tm,transdetail td,document d
        where td.account_id = Account.account_id
        and tm.transdetail_id = td.transdetail_id
        and td.document_id = d.document_id
        and tm.allocationdetail_id is not null
        and d.Documenttype_id not in (22,23,28,29)
    ), 0)                       Unallocated,
    -- eck130802 End
    Account.company_id          CompanyID,
    Account.sub_branch_id       SubBranchID
    from Account
    inner join TransDetail on Account.account_id = TransDetail.account_id
    inner join Document on TransDetail.document_id = Document.document_id
    inner join Party on Party.shortname = Account.short_code
    left outer join Party as AccExec on Party.consultant_cnt = AccExec.party_cnt
    group by
        Account.account_id,
        Account.account_name,
        Account.short_code,
        AccExec.shortname,
        Party.party_type_id,
        TransDetail.ref_date,
        TransDetail.amount,
        Document.document_date,
        Document.documenttype_id,       --eck130802
        TransDetail.transdetail_id,
        Account.company_id,    --TF030603
        Account.sub_branch_id  --TF030603
go

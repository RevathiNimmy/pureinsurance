set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'AccountsBalanceStatement'
go
create view AccountsBalanceStatement as select
    max(Account.company_id)                         CompanyID,
    max(Account.sub_branch_id)                      SubBranchID,
    Transaction_Export_Folder.insurance_holder_cnt  ClientID,
    max(Account.account_id)                         AccountID,
    TransDetail.transdetail_id                      TransDetailID,
    max(Document.document_date)                     InvoiceDate,
    max(TransDetail.ref_date)                       CoverDate,
    max(Document.document_ref)                      TransactionReference,
    max(TransDetail.comment)                        TransactionType,
    max(TransDetail.insurance_ref)                  PolicyReference,
    max(Risk_Code.code)                             RiskTypeCode,
    max(Risk_Code.description)                      RiskTypeDescription,
    max(TransDetail.Amount)                         Amount,
    sum(TransMatch.base_match_amount)               BaseMatchAmount
    -- Orion Tables
    from Account
    inner join Ledger on Account.ledger_id = Ledger.ledger_id
    inner join TransDetail on Account.account_id = TransDetail.account_id
    inner join Document on TransDetail.document_id = Document.document_id
    left outer join TransMatch
        inner join MatchGroup on TransMatch.match_id = MatchGroup.match_id
        on TransDetail.transdetail_id = TransMatch.transdetail_id
        and TransMatch.base_match_amount <> 0
    -- Broking Tables
    left outer join Transaction_Export_Folder
        on Document.document_ref = Transaction_Export_Folder.document_ref
        and Document.company_id = Transaction_Export_Folder.source_id
    left outer join Insurance_File
        on Transaction_Export_Folder.insurance_file_cnt = Insurance_File.insurance_file_cnt
        and Transaction_Export_Folder.accounts_export_status = 'c'
    left outer join Risk_Code
        on Insurance_File.risk_code_id = Risk_Code.risk_code_id
    -- Grouping
    group by TransDetail.transdetail_id, Transaction_Export_Folder.insurance_holder_cnt
go

set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'AccountTransactions'
go
-- NON-GENERIC REPORT FOR SPECIFIC CUSTOMER. DO NOT USE THIS AS A BASIS
-- FOR LINKING OR CREATING NEW VIEWS.
create view AccountTransactions as select
    Account.account_id,
    Account.account_key,
    TransDetail.amount              TransactionAmount,
    Document.document_ref           TransRef,
    TransDetail.accounting_date     AccountingDate,
    Account.company_id              CompanyID,
    Account.sub_branch_id           SubBranchID
    from Account
    inner join TransDetail on account.account_id = TransDetail.account_id
    inner join Document on TransDetail.document_id = document.document_id
go

set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'PolicyTransactions'
go
-- NON-GENERIC REPORT FOR SPECIFIC CUSTOMER. DO NOT USE THIS AS A BASIS
-- FOR LINKING OR CREATING NEW VIEWS.
create view PolicyTransactions as select distinct
    Party.party_cnt,
    Party.name,
    Party_Address_Usage.address_usage_type_id,
    Address.address1,
    Address.address2,
    Address.address3,
    Address.address4,
    Address.postal_code,
    PolicyFiles.AccountHandlerName,
    PolicyFiles.LeadInsurerName,
    PolicyFiles.RiskCode,
    PolicyFiles.LeadAgentName,
    AccountsBalanceStatement.AccountID,
    AccountsBalanceStatement.TransDetailID,
    AccountsBalanceStatement.TransactionReference,
    AccountsBalanceStatement.Amount
    from Party
    inner join Party_Address_Usage on Party.party_cnt = Party_Address_Usage.party_cnt
    inner join Address on Party_Address_Usage.address_cnt = Address.address_cnt
    inner join PolicyFolders on Party.party_cnt = PolicyFolders.ClientID
    inner join PolicyFiles on PolicyFolders.ID = PolicyFiles.PolicyFolderID
    right outer join AccountsBalanceStatement
        on Party.party_cnt = AccountsBalanceStatement.ClientID
        and AccountsBalanceStatement.PolicyReference = PolicyFiles.Number
go

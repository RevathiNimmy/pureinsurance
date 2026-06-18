set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientsCorporate'
go
create view ClientsCorporate as select
    Party.party_cnt                             ClientID,
    Party.name                                  TradingName,
    (select Contact.description
        from Contact
        inner join Contact_Type on Contact.contact_type_id = Contact_Type.contact_type_id
        inner join Party_Contact_Usage on Contact.contact_cnt = Party_Contact_Usage.contact_cnt
        where Party_Contact_Usage.party_cnt = Party.party_cnt
        and Contact_Type.code = 'MAIN')         MainContact,
    Party_Corporate_Client.party_business_id    Business,
    Party_Corporate_Client.trade_code           Trade,
    SIC_Code.code                               SICCode,
    SIC_Code.description                        SICDescription,
    Party_Corporate_Client.company_reg          CompanyReg,
    Party_Corporate_Client.no_of_offices        NumberOfOffices,
    Party_Corporate_Client.no_of_employees      NumberOfEmployees,
    Party_Corporate_Client.trading_since_date   TradingSince,
    Party_Corporate_Client.wage_roll            WageRoll,
    Party.tax_number				            VATCode,
    Party_Corporate_Client.turnover             Turnover,
    Party_Corporate_Client.financial_year       FinancialYear
    from Party
    inner join Party_Type on Party.party_type_id = Party_Type.party_type_id
    inner join Party_Corporate_Client on Party.party_cnt = Party_Corporate_Client.party_cnt
    left outer join SIC_Code on Party_Corporate_Client.SIC_code_id = SIC_Code.SIC_code_id
    where Party_Type.code = 'CC'
go

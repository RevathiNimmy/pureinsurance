set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'qryPolicyAddOns'
go

create view qryPolicyAddOns as

Select 
    ifi.insurance_ref AS policy_ref, 
    p.shortname AS client_code, 
    pf.fee_percentage, 
    pf.fee_amount, 
    pf.commission_amount, 
    pf.isIPTable, 
    pf.extra_scheme_id, 
    es.code,
    es.description AS scheme, 
    ft.description AS type_of_sale
From policy_fee pf
Left Join extra_scheme es 
    on pf.extra_scheme_id = es.extra_scheme_id
Join insurance_file ifi 
    on pf.insurance_file_cnt = ifi.insurance_file_cnt
Join party p 
    on pf.party_cnt = p.party_cnt
Join FSA_Type_Of_Sale ft 
    on pf.Fsa_Type_Of_Sale_Id = ft.Fsa_Type_Of_Sale_Id

go
set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClaimRiskQuestions'
go
create view ClaimRiskQuestions as select
    Claim_User_Defined_Risk_Data.claim_id   ClaimID,
    Risk_Data_Definition.caption            Question,
    Claim_User_Defined_Risk_Data.value      Answer,
    Risk_Data_Definition.type               Type
    from Claim_User_Defined_Risk_Data
    inner join Risk_Data_Definition on Claim_User_Defined_Risk_Data.risk_data_defn_id = Risk_Data_Definition.risk_data_defn_id
go

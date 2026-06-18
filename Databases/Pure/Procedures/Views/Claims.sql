set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'Claims'
go
create view Claims as select
    Claim.claim_id                  ID,
    Claim.policy_id                 PolicyID,
    Claim.Policy_Number             PolicyNumber,
    Handler.code                    HandlerCode,
    Handler.description             HandlerDescription,
    progress_status.code            ProgressStatusCode,
    progress_status.description     ProgressStatusDescription,
    (
        case Claim.claim_status_id
        when 1 then 'Provisional Open Claim'
        when 2 then 'Live Open Claim'
        when 3 then 'Closed'
        when 4 then 'Re-Opened'
        when 5 then 'Re-Closed'
        else null
        end
    ) as ClaimStatus,
    (
        case Claim.claim_status_id
        when 1 then 'Open'
        when 2 then 'Open'
        when 3 then 'Closed'
        when 4 then 'Open'
        when 5 then 'Closed'
        else null
        end
    ) as ClaimOpen,
    
    --PN 40525
    convert(datetime, convert(varchar(23),   Claim.Claims_Status_Date, 111))  ClaimStatusDate,
    convert(datetime, convert(varchar(23),   Claim.Claims_Status_Date, 114))  ClaimStatusTime,
    Claim.description               Description,
    primary_cause.code              PrimaryCauseCode,
    primary_cause.description       PrimaryCauseDescription,
    secondary_cause.code            SecondaryCauseCode,
    secondary_cause.description     SecondaryCauseDescription,
    Catastrophe_Code.code           CatastropheCode,
    Catastrophe_Code.description    CatastropheDescription,
    Town.code                       TownCode,
    Town.description                TownDescription,
    convert(datetime, convert(varchar(23), Claim.loss_from_date, 111))  LossDate,
    convert(datetime, convert(varchar(23), Claim.loss_from_date, 114))  LossTime,
    convert(datetime, convert(varchar(23), Claim.loss_to_date, 111))    LossToDate,
    convert(datetime, convert(varchar(23), Claim.loss_to_date, 114))    LossToTime,
    convert(datetime, convert(varchar(23), Claim.reported_date, 111))   ReportedDate,
    convert(datetime, convert(varchar(23), Claim.reported_date, 114))   ReportedTime,
    Claim.reported_to_date          ReportedToDate,
    convert(datetime, convert(varchar(23),   Claim.last_modified_date, 111))   LastModifiedDate,
    convert(datetime, convert(varchar(23),   Claim.last_modified_date, 114))   LastModifiedTime,
    Currency.code                   CurrencyCode,
    Currency.description            CurrencyDescription,
    Risk_Code.code                  RiskTypeCode,
    Risk_Code.description           RiskTypeDescription,
    Risk_Code.risk_group_id         RiskGroupID,
    Claim.info_only                 Information,
    Claim.likely_claim              LikelyToClaim,
    Claim.claim_number              ClientClaimNumber,
    Claim.Client_id                 ClientID,
    Claim.client_name               ClientName,
    Claim.Client_short_name         ClientShortName,
    ClientAddress.address1          ClientAddress1,
    ClientAddress.address2          ClientAddress2,
    ClientAddress.address3          ClientAddress3,
    ClientAddress.address4          ClientAddress4,
    ClientAddress.postal_code       ClientPostCode,
    Claim.client_tel_no             ClientHomeTelephone,
    Claim.client_tel_no_off         ClientOfficeTelephone,
    Claim.client_fax_no             ClientFax,
    Claim.client_mobile_no          ClientMobile,
    Claim.client_email              ClientEmail,
    Claim.insurer_claim_number      InsurerClaimNumber,
    Claim.insurer_name              InsurerName,
    Claim.Insurer_short_name        InsurerShortName,
    InsurerAddress.address1         InsurerAddress1,
    InsurerAddress.address2         InsurerAddress2,
    InsurerAddress.address3         InsurerAddress3,
    InsurerAddress.address4         InsurerAddress4,
    InsurerAddress.postal_code      InsurerPostCode,
    Claim.insurer_tel_no            InsurerTelephone,
    Claim.insurer_fax_no            InsurerFax,
    Claim.insurer_contact           InsurerContact,
    Claim.insurer_email             InsurerEmail,
    Claim.VAT_registered            VATRegistered,
    Claim.VAT_reg_no                VATRegistrationNumber,
    Claim.user_defined_field_A      UserDefinedFieldA,
    Claim.user_defined_field_B      UserDefinedFieldB,
    Claim.user_defined_field_C      UserDefinedFieldC,
    Claim.user_defined_field_D      UserDefinedFieldD,
    Claim.user_defined_field_E      UserDefinedFieldE,
    Claim.Location                  Location,
    Claim_Peril.description         PerilDescription,
    Reserve.Initial_Reserve         InitialReserve,
    Reserve.Revised_Reserve         RevisedReserve,
    Reserve.Paid_to_Date            PaidToDate,
    Reserve_Type.Description        ReserveType,
    (
        case when isnull(Reserve.revised_reserve_entered, 0) = 0 then (
            case when isnull(Reserve.revised_reserve, convert(money, 0)) = convert(money, 0) then (
                isnull(Reserve.initial_reserve, convert(money, 0)) - isnull(Reserve.paid_to_date, convert(money, 0))
            ) else (
                isnull(Reserve.revised_reserve, convert(money, 0)) - isnull(Reserve.paid_to_date, convert(money, 0))
            ) end
        ) else (
            isnull(Reserve.revised_reserve, convert(money, 0)) - isnull(Reserve.paid_to_date, convert(money, 0))
        ) end
    ) as CurrentReserve,
    /* USE FOR BROKING DATA TRANSFERS
    (
        select max(Party_Claim.name)
        from Party_Claim
        inner join Claim_Party_Type on Party_Claim.claim_party_type_id = Claim_Party_Type.claim_party_type_id
        inner join Peril_Party on Party_Claim.party_claim_id = Peril_Party.party_claim_id
        where Claim_Party_Type.code = 'DRV'
        and Peril_Party.claim_id = Claim.claim_id
    ) as DriverName
    */
    /* USE BEFORE VERSION 1.6.11
    (
        select max(Party_Claim.name)
        from Party_Claim
        inner join Claim_Party_Type on Party_Claim.claim_party_type_id = Claim_Party_Type.claim_party_type_id
        inner join Claim_Party_Claim on Party_Claim.party_claim_id = Claim_Party_Claim.party_claim_id
        where Claim_Party_Type.code = 'OTDRIVER'
        and Claim_Party_Claim.claim_id = Claim.claim_id
    ) as DriverName
    */
    /* DO NOT USE UNTIL VERSION 1.6.11*/
    (
        select TOP 1 Party.resolved_name
        from Party
        inner join Party_Type on Party.party_type_id = Party_Type.party_type_id
        inner join Claim_Party_Link on Party.party_cnt = Claim_Party_Link.party_cnt
        where Party_Type.code = 'OTDRIVER'
        and Claim_Party_Link.claim_id = Claim.claim_id
    ) as DriverName,
    (
	SELECT TOP 1 Party_Other.gender
	FROM Party  
	JOIN Party_Type ON Party.party_type_id = Party_Type.party_type_id  
	JOIN Party_Other ON Party.party_cnt = Party_Other.party_cnt  
	JOIN Claim_Party_Link ON Party.party_cnt = Claim_Party_Link.party_cnt  
	WHERE Party_Type.code = 'OTDRIVER'
	AND Claim_Party_Link.claim_id = Claim.claim_id
    ) AS DriverSex

from Claim
-- To Parents
left outer join Catastrophe_Code on Claim.catastrophe_code_id = Catastrophe_Code.catastrophe_code_id
left outer join Claim_Address as ClientAddress on Claim.client_address = ClientAddress.address_cnt
left outer join Claim_Address as InsurerAddress on Claim.insurer_address = InsurerAddress.address_cnt
left outer join Currency on Claim.currency_id = Currency.currency_id
left outer join Handler on Claim.handler_id = Handler.handler_id
left outer join primary_cause on Claim.primary_cause_id = primary_cause.primary_cause_id
left outer join progress_status on Claim.progress_status_id = progress_status.progress_status_id
left outer join Risk_Code on Claim.risk_type_id = Risk_Code.risk_code_id
left outer join secondary_cause on Claim.secondary_cause_id = secondary_cause.secondary_cause_id
left outer join Town on Claim.town = Town.town_id
-- To Children
left outer join Claim_Peril on Claim_Peril.claim_id = Claim.claim_id
left outer join Reserve on Reserve.claim_peril_id = Claim_Peril.claim_peril_id
left outer join Reserve_Type on Reserve.reserve_type_id = Reserve_Type.reserve_type_id

go

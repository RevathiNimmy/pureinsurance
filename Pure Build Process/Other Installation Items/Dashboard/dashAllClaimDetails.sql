DDLDropview 'dashAllClaimDetails'
go
Create view dashAllClaimDetails as 
select
	Claim.claim_id						as ClaimId,
	Claim_Peril.claim_peril_id,
	Claim.Policy_Id						as PolicyId,
	Claim.Policy_Number					as PolicyNumber,
	Claim.Policy_Number					as insurance_ref,
	Handler.code						as HandlerCode,
	Handler.description					as HandlerDescription,
	progress_status.code					as ProgressStatusCode,
	progress_status.description				as ProgressStatusDescription,
	(
		case Claim.claim_status_id
		when 1 then 'Provisional Open Claim'
		when 2 then 'Live Open Claim'
		when 3 then 'Closed'
		when 4 then 'Re-Opened'
		when 5 then 'Re-Closed'
		else null
		end
	)							as ClaimStatus,
	(
		case Claim.claim_status_id
		when 1 then 'Open'
		when 2 then 'Open'
		when 3 then 'Closed'
		when 4 then 'Open'
		when 5 then 'Closed'
		else null
		end
	)							as ClaimOpen,
	convert(datetime, convert(varchar(23), 
	   Claim.Claims_Status_Date, 111))			as ClaimStatusDate,
	Claim.description					as Description,
	primary_cause.code					as PrimaryCauseCode,
	primary_cause.description				as PrimaryCauseDescription,
	secondary_cause.code					as SecondaryCauseCode,
	secondary_cause.description				as SecondaryCauseDescription,
	Catastrophe_Code.code					as CatastropheCode,
	Catastrophe_Code.description				as CatastropheDescription,
	Town.code						as TownCode,
	Town.description					as TownDescription,
	convert(datetime, convert(varchar(23), 
	   Claim.loss_from_date, 111))				as LossDate,
	Year(convert(datetime, convert(varchar(23), 
	   Claim.loss_from_date, 111)))				as LossDateYear,
	month(convert(datetime, convert(varchar(23), 
	   Claim.loss_from_date, 111)))				as LossDateMonth,
	day(convert(datetime, convert(varchar(23), 
	   Claim.loss_from_date, 111)))				as LossDateDay,
	convert(datetime, convert(varchar(23),
	   Claim.loss_from_date, 114))				as LossTime,
	convert(datetime, convert(varchar(23), 
		Claim.loss_to_date, 111))			as LossToDate,
	convert(datetime, convert(varchar(23), 
		Claim.loss_to_date, 114))			as LossToTime,
	convert(datetime, convert(varchar(23), 
		Claim.reported_date, 111))			as ReportedDate,
	convert(datetime, convert(varchar(23), 
		Claim.reported_date, 114))			as ReportedTime,
	convert(datetime, convert(varchar(23), 
	   Claim.reported_to_date, 111))			as ReportedToDate,
	convert(datetime, convert(varchar(23), 
	   Claim.last_modified_date, 111))			as LastModifiedDate,
	Currency.code						as CurrencyCode,
	Currency.description					as CurrencyDescription,
	Risk_Code.code						as RiskTypeCode,
	Risk_Code.description					as RiskTypeDescription,
	Risk_Code.risk_group_id					as RiskGroupID,
	Claim.info_only						as Information,
	Claim.likely_claim					as LikelyToClaim,
	Claim.claim_number					as ClientClaimNumber,	
	Claim.client_name					as ClientName,
	Claim.Client_short_name					as ClientShortName,
	ClientAddress.address1					as ClientAddress1,
	ClientAddress.address2					as ClientAddress2,
	ClientAddress.address3					as ClientAddress3,
	ClientAddress.address4					as ClientAddress4,
	ClientAddress.postal_code				as ClientPostCode,
	Claim.client_tel_no					as ClientHomeTelephone,
	Claim.client_tel_no_off					as ClientOfficeTelephone,
	Claim.client_fax_no					as ClientFax,
	Claim.client_mobile_no					as ClientMobile,
	Claim.client_email					as ClientEmail,
	Claim.insurer_claim_number				as InsurerClaimNumber,
	Claim.insurer_name					as InsurerName,
	Claim.Insurer_short_name				as InsurerShortName,
	InsurerAddress.address1					as InsurerAddress1,
	InsurerAddress.address2					as InsurerAddress2,
	InsurerAddress.address3					as InsurerAddress3,
	InsurerAddress.address4					as InsurerAddress4,
	InsurerAddress.postal_code				as InsurerPostCode,
	Claim.insurer_tel_no					as InsurerTelephone,
	Claim.insurer_fax_no					as InsurerFax,
	Claim.insurer_contact					as InsurerContact,
	Claim.insurer_email					as InsurerEmail,
	case Claim.VAT_registered	
		when 0 then 'No'
		else  'Yes' end					as VATRegistered,
	Claim.VAT_reg_no					as VATRegistrationNumber,
	Claim.user_defined_field_A				as UserDefinedFieldA,
	Claim.user_defined_field_B				as UserDefinedFieldB,
	Claim.user_defined_field_C				as UserDefinedFieldC,
	Claim.user_defined_field_D				as UserDefinedFieldD,
	Claim.user_defined_field_E				as UserDefinedFieldE,
	Claim.Location						as Location,
	Peril_Type.description					as PerilDescription,
	Reserve.Initial_Reserve					as InitialReserve,
	Reserve.Revised_Reserve					as RevisedReserve,
	Reserve.Paid_to_Date					as PaidToDate,
	
	Reserve_Type.name				as ReserveTypeCode,
	Reserve_Type.Description		as ReserveType,
	
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
	)								as CurrentReserve,
	
	(
		select max(Party.resolved_name)
		from Party
		inner join Party_Type on Party.party_type_id = Party_Type.party_type_id
		inner join Claim_Party_Link on Party.party_cnt = Claim_Party_Link.party_cnt
		where Party_Type.code = 'OTDRIVER'
		and Claim_Party_Link.claim_id = Claim.claim_id
	)						as DriverName,
Claim_Expert_Service.Service 				as ExpertService,

convert(datetime, convert(varchar(23), 
	   Claim.date_passed_test, 111))		as TestDate,

Claim.client_claim_number,
recovery.revised_reserve as recovery_amount,
IsNull(Reserve.Initial_Reserve, 0) + IsNull(Reserve.Revised_Reserve,0) - IsNull(Reserve.Paid_to_Date, 0) - IsNull(recovery.revised_reserve, 0)	as current_reserve,

'SSP Nest FCEDI' as ClaimProduct, 
cp.amount as GrossClaimPayment, 
cp.date_of_payment,

--added on 26th Jan 2011 / modify on 26 Oct 2011
(
	select 
		sum(initial_reserve+revised_reserve) from reserve r, claim_peril cpp, claim cc
	where 
		r.claim_peril_id=cpp.claim_peril_id and
		cpp.claim_id = cc.claim_id and
		cc.policy_id = i.insurance_file_cnt and
		cc.version_id=Claim_Latest.version_id
) as TotalIncurred,

this_revision,
Claim_Latest.version_id as latest_version,
claim.version_id as version_id,
paid_to_date,
Reserve.initial_reserve+Reserve.revised_reserve as incurred_amount
from (select policy_id, max(version_id) as version_id from claim c where 
claim_version_description is not null and is_dirty=0 group by policy_id) as Claim_Latest,
insurance_file i,
Claim 
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
left join Claim_Peril on Claim_Peril.claim_id = Claim.claim_id
left outer join Peril_Type on Peril_Type.peril_type_id = Claim_Peril.Peril_type_id
inner join Reserve on Reserve.claim_peril_id = Claim_Peril.claim_peril_id and 
	(this_revision <>0 OR paid_to_date<>0 OR initial_reserve<>0 OR revised_reserve<>0)
	
left outer join Reserve_Type on Reserve.reserve_type_id = Reserve_Type.reserve_type_id
left outer join Claim_Expert_Service on Claim_Expert_Service.claim_id = Claim.claim_id
left outer join recovery on recovery.claim_peril_id = claim_peril.claim_peril_id
left outer join claim_payment cp on cp.claim_id = claim.claim_id
where 
	Claim_Latest.policy_id = claim.policy_id and 
	Claim.policy_id=i.insurance_file_cnt --and
	--i.product_id=36

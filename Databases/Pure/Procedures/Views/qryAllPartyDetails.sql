
DDLDropview 'qryAllPartyDetails'
go

create view qryAllPartyDetails as
								select
								--p.party_cnt 												as insured_cnt,
								p.party_cnt 												as party_cnt,
								pt.description												as PartyType,
								ps.description												as PartyStructure,
								p.party_cnt													as PartyCnt,
								case p.is_also_agent
									when 0 then 'No'
									else 'Yes' end											as AlsoAgent,
								isnull(pp.agent_reference,'N/A')							as ProspectingAgentReference,
								isnull(rtrim(pps.description),'N/A')						as ProspectingStatus,
								isnull(rtrim(ppi.resolved_name),'N/A')						as ProspectingPreviousInsurer,
								isnull(psc.description,'N/A')								as ProspectingStrengthCode,
								s.description												as Branch,
								--p.party_id													as PartyID,
								p.shortname													as ClientCode,
								p.name														as Name,
								p.resolved_name												as ResolvedName,
								c.description												as Currency,
								l.description												as Language,
								isnull(ct.description,'N/A')								as CollectType,
								isnull(att.description,'N/A')								as AccumTreatmentType,
								isnull(stt.description,'N/A')								as StatTreatmentType,
								isnull(agent.resolved_name,'N/A')							as Agent,
								isnull(pmu.username,'N/A')									as CreatedBy,
								isnull(pc.description,'N/A')								as PartyCategory,
								convert(datetime, convert(varchar(23), 
								   p.date_created, 111))									as DateCreated,
								year(p.date_created)										as DateCreatedYear,
								month(p.date_created)										as DateCreatedMonth,
								day(p.date_created)											as DateCreatedDay,
								convert(datetime, convert(varchar(23), 
								   p.last_modified, 111))									as LastModified,
								year(p.last_modified)										as LastModifiedYear,
								month(p.last_modified)										as LastModifiedMonth,
								day(p.last_modified)										as LastModifiedDay,
								isnull(consultant.resolved_name,'N/A')						as Consultant,
								isnull(modified.full_name,'N/A')							as ModifiedBy,
								p.payment_method_code										as PaymentMethod,
								p.payment_term_code											as PaymentTermCode,
								p.credit_card_code											as CreditCardCode,
								p.file_code													as FileCode,
								p.abc_count													as AbcCount,
								p.statements												as Statements,
								isnull(rm.description,'N/A')								as ReminderType,
								p.renewals													as Renewals,
								p.status													as Status,
								p.last_action_type											as LastActionType,
								case p.is_travel_agent
									when 0 then 'No'
									else 'Yes' end											as TravelAgent,
								case p.is_prospect
									when 0 then 'No'
									else 'Yes' end											as IsProspect,
								case p.is_deleted
									when 0 then 'No'
									else 'Yes' end											as IsDeleted,
								p.abi_code_on_406											as AbiCode406,
								p.abi_code_on_81											as AbiCode81,
								p.abi_codelist												as AbiCodeList,
								isnull(area.description,'N/A')								as Area,
								sl.description												as ServiceLevel,
								--p.invariant_key												as InvariantKey,
								p.record_status												as RecordStatus,
								p.CCJs														as CCJ,
								p.user_defined_data_id										as UserDefinedDataID,
								sg.description												as SeasonalGift,
								rsc.description												as RenewalStopCode,
								--p.swift_party_id,
								p.loyalty_number											as LoyaltyNumber,
								p.alternative_identifier									as AlternativeIdentifier,
								p.marketing_segment_ind										as MarketingSegment,
								p.trading_name												as TradingName,
								isnull(sb.description,'N/A')								as SubBranch,
								p.tob_letter,
								pl.date_of_birth											as LifestyleDOB,
								isnull(pl.gender_code,'N/A')								as LifestyleGender,
								isnull(pl.occupation_code,'N/A')							as LifestyleOccupation,
								isnull(pl.secondary_occupation_code,'N/A')					as LifestyleSecondaryOccupation,
								(case pl.is_smoker
									when 0 then	'No'
									when 1 then 'Yes'
									else 'N/A' end)											as LifestyleIsSmoker,
								isnull(pgt.description,'N/A')								as PartyGroupType,
								(case pgc.is_registered_charity
									when 0 then 'No'
									when 1 then 'Yes'
									else 'N/A' end)											as PartyGroupRegisteredCharity,
								isnull(pgc.charity_number,'N/A')							as PartyGroupCharityNumber,
								isnull(pgc.number_of_members,0)								as PartyGroupNumberOfMembers,
								isnull(pgc.turnover,0)										as PartyGroupTurnover,
								isnull(ppc.employment_status_code,'N/A')					as PartyPersonalEmployment_status,
								isnull(ppc.employer_business,'N/A')							as PartyPersonalEmployerBusiness,
								isnull(ppc.secondary_employer_business,'N/A')				as PartyPersonalSecondaryEmployerBusiness,
								isnull(ppc.marital_status_code,'N/A')						as PartyPersonalMaritalStatus,
								n.description												as PartyPersonalNationality,
								(case ppc.is_pet_owner
									when 0 then 'No'
									when 1 then 'Yes'
									else 'N/A' end)											as PartyPersonalPetOwner,
								isnull(ppc.accommodation_type_code,'N/A')					as PartyPersonalAccomodation,
								isnull(pcc.company_reg,'N/A')								as PartyCorporateCompanyReg,
								pcc.trading_since_date										as PartyCorporateTradingSince,
								pcc.Party_business_id										as PartyCorporateBusiness,
								pcc.no_of_offices											as PartyCorporateNoOfOffices,
								(case pcc.no_of_employees
									when 0 then 'Not Known'
									when 1 then '0 - 5'
									when 2 then '5 - 20'
									when 3 then '20 - 100'
									when 4 then '100 - 500'
									when 5 then '>500' end)									as PartyCorporateNoOfEmployees,
								isnull(pcc.trade_code,'N/A')								as PartyCorporateTrade,
								isnull(pcsc.description,'N/A')								as PartyCorporateSICCode,
								(case p.tax_exempt
									when 0 then 'No'
									when 1 then 'Yes'
									else 'N/A' end)											as PartyCorporateTaxExempt,
								pcc.wage_roll												as PartyCorporateWageRoll,
								pcc.turnover												as PartyCorporateTurnover,
								p.tax_number												as PartyCorporateTaxRegCode
								from party p 
								left join party_type pt on p.party_type_id = pt.party_type_id
								left join party_structure ps on p.party_structure_id = ps.party_structure_id
								left join source s on p.source_id = s.source_id
								left join currency c on p.currency_id = c.currency_id
								left join language l on p.language_id = l.language_id
								left join collect_type ct on p.collect_type_id = ct.collect_type_id
								left join accum_treatment_type att on p.accum_treatment_type_id = att.accum_treatment_type_id
								left join stats_treatment_type stt on p.stats_treatment_type_id = stt.stats_treatment_type_id
								left join party agent on p.agent_cnt = agent.party_cnt
								left join pmuser pmu on p.created_by_id = pmu.user_id
								left join party_category pc on p.party_category_id = pc.party_category_id
								left join party consultant on p.consultant_cnt = consultant.party_cnt
								left join pmuser modified on p.modified_by_id = modified.user_id
								left join reminder_type rm on p.reminder_type_id = rm.reminder_type_id
								left join area on p.area_id = area.area_id
								left join service_level sl on p.service_level_id = sl.service_level_id
								left join seasonal_gift sg on p.seasonal_gift_id = sg.seasonal_gift_id
								left join renewal_stop_code rsc on p.renewal_stop_code_id = rsc.renewal_stop_code_id
								left join sub_branch sb on p.sub_branch_id = sb.sub_branch_id
								left join party_prospect pp on p.party_cnt = pp.party_cnt
								left join prospect_status pps on pp.strength_code_id = pps.prospect_status_id
								left join party ppi on pp.previous_insurer_cnt = ppi.party_cnt
								left join strength_code psc on pp.strength_code_id = psc.strength_code_id
								left join party_lifestyle pl on p.party_cnt = pl.party_cnt AND pl.category = 1
								left join party_group_client pgc on p.party_cnt = pgc.party_cnt
								left join party_group_type pgt on pgc.party_group_type_id = pgt.party_group_type_id
								left join Party_Personal_client ppc on p.party_cnt = ppc.party_cnt
								left join nationality n on ppc.nationality_id = n.nationality_id
								left join Party_corporate_client pcc on p.party_cnt = pcc.party_cnt
								left join sic_code pcsc on pcc.sic_code_id = pcsc.sic_code_id
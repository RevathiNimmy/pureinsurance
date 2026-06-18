
DDLDropview 'qryAllPolicies'
go

Create view [dbo].[qryAllPolicies] as
			select 
			i.insurance_ref                         as Policy_Insurance_ref,
			--i.insurance_file_cnt,
			ifs.description							as FileStructure,
			ift.description							as FileType,
			isnull(ifstat.description,'Live')		as FileStatus,
			--i.insurance_file_id,
			s.description							as Branch,
			--i.insurance_folder_cnt,
			i.insurance_ref							as insurance_ref,
			prod.description						as Product,
			insurer.Resolved_name					as LeadInsurer,
			isnull(agent.resolved_name,'N/A')		as LeadAgent,
			i.lead_agent_percent					as LeadAgentPercentage,
			acchand.resolved_name					as AccountHandler,
			i.insured_cnt							as party_cnt,
			--i.insured_cnt							as InsuredCnt,
			bt.description							as BusinessType,
			i.collect_type_id,
			i.collection_from_cnt,
			i.branch_id								as BranchUnknown,
			c.description							as Currency,
			l.description							as Language,
			i.date_issued,
			YEar(i.cover_start_date)				as CoverStartDateYear,
			Month(i.cover_start_date)				as CoverStartDateMonth,
			day(i.cover_start_date)					as CoverStartDateDay,
			i.cover_start_date,
			i.expiry_date,
			i.renewal_date,
			isnull(rm.description,'N/A') 			as RenewalMethod,
			rf.description							as RenewalFreqency,
			case i.is_referred_at_renewal
				when 0 then 'No'
				else 'Yes' end						as ReferedAtRenewal,
			lr.description							as LapsedReason,
			i.lapsed_date,
			i.lapsed_description,
			case i.is_referred_on_mta
				when 0 then 'No'
				else 'Yes' end						as ReferedOnMTA,
			i.policy_version,
			case i.gemini_policy_status	
				When 0 THEN 'Incomplete'
				When 1 THEN 'Quote'
				When 10 THEN 'NB Complete'
				When 12 THEN 'Requote Required'
				When 15 THEN 'Requoted'
				When 20 THEN 'Pending'
				When 30 THEN 'Pending Transmitted'
				When 40 THEN 'Live Policy'
				When 50 THEN 'Cancelled Policy'
				When 1010 THEN 'Permanent MTA'
				When 1020 THEN 'Temporary MTA'
				When 1030 THEN 'Incomplete MTA'
				When 1040 THEN 'MTA Cancellation'
				When 1050 THEN 'Reinstatement'
				else 'N/A' end						as SQPolicyStatus,
			case i.gemini_business_type
				when 1 THEN 'Motor'
				when 2 then 'Home'
				when 3 then 'Truck'
				else 'N/A' end						as SQBusinessType,
			i.deferred_ind,
			i.policy_ignore,
			broker.resolved_name					as CommissionAccount,
			rc.description							as RiskCode,
			isnull(ac.description,'N/A')			as AnalysisCode,
			i.proposal_date,
			i.diary_date,
			i.review_date,
			i.renewal_day_number,
			pt.description							as PolicyType,
			i.indicator,
			i.clause,
			i.cover,
			i.area,
			i.long_term_undertaking_date,
			isnull(rsc.description,'N/A')			as RenewalStopCode,
			i.vbs_type,
			i.vbs_status,
			case i.is_insurer_rate_table
				when 0 then 'No'
				else 'Yes' end						as IsInsurerRateTable,
			case i.is_related_policies
			when 0 then 'No'
				else 'Yes' end						as IsRelatedPolicies,
			case i.is_retained_documents
			when 0 then 'No'
				else 'Yes' end						as IsRetainedDocuments,
			i.schemes_postcode,
			i.paid_direct,
			isnull(gs.scheme_desc,'N/A')			as Scheme,
			i.brokerage_amount,
			case i.is_minimum_brokerage_flag
			when 0 then 'No'
				else 'Yes' end						as MinimumBrokageFlag,
			i.annual_premium,
			i.this_premium,
			i.net_premium,
			i.commission_amount,
			i.iptable_amount,
			i.ipt_percentage * iptable_amount		as IPTValue,
			(i.iptable_amount -
			(i.ipt_percentage * iptable_amount))	as GrossPremium,
			i.ipt_percentage/100					as ipt_percentage,
			case i.is_ipt_overridden
			when 0 then 'No'
				else 'Yes' end						as IsIPTOverridden,
			i.tax_amount,
			i.vatable_amount,
			i.vat_percentage,
			i.vat_amount,
			isnull(i.payment_method,'N/A')			as PaymentMethod,
			i.user_defined_data_id,
			i.commission_percentage/100				as commission_percentage,
			--i.invariant_key,
			i.insured_name,
			i.alternate_reference,
			case i.is_client_invoiced
			when 0 then 'No'
				else 'Yes' end						as IsClientInvoiced,
			i.old_policy_number,
			i.quote_expiry_date,
			i.alternate_account_cnt,
			i.loyalty_scheme_flag,
			accexec.resolved_name					as AccountExec,
			i.anniversary_date,
			isnull(FSA_CompanyCategory.Description,'N/A') as FSACategory,
			--i.fsa_contract_location_id, -- no related table???
			underwriter.resolved_name				as FSAUnderwriter,
			--i.fsa_underwriter_cnt, -- no related table???
			case i.fsa_type_of_sale_id
				when 0 then 'Non Advised'
				else 'Advised' end					as FSATypeOfSale,
			i.fsa_renewal_consent,
			ps.description							as PolicyStyle,
			uy.description							as UnderwritingYear,
			pstat.description						as PolicyStatus,
			i.edi_message_sent,
			c1.description							as ReturnPremiumCurrency,
			eror.Description						as ExchangeRateOverride_Reason,
			c2.description							as BaseCurrency,
			i.currency_base_xrate,
			c3.description							as AgentAccountCurrency,
			i.agent_account_base_xrate,
			i.system_base_xrate,
			i.currency_base_date,
			i.account_base_date,
			i.system_base_date,
			i.inception_date_tpi,
			--i.cashlistitem_id,
			i.cashlistitem_valid,
			0 renewal_premium,	-- was renewal premium
			i.addon_created,
			case i.terms_agreed
				when 0 then 'No'
				else 'Yes' end						as TermsAgreed,
			i.terms_agreed_date						as TermsAgreedDate,
			i.inception_Date,
			year(i.inception_Date)					as InceptionDateYear,
			month(i.inception_Date)					as InceptionDateMonth,
			day(i.inception_Date)					as InceptionDateDay,
			i.policy_documents_issued_date,
			case i.policy_documents_correct
				when 0 then 'No'
				else 'Yes' end						as DocumentCorrect,
			i.error_notification_date
			from insurance_file i
			left join insurance_file_structure ifs on i.insurance_file_structure_id = ifs.insurance_file_structure_id
			left join insurance_file_type ift on i.insurance_file_type_id = ift.insurance_file_type_id
			left join insurance_file_status ifstat on i.insurance_file_status_id = ifstat.insurance_file_status_id
			left join source s on i.source_id = s.source_id
			left join product prod on i.product_id = prod.product_id
			left join party insurer on i.lead_insurer_cnt = insurer.party_cnt
			left join party agent on i.lead_agent_cnt = agent.party_cnt
			left join party acchand on i.account_handler_cnt = acchand.party_cnt
			left join business_type bt on i.business_type_id = bt.business_type_id
			left join currency c on i.currency_id = c.currency_id
			left join language l on i.language_id = l.language_id
			left join renewal_method rm on i.renewal_method_id = rm.renewal_method_id
			left join renewal_frequency rf on i.renewal_frequency_id = rf.renewal_frequency_id
			left join lapsed_reason lr on i.lapsed_reason_id = lr.lapsed_reason_id
			left join party broker on i.broker_cnt = broker.party_cnt
			left join risk_code rc on i.risk_code_id = rc.risk_code_id
			left join analysis_code ac on i.analysis_code_id = ac.analysis_code_id
			left join policy_type pt on i.policy_type_id = pt.policy_type_id
			left join renewal_stop_code rsc on i.renewal_stop_code_id = rsc.renewal_stop_code_id 
			left join party accexec on i.account_executive_cnt = accexec.party_cnt
			left join FSA_CompanyCategory on i.fsa_customer_category_id = FSA_CompanyCategory.FSA_CompanyCategory_id
			left join policy_style ps on i.policy_style_id = ps.policy_style_id
			left join underwriting_year uy on i.underwriting_year_id = uy.underwriting_year_id
			left join policy_status pstat on i.policy_status_id = pstat.policy_status_id
			left join currency c1 on i.return_premium_currency_id = c1.currency_id
			left join Exchange_Rate_Override_Reason eror on i.Exchange_Rate_Override_Reason_id = eror.Exchange_Rate_Override_Reason_id
			left join currency c2 on i.base_currency_id = c2.currency_id
			left join currency c3 on i.agent_account_currency_id = c3.currency_id
			left join gis_scheme gs on i.scheme = gs.gis_scheme_id
			left join party underwriter on i.fsa_underwriter_cnt = underwriter.party_cnt
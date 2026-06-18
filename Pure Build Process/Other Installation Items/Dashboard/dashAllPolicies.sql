DDLDropview 'dashAllPolicies'
go
Create view [dbo].[dashAllPolicies] as
select 
	i.insurance_folder_cnt 					as InsuranceFolderCnt,
	i.insurance_file_cnt 					as InsuranceFileCnt,
	i.insurance_ref                         as Policy_Insurance_ref,	
	ift.description							as FileType,
	
	CASE IFT.insurance_file_type_id
        WHEN 1 THEN IFT.description
        ELSE ISNULL(ifstat.Description, 'Live')
    END as FileTypeCode, 	
	
	isnull(ifstat.description,'Live')		as FileStatus,		
	ift.code							as 	Quotes_FileTypeCode,
	
	i.insurance_ref							as insurance_ref,
	prod.description						as Product,
		
	isnull(agent.resolved_name,'N/A')			as LeadAgent,
	isnull(agent.shortname,'N/A')			as LeadAgentCode,
	agent.party_cnt						as LeadAgentCnt,
	i.lead_agent_percent					as LeadAgentPercentage,
	acchand.resolved_name					as AccountHandler,
	i.insured_cnt							as party_cnt,	
	
	CASE Year(i.date_issued) WHEN 1899 Then Null Else i.date_issued End as date_issued,

	i.cover_start_date,
	i.expiry_date,
	i.renewal_date,	
	lr.description							as LapsedReason,	
	CASE Year(i.lapsed_date) WHEN 1899 Then Null ELse i.lapsed_date End as lapsed_date,
	i.policy_version,

	isnull(ac.description,'')			as AnalysisCode,
	
	CASE Year(i.proposal_date) WHEN 1899 Then Null ELse i.proposal_date End as proposal_date,
	coalesce(i.annual_premium, 0) as annual_premium,	        
	i.this_premium as Quotes_this_premium,
	coalesce(pstat.description, 'N/a')						as PolicyStatus,
	pstat.code						as PolicyStatusCode,
	insurance_folder.inception_Date,
	
	IFSS.date_created,

acom.commission_value as AgentCommissionValue,
acom.commission_percentage as AgentCommissionPercent,

/*
(SELECT count(insurance_ref) FROM insurance_file WHERE insurance_file_type_id=1) AS NoOfQuotes,
(SELECT count(insurance_ref) FROM insurance_file WHERE policy_status_id = 9) AS NoQuoted,
(SELECT count(insurance_ref) FROM insurance_file WHERE policy_status_id = 6) AS NoDeclined,
(SELECT count(insurance_ref) FROM insurance_file WHERE insurance_file_type_id = 2) AS NoSecured,
(SELECT sum(this_premium) FROM insurance_file WHERE insurance_file_type_id=1) AS PremiumQuoted,
(SELECT sum(this_premium) FROM insurance_file WHERE insurance_file_type_id=2) AS PremiumSecured,
(SELECT sum(this_premium) FROM insurance_file WHERE policy_status_id = 6) AS DeclinedPremium,
(select distinct count(insurance_ref) from insurance_file) AS NoOfQuotesAndPolicies,*/

--Added on 26th Oct 2010
i.policy_status_id,
i.policy_version as Versions,

(SELECT 
	MAX(policy_version) 
FROM 
	insurance_file i2 
WHERE 
	i2.insurance_folder_cnt=i.insurance_folder_cnt
AND ( 
		i2.insurance_file_status_id IS NULL --live 
        OR
        insurance_file_status_id = 1 --cancelled
    )
AND (
        i2.insurance_file_type_id = 2 --lapsed
        OR
        i2.insurance_file_type_id = 5 --replaced
        OR
        i2.insurance_file_type_id = 10 --renewal
    )
) as CurrentVersion

--, insurance_file_risk_link.risk_cnt

from insurance_file i
inner join party on party.party_cnt=i.insured_cnt and party.is_deleted=0 
inner join insurance_file_system IFSS ON IFSS.insurance_file_cnt=i.insurance_file_cnt
left join insurance_folder ON i.insurance_ref=insurance_folder.code
left join insurance_file_type ift on i.insurance_file_type_id = ift.insurance_file_type_id
left join insurance_file_status ifstat on i.insurance_file_status_id = ifstat.insurance_file_status_id
left join product prod on i.product_id = prod.product_id
left join party agent on i.lead_agent_cnt = agent.party_cnt
left join party acchand on i.account_handler_cnt = acchand.party_cnt
left join lapsed_reason lr on i.lapsed_reason_id = lr.lapsed_reason_id
left join analysis_code ac on i.analysis_code_id = ac.analysis_code_id
left join policy_status pstat on i.policy_status_id = pstat.policy_status_id
left join agent_commission acom on acom.insurance_file_cnt=i.insurance_file_cnt and 
			acom.commission_value>0 and acom.party_cnt=agent.party_cnt
where ISNULL(I.policy_ignore, 0) = 0 --AND	
	--i.product_id=36
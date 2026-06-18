SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'qryPolicyLineDetails'
go
CREATE VIEW qryPolicyLineDetails AS 
SELECT

	Insurance_file.insurance_file_cnt 													'PolicyID',
	(SELECT 
		SUM(PC.written_line_percentage) 
		FROM policy_coinsurers PC
		WHERE PC.insurance_file_cnt=policy_coinsurers.insurance_file_cnt 
	) 																					'TotalWrittenLinePercentage',
	(SELECT 
		SUM(PC.Signed_line_percentage) 
		FROM policy_coinsurers PC
		WHERE PC.insurance_file_cnt=policy_coinsurers.insurance_file_cnt 
	) 																					'TotalSignedLinePercentage',
	P1.name 																			'InsurerParty',
	ISNULL(P2.name,'Not Applicable') 													'Bureau',
	Policy_Coinsurers.Written_Line_Percentage 											'WrittenLinePercentage',
	Policy_Coinsurers.LineStands 														'LineStands',
	Policy_Coinsurers.Signed_Line_Percentage 											'SignedLinePercentage',
	ROUND((SELECT 
		SUM(PCS.premium_inc_tax) 
		FROM Policy_Coinsurers_section PCS
		WHERE PCS.insurance_file_cnt = Policy_Coinsurers_section.insurance_file_cnt
		AND PCS.party_cnt = Policy_Coinsurers_section.party_cnt
	),2)																				'SignedLinePremiumIncludingTax',
	ROUND(Policy_coinsurers.coinsurer_ipt_amount,2)										'SignedLineTaxValue',
	ROUND((SELECT 
		SUM(PCS.premium_exc_tax) 
		FROM Policy_Coinsurers_section PCS
		WHERE PCS.insurance_file_cnt = Policy_Coinsurers_section.insurance_file_cnt
		AND PCS.party_cnt = Policy_Coinsurers_section.party_cnt
	),2)																				'SignedLinePremiumExcludingTax',
	ROUND((SELECT 
		SUM(PCS.commission_inc_tax) 
		FROM Policy_Coinsurers_section PCS
		WHERE PCS.insurance_file_cnt = Policy_Coinsurers_section.insurance_file_cnt
		AND PCS.party_cnt = Policy_Coinsurers_section.party_cnt
	),2)																				'SignedLineCommissionValue',
	Policy_coinsurers.IsLeadUnderwriter 												'LeadUnderwriter',
	Insurance_file.insurance_ref 														'PolicyNumber',
	ROUND((SELECT 
		SUM(PCS.commission_charge) 
		FROM Policy_Coinsurers_section PCS
		WHERE PCS.insurance_file_cnt = Policy_Coinsurers_section.insurance_file_cnt
		AND PCS.party_cnt = Policy_Coinsurers_section.party_cnt
	),2)																				'LineDetailCommissionCharge',
	Tax_group.description 																'SectionTaxGroup',
	Tax_calculation.percentage 															'SectionTaxRate',
	Cob_rating_section.description 														'LineDetailSectionName',
	Policy_coinsurers_section.is_applied 												'LineDetailSectionApplied',
	Policy_coinsurers_section.commission_percent 										'LineDetailSectionCommissionPercent',
	ROUND(Policy_coinsurers_section.commission_charge,2)								'LineDetailSectionCommissionCharge',
	ROUND(Policy_coinsurers_section.premium_inc_tax,2)									'SignedLineSectionPremiumIncludingTax',
	ROUND(Policy_coinsurers_section.premium_exc_tax,2)									'SignedLineSectionPremiumExcludingTax',
	ROUND(Tax_calculation.value,2)														'SignedLineSectionTaxValue',
	Policy_coinsurers_section.share_percent									            'Signed Line Section Percentage'
FROM
policy_coinsurers 
LEFT JOIN Party P1
ON P1.Party_cnt = Policy_coinsurers.party_cnt
LEFT JOIN Party P2
ON P2.Party_cnt = Policy_coinsurers.bureau_party_cnt
LEFT JOIN Policy_Coinsurers_section
ON Policy_Coinsurers_Section.insurance_file_cnt = Policy_Coinsurers.insurance_file_cnt
AND Policy_Coinsurers_Section.party_cnt = Policy_Coinsurers.party_cnt
LEFT JOIN Insurance_file
ON Insurance_file.insurance_file_cnt = Policy_Coinsurers.insurance_file_cnt
LEFT JOIN Tax_group
ON Tax_group.tax_group_id=policy_coinsurers_section.tax_group_id
LEFT JOIN Tax_calculation
ON Tax_calculation.policy_coinsurers_section_id = policy_coinsurers_section.policy_coinsurers_section_id
LEFT JOIN Cob_rating_section
ON Cob_rating_section.cob_rating_section_id = policy_coinsurers_section.cob_rating_section_id

GO



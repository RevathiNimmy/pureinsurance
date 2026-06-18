DDLDROPPROCEDURE 'spu_wp_policysectionCoinsurer'
GO

CREATE PROCEDURE spu_wp_policysectionCoinsurer
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT 	
	pcs.share_percent 'SharePercent',
	pcs.premium_exc_tax 'PremiumExcTax',
	pcs.premium_inc_tax 'PremiumIncTax',
	tg1.description 'TaxGroup',
	tg1.code 'TaxGroupCode',
	pcs.commission_percent 'CommissionPercent',
	pcs.commission_charge 'CommissionCharge',
	pcs.commission_exc_tax 'CommissionExcTax',
	pcs.commission_inc_tax 'CommissionIncTax',
	tg2.description 'CommissionTaxGroup',
	tg2.code 'CommissionTaxGroupCode',
	tc.percentage 'SectionTaxRate',
	crs.description 'SectionName',
	(SELECT
		CASE pcs.is_applied
		WHEN 1 THEN 'Yes'
		ELSE 'No'
		END
	) 'IsApplied',
	tc.value 'TaxValue',
	(SELECT pc.written_line_percentage FROM policy_coinsurers pc WHERE pc.insurance_file_cnt = @InsuranceFileCnt and pc.party_cnt = pcs.party_cnt) 'WrittenLinePerc',
	(SELECT pc.signed_line_percentage FROM policy_coinsurers pc WHERE pc.insurance_file_cnt = @InsuranceFileCnt and pc.party_cnt = pcs.party_cnt) 'SignedLinePerc'

FROM	policy_coinsurers_section pcs
	join party p on p.party_cnt = pcs.party_cnt
	left outer join tax_group tg1 on pcs.tax_group_id = tg1.tax_group_id
	left outer join tax_group tg2 on pcs.commission_tax_group_id = tg2.tax_group_id
	LEFT JOIN tax_calculation tc ON pcs.policy_coinsurers_section_id = tc.policy_coinsurers_section_id
	LEFT JOIN cob_rating_section crs ON pcs.cob_rating_section_id = crs.cob_rating_section_id
WHERE	pcs.policy_coinsurers_section_id = @Instance3
DDLDROPPROCEDURE 'spu_wp_policysection'
GO

CREATE PROCEDURE spu_wp_policysection
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT 	ics.premium_excluding_tax 'PremiumExcludingTax',
	ics.tax_applied 'TaxApplied',
	ics.premium_including_tax 'PremiumIncludingTax',
	tg.description 'TaxgroupDescription',
	tg.code 'TaxgroupCode',
	tg.is_withholding_tax 'TaxgroupIsWithholding_tax',
	ics.commission_percentage 'CommissionPercentage',
	ics.commission_charge 'CommissionCharge',
	ics.commission_net 'CommissionNet',
	ics.commission_tax_applied 'CommissionTaxApplied',
	ics.commission_payable 'CommissionPayable',
	tg2.description 'CommissionTaxGroupDescription',
	tg2.code 'CommissiontaxgroupCode',
	tg2.is_withholding_tax 'commissiontaxgroupIsWithholdingTax'
FROM	insurance_cob_section ics
	join tax_group tg on ics.tax_group_id = tg.tax_group_id
	join tax_group tg2 on ics.commission_tax_group_id = tg2.tax_group_id
WHERE	ics.Insurance_section_id = @Instance2
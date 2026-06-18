ddldropprocedure spu_wp_debitSection
go

CREATE PROCEDURE spu_wp_debitSection
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
DECLARE @SharedIndicator INT
DECLARE @Share FLOAT

SELECT @SharedIndicator = CHARINDEX('|', @DocumentRef)

IF @SharedIndicator = 0
    SELECT @Share = 100
ELSE
BEGIN
    SELECT @Share = CONVERT(NUMERIC(15,11), RTRIM(SUBSTRING(@DocumentRef, @SharedIndicator + 1, 25 - @SharedIndicator)))
    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)
END

SELECT
	crs.description 'SectionName',
	(ISNULL(eics.Premium_Excluding_Tax,0)/100)*@Share 'SectionPremiumExcTax',
	(ISNULL(eics.Premium_Including_Tax,0)/100)*@Share 'SectionPremiumIncTax',
	(ISNULL(Round(etc.value,2),0)/100)*@Share 'SectionPremiumTax',
	tg1.description 'SectionPremiumTaxGroup',
	tg1.code 'SectionPremiumTaxGroupCode',
	eics.Commission_Percentage 'SectionCommissionPerc',
	(ISNULL(eics.Commission_Net,0)/100)*@Share 'SectionCommission',
	(ISNULL(eics.Commission_Charge,0)/100)*@Share 'SectionCommissionCharge',
	(ISNULL(eics.Commission_tax_applied,0)/100)*@Share 'SectionCommissionTax',
	tg2.description 'SectionCommissionTaxGroup',
	tg2.code 'SectionCommissionTaxGroupCode',
	eics.is_applied 'SectionApplied'
FROM
	insurance_file inf
	JOIN document d ON d.company_id=inf.source_id AND d.insurance_file_cnt=inf.insurance_file_cnt
	JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref AND tef.source_id = d.company_id AND tef.accounts_export_status = 'c'
	JOIN event_log e ON e.event_cnt = tef.event_log_id
	join event_insurance_cob_section eics on eics.insurance_file_cnt = e.event_cnt
	LEFT OUTER join tax_group tg1 on tg1.tax_group_id = eics.tax_group_id
	LEFT OUTER join tax_group tg2 on tg2.tax_group_id = eics.commission_tax_group_id
	join cob_rating_section crs on crs.cob_Rating_section_id = eics.cob_rating_section_id
	LEFT OUTER JOIN event_tax_calculation etc ON etc.insurance_file_cnt=eics.insurance_file_cnt AND etc.insurance_section_id=eics.insurance_section_id AND etc.is_commission_tax=0
WHERE
	inf.insurance_file_cnt=@insurancefilecnt and
 	d.document_ref = @DocumentRef and
	eics.insurance_section_id=@instance2

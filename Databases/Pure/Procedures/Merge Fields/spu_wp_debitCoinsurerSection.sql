ddldropprocedure spu_wp_debitCoinsurerSection
go

CREATE PROCEDURE spu_wp_debitCoinsurerSection
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
	p.shortname 'SectionCoinsurerCode',
	p.resolved_name 'SectionCoinsurerName',
	crs.description 'SectionName',
	(ISNULL(epcs.Premium_Exc_Tax,0)/100)*@Share 'SectionCoinsurerPremiumExcTax',
	(ISNULL(epcs.Premium_Inc_Tax,0)/100)*@Share 'SectionCoinsurerPremiumIncTax',
	(((isnull(epcs.Premium_Inc_Tax,0)/100)*@Share)-((isnull(epcs.Premium_Exc_Tax,0)/100)*@Share)) 'SectionCoinsurerPremiumTax',
	isnull(tg1.description,'') 'SectionCoinsurerPremiumTaxGroup',
	isnull(tg1.code,'') 'SectionCoinsurerPremiumTaxGroupCode',
	epcs.Commission_Percent 'SectionCoinsurerCommissionPerc',
	(ISNULL(epcs.Commission_inc_tax,0)/100)*@Share 'SectionCoinsurerCommission',
	(ISNULL(epcs.Commission_Charge,0)/100)*@Share 'SectionCoinsurerCommissionCharge',
	(((isnull(commission_inc_tax,0)/100)*@Share) - ((isnull(commission_exc_tax,0)/100)*@Share)) 'SectionCoinsurerCommissionTax',
	isnull(tg2.description,'') 'SectionCoinsurerCommissionTaxGroup',
	isnull(tg2.code,'') 'SectionCoinsurerCommissionTaxGroupCode'
FROM document d
	JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref AND tef.source_id = d.company_id AND tef.accounts_export_status = 'c'
	JOIN event_log e ON e.event_cnt = tef.event_log_id
	join event_policy_coinsurers_section epcs on epcs.insurance_file_cnt = e.event_cnt
	left outer join tax_group tg1 on tg1.tax_group_id = epcs.tax_group_id
	left outer join tax_group tg2 on tg2.tax_group_id = epcs.commission_tax_group_id
	join cob_rating_section crs on crs.cob_Rating_section_id = epcs.cob_rating_section_id
	join party p on p.party_cnt=epcs.party_cnt
WHERE 	d.document_ref = @DocumentRef and
	epcs.policy_coinsurers_section_id=@instance3


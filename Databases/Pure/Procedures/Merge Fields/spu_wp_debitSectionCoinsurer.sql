ddldropprocedure spu_wp_debitSectionCoinsurer
go

CREATE PROCEDURE spu_wp_debitSectionCoinsurer
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
DECLARE @event_insurance_file_cnt INT

SELECT
 @event_insurance_file_cnt = EIF.insurance_file_cnt
FROM
transaction_export_folder TEF
JOIN event_log EL
ON EL.event_cnt=TEF.event_log_id
JOIN event_insurance_file EIF
ON EIF.insurance_folder_cnt=EL.event_cnt
WHERE TEF.document_ref = @DocumentRef
AND TEF.accounts_export_status='c'
AND TEF.source_id = (SELECT source_id FROM insurance_file WHERE insurance_file_cnt = @InsuranceFileCnt)


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
	tg1.description 'SectionCoinsurerPremiumTaxGroup',
	tg1.code 'SectionCoinsurerPremiumTaxGroupCode',
	epcs.Commission_Percent 'SectionCoinsurerCommissionPerc',
	(ISNULL(epcs.Commission_inc_tax,0)/100)*@Share 'SectionCoinsurerCommission',
	(ISNULL(epcs.Commission_Charge,0)/100)*@Share 'SectionCoinsurerCommissionCharge',
	(((isnull(commission_inc_tax,0)/100)*@Share) - ((isnull(commission_exc_tax,0)/100)*@Share)) 'SectionCoinsurerCommissionTax',
	tg2.description 'SectionCoinsurerCommissionTaxGroup',
	tg2.code 'SectionCoinsurerCommissionTaxGroupCode',
	epcs.is_applied 'SectionApplied',
	etc.percentage 'SectionTaxRate',
	(SELECT TOP 1 epc.written_line_percentage FROM event_policy_coinsurers epc WHERE epc.insurance_file_cnt = @event_insurance_file_cnt and p.party_cnt = epc.party_cnt) 'WrittenLinePerc',
	(SELECT TOP 1 epc.signed_line_percentage FROM event_policy_coinsurers epc WHERE epc.insurance_file_cnt = @event_insurance_file_cnt and p.party_cnt = epc.party_cnt) 'SignedLinePerc'
FROM document d
	JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref AND tef.source_id = d.company_id AND tef.accounts_export_status = 'c'
	JOIN event_log e ON e.event_cnt = tef.event_log_id
	join event_policy_coinsurers_section epcs on epcs.insurance_file_cnt = e.event_cnt
	LEFT join tax_group tg1 on tg1.tax_group_id = epcs.tax_group_id
	LEFT join tax_group tg2 on tg2.tax_group_id = epcs.commission_tax_group_id
	join cob_rating_section crs on crs.cob_Rating_section_id = epcs.cob_rating_section_id
	join party p on p.party_cnt=epcs.party_cnt
	LEFT JOIN event_tax_calculation etc ON etc.policy_coinsurers_section_id = epcs.policy_coinsurers_section_id
WHERE 	d.document_ref = @DocumentRef and
	epcs.policy_coinsurers_section_id=@instance2
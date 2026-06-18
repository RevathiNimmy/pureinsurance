ddldropprocedure spu_wp_debitCoinsurer
go

CREATE PROCEDURE spu_wp_debitCoinsurer
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
	p.resolved_name 'CoinsurerName',
	p.shortname 'CoinsurerCode',
	epc.coinsurer_percentage 'CoinsurerPerc',
	epc.coinsurer_cover_percentage 'CoinsurerCoverPerc',
	(select sum(premium_exc_tax) from event_policy_coinsurers_section epcs where epcs.party_cnt=p.party_cnt and epcs.insurance_file_cnt = e.event_cnt) 'CoinsurerPremiumExcTax',
	(select sum(premium_inc_tax) from event_policy_coinsurers_section epcs where epcs.party_cnt=p.party_cnt and epcs.insurance_file_cnt = e.event_cnt) 'CoinsurerPremiumIncTax',
	ad.address1 'CoinsurerAddressLine1',
	ad.address2 'CoinsurerAddressLine2',
	ad.address3 'CoinsurerAddressLine3',
	ad.address4 'CoinsurerAddressLine4',
	ad.postal_code 'CoinsurerAddressPostCode',
	(select sum(commission_exc_tax) from event_policy_coinsurers_section epcs where epcs.party_cnt=p.party_cnt and epcs.insurance_file_cnt = e.event_cnt) 'CoinsurerCommissionExcTax',
	(select (sum(commission_inc_tax)-sum(commission_exc_tax)) from event_policy_coinsurers_section epcs where epcs.party_cnt=p.party_cnt and epcs.insurance_file_cnt = e.event_cnt) 'CoinsurerCommissionTax',
	(select (sum(premium_inc_tax)-sum(premium_exc_tax)) from event_policy_coinsurers_section epcs where epcs.party_cnt=p.party_cnt and epcs.insurance_file_cnt = e.event_cnt) 'CoinsurerPremiumTax',
	epc.written_line_percentage 'WrittenLinePerc',
	epc.signed_line_percentage 'SignedLinePerc',
	(SELECT
		CASE epc.linestands
		WHEN 1 THEN 'Yes'
		ELSE 'No'
		END
	) 'LineStands',
	ISNULL(p1.resolved_name,'') 'Bureau',
	(SELECT SUM(commission_inc_tax) FROM event_policy_coinsurers_section epcs WHERE epcs.party_cnt=p.party_cnt and epcs.insurance_file_cnt = e.event_cnt) 'SignedLineComm',
	(SELECT
		CASE epc.isleadunderwriter
		WHEN 1 THEN 'Yes'
		ELSE 'No'
		END
	) 'LeadUnderwriter',
	epc.coinsurer_policy_number 'CoinsurerPolicyNumber',
	(SELECT SUM(commission_charge) FROM event_policy_coinsurers_section epcs WHERE epcs.party_cnt=p.party_cnt and epcs.insurance_file_cnt = e.event_cnt) 'SignedLineCommCharge'

FROM document d
	JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref AND tef.source_id = d.company_id AND tef.accounts_export_status = 'c'
	JOIN event_log e ON e.event_cnt = tef.event_log_id
	join event_policy_coinsurers epc on epc.insurance_file_cnt = e.event_cnt
	join party p on p.party_cnt=epc.party_cnt
	LEFT JOIN party p1 ON p1.party_cnt = epc.bureau_party_cnt
	join party_address_usage pau on pau.party_cnt = epc.party_cnt
	join address_usage_type aut on aut.address_usage_type_id = pau.address_usage_type_id 
	join address ad on ad.address_cnt = pau.address_cnt
WHERE 	d.document_ref = @DocumentRef and
	epc.party_cnt=@instance2
	and epc.insurance_file_cnt = e.event_cnt
	and aut.code = '3131 XCO'

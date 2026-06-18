ddldropprocedure 'spu_Brokerlink_EBordereau_Get_Data'
go

CREATE PROCEDURE spu_Brokerlink_EBordereau_Get_Data
    @cashlist_id INT
AS
SELECT DISTINCT
	(
		SELECT
			ISNULL(MAX(insurance_ref), '')
		FROM TransDetail
		WHERE transdetail_id = PaidTD.transdetail_id
	) 'PolicyNumber',
	PaidD.document_date 'PolicyTransactionDate',
	tt.code 'TransactionType',
	PaidD.document_ref 'DocumentRef',
	(
		SELECT
			ISNULL(P1.resolved_name, '')
		FROM Transdetail TD1
		JOIN Account A1
			ON A1.account_id = TD1.account_id
		JOIN Party P1
			ON P1.party_cnt = A1.account_key
		WHERE TD1.document_id = PaidD.document_id
			AND TD1.document_sequence =
			(
				SELECT
					MIN(document_sequence)
				FROM transdetail
				WHERE document_id = td1.document_id
				AND ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
			)
	) 'ClientName',

	(
		SELECT
			ISNULL(A1.short_code, '')
		FROM Transdetail TD1
		JOIN Account A1
			ON A1.account_id = TD1.account_id
		WHERE TD1.document_id = PaidD.document_id
		AND TD1.document_sequence =
			(
				SELECT
				MIN(document_sequence)
				FROM transdetail
				WHERE document_id = td1.document_id
				AND ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
			)
	) 'ClientCode',
	EIF.tax_amount 'TotalPremiumGST',
	(
		SELECT 
			SUM(commission_net) 
		FROM
			event_insurance_COB_section
		WHERE insurance_file_cnt=EIF.insurance_file_cnt
	) 'BrokerFeeAmount',
	(
		SELECT 
			round(SUM(commission_tax_applied),2) 
		FROM
			event_insurance_COB_section
		WHERE insurance_file_cnt=EIF.insurance_file_cnt
	) 'BrokerFeeGSTOSTAmount',
	isnull(eif.this_premium,0)-isnull(eif.commission_amount,0) 'NetPremium',
	(
        	SELECT
			ISNULL(SUM(ROUND(cli.amount,2)),0)
		FROM Cashlistitem cli
		WHERE cli.cashlist_id = C.cashlist_id
	) 'PaymentAmount' ,
	(
		select 
			CASE WHEN gq.code = 'BROKERLINK' THEN 1
	                ELSE 0 END
		from
			insurance_file iff
			left outer join GIS_Policy_Link gpl on gpl.insurance_file_cnt = iff.insurance_file_cnt
			left outer join GIS_QEM_Usage gqu on gqu.gis_scheme_id = gpl.gis_scheme_id
			left outer join gis_qem gq on gq.gis_qem_id = gqu.gis_qem_id
			where iff.insurance_file_cnt = insfile.insurance_file_cnt
	) 'IsBrokerlink',
        (
		CASE
		WHEN d.documenttype_id IN (33,34) THEN 1
                ELSE 0 END
        ) 'IsDirect',
        (
            CASE
                WHEN paidd.documenttype_id IN (33,34) THEN 1
                ELSE 0 END
        ) 'IsOriginalDirect',
EIF.insurance_file_cnt 'Event_Insurance_File_Cnt'
FROM CashList C
JOIN CashListItem I
    ON C.cashlist_id = I.cashlist_id
JOIN TransDetail TD
    ON I.transdetail_id = TD.transdetail_id
JOIN Document D
    ON D.document_id = TD.document_id
JOIN Account A
    ON TD.account_id = A.account_id
JOIN AllocationDetail AL
    ON AL.cashlistitem_id = I.cashlistitem_id
JOIN TransDetail PaidTD
    ON AL.transdetail_id = PaidTD.transdetail_id
JOIN Document PaidD
    ON PaidTD.document_id = PaidD.document_id
JOIN Transmatch PaidTM
    ON PaidTM.transdetail_id = PaidTD.transdetail_id
LEFT JOIN Transaction_Export_Folder TransExp
    ON TransExp.Document_Ref = PaidD.Document_Ref
    AND TransExp.source_id = PaidD.company_id
LEFT JOIN Insurance_File InsFile
    ON TransExp.Insurance_file_cnt = InsFile.insurance_file_cnt
JOIN Party P2
    ON InsFile.lead_insurer_cnt=P2.party_cnt
LEFT JOIN event_log EL
	ON EL.transaction_export_folder_cnt = TransExp.transaction_export_folder_cnt
LEFT JOIN event_insurance_file EIF
	ON EIF.insurance_folder_cnt = EL.event_cnt
LEFT JOIN transaction_type TT
	ON TT.Transaction_type_id = TransExp.Transaction_type_id
WHERE c.cashlist_id = @cashlist_id
AND PaidTM.allocationdetail_id IS NOT NULL

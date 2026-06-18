DDLDropview 'dashAllTransactionsOthers'
go
create view [dbo].[dashAllTransactionsOthers] as
select 
td.transdetail_id 						as TransactionId,	
td.document_id,
mt.description							as TransactionPaymentMethod,
tm.base_match_amount as CashAllocAmount,
EIF.this_premium as renewal_premium,
ISNULL(EIF.commission_amount,0) as comm_amount,
pfi.amount as DueAmount,
pfi.DueDate as DueDate,

(SELECT ISNULL(SUM(event_policy_fee.commission_amount),0) FROM event_policy_fee INNER JOIN party ON event_policy_fee.party_cnt=party.party_cnt WHERE event_policy_fee.insurance_file_cnt=EIF.insurance_file_cnt AND party.party_type_id=10) as renewal_comm,

(SELECT ISNULL(SUM(event_policy_fee.total_fee),0) FROM event_policy_fee INNER JOIN party ON event_policy_fee.party_cnt=party.party_cnt WHERE event_policy_fee.insurance_file_cnt=EIF.insurance_file_cnt AND party.party_type_id=10) as renewal_extra,

(SELECT ISNULL(SUM(event_policy_fee.total_fee),0) FROM event_policy_fee INNER JOIN party ON event_policy_fee.party_cnt=party.party_cnt WHERE event_policy_fee.insurance_file_cnt=EIF.insurance_file_cnt AND party.party_type_id=9) as renewal_fee,

(SELECT ISNULL(SUM(TDD.outstanding_amount),0) FROM transdetail TDD INNER JOIN account AA ON TDD.account_id=AA.account_id INNER JOIN ledger LL ON AA.ledger_id=LL.ledger_id WHERE TDD.document_id=D.document_id AND LL.ledger_short_name='SA') as renewal_os,

(SELECT ISNULL(SUM(TM.base_match_amount),0)*-1 FROM TransMatch TM JOIN MatchGroup MG ON MG.match_id = TM.match_id
WHERE TM.transdetail_id = td.transdetail_id AND MG.match_date <= GetDate()) as accountenquiry_paid,	

(SELECT ISNULL(SUM(TM.base_match_amount),0)*-1 FROM TransMatch TM JOIN MatchGroup MG ON MG.match_id = TM.match_id
WHERE TM.transdetail_id = td.transdetail_id AND MG.match_date > GetDate()) as accountenquiry_os,

(SELECT SUM(ad.alloc_base_amount) FROM allocationdetail ad	WHERE td.transdetail_id = ad.transdetail_id ) as Settled

from Insurance_File as InsFile 
inner join document d on d.insurance_file_cnt = InsFile.Insurance_file_cnt 
left join transdetail td on td.document_id = d.document_id
left join cashlistitem cli on cli.transdetail_id=td.transdetail_id 
left join mediatype mt on cli.mediatype_id = mt.mediatype_id 
left JOIN transaction_export_folder TEF ON D.document_ref=TEF.document_ref AND D.company_id=TEF.source_id --AND TEF.product_id=36
left JOIN event_log EL ON TEF.event_log_id=EL.event_cnt
left JOIN event_insurance_file EIF ON EIF.insurance_file_cnt=EL.event_cnt AND EIF.product_id=36
left join pfPremiumFinance pf on td.TransDetail_id = pf.PlanTransaction_id and pf.statusIND IN ('040', '011', '900')		
left join pfInstalments pfi on pfi.pfprem_finance_cnt = pf.pfprem_finance_cnt AND pfi.pfprem_finance_version = pf.pfprem_finance_version and pfi.status <> 3			
left join allocationdetail ad ON cli.cashlistitem_id = ad.cashlistitem_id
left JOIN transmatch tm ON tm.allocationdetail_id = ad.allocationdetail_id AND tm.transdetail_id = ad.transdetail_id
--where  InsFile.product_id = 36
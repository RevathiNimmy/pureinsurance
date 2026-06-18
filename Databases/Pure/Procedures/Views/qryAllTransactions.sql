
DDLDropview 'qryAllTransactions'
go

create view [dbo].[qryAllTransactions] as
									select 
									(case when (
											select sum(amount)
											from TransDetail as TD1
											where spare = 'GROSS'
											and TD1.document_id = td.document_id
										) < 0 then (
											select sum(amount) + sum(ref_amount)
											from TransDetail as TD1
											where spare = 'GROSS'
											and TD1.document_id = td.document_id
										) else (
											select sum(amount) - sum(ref_amount)
											from TransDetail as TD1
											where spare = 'GROSS'
											and TD1.document_id = td.document_id
										) end)                                                  GROSS,
									(select sum(amount)
											from TransDetail as TD1
											where spare IN ('BROK', 'BROK ADJ')
											and TD1.document_id = td.document_id)       BKG,
									(select sum(amount)
										from TransDetail as TD1
										where spare IN ('AGENT', 'AGENT ADJ')
										and TD1.document_id = td.document_id)       AGENT,
									(select sum(amount)
										from TransDetail as TD1
										where spare IN ('COMM', 'COMM ADJ')
										and TD1.document_id = td.document_id)       Commission,
									(select sum(amount)
										from TransDetail as TD1
										where spare = ''
										and TD1.document_id = td.document_id)       CLIENT,
									(select sum(amount)
										from TransDetail as TD1
										where spare not in ('COMM', 'COMM ADJ', 'BROK', 'BROK ADJ', 'GROSS', 'AGENT', 'AGENT ADJ', '')
										and TD1.document_id = td.document_id)       OTHER,
									(select sum(Base_Match_Amount)
										from TransMatch
										inner join TransDetail as TD1 on TransMatch.transdetail_id = TD1.transdetail_id
										where TransMatch.base_match_amount <> TD1.amount
										and account_id = a.account_id
										and TD1.document_id = td.document_id
										and TransMatch.allocationdetail_id is not null)     Unallocated,
									td.account_id								as account_id,
									ps.description								as PostingStatus,
									s.description								as Branch,
									p.period_name								as Period,
									p.year_name									as PeriodYear,
									d.document_ref								as DocumentRef,
									convert(datetime,
										convert(char(10),
										d.document_date,103),103)				as TransactionDate,
									year(d.document_date)						as TransactionDateYear,
									month(d.document_date)						as TransactionDateMonth,
									day(d.document_date)						as TransactionDateDay,
									td.ref_date									as accounting_date,
									year(td.ref_date)							as AccountingYear,
									month(td.ref_date)							as AccountingMonth,
									Day(td.ref_date)							as AccountingDay,
									cur.description								as Currency,
									td.amount ,
									case td.fully_matched
										when 1 Then 'Yes'
										else 'No' END							as FullyMatched,
									td.currency_amount ,
									td.currency_base_xrate						as ExchangeRate,
									td.comment as TransactionType,
									td.insurance_ref							as insurance_ref,
									td.insurance_ref							as transaction_insurance_ref,
									pu.username									as Operator,
									td.purchase_order_no,
									td.purchase_invoice_no,
									td.spare,
									td.ref_date									as CoverDate,
									td.document_sequence						as DocumentSequence,
									case 
										when td.amount < 0 then (-1* td.ref_amount)
										else td.ref_amount end					as IPT,
									isnull(dep.description,'Not Known')			as Department,
									td.outstanding_amount,
									convert(datetime, convert(varchar(23), 
											td.amount_updated, 111))
																				as MatchedDate,
									tdtype.description							as TransdetailType,
									case td.risk_transfer
										wHEN 0 then 'RT'
										wHEN 1 then 'Unpaid'
										when 2 then 'Paid'
										when 3 then 'PaidM'
										when 4 then 'Rec' end					as RiskTransfer,
									td.risk_transfer_reconciliation_date		as RiskTransferDate,
									Consultant.shortname                        as AccountExecCode,
									Consultant.resolved_name                    as AccountExec,
									AccountHandler.shortname                    as AccountHandlerCode,
									AccountHandler.resolved_name                as AccountHandler,
									LeadInsurer.shortname                       as LeadInsurerCode,
									LeadInsurer.resolved_name                   as LeadInsurer,
									--Party.party_cnt                             as PartyID,
									Party.shortname                             as ClientCode,
									Party.Resolved_Name							as ClientResolvedName,
									dt.description								as DocumentType,
									convert(datetime, convert(varchar(23), 
											d.document_date, 111))				as document_date,
									td.document_id
									from transdetail td 
left join postingstatus ps on td.postingstatus_id = ps.postingstatus_id
									left join source s on td.company_id = s.source_id
									left join currency cur on td.currency_id = cur.currency_id
									left join period p on td.period_id = p.period_id
									left join document d on td.document_id = d.document_id
									left join pmuser pu on td.operator_ID = pu.user_id
									left join department dep on td.department_id = dep.department_id
									left join transdetail_type tdtype on td.transdetail_type_id = tdtype.transdetail_type_id
 									left join Insurance_File as InsFile on d.insurance_file_cnt = InsFile.Insurance_file_cnt
									left join account a on td.account_id = a.account_id
 									left join party on a.account_key = party.party_cnt
									left join Party as Consultant on Party.consultant_cnt = Consultant.party_cnt
									left join Party as AccountHandler on InsFile.Account_Handler_cnt = AccountHandler.Party_cnt
									left join Party as LeadInsurer on InsFile.Lead_Insurer_cnt = LeadInsurer.Party_cnt
									left join documenttype dt on d.documenttype_id = dt.documenttype_id
									where td.not_reported is null
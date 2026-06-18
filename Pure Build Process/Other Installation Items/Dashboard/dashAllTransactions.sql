DDLDropview 'dashAllTransactions'
go
create view [dbo].[dashAllTransactions] as
select 
InsFile.insurance_folder_cnt 			as InsuranceFolderCnt,
td.transdetail_id 						as TransactionId,	
td.account_id							as account_id,
a.short_code							as AccountCode,
ps.description							as PostingStatus,
s.description							as Branch,
p.period_name							as Period,
p.year_name								as PeriodYear,
d.document_ref							as DocumentRef,
d.document_date							as TransactionDate,
td.ref_date								as accounting_date,

--Modify on 19 Aug 2009 to get the correct IPT sync with S4B Demo Blade
/*
case 
	when td.amount < 0 then (-1* td.ref_amount)
	else td.ref_amount end					as IPT,
year(d.document_date)						as TransactionDateYear,
month(d.document_date)						as TransactionDateMonth,
day(d.document_date)						as TransactionDateDay,
year(td.ref_date)						as AccountingYear,
month(td.ref_date)						as AccountingMonth,
Day(td.ref_date)						as AccountingDay,
td.currency_amount ,
td.currency_base_xrate						as ExchangeRate,
cur.description							as Currency,
td.purchase_order_no,
td.purchase_invoice_no,
isnull(dep.description,'Not Known')			as Department,
td.outstanding_amount,
convert(datetime, convert(varchar(23), td.amount_updated, 111))	as MatchedDate,
convert(datetime, convert(varchar(23), d.document_date, 111))			as document_date,
TransInsurer.shortname 	as TransInsurer,
LeadInsurer.shortname                       as LeadInsurerCode,
LeadInsurer.resolved_name                   as LeadInsurer,

--For Account Enquiry - Outstanding Amount
--case when MG.match_date <= GetDate() then ISNULL(TM.base_match_amount,0) else 0 end	as accountenquiry_paid,	
--case when MG.match_date > GetDate() then ISNULL(TM.base_match_amount,0)	else 0 end	as accountenquiry_os,

mt.description							as TransactionPaymentMethod,
IsNull(s.description, 'N/A')			as Branch,
CASE dt.code
	WHEN 'JN' THEN 'JRN'
	WHEN 'SND' THEN 'NBD'
	WHEN 'SNC' THEN 'NBC'
	WHEN 'SWD' THEN 'WO'
	WHEN 'SRD' THEN 'RND'
	WHEN 'SRC' THEN 'RNC'
	WHEN 'SED' THEN 'END'
	WHEN 'SEC' THEN 'END'
	WHEN 'SRP' THEN 'REC'
	WHEN 'SPY' THEN 'PAY'
	WHEN 'CLP' THEN 'CLMP'
	WHEN 'CLR' THEN 'CLMR'
	ELSE dt.code
END as TransType,
td.amount - td.outstanding_amount as AllocAmount,
DATEDIFF(DAY,d.document_date, GetDate()) as DaysOverDue,
(SELECT td.amount WHERE spare = 'ALLOCATED') as TransferAllocAmount
case td.fully_matched when 1 Then 'Yes' else 'No' END		as FullyMatched,
*/

td.amount ,
td.outstanding_amount,
td.fully_matched as FullyMatched,
td.comment as TransactionType,
td.insurance_ref						as insurance_ref,
d.insurance_file_cnt						as insurance_file_cnt,
td.insurance_ref						as transaction_insurance_ref,
pu.username							as Operator,
td.spare,
td.ref_date							as CoverDate,
td.document_sequence						as DocumentSequence,
tdtype.description						as TransdetailType,
tdtype.code							as TransdetailCode,
Consultant.shortname                        as AccountExecCode,
Consultant.resolved_name                    as AccountExec,
AccountHandler.shortname                    as AccountHandlerCode,
AccountHandler.resolved_name                as AccountHandler,
Party.party_cnt                           as PartyCnt,
Party.shortname                             as ClientCode,
Party.Resolved_Name					as ClientResolvedName,
dt.description						as DocumentType,
dt.code as DocTypeCode,
td.document_id,
l.ledger_short_name						as LedgerCode, 
pt.code								as PartyCode,
pt.description 							as PartyDescription

from Insurance_File as InsFile 
INNER join document d on d.insurance_file_cnt = InsFile.Insurance_file_cnt
left join transdetail td on td.document_id = d.document_id
left join postingstatus ps on td.postingstatus_id = ps.postingstatus_id
left join period p on td.period_id = p.period_id
left join source s on d.company_id = s.source_id
left join pmuser pu on td.operator_ID = pu.user_id
left join transdetail_type tdtype on td.transdetail_type_id = tdtype.transdetail_type_id
left join account a on td.account_id = a.account_id
left join ledger l on l.ledger_id = a.ledger_id
left join party on a.account_key = party.party_cnt
left join party_type pt on pt.party_type_id = party.party_type_id
left join Party as Consultant on Party.consultant_cnt = Consultant.party_cnt
left join Party as AccountHandler on InsFile.Account_Handler_cnt = AccountHandler.Party_cnt
left join documenttype dt on d.documenttype_id = dt.documenttype_id
--WHERE InsFile.product_id = 36
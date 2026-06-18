DDLDropview 'dashAllPartyDetails'
go
create view dashAllPartyDetails as
select
p.party_cnt 												as party_cnt,
pt.description												as PartyType,
p.party_cnt												as PartyCnt,
p.shortname									as ClientCode,
p.name										as Name,
p.resolved_name									as ResolvedName,
case p.is_deleted
	when 0 then 'No'
	else 'Yes' end								as IsDeleted,
-- KCU
replace(p.name,'''','')										as KCUName,
isnull(consultant.resolved_name,'N/A')						as Consultant
from party p 
left join party_type pt on p.party_type_id = pt.party_type_id
left join party consultant on p.consultant_cnt = consultant.party_cnt
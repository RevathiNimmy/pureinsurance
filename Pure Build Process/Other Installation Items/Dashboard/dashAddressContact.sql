DDLDropview 'dashAddressContact'
go
create view [dbo].[dashAddressContact] as 
select
    PAU.party_cnt   ClientID,
    CASE 
		WHEN len(rtrim(extension))=0 THEN rtrim(area_code) + ' ' + rtrim(number)
		ELSE rtrim(area_code) + ' ' + rtrim(number) + ' ex ' + rtrim(extension)
	END as Telephone,
	area_code,
	number,
	extension
from Party_Address_Usage PAU
join Contact_Address_Usage CAU 
	on CAU.address_cnt = PAU.address_cnt
JOIN Contact C
		ON C.contact_cnt = CAU.contact_cnt
JOIN Contact_Type CT
		ON CT.contact_type_id = C.contact_type_id AND CT.code = 'TELEPHONE'

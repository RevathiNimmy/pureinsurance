SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_EDI_DbData'
GO

CREATE PROCEDURE spu_Get_EDI_DbData
	@InsFileCnt integer
AS
select 
	source.sender_mailbox_id 'SenderIdOperator', 
	source.description 'SenderIdDescription', 
        gs.edi_mail_box 'RecipientIdOperator',
        p2.name 'RecipientIdDescription',
	iff.insurance_ref 'PolicyNo', 
	iff.cover_start_date 'StartDate',
	convert(varchar,iff.cover_start_date,108) 'StartTime',
	iff.renewal_date 'EndDate',
	convert(varchar,iff.renewal_date,108) 'EndTime',
	renewal_frequency.number_of_months 'RenewalFrequency', 
        CASE
            WHEN party_type.code = 'PC' then 'P'
            WHEN party_type.code = 'CC' then 'C'
            else ''
        END as 'PartyType',
	iff.this_premium 'PremiumIncIpt', 
	party_insurer.agency_number 'AgencyAccountNumber',
        CASE
            WHEN insurance_file_type.code='QUOTE' THEN ''
            WHEN insurance_file_type.code in ('MTAQUOTE', 'MTA PERM','MTA TEMP') then 'ADJ'
            WHEN insurance_file_Type.code='POLICY' and insurance_file_status.code is null THEN 'PRO'
            WHEN insurance_file_type.code='POLICY' and insurance_file_status.code='CAN' THEN 'CAN'
            WHEN insurance_file_type.code='POLICY' and insurance_file_status.code='LAP' THEN 'PLL'
            WHEN insurance_file_type.code='POLICY' and insurance_file_status.code='REN' THEN 'RNC'
            WHEN insurance_file_type.code='POLICY' and insurance_file_status.code='REP' THEN 'RNC'
            ELSE ''
        END as 'POLPREVEVENT',
        CASE
            WHEN insurance_file_type.code='QUOTE' THEN ltrim(right(rtrim(iff.insurance_ref),11))
            WHEN insurance_file_type.code in ('MTAQUOTE', 'MTA PERM','MTA TEMP') then ltrim(right(rtrim(iff.insurance_ref),11)) + 'ADJ'
            WHEN insurance_file_Type.code='POLICY' and insurance_file_status.code is null THEN ltrim(right(rtrim(iff.insurance_ref),11)) + 'PRO'
            WHEN insurance_file_type.code='POLICY' and insurance_file_status.code='CAN' THEN ltrim(right(rtrim(iff.insurance_ref),11)) + 'CAN'
            WHEN insurance_file_type.code='POLICY' and insurance_file_status.code='LAP' THEN ltrim(right(rtrim(iff.insurance_ref),11)) + 'PLL'
            WHEN insurance_file_type.code='POLICY' and insurance_file_status.code='REN' THEN ltrim(right(rtrim(iff.insurance_ref),11)) + 'RNC'
            WHEN insurance_file_type.code='POLICY' and insurance_file_status.code='REP' THEN ltrim(right(rtrim(iff.insurance_ref),11)) + 'RNC'
            ELSE ltrim(right(rtrim(iff.insurance_ref),11))
        END as 'MSGREF'
from insurance_file iff 
join source on source.source_id = iff.source_id
join renewal_frequency on renewal_frequency.renewal_frequency_id = iff.renewal_frequency_id
join party on party.party_cnt = iff.insured_cnt
join party_type on party_type.party_type_id = party.party_type_id
join party_insurer on party_insurer.party_cnt = iff.lead_insurer_cnt
join party p2 on p2.party_cnt = iff.lead_insurer_cnt
left outer join gis_policy_link gpl on gpl.insurance_file_cnt = iff.insurance_file_cnt
left outer join gis_scheme gs on gs.gis_scheme_id = gpl.gis_scheme_id
left outer join insurance_file_status on insurance_file_status.insurance_file_status_id = iff.insurance_file_status_id
left outer join insurance_file_type on insurance_file_type.insurance_file_type_id = iff.insurance_file_type_id
where iff.insurance_file_cnt=@InsFileCnt

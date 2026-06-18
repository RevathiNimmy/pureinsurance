--For Risk Bdx Report
DDLDropview 'dashAllPolicies_Rating'
go
Create view [dbo].[dashAllPolicies_Rating] as
select distinct insurance_file.insurance_ref,
	insurance_file.insurance_file_cnt,
	insurance_file.insurance_folder_cnt,
	
	insurance_file_risk_link.risk_cnt,
	rating_section_type.description as RatingSection,
	rating_section.this_premium as Rating_Premium 
from	
	insurance_file,
	insurance_file_risk_link,
	rating_section,
	rating_section_type
where 
	insurance_file.insurance_file_type_id in (2, 5, 6, 8, 9) and
	insurance_file.insurance_file_cnt=insurance_file_risk_link.insurance_file_cnt and
	rating_section.risk_cnt = insurance_file_risk_link.risk_cnt and
	--insurance_file_risk_link.status_flag='C' and
	rating_section_type.rating_section_type_id=rating_section.rating_section_type_id --and
	--insurance_file.product_id=36
	--and rating_section.this_premium>0
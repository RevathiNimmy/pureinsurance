ddldropprocedure 'spu_Brokerlink_EBordereau_Get_Section_Data'
go

CREATE PROCEDURE spu_Brokerlink_EBordereau_Get_Section_Data
    @event_insurance_file_cnt INT
AS
select 
crs.code 'SectionCode', 
crs.description 'SectionDescription', 
eics.Premium_Excluding_Tax 'PremiumExcludingTax'
from 
event_insurance_COB_section eics
join COB_Rating_section crs on crs.COB_Rating_section_id = eics.COB_Rating_section_id
where 
insurance_file_cnt=@event_insurance_file_cnt

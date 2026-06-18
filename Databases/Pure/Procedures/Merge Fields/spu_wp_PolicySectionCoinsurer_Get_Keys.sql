ddldropprocedure 'spu_wp_PolicySectionCoinsurer_Get_Keys'
go

CREATE PROCEDURE spu_wp_PolicySectionCoinsurer_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
select 
	pcs.policy_coinsurers_section_id 
from 
	policy_coinsurers_section pcs
	join insurance_cob_section ics 
		on ics.cob_rating_section_id = pcs.cob_rating_section_id
		and ics.insurance_file_cnt = pcs.insurance_file_cnt
where 
	pcs.insurance_file_cnt=@InsuranceFileCnt and 
	ics.insurance_section_id=@Instance2
go
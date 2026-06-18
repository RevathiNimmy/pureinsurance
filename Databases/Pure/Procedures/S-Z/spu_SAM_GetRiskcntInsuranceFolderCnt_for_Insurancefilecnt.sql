SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_GetRiskcntInsuranceFolderCnt_for_Insurancefilecnt'
GO

create procedure spu_SAM_GetRiskcntInsuranceFolderCnt_for_Insurancefilecnt
	@insurance_file_cnt integer
as
select 
	ifrl.risk_cnt,
	iff.insurance_folder_cnt
from 
	insurance_file iff
	join insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = iff.insurance_file_cnt
where 
	iff.insurance_file_cnt = @insurance_file_cnt
	and ifrl.status_flag='C'
GO

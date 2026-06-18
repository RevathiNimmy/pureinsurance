ddldropprocedure 'spu_sam_getPfRecordsForCurrentPeriod'
go

CREATE PROCEDURE spu_sam_getPfRecordsForCurrentPeriod
    @insurance_file_cnt integer
AS
Begin
	select isnull(pfpf.pfprem_finance_cnt,isnull(pfti.pfprem_finance_cnt,0)), isnull(pfpf.pfprem_finance_version,isnull(pfti.pfprem_finance_version,0))  from insurance_file iff
	join insurance_file iff2 on iff2.insurance_folder_cnt = iff.insurance_folder_cnt and iff2.inception_date_tpi = iff.inception_date_tpi and iff.insurance_file_cnt <> iff2.insurance_file_cnt
left outer join pftransaction_id pfti on pfti.insurance_file_cnt = iff2.insurance_file_cnt
left outer join pfpremiumfinance pfpf on pfpf.insurance_file_cnt = iff2.insurance_file_cnt
	where iff.insurance_file_cnt = @insurance_file_cnt and ((pfti.pfprem_finance_cnt is not null) or (pfpf.pfprem_finance_cnt is not null))
End
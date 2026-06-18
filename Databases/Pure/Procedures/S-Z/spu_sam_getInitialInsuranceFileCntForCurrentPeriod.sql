ddldropprocedure 'spu_sam_getInitialInsuranceFileCntForCurrentPeriod'
go

create procedure spu_sam_getInitialInsuranceFileCntForCurrentPeriod
	@insurance_file_cnt integer
as
declare @inception_date_tpi datetime
declare @insurance_folder_cnt integer

select @inception_Date_tpi = inception_Date_tpi, @insurance_folder_cnt = insurance_folder_cnt from insurance_file where insurance_file_cnt=@insurance_file_cnt

select min(insurance_file_cnt) from insurance_file where insurance_foldeR_cnt=@insurance_folder_cnt
and inception_date_tpi=@inception_Date_tpi

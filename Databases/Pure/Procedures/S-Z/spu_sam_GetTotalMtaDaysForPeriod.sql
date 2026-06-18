ddldropprocedure 'spu_sam_GetTotalMtaDaysForPeriod'
go

create procedure spu_sam_GetTotalMtaDaysForPeriod
	@insurance_file_cnt integer
as
declare @inception_date_tpi datetime
declare @insurance_folder_cnt integer

select @inception_Date_tpi = inception_Date_tpi, @insurance_folder_cnt = insurance_folder_cnt from insurance_file where insurance_file_cnt=@insurance_file_cnt

select isnull(sum(datediff(dd,cover_start_date,expiry_date)),0) from insurance_file join insurance_file_type on insurance_file_type.insurance_file_type_id = insurance_file.insurance_file_type_id where insurance_foldeR_cnt=@insurance_folder_cnt
and inception_date_tpi=@inception_Date_tpi and insurance_file_type.code in ('MTA PERM','MTA TEMP')

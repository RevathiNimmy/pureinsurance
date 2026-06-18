SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_mta_quotes_sel'
GO


CREATE PROCEDURE spu_SAM_mta_quotes_sel
    @Insurance_Folder_Cnt integer
AS
	select 
		insurance_file.insurance_file_cnt,
		insurance_file_system.date_created,
		case insurance_file_type.code 
			when 'MTAQUOTE' then 'Permanent' 
			when 'MTAQTETEMP' then 'Temporary'
			else '' 
		end MTA_Type,
		event_log.description 'MTA Description',
		source.code 'Branch Code'
	from 
		insurance_file
		join insurance_file_type on insurance_file.insurance_file_type_id = insurance_file_type.insurance_file_type_id
		join insurance_file_system on insurance_file.insurance_file_cnt = insurance_file_system.insurance_file_cnt
		join event_log on event_log.insurance_file_cnt = insurance_file.insurance_file_cnt
		join event_type on event_type.event_type_id = event_log.event_type_id
		join source on source.source_id = insurance_file.source_id
	where 
		insurance_file_type.code in ('MTAQUOTE','MTAQTETEMP') and 
		event_type.code='NEWPOLICY' and
		insurance_file.insurance_folder_cnt = @Insurance_Folder_Cnt

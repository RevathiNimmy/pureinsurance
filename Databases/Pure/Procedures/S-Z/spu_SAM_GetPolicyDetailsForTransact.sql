SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_GetPolicyDetailsForTransact'
GO

create procedure spu_SAM_GetPolicyDetailsForTransact
	@insurance_file_cnt integer
as
	select cover_start_date,lead_insurer_cnt, risk_code_id from insurance_file where insurance_file_cnt = @insurance_file_cnt

GO

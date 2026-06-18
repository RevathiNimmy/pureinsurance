SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_ins_folder_file_cnt'
GO

CREATE PROCEDURE spu_get_ins_folder_file_cnt
	@claim_id int
AS

select insurance_file_cnt,insurance_folder_cnt
from insurance_file
inner join claim on claim.policy_id = insurance_file.insurance_file_cnt
where claim.claim_id=@claim_id

GO
ddldropprocedure 'spu_SAM_subbranch_check'
go

CREATE PROCEDURE spu_SAM_subbranch_check
	@branch_code varchar(10),
	@subbranch_code varchar(10)

AS

	select 
		sub_branch.sub_branch_id 
	from 
		source 
		join sub_Branch on source.source_id = sub_branch.source_id 
	where 
		source.code = @branch_code and 
		sub_branch.code = @subbranch_code

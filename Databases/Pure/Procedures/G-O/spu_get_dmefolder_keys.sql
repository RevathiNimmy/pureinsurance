
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_dmefolder_keys'
GO

CREATE procedure spu_get_dmefolder_keys 
	@folder_num	int,
	@error_code	 varchar(50) = null output
as

declare @Party_cnt				int,
		@insurance_folder_cnt	int,
		@claim_cnt				int,
		@insurance_file_cnt		int,
		@folder_level			tinyint,
		@ex_code				varchar(20),
		@parent_num				int




select	@ex_code = ex_code,
		@folder_level = folder_level,
		@parent_num = parent_num 
from	doc_folder
where	folder_num = @folder_num	


if rtrim(ltrim(@ex_code)) = '' or lower(rtrim(ltrim(@ex_code))) = 'general'
	begin
		GOTO Err_Add_Stats_Details
	end


if  @folder_level = 1
	begin
			--Get th Party Key
			set @party_cnt = convert(int,@ex_code)
	end
else if @folder_level <> 1
	if LOWER(left(@ex_code,6)) = 'c00000'  -- This is Claims Folder
		begin
			set @claim_cnt = convert(int,right(@ex_code,len(@ex_code)-6))
			
			select	@insurance_file_cnt = policy_id, 
					@insurance_folder_cnt = insurance_folder_cnt, 
					@party_cnt = insured_cnt 
			from	claim c
			inner join insurance_file inf
			on inf.insurance_file_cnt = c.policy_id
			where claim_id = @claim_cnt
		end
	else	-- This is Insurance Folder					
		begin
			set  @insurance_folder_cnt = convert(int,@ex_code)
			
			select	@insurance_folder_cnt = insurance_folder_cnt, 
					@party_cnt = insurance_holder_cnt
			from	insurance_folder
			where	insurance_folder_cnt = @insurance_folder_cnt

		end

select	@party_cnt	party_cnt,
		@insurance_file_cnt	insurance_file_cnt,
		@insurance_folder_cnt	insurance_folder_cnt,
		@claim_cnt	claim_cnt
return

Err_Add_Stats_Details: 
select	@party_cnt	party_cnt,
		@insurance_file_cnt	insurance_file_cnt,
		@insurance_folder_cnt	insurance_folder_cnt,
		@claim_cnt	claim_cnt
set @error_code = 'NoKeysAttached'
	
	
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO






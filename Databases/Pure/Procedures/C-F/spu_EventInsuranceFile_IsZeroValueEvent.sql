ddldropprocedure 'spu_EventInsuranceFile_IsZeroValueEvent'
go

create procedure spu_EventInsuranceFile_IsZeroValueEvent
	(@Insurance_File_Cnt integer)
as
declare @return integer
if (
	select 
		count(insurance_file_cnt) 
	from 
		event_insurance_cob_section 
	where 
		insurance_file_cnt = @Insurance_File_Cnt
		and (premium_excluding_tax <> 0 or premium_including_tax <> 0 or commission_net <> 0 or commission_payable <> 0)) > 0
Begin 
	select @return = 0 
end
else 
	Begin 
		select @return = 1
	end

if @return = 1
Begin
	--check if
	if (select count(*) from event_policy_fee)>0
	Begin
		select @return = 0	
	End
End 

select @return
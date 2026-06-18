ddldropprocedure spu_coinsurer_event_del
go

create procedure spu_coinsurer_event_del 
	@insurance_file_cnt int
as	
	delete from event_policy_coinsurers where insurance_file_cnt=@insurance_file_cnt
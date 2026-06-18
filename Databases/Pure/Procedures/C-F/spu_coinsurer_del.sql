ddldropprocedure spu_coinsurer_del
go

create procedure spu_coinsurer_del 
	@insurance_file_cnt int
as	
	delete from policy_coinsurers where insurance_file_cnt=@insurance_file_cnt
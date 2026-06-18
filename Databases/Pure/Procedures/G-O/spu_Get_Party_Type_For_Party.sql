SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Party_Type_For_Party'
GO

create procedure spu_Get_Party_Type_For_Party
	@PartyCnt	int
as
begin

	select	pt.party_type_id,
		pt.caption_id,
		pt.code,
		pt.description,
		pt.party_other_posting_type_id
	from	party_type pt
		join party p on p.party_type_id = pt.party_type_id
	where	p.party_cnt = @PartyCnt

end
GO
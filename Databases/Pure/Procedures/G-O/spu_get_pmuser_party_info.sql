SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_pmuser_party_info'
GO

CREATE PROCEDURE spu_get_pmuser_party_info
	@user_id as integer

AS

BEGIN

	select 	p1.name,
		pt1.code,
		p2.resolved_name,
		pt2.code,
		ch.description,
		p3.resolved_name,   --(RC) WR34
		pu.party_cnt,
		p1.shortname
	from 	pmuser pu
	left outer join	party p1
	on	pu.party_cnt = p1.party_cnt
	left outer join	party_type pt1
	on	pt1.party_type_id = p1.party_type_id
	left outer join	party p2
	on 	pu.party_handler_id = p2.party_cnt
	left outer join	party p3  --(RC) WR34
	on 	pu.other_party_id = p3.party_cnt
	left outer join	party_type pt2
	on	pt2.party_type_id = p2.party_type_id
	left outer join	handler ch
	on	pu.claim_handler_id = ch.handler_id
	where	pu.user_id = @user_id

END
GO

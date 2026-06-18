SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_clm_party_dtls'
GO


CREATE PROCEDURE spu_get_clm_party_dtls
 @shortname char(20),
 @addresstype int

AS

Declare @PrtyCnt int

	select 	@PrtyCnt=Count(c.area_code)
 		from 	party p,
			party_address_usage pau,
			address a,
			contact_address_Usage cau,
			contact c,
			contact_type ct
		where 	p.shortname = @shortname
		and 	pau.party_cnt = p.party_cnt
		and	pau.address_usage_type_id = @addresstype
		and	a.address_cnt = pau.address_cnt
		and	cau.address_cnt = a.address_cnt
 		and	c.contact_cnt = cau.contact_cnt
 		and	ct.contact_type_id = c.contact_type_id 		

	If @PrtyCnt > 0
	BEGIN
		select 	p.name, p.shortname, 
			a.address1, a.address2, a.address3, a.address4, a.postal_code,
			c.area_code, c.number, c.extension, 
			ct.contact_type_id, ct.code, p.party_cnt, a.address_cnt
 		from 	party p,
			party_address_usage pau,
			address a,
			contact_address_Usage cau,
			contact c,
			contact_type ct
		where 	p.shortname = @shortname
		and 	pau.party_cnt = p.party_cnt
		and	pau.address_usage_type_id = @addresstype
		and	a.address_cnt = pau.address_cnt
		and	cau.address_cnt = a.address_cnt
 		and	c.contact_cnt = cau.contact_cnt
 		and	ct.contact_type_id = c.contact_type_id 		
	END
	else
	BEGIN
		SELECT Party.name, Party.shortname, Address.address1,
 		Address.address2, Address.address3, Address.address4,
 		Address.postal_code, NULL area_code, NULL number,
 		NULL extension, NULL contact_type_id,
 		NULL code, Party.party_cnt, Address.address_cnt
		from Party,party_Address_usage,Address
		where Party.party_cnt= Party_Address_Usage.Party_cnt AND
		Party_Address_Usage.Address_cnt=Address.Address_cnt AND
		party_address_usage.address_usage_type_id = @addresstype
		AND Party.shortname = @shortname
	END
GO

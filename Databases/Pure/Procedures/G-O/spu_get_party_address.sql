SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_party_address'
GO


CREATE PROCEDURE spu_get_party_address
 @shortname char(20),
 @addresstype int

AS

Declare @PrtyCnt int

	IF @addresstype > 0
	BEGIN
		SELECT 	pau.address_usage_type_id,
				a.address1,
				a.address2,
				a.address3,
				a.address4,
				a.postal_code,
				a.address_cnt

 		FROM	 	party p,
				party_address_usage pau,
				address a

		WHERE	p.shortname = @shortname
			and 	pau.party_cnt = p.party_cnt
			and	pau.address_usage_type_id = @addresstype
			and	a.address_cnt = pau.address_cnt

	END
	ELSE
	BEGIN
		SELECT	pau.address_usage_type_id,
				a.address1,
 				a.address2,
				a.address3,
				a.address4,
 				a.postal_code,
				a.address_cnt

		FROM		party p,
				party_address_usage pau,
				address a

		WHERE	p.party_cnt = pau.party_cnt
			and	pau.address_cnt = a.address_cnt
			and	pau.address_usage_type_id = @addresstype
			and	p.shortname = @shortname
	END
GO









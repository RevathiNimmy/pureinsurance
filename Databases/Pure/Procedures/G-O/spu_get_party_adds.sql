SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_party_adds'
GO


CREATE PROCEDURE spu_get_party_adds
    @prty_id int
AS


SELECT address_cnt
FROM Party_Address_Usage
WHERE (party_cnt = @prty_id)
GO



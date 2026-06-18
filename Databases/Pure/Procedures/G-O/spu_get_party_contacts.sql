SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_party_contacts'
GO


CREATE PROCEDURE spu_get_party_contacts
    @prty_id int
AS


SELECT contact_cnt
FROM Party_Contact_Usage
WHERE (party_cnt = @prty_id)
GO



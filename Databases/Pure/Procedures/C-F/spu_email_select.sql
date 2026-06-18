SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_email_select'
GO


CREATE PROCEDURE spu_email_select
    @party_cnt int
AS BEGIN

--**********************************************************************************************
-- Author : Paul Lee 
-- 
-- History: 29/05/2003 PSL - Created 
--**********************************************************************************************

    SELECT c.contact_cnt, c.area_code, c.number, c.extension, ct.code, aut.code, c.description
    FROM 
        contact c, 
        contact_type ct, 
        party_address_usage pau, 
        address_usage_type aut, 
        party_contact_usage pcu 
    WHERE pau.party_cnt = @party_cnt 
    AND pau.party_cnt = pcu.party_cnt
    AND pcu.contact_cnt =c.contact_cnt 
    AND pau.address_usage_type_id = aut.address_usage_type_id 
    AND c.contact_type_id = ct.contact_type_id
END

GO


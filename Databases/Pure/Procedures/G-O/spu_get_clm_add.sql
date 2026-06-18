SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_clm_add'
GO


CREATE PROCEDURE spu_get_clm_add
    @add_cnt int
AS

-- 05/06/2001 Jude Killip - retrieve country ID
SELECT address1, address2, address3, address4, postal_code, country_id
FROM Claim_Address
WHERE (address_cnt = @add_cnt)
GO



/*
**********************************************************************************************************************************
** WHEN     WHO     WHAT
** 19-01-2005   JT      For Matching the Duplicates
** 20-01-2005   RKS     Modified
***********************************************************************************************************************************
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_party_matchbyshortname'
GO

CREATE PROCEDURE spu_party_matchbyshortname
    @shortname varchar(20)
AS

IF EXISTS(SELECT * FROM party WHERE shortname = @shortname)
    SELECT top 500 party.shortname, party.resolved_name, address.address1,
        address.address2, address.postal_code,party_type.description AS party_type,
        source.description AS branch,
        party.party_cnt,
        party_type.code as party_type_code 
    FROM party
    JOIN source ON source.source_id = party.source_id 
    JOIN party_type ON party_type.party_type_id = party.party_type_id 
    JOIN party_address_usage pau ON pau.party_cnt = party.party_cnt 
    JOIN Address ON Address.address_cnt = pau.address_cnt 
    WHERE shortname LIKE @shortname + '%' 
    ORDER BY shortname

GO
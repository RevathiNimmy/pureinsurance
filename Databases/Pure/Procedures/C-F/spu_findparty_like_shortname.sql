SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_findparty_like_shortname'
GO


CREATE PROCEDURE spu_findparty_like_shortname
    @shortname varchar(60),
    @addresstypecode varchar(10),
    @agentonly integer
AS


BEGIN
IF @agentonly=1
    BEGIN

    SELECT      party.party_cnt,
            party_type.description,
            party.shortname,
            party.name,
            address.address1,
            address.postal_code,
            party.source_ID,
            party.party_ID
          FROM  party,
            party_agent,
            party_type,
            party_address_usage,
            address,
            address_usage_type

          WHERE party.shortname like @shortname + '%'
            AND party_address_usage.party_cnt = party.party_cnt
            AND party_address_usage.address_cnt = address.address_cnt
            AND address_usage_type.address_usage_type_id
=party_address_usage.address_usage_type_id
            AND address_usage_type.code = @addresstypecode
            AND party.party_type_id = party_type.party_type_id
            AND party.party_cnt = party_agent.party_cnt

        ORDER BY shortname
    END
ELSE
    BEGIN

    SELECT      party.party_cnt,
            party_type.description,
            party.shortname,
            party.name,
            address.address1,
            party.source_ID,
            party.party_ID,
            address.postal_code
          FROM  party,
            party_type,
            party_address_usage,
            address,
            address_usage_type

          WHERE party.shortname like @shortname + '%'
            AND party_address_usage.party_cnt = party.party_cnt
            AND party_address_usage.address_cnt = address.address_cnt
            AND address_usage_type.address_usage_type_id =
party_address_usage.address_usage_type_id
            AND address_usage_type.code = @addresstypecode
            AND party.party_type_id = party_type.party_type_id

        ORDER BY shortname
    END
END
GO



SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_FindParty_NoFilter'
GO


CREATE PROCEDURE [dbo].[spu_FindParty_NoFilter]
    @MaxRecords INT = 500,
    @SourceId   INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #PartyKeys (party_cnt INT PRIMARY KEY)

    INSERT INTO #PartyKeys (party_cnt)
    SELECT TOP (@MaxRecords) p.party_cnt
    FROM Party p WITH(NOLOCK)
    WHERE p.is_deleted = 0
        AND (@SourceId IS NULL OR p.source_id = @SourceId)
    ORDER BY p.shortname

    SELECT
        p.party_cnt,
        pc.caption,
        p.shortname,
        p.resolved_name,
        a.address1,
        CASE
            WHEN a.postal_code = CONVERT(VARCHAR(20), a.address_id) THEN ''
            ELSE a.postal_code
        END AS postal_code,
        p.source_id,
        p.party_id,
        '' AS area_code,
        '' AS number,
        p.is_prospect,
        p.invariant_key,
        'Sirius',
        ' ',
        CASE pt.code
            WHEN 'AG' THEN 'Agent'
            WHEN 'CC' THEN 'Corporate Client'
            WHEN 'GC' THEN 'Group Client'
            WHEN 'PC' THEN 'Personal Client'
            ELSE ' '
        END,
        p.file_code,
        (SELECT TOP 1 pl.date_of_birth
         FROM Party_Lifestyle pl WITH(NOLOCK)
         WHERE pl.party_cnt = p.party_cnt AND pl.category = 1) AS date_of_birth,
        p.swift_party_id,
        a.address2,
        ' ',
        CASE s.is_deleted
            WHEN 1 THEN RTRIM(s.description) + ' (Closed)'
            WHEN 0 THEN s.description
        END AS description,
        p.Agent_Cnt,
        p.record_status,
        pt.code,
        pa.date_cancelled,
        pnd.online_status,
        CASE WHEN p.party_type_id = '1' THEN p.name ELSE '' END AS name,
        CASE WHEN p.party_type_id = '1' THEN ppc.forename ELSE '' END AS forename,
        a.address3,
        a.address4,
        p.currency_id,
        p.domiciled_for_tax,
        '',
        sl.Code AS ServiceLevelCode,
        sl.Description AS ServiceLevelDescription
    FROM #PartyKeys pk
    INNER JOIN Party p WITH(NOLOCK)
        ON pk.party_cnt = p.party_cnt
    INNER JOIN Party_Type pt WITH(NOLOCK)
        ON p.party_type_id = pt.party_type_id
    LEFT OUTER JOIN source s WITH(NOLOCK)
        ON p.source_id = s.source_id
    INNER JOIN PMCaption pc WITH(NOLOCK)
        ON pt.caption_id = pc.caption_id
    LEFT OUTER JOIN (
        Party_Address_Usage pau WITH(NOLOCK)
        INNER JOIN Address a WITH(NOLOCK)
            ON pau.address_cnt = a.address_cnt
        INNER JOIN Address_Usage_Type aut WITH(NOLOCK)
            ON pau.address_usage_type_id = aut.address_usage_type_id
            AND aut.code = '3131 XCO'
    ) ON p.party_cnt = pau.party_cnt
    LEFT OUTER JOIN Party_Agent pa WITH(NOLOCK)
        ON p.party_cnt = pa.party_cnt
    LEFT OUTER JOIN party_net_data pnd WITH(NOLOCK)
        ON p.party_cnt = pnd.party_cnt
    LEFT OUTER JOIN Party_Personal_Client ppc WITH(NOLOCK)
        ON p.party_cnt = ppc.party_cnt
    LEFT OUTER JOIN Service_Level sl WITH(NOLOCK)
        ON p.service_level_id = sl.service_level_id
    ORDER BY p.shortname

    DROP TABLE #PartyKeys
END
GO

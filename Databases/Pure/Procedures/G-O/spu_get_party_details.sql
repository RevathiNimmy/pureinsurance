

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_party_details'
GO

CREATE PROCEDURE spu_get_party_details
        @party_type CHAR(10)
        
AS

BEGIN

    SELECT  p.party_cnt, 
        p.shortname, 
        p.resolved_name,
        ph.forename,
        ph.initials,
        ph.department_id,
        ph.party_title_code,
        p.currency_id,
        p.name,
        ph.commission_cnt
    FROM    party p
    JOIN    party_type pt
    ON  p.party_type_id = pt.party_type_id
    AND pt.code = @party_type
    LEFT OUTER JOIN     party_handler ph
    ON  ph.party_cnt = p.party_cnt
    ORDER BY p.name
END
GO

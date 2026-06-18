SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMB_Get_Commission_Account'
GO

CREATE PROCEDURE spu_PMB_Get_Commission_Account
    @party_type_id integer
AS

SELECT
    party_cnt,
    shortname,
    resolved_name,
    source_id
FROM
    party
WHERE
    is_deleted = 0 AND
    party_type_id = @party_type_id
ORDER BY
    source_id, 
    shortname

GO

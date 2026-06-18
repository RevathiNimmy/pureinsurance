SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMB_Source_Sel'
GO

CREATE PROCEDURE spu_PMB_Source_Sel
AS

SELECT
    [source_id],
    [code],
    [description]
FROM
    [source]
WHERE
    [is_deleted] = 0
UNION
SELECT 0, '(All)', 'All branches'
ORDER BY [code]

GO

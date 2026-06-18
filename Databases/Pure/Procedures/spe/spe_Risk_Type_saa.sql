SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_Type_saa'
GO

-- AMB 30 May 03: changed sort order
CREATE PROCEDURE spe_Risk_Type_saa

AS

SELECT
    risk_type_id,
    code,
    [description],
    effective_date,
    is_deleted
FROM 
    Risk_Type
ORDER BY 
    is_deleted ASC, 
    code ASC

GO


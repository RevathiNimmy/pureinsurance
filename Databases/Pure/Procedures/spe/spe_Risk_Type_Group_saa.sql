SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_Type_Group_saa'
GO

CREATE PROCEDURE spe_Risk_Type_Group_saa

AS

SELECT
    risk_type_group_id,
    code,
    description,
    effective_date

 FROM Risk_Type_Group

WHERE is_deleted <> 1

ORDER BY risk_type_group_id ASC

GO


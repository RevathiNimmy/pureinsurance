SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Rule_Type_Saa'
GO

CREATE PROCEDURE spe_Rule_Type_Saa
AS

SELECT
    description,
    risk_type_rule_set_type_id
FROM risk_type_rule_set_type 
WHERE is_deleted = 0
ORDER BY description

GO

